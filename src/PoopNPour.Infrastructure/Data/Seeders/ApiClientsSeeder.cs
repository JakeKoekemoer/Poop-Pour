using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.Identity;

namespace PoopNPour.Infrastructure.Data.Seeders;

public class ApiClientsSeeder(
    IIdentityService identityService,
    IJwtTokenService jwtTokenService,
    IConfiguration configuration,
    ILogger<ApiClientsSeeder> logger) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
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
