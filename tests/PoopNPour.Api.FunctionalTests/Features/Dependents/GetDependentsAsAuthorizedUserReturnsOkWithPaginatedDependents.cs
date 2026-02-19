using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Dependents;

public class GetDependentsAsAuthorizedUserReturnsOkWithPaginatedDependents : AuthenticatedTestBase
{
    public GetDependentsAsAuthorizedUserReturnsOkWithPaginatedDependents(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetDependents_AsAuthorizedUser_ReturnsOkWithPaginatedDependents()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/dependents?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<DependentDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
