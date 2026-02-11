using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Queries;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Queries;

public class GetMyProfileQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly IUser _currentUser;
    private readonly GetMyProfileQueryHandler _sut;

    public GetMyProfileQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _currentUser = CurrentUserBuilder.CreateAuthenticated("user-123", "me", "me@test.com");
        _sut = new GetMyProfileQueryHandler(_userService, _currentUser);
    }

    [Fact]
    public async Task Handle_ReturnsCurrentUserProfile()
    {
        var profile = new UserDtoBuilder()
            .WithId("user-123")
            .WithUserName("me")
            .WithEmail("me@test.com")
            .Build();
        _userService.GetUserByIdAsync("user-123", Arg.Any<CancellationToken>()).Returns(profile);

        var query = new GetMyProfileQuery();
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeSameAs(profile);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        _userService.GetUserByIdAsync("user-123", Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var query = new GetMyProfileQuery();
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*user-123*");
    }
}
