using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.DiperLogs.Queries.GetDiperLogList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.DiperLogs.Queries;

public class GetDiperLogListQueryHandlerTests
{
    private readonly IDiperLogService _diperLogService;
    private readonly GetDiperLogListQueryHandler _sut;

    public GetDiperLogListQueryHandlerTests()
    {
        _diperLogService = Substitute.For<IDiperLogService>();
        _sut = new GetDiperLogListQueryHandler(_diperLogService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResults()
    {
        var logs = new[] { new DiperLogDtoBuilder().Build(), new DiperLogDtoBuilder().Build() };
        _diperLogService.GetDiperLogsAsync(1, 10, null, null, null, null, Arg.Any<CancellationToken>()).Returns((logs, 2));

        var query = new GetDiperLogListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
    }
}
