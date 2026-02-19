using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Application.Families.Queries.GetFamilyById;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Queries;

public class GetFamilyByIdQueryHandlerTests
{
    private readonly IFamilyService _familyService;
    private readonly GetFamilyByIdQueryHandler _sut;

    public GetFamilyByIdQueryHandlerTests()
    {
        _familyService = Substitute.For<IFamilyService>();
        _sut = new GetFamilyByIdQueryHandler(_familyService);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsFamily()
    {
        var familyId = Guid.NewGuid();
        var family = new FamilyDtoBuilder()
            .WithFamilyId(familyId)
            .WithFamilyName("Smith Family")
            .Build();
        _familyService.GetFamilyByIdAsync(familyId, Arg.Any<CancellationToken>()).Returns(family);

        var query = new GetFamilyByIdQuery(familyId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeSameAs(family);
        result.FamilyId.Should().Be(familyId);
    }

    [Fact]
    public async Task Handle_FamilyNotFound_ThrowsFamilyNotFoundException()
    {
        var familyId = Guid.NewGuid();
        _familyService.GetFamilyByIdAsync(familyId, Arg.Any<CancellationToken>()).Returns((FamilyDto?)null);

        var query = new GetFamilyByIdQuery(familyId);
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<FamilyNotFoundException>();
    }
}
