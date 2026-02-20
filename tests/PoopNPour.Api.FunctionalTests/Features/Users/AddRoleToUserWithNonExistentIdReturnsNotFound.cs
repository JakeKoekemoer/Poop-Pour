using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class AddRoleToUserWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public AddRoleToUserWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task AddRoleToUser_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();

        var bogusId = Guid.NewGuid().ToString();
        var command = new AddUserRoleCommand(bogusId, Roles.Family_Head);
        var response = await client.PostAsJsonAsync($"/api/users/{bogusId}/roles", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
