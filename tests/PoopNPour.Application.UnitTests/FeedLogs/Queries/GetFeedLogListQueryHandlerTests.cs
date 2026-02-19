using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.FeedLogs.Queries.GetFeedLogList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FeedLogs.Queries;

public class GetFeedLogListQueryHandlerTests
{
    private readonly IFeedLogService _feedLogService;
    private readonly GetFeedLogListQueryHandler _sut;

    public GetFeedLogListQueryHandlerTests()
    {
        _feedLogService = Substitute.For<IFeedLogService>();
        _sut = new GetFeedLogListQueryHandler(_feedLogService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResults()
    {
        var logs = new[] { new FeedLogDtoBuilder().Build(), new FeedLogDtoBuilder().Build() };
        _feedLogService.GetFeedLogsAsync(1, 10, null, null, null, Arg.Any<CancellationToken>()).Returns((logs, 2));

        var query = new GetFeedLogListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
    }
}
