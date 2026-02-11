using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public GetUsersWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange - no token at all
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "unauthenticated requests should be rejected by AuthorizationBehavior");
    }
}
