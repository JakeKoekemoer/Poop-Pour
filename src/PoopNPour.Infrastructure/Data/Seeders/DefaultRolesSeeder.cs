using System.Reflection;
using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Infrastructure.Data.Seeders;

public class DefaultRolesSeeder(IIdentityService identityService, ILogger<DefaultRolesSeeder> logger) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
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
}
