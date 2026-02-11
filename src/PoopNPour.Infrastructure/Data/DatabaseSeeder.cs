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
public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        ApplicationDbContext context,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Ensure database is created and migrations are applied
            await _context.Database.MigrateAsync(cancellationToken);

            // Seed default roles first (required before creating users)
            await SeedDefaultRolesAsync(cancellationToken);

            // Seed default admin user
            await SeedDefaultAdminAsync(cancellationToken);

            // Seed API client users (Web API, Mobile API, etc.)
            await SeedApiClientsAsync(cancellationToken);

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedDefaultAdminAsync(CancellationToken cancellationToken)
    {
        var adminSection = _configuration.GetSection("DefaultAdmin");
        var userName = adminSection["UserName"];
        var email = adminSection["Email"];
        var password = adminSection["Password"];
        var firstName = adminSection["FirstName"];
        var lastName = adminSection["LastName"];

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Default admin configuration is missing. Skipping admin user seeding.");
            return;
        }

        // Check if admin user already exists
        var existingUser = await _identityService.GetUserByUserNameAsync(userName, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogInformation("Default admin user already exists. Skipping admin user creation.");
            return;
        }

        // Create admin user
        _logger.LogInformation("Creating default admin user: {UserName}", userName);
        var adminUser = await _identityService.CreateUserAsync(
            userName,
            email,
            password,
            firstName,
            lastName,
            cancellationToken);

        // Add admin role
        await _identityService.AddUserToRoleAsync(adminUser, Roles.Administrator, cancellationToken);

        _logger.LogInformation("Default admin user created successfully: {UserName}", userName);
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
            _logger.LogWarning("No roles found in Roles class. Skipping role seeding.");
            return;
        }

        _logger.LogInformation("Seeding {Count} roles from Roles class", roleFields.Count);

        foreach (var role in roleFields)
        {
            await _identityService.EnsureRoleExistsAsync(role!, cancellationToken);
        }

        _logger.LogInformation("Successfully seeded {Count} roles: {Roles}", 
            roleFields.Count, 
            string.Join(", ", roleFields));
    }

    private async Task SeedApiClientsAsync(CancellationToken cancellationToken)
    {
        var apiClientsSection = _configuration.GetSection("ApiClients");
        var apiClients = apiClientsSection.GetChildren();

        if (!apiClients.Any())
        {
            _logger.LogInformation("No API clients configured. Skipping API client seeding.");
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
                _logger.LogWarning("API client UserName is required. Skipping this API client.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("API client Email is required for {UserName}. Skipping.", userName);
                continue;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("API client Password is required for {UserName}. Skipping.", userName);
                continue;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                _logger.LogWarning("API client Role is required for {UserName}. Skipping.", userName);
                continue;
            }

            // Check if API client user already exists
            var existingUser = await _identityService.GetUserByUserNameAsync(userName, cancellationToken);
            if (existingUser != null)
            {
                _logger.LogInformation("API client user {UserName} already exists. Skipping creation.", userName);
                continue;
            }

            // Create API client user
            _logger.LogInformation("Creating API client user: {UserName}", userName);
            var apiClientUser = await _identityService.CreateUserAsync(
                userName,
                email,
                password,
                firstName ?? "API",
                lastName ?? "Client",
                cancellationToken);

            // Add role to the user
            await _identityService.AddUserToRoleAsync(apiClientUser, role, cancellationToken);

            // Generate and store API token
            _logger.LogInformation("Generating API token for {UserName} with role {Role}", userName, role);
            var apiToken = _jwtTokenService.GenerateApiToken(apiClientUser.Id, new[] { role });
            
            await _identityService.SetAuthenticationTokenAsync(
                apiClientUser,
                "PoopNPour",
                "ApiToken",
                apiToken,
                cancellationToken);

            _logger.LogInformation("API client user {UserName} created successfully with API token", userName);
            _logger.LogDebug("API Token for {UserName}: {Token}", userName, apiToken);
        }

        _logger.LogInformation("API client seeding completed");
    }
}
