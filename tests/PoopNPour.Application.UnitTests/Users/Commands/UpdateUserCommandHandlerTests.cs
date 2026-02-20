using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly UpdateUserCommandHandler _sut;

    public UpdateUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new UpdateUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsUpdatedUser()
    {
        var userId = Guid.NewGuid().ToString();
        var existing = new UserDtoBuilder().WithId(userId).Build();
        var updated = new UserDtoBuilder().WithId(userId).WithFirstName("Updated").Build();

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existing);
        _userService.UpdateUserAsync(userId, "Updated", null, null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateUserCommand(userId, "Updated");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(updated);
        result.FirstName.Should().Be("Updated");
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var command = new UpdateUserCommand(userId, "Updated");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().UpdateUserAsync(
            Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UpdatesAllFields_PassesAllFieldsToService()
    {
        var userId = Guid.NewGuid().ToString();
        var existing = new UserDtoBuilder().WithId(userId).Build();
        var updated = new UserDtoBuilder()
            .WithId(userId)
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithEmail("john@test.com")
            .Build();

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existing);
        _userService.UpdateUserAsync(userId, "John", "Doe", "john@test.com", Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateUserCommand(userId, "John", "Doe", "john@test.com");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("john@test.com");
    }

    [Fact]
    public async Task Handle_EmailAlreadyInUse_ThrowsEmailAlreadyInUseException()
    {
        var userId = Guid.NewGuid().ToString();
        var existing = new UserDtoBuilder().WithId(userId).Build();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existing);
        _userService
            .UpdateUserAsync(userId, null, null, "taken@test.com", Arg.Any<CancellationToken>())
            .Returns(Task.FromException<UserDto>(new EmailAlreadyInUseException("taken@test.com")));

        var command = new UpdateUserCommand(userId, Email: "taken@test.com");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<EmailAlreadyInUseException>().WithMessage("*taken@test.com*");
    }
}
