using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.User;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class AddRoleToUserAsAdminReturnsOk : AuthenticatedTestBase
{
    public AddRoleToUserAsAdminReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task AddRoleToUser_AsAdmin_ReturnsOkWithUpdatedUser()
    {
        using var adminClient = await CreateAdminClientAsync();
        var webApiClient = CreateWebApiClient();

        // Register a fresh user
        var registerRequest = new RegisterRequestDto
        {
            Email = $"role-test-{Guid.NewGuid():N}@example.com",
            UserName = $"roleuser-{Guid.NewGuid():N}",
            Password = "Password123!",
            FirstName = "Role",
            LastName = "Tester"
        };
        var registerResponse = await webApiClient.PostAsJsonAsync("/api/authentication/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();
        var registered = await registerResponse.Content.ReadFromJsonAsync<RegisterResponseDto>();
        var userId = registered!.User.Id;

        // Add Family_Head role to the new user
        var command = new AddUserRoleCommand(userId, Roles.Family_Head);
        var response = await adminClient.PostAsJsonAsync($"/api/users/{userId}/roles", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Roles.Should().Contain(Roles.Family_Head);
    }
}
