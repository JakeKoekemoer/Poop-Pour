using NSubstitute;
using PoopNPour.Abstractions.Identity;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Builds a mock IUser for testing.
/// </summary>
public static class CurrentUserBuilder
{
    public static IUser CreateAuthenticated(string id, string? userName = null, string? email = null)
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns(id);
        user.UserName.Returns(userName ?? "user");
        user.Email.Returns(email ?? "user@example.com");
        user.IsAuthenticated.Returns(true);
        user.IsAdmin.Returns(false);
        user.FamilyIds.Returns(Enumerable.Empty<Guid>());
        return user;
    }

    public static IUser CreateAdmin(string id = "admin-1")
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns(id);
        user.UserName.Returns("admin");
        user.Email.Returns("admin@example.com");
        user.IsAuthenticated.Returns(true);
        user.IsAdmin.Returns(true);
        user.FamilyIds.Returns(Enumerable.Empty<Guid>());
        return user;
    }

    public static IUser CreateUnauthenticated()
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns((string?)null);
        user.UserName.Returns((string?)null);
        user.Email.Returns((string?)null);
        user.IsAuthenticated.Returns(false);
        user.IsAdmin.Returns(false);
        user.FamilyIds.Returns(Enumerable.Empty<Guid>());
        return user;
    }

    public static IUser WithFamilyIds(this IUser user, IEnumerable<Guid> familyIds)
    {
        user.FamilyIds.Returns(familyIds);
        return user;
    }
}
