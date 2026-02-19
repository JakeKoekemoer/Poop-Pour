using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.DiperLogs.Commands.CreateDiperLog;
using PoopNPour.Application.DiperLogs.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Enums.DiperLog;
using Xunit;

namespace PoopNPour.Application.UnitTests.DiperLogs.Commands;

public class CreateDiperLogCommandHandlerTests
{
    private readonly IDiperLogService _diperLogService;
    private readonly CreateDiperLogCommandHandler _sut;

    public CreateDiperLogCommandHandlerTests()
    {
        _diperLogService = Substitute.For<IDiperLogService>();
        _sut = new CreateDiperLogCommandHandler(_diperLogService);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesDiperLogSuccessfully()
    {
        var dependentId = Guid.NewGuid();
        var created = new DiperLogDtoBuilder().WithDependentId(dependentId).Build();
        _diperLogService.IsDiperLogTimestampDuplicateAsync(dependentId, Arg.Any<DateTimeOffset>(), null, Arg.Any<CancellationToken>()).Returns(false);
        _diperLogService.CreateDiperLogAsync(dependentId, Arg.Any<DateTimeOffset>(), FecalDischargeColour.BROWN, UrinalDischargeColour.YELLOW, null, Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateDiperLogCommand(dependentId, DateTimeOffset.UtcNow, FecalDischargeColour.BROWN, UrinalDischargeColour.YELLOW, null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
    }

    [Fact]
    public async Task Handle_DuplicateTimestamp_ThrowsDuplicateDiperLogTimestampException()
    {
        var dependentId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        _diperLogService.IsDiperLogTimestampDuplicateAsync(dependentId, timestamp, null, Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateDiperLogCommand(dependentId, timestamp, FecalDischargeColour.BROWN, UrinalDischargeColour.YELLOW);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateDiperLogTimestampException>();
    }
}
