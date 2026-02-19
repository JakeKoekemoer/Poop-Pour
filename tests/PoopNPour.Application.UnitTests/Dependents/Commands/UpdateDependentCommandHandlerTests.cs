using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Dependents.Commands.UpdateDependent;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class UpdateDependentCommandHandlerTests
{
    private readonly IDependentService _dependentService;
    private readonly UpdateDependentCommandHandler _sut;

    public UpdateDependentCommandHandlerTests()
    {
        _dependentService = Substitute.For<IDependentService>();
        _sut = new UpdateDependentCommandHandler(_dependentService);
    }

    [Fact]
    public async Task Handle_UpdatesDependentNameOnly_ReturnsUpdatedDependent()
    {
        var dependentId = Guid.NewGuid();
        var current = new DependentDtoBuilder().WithDependentId(dependentId).Build();
        var updated = new DependentDtoBuilder().WithDependentId(dependentId).WithDependentName("Updated").Build();
        _dependentService.GetDependentByIdAsync(dependentId, Arg.Any<CancellationToken>()).Returns(current);
        _dependentService.IsDependentNameDuplicateAsync(Arg.Any<Guid>(), "Updated", Arg.Any<string>(), dependentId, Arg.Any<CancellationToken>()).Returns(false);
        _dependentService.UpdateDependentAsync(dependentId, "Updated", null, null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateDependentCommand(dependentId, "Updated", null, null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.DependentName.Should().Be("Updated");
    }

    [Fact]
    public async Task Handle_DependentNotFound_ThrowsDependentNotFoundException()
    {
        var dependentId = Guid.NewGuid();
        _dependentService.UpdateDependentAsync(dependentId, "Name", null, null, Arg.Any<CancellationToken>()).Returns((DependentDto?)null);

        var command = new UpdateDependentCommand(dependentId, "Name", null, null);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DependentNotFoundException>();
    }
}
