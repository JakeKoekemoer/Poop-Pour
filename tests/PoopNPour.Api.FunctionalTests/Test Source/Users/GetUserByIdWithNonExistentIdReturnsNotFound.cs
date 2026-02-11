using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetUserByIdWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public GetUserByIdWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetUserById_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid().ToString();

        // Act
        var response = await client.GetAsync($"/api/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
