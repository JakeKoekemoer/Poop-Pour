using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Queries;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly GetUserByIdQueryHandler _sut;

    public GetUserByIdQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new GetUserByIdQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidUserId_ReturnsUserDto()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new UserDtoBuilder().WithId(userId).WithUserName("testuser").Build();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);

        var query = new GetUserByIdQuery(userId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeSameAs(user);
    }

    [Fact]
    public async Task Handle_NonExistentUser_ThrowsUserNotFoundException()
    {
        var userId = Guid.NewGuid().ToString();
        _userService.GetUserByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var query = new GetUserByIdQuery(userId);
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("*" + userId + "*");
    }
}
