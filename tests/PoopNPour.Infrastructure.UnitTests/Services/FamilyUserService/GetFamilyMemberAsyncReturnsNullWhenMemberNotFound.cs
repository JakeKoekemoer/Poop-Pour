using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Entities;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Services.FamilyUserService;

public class GetFamilyMemberAsyncReturnsNullWhenMemberNotFound
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
    public async Task GetFamilyMemberAsync_NoMatchingRecord_ReturnsNull()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = BuildOptions(dbName);
        var familyId = Guid.NewGuid();

        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Families.Add(new Family { FamilyId = familyId, FamilyName = "Empty", FamilyLastName = "Family" });
            await seed.SaveChangesAsync();
        }

        await using var ctx = new ApplicationDbContext(options, MockAdmin());
        var sut = new Infrastructure.Services.FamilyUserService.FamilyUserService(ctx);

        var result = await sut.GetFamilyMemberAsync(familyId, "no-such-user");

        result.Should().BeNull();
    }
}
