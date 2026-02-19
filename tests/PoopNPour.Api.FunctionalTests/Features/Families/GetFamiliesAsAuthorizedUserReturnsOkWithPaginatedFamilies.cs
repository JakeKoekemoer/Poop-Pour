using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class GetFamiliesAsAuthorizedUserReturnsOkWithPaginatedFamilies : AuthenticatedTestBase
{
    public GetFamiliesAsAuthorizedUserReturnsOkWithPaginatedFamilies(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFamilies_AsAuthorizedUser_ReturnsOkWithPaginatedFamilies()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/families?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<FamilyDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
