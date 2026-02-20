using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class RemoveRoleFromUserWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public RemoveRoleFromUserWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RemoveRoleFromUser_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();

        var bogusId = Guid.NewGuid().ToString();
        var response = await client.DeleteAsync($"/api/users/{bogusId}/roles/{Roles.Family_Head}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
