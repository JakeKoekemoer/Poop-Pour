using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly IUser _currentUser;
    private readonly UpdateMyProfileCommandHandler _sut;

    public UpdateMyProfileCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _currentUser = CurrentUserBuilder.CreateAuthenticated("user-123");
        _sut = new UpdateMyProfileCommandHandler(_userService, _currentUser);
    }

    [Fact]
    public async Task Handle_UpdatesFirstNameOnly_ReturnsUpdatedUser()
    {
        var updated = new UserDtoBuilder().WithId("user-123").WithFirstName("Updated").Build();
        _userService.UpdateUserAsync("user-123", "Updated", null, null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateMyProfileCommand("Updated", null, null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(updated);
        result.FirstName.Should().Be("Updated");
    }

    [Fact]
    public async Task Handle_UpdatesLastNameOnly_ReturnsUpdatedUser()
    {
        var updated = new UserDtoBuilder().WithId("user-123").WithLastName("Updated").Build();
        _userService.UpdateUserAsync("user-123", null, "Updated", null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateMyProfileCommand(null, "Updated", null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.LastName.Should().Be("Updated");
    }

    [Fact]
    public async Task Handle_UpdatesEmailOnly_ReturnsUpdatedUser()
    {
        var updated = new UserDtoBuilder().WithId("user-123").WithEmail("new@test.com").Build();
        _userService.UpdateUserAsync("user-123", null, null, "new@test.com", Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateMyProfileCommand(null, null, "new@test.com");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Email.Should().Be("new@test.com");
    }

    [Fact]
    public async Task Handle_UpdatesAllFields_ReturnsUpdatedUser()
    {
        var updated = new UserDtoBuilder()
            .WithId("user-123")
            .WithFirstName("F")
            .WithLastName("L")
            .WithEmail("e@test.com")
            .Build();
        _userService.UpdateUserAsync("user-123", "F", "L", "e@test.com", Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateMyProfileCommand("F", "L", "e@test.com");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.FirstName.Should().Be("F");
        result.LastName.Should().Be("L");
        result.Email.Should().Be("e@test.com");
    }

    [Fact]
    public async Task Handle_EmailAlreadyInUse_ThrowsEmailAlreadyInUseException()
    {
        _userService
            .UpdateUserAsync("user-123", null, null, "taken@test.com", Arg.Any<CancellationToken>())
            .Returns(Task.FromException<UserDto>(new EmailAlreadyInUseException("taken@test.com")));

        var command = new UpdateMyProfileCommand(null, null, "taken@test.com");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<EmailAlreadyInUseException>().WithMessage("*taken@test.com*");
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        _userService
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<UserDto>(new UserNotFoundException("user-123")));

        var command = new UpdateMyProfileCommand("F", null, null);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}
