using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new CreateUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesUserAndReturnsUserDto()
    {
        var expected = new UserDtoBuilder()
            .WithUserName("newuser")
            .WithEmail("new@test.com")
            .WithFirstName("New")
            .WithLastName("User")
            .WithRoles("Web_Api")
            .Build();
        _userService
            .CreateUserAsync("newuser", "new@test.com", "Password1!", "New", "User", null, Arg.Any<CancellationToken>())
            .Returns(expected);

        var command = new CreateUserCommand("newuser", "new@test.com", "Password1!", "New", "User");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(expected);
        result.UserName.Should().Be("newuser");
        result.Email.Should().Be("new@test.com");
    }

    [Fact]
    public async Task Handle_WithExplicitRole_PassesRoleToService()
    {
        var expected = new UserDtoBuilder()
            .WithUserName("adminuser")
            .WithRoles("Administrator")
            .Build();
        _userService
            .CreateUserAsync("adminuser", "admin@test.com", "Password1!", null, null, "Administrator", Arg.Any<CancellationToken>())
            .Returns(expected);

        var command = new CreateUserCommand("adminuser", "admin@test.com", "Password1!", Role: "Administrator");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(expected);
        await _userService.Received(1).CreateUserAsync(
            "adminuser", "admin@test.com", "Password1!", null, null, "Administrator", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithoutRole_PassesNullRoleToService()
    {
        var expected = new UserDtoBuilder().WithUserName("user").Build();
        _userService
            .CreateUserAsync("user", "user@test.com", "Password1!", null, null, null, Arg.Any<CancellationToken>())
            .Returns(expected);

        var command = new CreateUserCommand("user", "user@test.com", "Password1!");
        await _sut.Handle(command, CancellationToken.None);

        await _userService.Received(1).CreateUserAsync(
            "user", "user@test.com", "Password1!", null, null, null, Arg.Any<CancellationToken>());
    }
}
