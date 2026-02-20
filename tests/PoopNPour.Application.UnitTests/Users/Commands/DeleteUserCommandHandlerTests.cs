using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly DeleteUserCommandHandler _sut;

    public DeleteUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new DeleteUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ExistingUser_DeletesUserAndReturnsUnit()
    {
        var userId = Guid.NewGuid().ToString();
        var existing = new UserDtoBuilder().WithId(userId).Build();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existing);

        var command = new DeleteUserCommand(userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _userService.Received(1).DeleteUserAsync(userId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var command = new DeleteUserCommand(userId);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().DeleteUserAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
