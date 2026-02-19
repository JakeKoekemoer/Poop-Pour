using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.MedicineLogs;

public class GetMedicineLogsAsAuthorizedUserReturnsOkWithPaginatedLogs : AuthenticatedTestBase
{
    public GetMedicineLogsAsAuthorizedUserReturnsOkWithPaginatedLogs(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetMedicineLogs_AsAuthorizedUser_ReturnsOkWithPaginatedLogs()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/medicine-logs?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<MedicineLogDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
    }
}
