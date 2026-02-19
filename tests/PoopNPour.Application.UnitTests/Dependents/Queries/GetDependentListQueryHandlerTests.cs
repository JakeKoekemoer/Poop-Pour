using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Dependents.Queries.GetDependentList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Queries;

public class GetDependentListQueryHandlerTests
{
    private readonly IDependentService _dependentService;
    private readonly GetDependentListQueryHandler _sut;

    public GetDependentListQueryHandlerTests()
    {
        _dependentService = Substitute.For<IDependentService>();
        _sut = new GetDependentListQueryHandler(_dependentService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResults()
    {
        var dependents = new[]
        {
            new DependentDtoBuilder().Build(),
            new DependentDtoBuilder().Build()
        };
        _dependentService.GetDependentsAsync(1, 10, null, null, Arg.Any<CancellationToken>()).Returns((dependents, 2));

        var query = new GetDependentListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.TotalCount.Should().Be(2);
    }
}
