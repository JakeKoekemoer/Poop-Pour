using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMembersAsyncOrdersByJoinedOn
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
    public async Task GetFamilyMembersAsync_OrdersResultsByCreatedOnAscending()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var familyId = Guid.NewGuid();
        var earlier = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var later   = new DateTimeOffset(2025, 6, 1, 0, 0, 0, TimeSpan.Zero);

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Users.AddRange(
                new ApplicationUser { Id = "u1", UserName = "first",  NormalizedUserName = "FIRST",  Email = "first@t.com",  NormalizedEmail = "FIRST@T.COM",  SecurityStamp = Guid.NewGuid().ToString() },
                new ApplicationUser { Id = "u2", UserName = "second", NormalizedUserName = "SECOND", Email = "second@t.com", NormalizedEmail = "SECOND@T.COM", SecurityStamp = Guid.NewGuid().ToString() }
            );
            seed.Families.Add(new Family { FamilyId = familyId, FamilyName = "Test", FamilyLastName = "Family" });
            // Insert later-joined member first to ensure ordering is by CreatedOn, not insert order
            seed.FamilyUsers.AddRange(
                new FamilyUser { FamilyId = familyId, UserId = "u2", CreatedOn = later   },
                new FamilyUser { FamilyId = familyId, UserId = "u1", CreatedOn = earlier }
            );
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var (members, _) = await sut.GetFamilyMembersAsync(familyId, 1, 20);
        var list = members.ToList();

        list[0].UserId.Should().Be("u1"); // joined earlier
        list[1].UserId.Should().Be("u2");
        list[0].JoinedOn.Should().BeBefore(list[1].JoinedOn);
    }
}
