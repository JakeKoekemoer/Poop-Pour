using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Families.Queries.GetFamilyList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Queries;

public class GetFamilyListQueryHandlerTests
{
    private readonly IFamilyService _familyService;
    private readonly GetFamilyListQueryHandler _sut;

    public GetFamilyListQueryHandlerTests()
    {
        _familyService = Substitute.For<IFamilyService>();
        _sut = new GetFamilyListQueryHandler(_familyService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResultsWithCorrectPageNumberAndSize()
    {
        var families = new[]
        {
            new FamilyDtoBuilder().WithFamilyName("Family 1").Build(),
            new FamilyDtoBuilder().WithFamilyName("Family 2").Build()
        };
        _familyService.GetFamiliesAsync(1, 10, null, Arg.Any<CancellationToken>()).Returns((families, 2));

        var query = new GetFamilyListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithSearchTerm_FiltersResults()
    {
        var families = new[] { new FamilyDtoBuilder().WithFamilyName("Smith").Build() };
        _familyService.GetFamiliesAsync(1, 10, "Smith", Arg.Any<CancellationToken>()).Returns((families, 1));

        var query = new GetFamilyListQuery(1, 10, "Smith");
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().ContainSingle();
        result.TotalCount.Should().Be(1);
        await _familyService.Received().GetFamiliesAsync(1, 10, "Smith", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptySearch_ReturnsAllFamilies()
    {
        var families = new[]
        {
            new FamilyDtoBuilder().WithFamilyName("Smith").Build(),
            new FamilyDtoBuilder().WithFamilyName("Johnson").Build()
        };
        _familyService.GetFamiliesAsync(2, 5, null, Arg.Any<CancellationToken>()).Returns((families, 10));

        var query = new GetFamilyListQuery(2, 5, null);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(10);
    }

    [Fact]
    public async Task Handle_PageBeyondTotal_ReturnsEmptyItems()
    {
        _familyService.GetFamiliesAsync(99, 10, null, Arg.Any<CancellationToken>()).Returns((Array.Empty<FamilyDto>(), 3));

        var query = new GetFamilyListQuery(99, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(99);
    }
}
