using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMemberAsyncReturnsEnrichedMember
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
    public async Task GetFamilyMemberAsync_ValidIds_ReturnsEnrichedDto()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var familyId = Guid.NewGuid();
        const string userId = "user-xyz";

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Users.Add(new ApplicationUser
            {
                Id        = userId,
                UserName  = "alice",
                Email     = "alice@example.com",
                FirstName = "Alice",
                LastName  = "Wonderland",
                NormalizedUserName = "ALICE",
                NormalizedEmail    = "ALICE@EXAMPLE.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            });
            seed.Families.Add(new Family { FamilyId = familyId, FamilyName = "Wonder", FamilyLastName = "Land" });
            seed.FamilyUsers.Add(new FamilyUser { FamilyId = familyId, UserId = userId, Role = FamilyRole.Admin });
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var result = await sut.GetFamilyMemberAsync(familyId, userId);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
        result.UserName.Should().Be("alice");
        result.Email.Should().Be("alice@example.com");
        result.FirstName.Should().Be("Alice");
        result.LastName.Should().Be("Wonderland");
        result.FamilyId.Should().Be(familyId);
        result.FamilyName.Should().Be("Wonder");
        result.FamilyLastName.Should().Be("Land");
        result.Role.Should().Be(FamilyRole.Admin);
    }
}
