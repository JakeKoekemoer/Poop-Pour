using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetMyProfileWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public GetMyProfileWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetMyProfile_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange - no token
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
