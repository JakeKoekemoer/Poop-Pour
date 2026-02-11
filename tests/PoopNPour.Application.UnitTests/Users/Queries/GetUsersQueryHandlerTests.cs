using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Queries;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Queries;

public class GetUsersQueryHandlerTests
{
    private readonly IUserService _userService;
    private readonly GetUsersQueryHandler _sut;

    public GetUsersQueryHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _sut = new GetUsersQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResultsWithCorrectPageNumberAndSize()
    {
        var users = new[]
        {
            new UserDtoBuilder().WithUserName("u1").Build(),
            new UserDtoBuilder().WithUserName("u2").Build()
        };
        _userService.GetUsersAsync(1, 10, null, Arg.Any<CancellationToken>()).Returns((users, 2));

        var query = new GetUsersQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithSearchTerm_FiltersResults()
    {
        var users = new[] { new UserDtoBuilder().WithUserName("admin").Build() };
        _userService.GetUsersAsync(1, 10, "admin", Arg.Any<CancellationToken>()).Returns((users, 1));

        var query = new GetUsersQuery(1, 10, "admin");
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().ContainSingle();
        result.TotalCount.Should().Be(1);
        await _userService.Received().GetUsersAsync(1, 10, "admin", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptySearch_ReturnsAllUsers()
    {
        var users = new[]
        {
            new UserDtoBuilder().WithUserName("a").Build(),
            new UserDtoBuilder().WithUserName("b").Build()
        };
        _userService.GetUsersAsync(2, 5, null, Arg.Any<CancellationToken>()).Returns((users, 10));

        var query = new GetUsersQuery(2, 5, null);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(10);
    }

    [Fact]
    public async Task Handle_PageBeyondTotal_ReturnsEmptyItems()
    {
        _userService.GetUsersAsync(99, 10, null, Arg.Any<CancellationToken>()).Returns((Array.Empty<UserDto>(), 3));

        var query = new GetUsersQuery(99, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(99);
    }
}
