using NSubstitute;
using PoopNPour.Abstractions.Identity;

namespace PoopNPour.Infrastructure.UnitTests.TestHelpers;

public static class UserMockBuilder
{
    public static IUser CreateAuthenticated(string id = "user-123")
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns(id);
        user.IsAuthenticated.Returns(true);
        user.IsAdmin.Returns(false);
        return user;
    }

    public static IUser CreateAdmin(string id = "admin-123")
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns(id);
        user.IsAuthenticated.Returns(true);
        user.IsAdmin.Returns(true);
        return user;
    }

    public static IUser CreateUnauthenticated()
    {
        var user = Substitute.For<IUser>();
        user.Id.Returns((string?)null);
        user.IsAuthenticated.Returns(false);
        user.IsAdmin.Returns(false);
        return user;
    }
}
