using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUsersWithWebApiTokenReturnsForbidden : AuthenticatedTestBase
{
    public GetUsersWithWebApiTokenReturnsForbidden(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_WithWebApiToken_ReturnsForbidden()
    {
        // Arrange - Web_Api role does NOT satisfy CanViewUsers (requires Administrator)
        using var client = CreateWebApiClient();

        // Act
        var response = await client.GetAsync("/api/users?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden,
            "Web_Api role should not satisfy the CanViewUsers policy (requires Administrator)");
    }
}
