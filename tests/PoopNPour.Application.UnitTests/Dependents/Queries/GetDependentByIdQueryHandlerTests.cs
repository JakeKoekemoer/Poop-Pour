using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Application.Dependents.Queries.GetDependentById;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Queries;

public class GetDependentByIdQueryHandlerTests
{
    private readonly IDependentService _dependentService;
    private readonly GetDependentByIdQueryHandler _sut;

    public GetDependentByIdQueryHandlerTests()
    {
        _dependentService = Substitute.For<IDependentService>();
        _sut = new GetDependentByIdQueryHandler(_dependentService);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsDependent()
    {
        var dependentId = Guid.NewGuid();
        var dependent = new DependentDtoBuilder().WithDependentId(dependentId).Build();
        _dependentService.GetDependentByIdAsync(dependentId, Arg.Any<CancellationToken>()).Returns(dependent);

        var query = new GetDependentByIdQuery(dependentId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeSameAs(dependent);
    }

    [Fact]
    public async Task Handle_DependentNotFound_ThrowsDependentNotFoundException()
    {
        var dependentId = Guid.NewGuid();
        _dependentService.GetDependentByIdAsync(dependentId, Arg.Any<CancellationToken>()).Returns((DependentDto?)null);

        var query = new GetDependentByIdQuery(dependentId);
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<DependentNotFoundException>();
    }
}
