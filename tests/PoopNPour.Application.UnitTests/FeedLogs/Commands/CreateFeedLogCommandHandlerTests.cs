using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.FeedLogs.Commands.CreateFeedLog;
using PoopNPour.Application.FeedLogs.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Enums.FeedLog;
using Xunit;

namespace PoopNPour.Application.UnitTests.FeedLogs.Commands;

public class CreateFeedLogCommandHandlerTests
{
    private readonly IFeedLogService _feedLogService;
    private readonly CreateFeedLogCommandHandler _sut;

    public CreateFeedLogCommandHandlerTests()
    {
        _feedLogService = Substitute.For<IFeedLogService>();
        _sut = new CreateFeedLogCommandHandler(_feedLogService);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesFeedLogSuccessfully()
    {
        var dependentId = Guid.NewGuid();
        var created = new FeedLogDtoBuilder().WithDependentId(dependentId).Build();
        _feedLogService.IsFeedLogTimestampDuplicateAsync(dependentId, Arg.Any<DateTimeOffset>(), null, Arg.Any<CancellationToken>()).Returns(false);
        _feedLogService.CreateFeedLogAsync(dependentId, FeedLogType.BREAST_MILK, Arg.Any<DateTimeOffset>(), 120m, null, Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateFeedLogCommand(dependentId, FeedLogType.BREAST_MILK, DateTimeOffset.UtcNow, 120m, null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
    }

    [Fact]
    public async Task Handle_DuplicateTimestamp_ThrowsDuplicateFeedLogTimestampException()
    {
        var dependentId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        _feedLogService.IsFeedLogTimestampDuplicateAsync(dependentId, timestamp, null, Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateFeedLogCommand(dependentId, FeedLogType.BREAST_MILK, timestamp);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateFeedLogTimestampException>();
    }
}
