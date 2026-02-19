using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersWithPageBeyondTotalReturnsEmptyResults : AuthenticatedTestBase
{
    public GetUsersWithPageBeyondTotalReturnsEmptyResults(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_WithPageBeyondTotal_ReturnsEmptyResults()
    {
        // Arrange
        using var client = await CreateAdminClientAsync();
        var firstPageResponse = await client.GetAsync("/api/users?page=1&pageSize=10");
        firstPageResponse.EnsureSuccessStatusCode();
        var firstPage = await firstPageResponse.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>();
        var totalPages = firstPage!.TotalPages;

        // Act - request page beyond total
        var response = await client.GetAsync($"/api/users?page={totalPages + 10}&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().BeEmpty();
    }
}
