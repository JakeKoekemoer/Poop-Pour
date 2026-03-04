using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMembersAsyncReturnsOnlyMembersOfRequestedFamily
{
    private static DbContextOptions<ApplicationDbContext> BuildOptions(string dbName) =>
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

    private static IUser MockAdmin() =>
        Substitute.For<IUser>() is var u
            ? (u.IsAdmin.Returns(true), u).Item2
            : null!;

    [Fact]
    public async Task GetFamilyMembersAsync_DoesNotReturnMembersOfOtherFamilies()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var targetFamilyId = Guid.NewGuid();
        var otherFamilyId  = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Users.AddRange(
                new ApplicationUser { Id = "u1", UserName = "u1", NormalizedUserName = "U1", Email = "u1@test.com", NormalizedEmail = "U1@TEST.COM", SecurityStamp = Guid.NewGuid().ToString() },
                new ApplicationUser { Id = "u2", UserName = "u2", NormalizedUserName = "U2", Email = "u2@test.com", NormalizedEmail = "U2@TEST.COM", SecurityStamp = Guid.NewGuid().ToString() }
            );
            seed.Families.AddRange(
                new Family { FamilyId = targetFamilyId, FamilyName = "Target", FamilyLastName = "Family" },
                new Family { FamilyId = otherFamilyId,  FamilyName = "Other",  FamilyLastName = "Family" }
            );
            seed.FamilyUsers.AddRange(
                new FamilyUser { FamilyId = targetFamilyId, UserId = "u1" },
                new FamilyUser { FamilyId = otherFamilyId,  UserId = "u2" }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var (members, totalCount) = await sut.GetFamilyMembersAsync(targetFamilyId, 1, 20);

        totalCount.Should().Be(1);
        members.Single().UserId.Should().Be("u1");
    }
}
