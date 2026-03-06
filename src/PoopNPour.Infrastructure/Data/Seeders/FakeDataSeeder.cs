using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Domain.Enums.DiperLog;
using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Infrastructure.Data.Seeders;

public class FakeDataSeeder(
    ApplicationDbContext context,
    IIdentityService identityService,
    IConfiguration configuration,
    ILogger<FakeDataSeeder> logger) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!configuration.GetValue<bool>("SeedFakeData"))
        {
            logger.LogDebug("SeedFakeData is disabled. Skipping fake data seeding.");
            return;
        }

        var adminSection = configuration.GetSection("DefaultAdmin");
        var adminUserName = adminSection["UserName"];
        var adminPassword = adminSection["Password"];
        if (string.IsNullOrWhiteSpace(adminUserName))
        {
            logger.LogWarning("SeedFakeData is enabled but DefaultAdmin is not configured. Skipping fake data seeding.");
            return;
        }

        var adminUser = await identityService.GetUserByUserNameAsync(adminUserName, cancellationToken);
        if (adminUser == null)
        {
            logger.LogWarning("SeedFakeData is enabled but admin user does not exist. Skipping fake data seeding.");
            return;
        }

        const string firstDevFamilyName = "Dev Family 1";
        var existingFamily = await context.Families
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.FamilyName == firstDevFamilyName, cancellationToken);

        if (existingFamily != null)
        {
            logger.LogInformation("Fake data already exists. Skipping.");
            return;
        }

        logger.LogInformation("Seeding fake data for development/testing (5 families, 3 members + 3 dependents each, 12-20 logs per dependent)...");

        var now = DateTimeOffset.UtcNow;
        const string devPassword = "DevUser@123";
        var rnd = new Random(42);
        var faker = new Faker();

        var medicineNames = new[] { "Vitamin D Drops", "Children's Tylenol", "Infant Motrin", "Probiotic Drops", "Iron Supplement", "Multivitamin", "Calpol", "Gripe Water" };
        var dosages = new[] { "1 drop daily", "5ml", "2.5ml", "0.5ml", "1 tablet", "as directed" };
        var feedTypes = new[] { FeedLogType.BREAST_MILK, FeedLogType.FORMULA, FeedLogType.SOLID_FOOD };
        var fecalColours = new[] { FecalDischargeColour.BROWN, FecalDischargeColour.YELLOW, FecalDischargeColour.GREEN };
        var urinaryColours = new[] { UrinalDischargeColour.YELLOW, UrinalDischargeColour.ORANGE };

        var allDependents = new List<Dependent>();

        var masterPassword = !string.IsNullOrWhiteSpace(adminPassword) ? adminPassword : devPassword;
        var theMaster = await GetOrCreateTheMasterUserAsync(masterPassword, cancellationToken);

        for (var f = 0; f < 5; f++)
        {
            var familyId = Guid.NewGuid();
            var familyLastName = faker.Name.LastName();
            var familyName = $"Dev Family {f + 1}";

            context.Families.Add(new Family
            {
                FamilyId = familyId,
                FamilyName = familyName,
                FamilyLastName = familyLastName,
                CreatedOn = now,
                LastModifiedOn = now
            });

            if (f == 0)
            {
                context.FamilyUsers.Add(new FamilyUser
                {
                    FamilyId = familyId,
                    UserId = adminUser.Id,
                    Role = FamilyRole.Owner,
                    CreatedOn = now,
                    LastModifiedOn = now
                });

                var devUser1 = await GetOrCreateDevUserAsync(faker, "devuser1_f1", devPassword, cancellationToken);
                var devUser2 = await GetOrCreateDevUserAsync(faker, "devuser2_f1", devPassword, cancellationToken);
                context.FamilyUsers.Add(new FamilyUser { FamilyId = familyId, UserId = devUser1.Id, Role = FamilyRole.Member, CreatedOn = now, LastModifiedOn = now });
                context.FamilyUsers.Add(new FamilyUser { FamilyId = familyId, UserId = devUser2.Id, Role = FamilyRole.Admin, CreatedOn = now, LastModifiedOn = now });
            }
            else
            {
                for (var m = 0; m < 3; m++)
                {
                    var userName = $"devuser{m + 1}_f{f + 1}";
                    var user = await GetOrCreateDevUserAsync(faker, userName, devPassword, cancellationToken);
                    var role = m == 0 ? FamilyRole.Owner : (m == 1 ? FamilyRole.Admin : FamilyRole.Member);
                    context.FamilyUsers.Add(new FamilyUser { FamilyId = familyId, UserId = user.Id, Role = role, CreatedOn = now, LastModifiedOn = now });
                }
            }

            context.FamilyUsers.Add(new FamilyUser
            {
                FamilyId = familyId,
                UserId = theMaster.Id,
                Role = FamilyRole.Member,
                CreatedOn = now,
                LastModifiedOn = now
            });

            for (var d = 0; d < 3; d++)
            {
                var dob = d switch
                {
                    0 => now.AddMonths(-2),
                    1 => now.AddMonths(-8),
                    _ => now.AddYears(-2)
                };

                var dep = new Dependent
                {
                    DependentId = Guid.NewGuid(),
                    FamilyId = familyId,
                    DependentName = faker.Name.FirstName(),
                    DependentSurname = familyLastName,
                    DateOfBirth = dob,
                    CreatedOn = now,
                    LastModifiedOn = now
                };
                context.Dependents.Add(dep);
                allDependents.Add(dep);
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        foreach (var dep in allDependents)
        {
            var targetLogCount = rnd.Next(12, 21);
            var feedCount = rnd.Next(4, 8);
            var diperCount = rnd.Next(4, 7);
            var medCount = targetLogCount - feedCount - diperCount;
            if (medCount < 2)
            {
                medCount = 2;
                var remaining = targetLogCount - medCount;
                feedCount = Math.Min(feedCount, Math.Max(4, remaining - 4));
                diperCount = remaining - feedCount;
            }

            for (var i = 0; i < feedCount; i++)
            {
                context.FeedLogs.Add(new FeedLog
                {
                    FeedLogId = Guid.NewGuid(),
                    DependentId = dep.DependentId,
                    FeedType = feedTypes[rnd.Next(feedTypes.Length)],
                    TimeFed = now.AddHours(-rnd.Next(1, 168)),
                    MililitersFed = rnd.Next(60, 250),
                    CreatedOn = now,
                    LastModifiedOn = now
                });
            }
            for (var i = 0; i < diperCount; i++)
            {
                context.DiperLogs.Add(new DiperLog
                {
                    DiperLogId = Guid.NewGuid(),
                    DependentId = dep.DependentId,
                    DiperDate = now.AddHours(-rnd.Next(1, 168)),
                    FecalDischargeColour = fecalColours[rnd.Next(fecalColours.Length)],
                    UrinaryDischargeColour = urinaryColours[rnd.Next(urinaryColours.Length)],
                    CreatedOn = now,
                    LastModifiedOn = now
                });
            }
            for (var i = 0; i < medCount; i++)
            {
                context.MedicineLogs.Add(new MedicineLog
                {
                    MedicineLogId = Guid.NewGuid(),
                    DependentId = dep.DependentId,
                    MedicineName = medicineNames[rnd.Next(medicineNames.Length)],
                    Dosage = dosages[rnd.Next(dosages.Length)],
                    TimeAdministered = now.AddHours(-rnd.Next(1, 336)),
                    CreatedOn = now,
                    LastModifiedOn = now
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        var totalLogs = allDependents.Count * 15;
        logger.LogInformation("Fake data seeded successfully: 5 families, 15 members, 15 dependents, ~{TotalLogs} logs.", totalLogs);
    }

    private async Task<ApplicationUser> GetOrCreateDevUserAsync(Faker faker, string userName, string password, CancellationToken cancellationToken)
    {
        var existing = await identityService.GetUserByUserNameAsync(userName, cancellationToken);
        if (existing != null)
            return existing;
        var firstName = faker.Name.FirstName();
        var lastName = faker.Name.LastName();
        var user = await identityService.CreateUserAsync(
            userName, $"{userName}@dev.poopnpour.com", password, firstName, lastName, cancellationToken);
        await identityService.AddUserToRoleAsync(user, Roles.Tenant, cancellationToken);
        return user;
    }

    private async Task<ApplicationUser> GetOrCreateTheMasterUserAsync(string password, CancellationToken cancellationToken)
    {
        const string userName = "themaster";
        var existing = await identityService.GetUserByUserNameAsync(userName, cancellationToken);
        if (existing != null)
            return existing;
        var user = await identityService.CreateUserAsync(
            userName, "themaster@dev.poopnpour.com", password, "The", "Master", cancellationToken);
        await identityService.AddUserToRoleAsync(user, Roles.Tenant, cancellationToken);
        return user;
    }
}
