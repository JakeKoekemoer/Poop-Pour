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

public class RemoveRoleFromUserAsAdminReturnsOk : AuthenticatedTestBase
{
    public RemoveRoleFromUserAsAdminReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RemoveRoleFromUser_AsAdmin_ReturnsOkWithRoleAbsent()
    {
        using var adminClient = await CreateAdminClientAsync();
        var webApiClient = CreateWebApiClient();

        // Register a fresh user
        var registerRequest = new RegisterRequestDto
        {
            Email = $"remove-role-{Guid.NewGuid():N}@example.com",
            UserName = $"removeroleuser-{Guid.NewGuid():N}",
            Password = "Password123!",
            FirstName = "Remove",
            LastName = "Role"
        };
        var registerResponse = await webApiClient.PostAsJsonAsync("/api/authentication/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();
        var registered = await registerResponse.Content.ReadFromJsonAsync<RegisterResponseDto>();
        var userId = registered!.User.Id;

        // First add the Tenant role
        var addCommand = new AddUserRoleCommand(userId, Roles.Tenant);
        var addResponse = await adminClient.PostAsJsonAsync($"/api/users/{userId}/roles", addCommand);
        addResponse.EnsureSuccessStatusCode();

        // Now remove it
        var removeResponse = await adminClient.DeleteAsync($"/api/users/{userId}/roles/{Roles.Tenant}");

        removeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await removeResponse.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Roles.Should().NotContain(Roles.Tenant);
    }
}
