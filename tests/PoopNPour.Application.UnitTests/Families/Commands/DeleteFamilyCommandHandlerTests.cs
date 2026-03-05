using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Families.Commands.DeleteFamily;
using PoopNPour.Application.Families.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class DeleteFamilyCommandHandlerTests
{
    private readonly IFamilyService _familyService = Substitute.For<IFamilyService>();
    private readonly DeleteFamilyCommandHandler _sut;

    public DeleteFamilyCommandHandlerTests()
    {
        _sut = new DeleteFamilyCommandHandler(_familyService);
    }

    [Fact]
    public async Task Handle_ExistingFamily_DeletesAndReturnsUnit()
    {
        var familyId = Guid.NewGuid();
        _familyService.DeleteFamilyAsync(familyId, Arg.Any<CancellationToken>()).Returns(true);

        var command = new DeleteFamilyCommand(familyId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _familyService.Received(1).DeleteFamilyAsync(familyId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistentFamily_ThrowsFamilyNotFoundException()
    {
        var familyId = Guid.NewGuid();
        _familyService.DeleteFamilyAsync(familyId, Arg.Any<CancellationToken>()).Returns(false);

        var command = new DeleteFamilyCommand(familyId);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FamilyNotFoundException>();
    }
}
