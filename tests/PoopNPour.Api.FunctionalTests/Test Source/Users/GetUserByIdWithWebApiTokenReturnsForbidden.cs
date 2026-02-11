using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUserByIdWithWebApiTokenReturnsForbidden : AuthenticatedTestBase
{
    public GetUserByIdWithWebApiTokenReturnsForbidden(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUserById_WithWebApiToken_ReturnsForbidden()
    {
        // Arrange - Web API token doesn't have CanViewUsers policy
        var client = CreateWebApiClient();
        var userId = Guid.NewGuid().ToString();

        // Act
        var response = await client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
