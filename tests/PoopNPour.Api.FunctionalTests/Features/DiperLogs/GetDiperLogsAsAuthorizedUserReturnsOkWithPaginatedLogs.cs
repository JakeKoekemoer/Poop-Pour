using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.DiperLogs;

public class GetDiperLogsAsAuthorizedUserReturnsOkWithPaginatedLogs : AuthenticatedTestBase
{
    public GetDiperLogsAsAuthorizedUserReturnsOkWithPaginatedLogs(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetDiperLogs_AsAuthorizedUser_ReturnsOkWithPaginatedLogs()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/diper-logs?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<DiperLogDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
    }
}
