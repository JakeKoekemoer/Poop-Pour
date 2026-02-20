using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserClaimsCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly RemoveUserClaimsCommandHandler _sut;

    public RemoveUserClaimsCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new RemoveUserClaimsCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidClaims_RemovesClaimsAndReturnsRemainingList()
    {
        var userId = Guid.NewGuid().ToString();
        var existingUser = new UserDtoBuilder().WithId(userId).Build();
        var claimsToRemove = ClaimDtoBuilder.BuildList(("department", "engineering"));
        var remaining = ClaimDtoBuilder.BuildList(("level", "senior"));

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existingUser);
        _userService.RemoveUserClaimsAsync(userId, claimsToRemove, Arg.Any<CancellationToken>()).Returns(remaining);

        var command = new RemoveUserClaimsCommand(userId, claimsToRemove);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().ContainSingle(c => c.Type == "level" && c.Value == "senior");
        result.Should().NotContain(c => c.Type == "department");
    }

    [Fact]
    public async Task Handle_RemovingAllClaims_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid().ToString();
        var existingUser = new UserDtoBuilder().WithId(userId).Build();
        var claimsToRemove = ClaimDtoBuilder.BuildList(("department", "engineering"));

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existingUser);
        _userService.RemoveUserClaimsAsync(userId, claimsToRemove, Arg.Any<CancellationToken>())
            .Returns(new List<ClaimDto>());

        var command = new RemoveUserClaimsCommand(userId, claimsToRemove);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var claims = ClaimDtoBuilder.BuildList(("department", "engineering"));
        var command = new RemoveUserClaimsCommand(userId, claims);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().RemoveUserClaimsAsync(
            Arg.Any<string>(), Arg.Any<IEnumerable<ClaimDto>>(), Arg.Any<CancellationToken>());
    }
}
