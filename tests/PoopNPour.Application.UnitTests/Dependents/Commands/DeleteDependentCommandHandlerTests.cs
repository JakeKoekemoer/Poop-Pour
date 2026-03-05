using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Dependents.Commands.DeleteDependent;
using PoopNPour.Application.Dependents.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class DeleteDependentCommandHandlerTests
{
    private readonly IDependentService _dependentService = Substitute.For<IDependentService>();
    private readonly DeleteDependentCommandHandler _sut;

    public DeleteDependentCommandHandlerTests()
    {
        _sut = new DeleteDependentCommandHandler(_dependentService);
    }

    [Fact]
    public async Task Handle_ExistingDependent_DeletesAndReturnsUnit()
    {
        var dependentId = Guid.NewGuid();
        _dependentService.DeleteDependentAsync(dependentId, Arg.Any<CancellationToken>()).Returns(true);

        var command = new DeleteDependentCommand(dependentId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _dependentService.Received(1).DeleteDependentAsync(dependentId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistentDependent_ThrowsDependentNotFoundException()
    {
        var dependentId = Guid.NewGuid();
        _dependentService.DeleteDependentAsync(dependentId, Arg.Any<CancellationToken>()).Returns(false);

        var command = new DeleteDependentCommand(dependentId);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DependentNotFoundException>();
    }
}
