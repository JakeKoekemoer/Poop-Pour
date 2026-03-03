using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Infrastructure.Data;

/// <summary>
/// Seeds the database with initial data
/// </summary>
public class DatabaseSeeder(
    ApplicationDbContext context,
    IIdentityService identityService,
    IJwtTokenService jwtTokenService,
    IConfiguration configuration,
    ILogger<DatabaseSeeder> logger)
{
    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Ensure database is created and migrations are applied
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
            else
            {
                await context.Database.EnsureCreatedAsync(cancellationToken);
            }

            // Seed default roles first (required before creating users)
            await SeedDefaultRolesAsync(cancellationToken);

            // Seed default admin user
            await SeedDefaultAdminAsync(cancellationToken);

            // Seed API client users (Web API, Mobile API, etc.)
            await SeedApiClientsAsync(cancellationToken);

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedDefaultAdminAsync(CancellationToken cancellationToken)
    {
        var adminSection = configuration.GetSection("DefaultAdmin");
        var userName = adminSection["UserName"];
        var email = adminSection["Email"];
        var password = adminSection["Password"];
        var firstName = adminSection["FirstName"];
        var lastName = adminSection["LastName"];

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Default admin configuration is missing. Skipping admin user seeding.");
            return;
        }

        var existingUser = await identityService.GetUserByUserNameAsync(userName, cancellationToken);
        if (existingUser != null)
        {
            logger.LogInformation("Default admin user already exists. Skipping admin user creation.");
            return;
        }

        logger.LogInformation("Creating default admin user: {UserName}", userName);
        var adminUser = await identityService.CreateUserAsync(
            userName,
            email,
            password,
            firstName,
            lastName,
            cancellationToken);

        await identityService.AddUserToRoleAsync(adminUser, Roles.Administrator, cancellationToken);

        logger.LogInformation("Default admin user created successfully: {UserName}", userName);
    }

    private async Task SeedDefaultRolesAsync(CancellationToken cancellationToken)
    {
        // Use reflection to get all public const string fields from the Roles class
        var rolesType = typeof(Roles);
        var roleFields = rolesType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(fi => fi.GetValue(null) as string)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .ToList();

        if (roleFields.Count == 0)
        {
            logger.LogWarning("No roles found in Roles class. Skipping role seeding.");
            return;
        }

        logger.LogInformation("Seeding {Count} roles from Roles class", roleFields.Count);

        foreach (var role in roleFields)
        {
            await identityService.EnsureRoleExistsAsync(role!, cancellationToken);
        }

        logger.LogInformation("Successfully seeded {Count} roles: {Roles}", 
            roleFields.Count, 
            string.Join(", ", roleFields));
    }

    private async Task SeedApiClientsAsync(CancellationToken cancellationToken)
    {
        var apiClientsSection = configuration.GetSection("ApiClients");
        var apiClients = apiClientsSection.GetChildren();

        if (!apiClients.Any())
        {
            logger.LogInformation("No API clients configured. Skipping API client seeding.");
            return;
        }

        foreach (var apiClientConfig in apiClients)
        {
            var userName = apiClientConfig["UserName"];
            var email = apiClientConfig["Email"];
            var password = apiClientConfig["Password"];
            var role = apiClientConfig["Role"];
            var firstName = apiClientConfig["FirstName"];
            var lastName = apiClientConfig["LastName"];

            if (string.IsNullOrWhiteSpace(userName))
            {
                logger.LogWarning("API client UserName is required. Skipping this API client.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                logger.LogWarning("API client Email is required for {UserName}. Skipping.", userName);
                continue;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                logger.LogWarning("API client Password is required for {UserName}. Skipping.", userName);
                continue;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                logger.LogWarning("API client Role is required for {UserName}. Skipping.", userName);
                continue;
            }

            var existingUser = await identityService.GetUserByUserNameAsync(userName, cancellationToken);
            
            if (existingUser == null)
            {
                logger.LogInformation("Creating API client user: {UserName}", userName);
                existingUser = await identityService.CreateUserAsync(
                    userName,
                    email,
                    password,
                    firstName ?? "API",
                    lastName ?? "Client",
                    cancellationToken);

                await identityService.AddUserToRoleAsync(existingUser, role, cancellationToken);
                
                logger.LogInformation("API client user {UserName} created successfully", userName);
                
                // Always generate token for new users
                logger.LogInformation("Generating API token for {UserName} with role {Role}", userName, role);
                var apiToken = jwtTokenService.GenerateApiToken(existingUser.Id, new[] { role });
                
                await identityService.SetAuthenticationTokenAsync(
                    existingUser,
                    "PoopNPour",
                    "ApiToken",
                    apiToken,
                    cancellationToken);

                logger.LogInformation("API Token for {UserName}: {Token}", userName, apiToken);
            }
            else
            {
                logger.LogInformation("API client user {UserName} already exists. Skipping.", userName);
            }
        }

        logger.LogInformation("API client seeding completed");
    }
}
