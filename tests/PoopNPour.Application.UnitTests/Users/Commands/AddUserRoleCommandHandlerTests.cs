using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserRoleCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly AddUserRoleCommandHandler _sut;

    public AddUserRoleCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new AddUserRoleCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidRole_AddsRoleAndReturnsUpdatedUserDto()
    {
        var userId = Guid.NewGuid().ToString();
        var existingUser = new UserDtoBuilder().WithId(userId).WithRoles("Family_Member").Build();
        var updatedUser = new UserDtoBuilder().WithId(userId).WithRoles("Family_Member", "Family_Head").Build();

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existingUser);
        _userService.AddUserRoleAsync(userId, "Family_Head", Arg.Any<CancellationToken>()).Returns(updatedUser);

        var command = new AddUserRoleCommand(userId, "Family_Head");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Roles.Should().Contain("Family_Head");
        await _userService.Received(1).AddUserRoleAsync(userId, "Family_Head", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var command = new AddUserRoleCommand(userId, "Family_Head");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().AddUserRoleAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
