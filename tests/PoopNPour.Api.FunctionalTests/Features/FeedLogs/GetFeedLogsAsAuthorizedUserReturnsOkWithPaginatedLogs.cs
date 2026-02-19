using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.FeedLogs;

public class GetFeedLogsAsAuthorizedUserReturnsOkWithPaginatedLogs : AuthenticatedTestBase
{
    public GetFeedLogsAsAuthorizedUserReturnsOkWithPaginatedLogs(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFeedLogs_AsAuthorizedUser_ReturnsOkWithPaginatedLogs()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/feed-logs?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<FeedLogDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
    }
}
