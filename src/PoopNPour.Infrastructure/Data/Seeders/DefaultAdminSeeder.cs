using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Infrastructure.Data.Seeders;

public class DefaultAdminSeeder(
    IIdentityService identityService,
    IConfiguration configuration,
    ILogger<DefaultAdminSeeder> logger) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
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
}
