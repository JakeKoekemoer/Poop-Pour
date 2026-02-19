using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersAsAdminReturnsOkWithPaginatedUsers : AuthenticatedTestBase
{
    public GetUsersAsAdminReturnsOkWithPaginatedUsers(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_AsAdmin_ReturnsOkWithPaginatedUsers()
    {
        // Arrange - login as admin (has Administrator role -> satisfies CanViewUsers)
        using var client = await CreateAdminClientAsync();

        // Act
        var response = await client.GetAsync("/api/users?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty("the seeder created admin + 2 API client users");
        result.TotalCount.Should().BeGreaterThanOrEqualTo(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
