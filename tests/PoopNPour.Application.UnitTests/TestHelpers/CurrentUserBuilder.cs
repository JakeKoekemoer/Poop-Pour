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
        return user;
    }

    public static IUser CreateUnauthenticated()
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns((string?)null);
        user.UserName.Returns((string?)null);
        user.Email.Returns((string?)null);
        user.IsAuthenticated.Returns(false);
        return user;
    }
}
