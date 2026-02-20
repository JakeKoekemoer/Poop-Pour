using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class RemoveRoleFromUserWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public RemoveRoleFromUserWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RemoveRoleFromUser_WithoutAuth_ReturnsUnauthorized()
    {
        var client = Factory.CreateClient();

        var bogusId = Guid.NewGuid().ToString();
        var response = await client.DeleteAsync($"/api/users/{bogusId}/roles/{Roles.Family_Head}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
