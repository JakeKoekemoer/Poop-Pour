using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersWithCustomPageSizeReturnsCorrectCount : AuthenticatedTestBase
{
    public GetUsersWithCustomPageSizeReturnsCorrectCount(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Theory]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(50)]
    public async Task GetUsers_WithCustomPageSize_ReturnsCorrectCount(int pageSize)
    {
        // Arrange
        using var client = await CreateAdminClientAsync();

        // Act
        var response = await client.GetAsync($"/api/users?page=1&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>();
        result.Should().NotBeNull();
        result!.PageSize.Should().Be(pageSize);
        result.Items.Count().Should().BeLessThanOrEqualTo(pageSize);
    }
}
