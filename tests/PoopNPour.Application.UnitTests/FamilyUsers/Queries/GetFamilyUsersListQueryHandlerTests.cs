using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyUsersList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyUsersListQueryHandlerTests
{
    private readonly IFamilyUserService _familyUserService;
    private readonly GetFamilyUsersListQueryHandler _sut;

    public GetFamilyUsersListQueryHandlerTests()
    {
        _familyUserService = Substitute.For<IFamilyUserService>();
        _sut = new GetFamilyUsersListQueryHandler(_familyUserService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResults()
    {
        var familyUsers = new[]
        {
            new FamilyUserDtoBuilder().Build(),
            new FamilyUserDtoBuilder().Build()
        };
        _familyUserService.GetFamilyUsersAsync(1, 10, null, null, Arg.Any<CancellationToken>()).Returns((familyUsers, 2));

        var query = new GetFamilyUsersListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_FiltersByFamilyId()
    {
        var familyId = Guid.NewGuid();
        var familyUsers = new[] { new FamilyUserDtoBuilder().WithFamilyId(familyId).Build() };
        _familyUserService.GetFamilyUsersAsync(1, 10, familyId, null, Arg.Any<CancellationToken>()).Returns((familyUsers, 1));

        var query = new GetFamilyUsersListQuery(1, 10, familyId, null);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().ContainSingle();
        result.TotalCount.Should().Be(1);
        await _familyUserService.Received().GetFamilyUsersAsync(1, 10, familyId, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PageBeyondTotal_ReturnsEmptyItems()
    {
        _familyUserService.GetFamilyUsersAsync(99, 10, null, null, Arg.Any<CancellationToken>()).Returns((Array.Empty<FamilyUserDto>(), 3));

        var query = new GetFamilyUsersListQuery(99, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(99);
    }
}
