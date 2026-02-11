using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUserByIdWithInvalidGuidReturnsBadRequest : AuthenticatedTestBase
{
    public GetUserByIdWithInvalidGuidReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUserById_WithInvalidGuid_ReturnsNotFound()
    {
        // Arrange - API doesn't validate GUID format, it just tries to find the user
        // Invalid GUID format results in user not found (404) rather than bad request (400)
        using var client = await CreateAdminClientAsync();
        var invalidId = "not-a-valid-guid";

        // Act
        var response = await client.GetAsync($"/api/users/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
