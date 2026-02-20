using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Domain.Enums.DiperLog;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Data;

public class FamilyTenantFilterTests
{
    private static DbContextOptions<ApplicationDbContext> BuildOptions(string dbName) =>
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

    private static IUser MockUser(string userId, bool isAdmin = false)
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns(userId);
        user.IsAdmin.Returns(isAdmin);
        return user;
    }

    [Fact]
    public async Task Family_NonAdmin_OnlyOwnFamilyReturned()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);

        var family1Id = Guid.NewGuid();
        var family2Id = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.AddRange(
                new Family { FamilyId = family1Id, FamilyName = "Smith", FamilyLastName = "Smith" },
                new Family { FamilyId = family2Id, FamilyName = "Jones", FamilyLastName = "Jones" }
            );
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = family1Id, UserId = "user1" });
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockUser("user1", isAdmin: false));
        var results = await ctx.Families.ToListAsync();

        results.Should().HaveCount(1);
        results.Single().FamilyId.Should().Be(family1Id);
    }

    [Fact]
    public async Task Family_Admin_AllFamiliesReturned()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);

        var family1Id = Guid.NewGuid();
        var family2Id = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.AddRange(
                new Family { FamilyId = family1Id, FamilyName = "Smith", FamilyLastName = "Smith" },
                new Family { FamilyId = family2Id, FamilyName = "Jones", FamilyLastName = "Jones" }
            );
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = family1Id, UserId = "user1" });
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockUser("admin1", isAdmin: true));
        var results = await ctx.Families.ToListAsync();

        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task FamilyUser_NonAdmin_OnlyOwnFamilysUsersReturned()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);

        var family1Id = Guid.NewGuid();
        var family2Id = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.AddRange(
                new Family { FamilyId = family1Id, FamilyName = "Smith", FamilyLastName = "Smith" },
                new Family { FamilyId = family2Id, FamilyName = "Jones", FamilyLastName = "Jones" }
            );
            seed.FamilyUsers.AddRange(
                new FamilyUser { FamilyId = family1Id, UserId = "user1" },
                new FamilyUser { FamilyId = family2Id, UserId = "user2" }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockUser("user1", isAdmin: false));
        var results = await ctx.FamilyUsers.ToListAsync();

        results.Should().HaveCount(1);
        results.Single().FamilyId.Should().Be(family1Id);
    }

    [Fact]
    public async Task Dependent_NonAdmin_OnlyDependentsInOwnFamilyReturned()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);

        var family1Id = Guid.NewGuid();
        var family2Id = Guid.NewGuid();
        var dep1Id = Guid.NewGuid();
        var dep2Id = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.AddRange(
                new Family { FamilyId = family1Id, FamilyName = "Smith", FamilyLastName = "Smith" },
                new Family { FamilyId = family2Id, FamilyName = "Jones", FamilyLastName = "Jones" }
            );
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = family1Id, UserId = "user1" });
            seed.Dependents.AddRange(
                new Dependent { DependentId = dep1Id, FamilyId = family1Id, DependentName = "Alice", DependentSurname = "Smith", DateOfBirth = DateTimeOffset.UtcNow.AddYears(-2) },
                new Dependent { DependentId = dep2Id, FamilyId = family2Id, DependentName = "Bob", DependentSurname = "Jones", DateOfBirth = DateTimeOffset.UtcNow.AddYears(-1) }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockUser("user1", isAdmin: false));
        var results = await ctx.Dependents.ToListAsync();

        results.Should().HaveCount(1);
        results.Single().DependentId.Should().Be(dep1Id);
    }

    [Fact]
    public async Task DiperLog_NonAdmin_OnlyLogsForOwnFamilysDependent()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);

        var family1Id = Guid.NewGuid();
        var family2Id = Guid.NewGuid();
        var dep1Id = Guid.NewGuid();
        var dep2Id = Guid.NewGuid();
        var log1Id = Guid.NewGuid();
        var log2Id = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.AddRange(
                new Family { FamilyId = family1Id, FamilyName = "Smith", FamilyLastName = "Smith" },
                new Family { FamilyId = family2Id, FamilyName = "Jones", FamilyLastName = "Jones" }
            );
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = family1Id, UserId = "user1" });
            seed.Dependents.AddRange(
                new Dependent { DependentId = dep1Id, FamilyId = family1Id, DependentName = "Alice", DependentSurname = "Smith", DateOfBirth = DateTimeOffset.UtcNow.AddYears(-2) },
                new Dependent { DependentId = dep2Id, FamilyId = family2Id, DependentName = "Bob", DependentSurname = "Jones", DateOfBirth = DateTimeOffset.UtcNow.AddYears(-1) }
            );
            seed.DiperLogs.AddRange(
                new DiperLog { DiperLogId = log1Id, DependentId = dep1Id, DiperDate = DateTimeOffset.UtcNow, FecalDischargeColour = FecalDischargeColour.YELLOW, UrinaryDischargeColour = UrinalDischargeColour.YELLOW },
                new DiperLog { DiperLogId = log2Id, DependentId = dep2Id, DiperDate = DateTimeOffset.UtcNow, FecalDischargeColour = FecalDischargeColour.GREEN, UrinaryDischargeColour = UrinalDischargeColour.ORANGE }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockUser("user1", isAdmin: false));
        var results = await ctx.DiperLogs.ToListAsync();

        results.Should().HaveCount(1);
        results.Single().DiperLogId.Should().Be(log1Id);
    }
}
