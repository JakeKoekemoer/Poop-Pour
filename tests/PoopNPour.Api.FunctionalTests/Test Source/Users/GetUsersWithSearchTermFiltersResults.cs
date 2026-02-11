using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersWithSearchTermFiltersResults : AuthenticatedTestBase
{
    public GetUsersWithSearchTermFiltersResults(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_WithSearchTerm_FiltersResults()
    {
        // Arrange
        using var client = await CreateAdminClientAsync();

        // Act - search for admin user
        var response = await client.GetAsync($"/api/users?page=1&pageSize=10&searchTerm={TestUsers.Admin.UserName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().Contain(u => u.UserName.Contains(TestUsers.Admin.UserName, StringComparison.OrdinalIgnoreCase) ||
                                           u.Email.Contains(TestUsers.Admin.UserName, StringComparison.OrdinalIgnoreCase));
    }
}
