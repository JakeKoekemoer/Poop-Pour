using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class AddRoleToUserWithInvalidRoleReturnsBadRequest : AuthenticatedTestBase
{
    public AddRoleToUserWithInvalidRoleReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task AddRoleToUser_WithInvalidRole_ReturnsBadRequest()
    {
        using var adminClient = await CreateAdminClientAsync();
        var webApiClient = CreateWebApiClient();

        // Register a fresh user
        var registerRequest = new RegisterRequestDto
        {
            Email = $"invalid-role-{Guid.NewGuid():N}@example.com",
            UserName = $"invalidroleuser-{Guid.NewGuid():N}",
            Password = "Password123!",
            FirstName = "Invalid",
            LastName = "Role"
        };
        var registerResponse = await webApiClient.PostAsJsonAsync("/api/authentication/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();
        var registered = await registerResponse.Content.ReadFromJsonAsync<RegisterResponseDto>();
        var userId = registered!.User.Id;

        // Attempt to add an unknown role
        var command = new AddUserRoleCommand(userId, "HackerRole");
        var response = await adminClient.PostAsJsonAsync($"/api/users/{userId}/roles", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
