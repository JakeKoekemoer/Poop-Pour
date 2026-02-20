using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Queries;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Queries;

public class GetUserClaimsQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly GetUserClaimsQueryHandler _sut;

    public GetUserClaimsQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new GetUserClaimsQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidUserId_ReturnsClaimsList()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new UserDtoBuilder().WithId(userId).Build();
        var claims = ClaimDtoBuilder.BuildList(("department", "engineering"), ("level", "senior"));

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _userService.GetUserClaimsAsync(userId, Arg.Any<CancellationToken>()).Returns(claims);

        var query = new GetUserClaimsQuery(userId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Type == "department" && c.Value == "engineering");
        result.Should().Contain(c => c.Type == "level" && c.Value == "senior");
    }

    [Fact]
    public async Task Handle_UserWithNoClaims_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new UserDtoBuilder().WithId(userId).Build();

        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _userService.GetUserClaimsAsync(userId, Arg.Any<CancellationToken>()).Returns(new List<ClaimDto>());

        var query = new GetUserClaimsQuery(userId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var query = new GetUserClaimsQuery(userId);
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
        await _userService.DidNotReceive().GetUserClaimsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
