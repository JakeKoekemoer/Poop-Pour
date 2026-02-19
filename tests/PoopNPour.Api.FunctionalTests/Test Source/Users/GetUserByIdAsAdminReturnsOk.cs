using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUserByIdAsAdminReturnsOk : AuthenticatedTestBase
{
    public GetUserByIdAsAdminReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUserById_AsAdmin_ReturnsOkWithUserDto()
    {
        // Arrange - login as admin and get a user ID from the list
        using var client = await CreateAdminClientAsync();
        var listResponse = await client.GetAsync("/api/users?page=1&pageSize=1");
        listResponse.EnsureSuccessStatusCode();
        var listResult = await listResponse.Content.ReadFromJsonAsync<PoopNPour.Application.Common.Models.PaginatedResponseDto<UserDto>>();
        var userId = listResult!.Items.First().Id;

        // Act
        var response = await client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
    }
}
