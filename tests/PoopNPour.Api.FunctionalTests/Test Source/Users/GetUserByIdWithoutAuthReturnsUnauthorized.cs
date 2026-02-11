using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUserByIdWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public GetUserByIdWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUserById_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var client = Factory.CreateClient();
        var userId = Guid.NewGuid().ToString();

        // Act
        var response = await client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
