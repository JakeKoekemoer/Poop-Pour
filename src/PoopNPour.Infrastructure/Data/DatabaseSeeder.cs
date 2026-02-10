using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        ApplicationDbContext context,
        IIdentityService identityService,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _identityService = identityService;
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

            // Seed default admin user
            await SeedDefaultAdminAsync(cancellationToken);

            // Seed default roles
            await SeedDefaultRolesAsync(cancellationToken);

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
}
