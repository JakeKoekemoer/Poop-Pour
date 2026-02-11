using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authentication.Commands;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly RegisterUserCommandHandler _sut;

    public RegisterUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _sut = new RegisterUserCommandHandler(_userService, _jwtTokenService);
    }

    [Fact]
    public async Task Handle_ValidRegistration_CreatesUserAndReturnsToken()
    {
        _userService.GetUserByEmailAsync("new@test.com", Arg.Any<CancellationToken>()).Returns((UserDto?)null);
        _userService.GetUserByUserNameAsync("newuser", Arg.Any<CancellationToken>()).Returns((UserDto?)null);
        var createdUser = new UserDtoBuilder()
            .WithEmail("new@test.com")
            .WithUserName("newuser")
            .WithFirstName("New")
            .WithLastName("User")
            .Build();
        _userService
            .CreateUserAsync("newuser", "new@test.com", "Password1!", "New", "User", Arg.Any<CancellationToken>())
            .Returns(createdUser);
        _jwtTokenService
            .GenerateTokenAsync(createdUser.Id, createdUser.Email, createdUser.UserName, createdUser.Roles)
            .Returns(("jwt-token", new DateTime(2025, 12, 31)));

        var command = new RegisterUserCommand(
            "new@test.com",
            "newuser",
            "Password1!",
            "New",
            "User");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.User.Should().BeSameAs(createdUser);
        result.Token.Should().Be("jwt-token");
        result.ExpiresAt.Should().Be(new DateTime(2025, 12, 31));
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ThrowsUserAlreadyExistsException()
    {
        var existingUser = new UserDtoBuilder().WithEmail("existing@test.com").Build();
        _userService.GetUserByEmailAsync("existing@test.com", Arg.Any<CancellationToken>()).Returns(existingUser);

        var command = new RegisterUserCommand(
            "existing@test.com",
            "newuser",
            "Password1!",
            null,
            null);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
        await _userService.DidNotReceive().CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DuplicateUserName_ThrowsUserAlreadyExistsException()
    {
        _userService.GetUserByEmailAsync("new@test.com", Arg.Any<CancellationToken>()).Returns((UserDto?)null);
        var existingUser = new UserDtoBuilder().WithUserName("existinguser").Build();
        _userService.GetUserByUserNameAsync("existinguser", Arg.Any<CancellationToken>()).Returns(existingUser);

        var command = new RegisterUserCommand(
            "new@test.com",
            "existinguser",
            "Password1!",
            null,
            null);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
        await _userService.DidNotReceive().CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }
}
