using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserClaimsCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly AddUserClaimsCommandHandler _sut;

    public AddUserClaimsCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new AddUserClaimsCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidClaims_AddsClaimsAndReturnsUpdatedList()
    {
        var userId = Guid.NewGuid().ToString();
        var existingUser = new UserDtoBuilder().WithId(userId).Build();
        var newClaims = ClaimDtoBuilder.BuildList(("department", "engineering"), ("level", "senior"));
        var updatedClaims = ClaimDtoBuilder.BuildList(("department", "engineering"), ("level", "senior"));

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existingUser);
        _userService.AddUserClaimsAsync(userId, newClaims, Arg.Any<CancellationToken>()).Returns(updatedClaims);

        var command = new AddUserClaimsCommand(userId, newClaims);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Type == "department" && c.Value == "engineering");
        result.Should().Contain(c => c.Type == "level" && c.Value == "senior");
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var claims = ClaimDtoBuilder.BuildList(("department", "engineering"));
        var command = new AddUserClaimsCommand(userId, claims);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().AddUserClaimsAsync(
            Arg.Any<string>(), Arg.Any<IEnumerable<ClaimDto>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SingleClaim_AddsAndReturnsIt()
    {
        var userId = Guid.NewGuid().ToString();
        var existingUser = new UserDtoBuilder().WithId(userId).Build();
        var newClaims = ClaimDtoBuilder.BuildList(("role_override", "manager"));
        var updatedClaims = ClaimDtoBuilder.BuildList(("role_override", "manager"));

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(existingUser);
        _userService.AddUserClaimsAsync(userId, newClaims, Arg.Any<CancellationToken>()).Returns(updatedClaims);

        var command = new AddUserClaimsCommand(userId, newClaims);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().ContainSingle(c => c.Type == "role_override" && c.Value == "manager");
    }
}
