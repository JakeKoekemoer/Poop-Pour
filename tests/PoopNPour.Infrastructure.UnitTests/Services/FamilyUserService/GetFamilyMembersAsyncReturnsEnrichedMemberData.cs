using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using PoopNPour.Infrastructure.Services.FamilyUserService;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMembersAsyncReturnsEnrichedMemberData
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
    public async Task GetFamilyMembersAsync_ReturnsUserNameEmailFamilyName()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var familyId = Guid.NewGuid();
        const string userId = "user-abc";

        await using (var seed = new ApplicationDbContext(options))
        {
            var appUser = new ApplicationUser
            {
                Id        = userId,
                UserName  = "jsmith",
                Email     = "j@smith.com",
                FirstName = "John",
                LastName  = "Smith",
                NormalizedUserName = "JSMITH",
                NormalizedEmail = "J@SMITH.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            seed.Users.Add(appUser);
            seed.Families.Add(new Family { FamilyId = familyId, FamilyName = "Smith", FamilyLastName = "Family" });
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = familyId, UserId = userId, Role = FamilyRole.Owner });
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var (members, totalCount) = await sut.GetFamilyMembersAsync(familyId, 1, 20);

        totalCount.Should().Be(1);
        var member = members.Single();
        member.UserId.Should().Be(userId);
        member.UserName.Should().Be("jsmith");
        member.Email.Should().Be("j@smith.com");
        member.FirstName.Should().Be("John");
        member.LastName.Should().Be("Smith");
        member.FamilyName.Should().Be("Smith");
        member.FamilyLastName.Should().Be("Family");
        member.Role.Should().Be(FamilyRole.Owner);
    }
}
