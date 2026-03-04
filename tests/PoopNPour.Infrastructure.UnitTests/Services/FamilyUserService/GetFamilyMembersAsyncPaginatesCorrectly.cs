using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMembersAsyncPaginatesCorrectly
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
    public async Task GetFamilyMembersAsync_PageSizeOne_ReturnsOneItemAndCorrectTotalCount()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var familyId = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Users.AddRange(
                new ApplicationUser { Id = "u1", UserName = "u1", NormalizedUserName = "U1", Email = "u1@t.com", NormalizedEmail = "U1@T.COM", SecurityStamp = Guid.NewGuid().ToString() },
                new ApplicationUser { Id = "u2", UserName = "u2", NormalizedUserName = "U2", Email = "u2@t.com", NormalizedEmail = "U2@T.COM", SecurityStamp = Guid.NewGuid().ToString() },
                new ApplicationUser { Id = "u3", UserName = "u3", NormalizedUserName = "U3", Email = "u3@t.com", NormalizedEmail = "U3@T.COM", SecurityStamp = Guid.NewGuid().ToString() }
            );
            seed.Families.Add(new Family { FamilyId = familyId, FamilyName = "Test", FamilyLastName = "Family" });
            seed.FamilyUsers.AddRange(
                new FamilyUser { FamilyId = familyId, UserId = "u1" },
                new FamilyUser { FamilyId = familyId, UserId = "u2" },
                new FamilyUser { FamilyId = familyId, UserId = "u3" }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var (page1Items, totalCount) = await sut.GetFamilyMembersAsync(familyId, 1, 1);
        var (page4Items, _)          = await sut.GetFamilyMembersAsync(familyId, 4, 1);

        totalCount.Should().Be(3);
        page1Items.Should().HaveCount(1);
        page4Items.Should().BeEmpty();
    }
}
