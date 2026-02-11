using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Middleware;

public class ExceptionHandlingMiddlewareTests : AuthenticatedTestBase
{
    public ExceptionHandlingMiddlewareTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UnauthorizedAccessException_Returns401()
    {
        // Arrange - unauthenticated request to protected endpoint
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(401);
        error.Message.Should().Be("Unauthorized");
    }

    [Fact]
    public async Task ForbiddenAccessException_Returns403()
    {
        // Arrange - Web API token doesn't have CanViewUsers policy
        var client = CreateWebApiClient();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(403);
        error.Message.Should().Be("Forbidden");
    }

    [Fact]
    public async Task UserNotFoundException_Returns404()
    {
        // Arrange
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid().ToString();

        // Act
        var response = await client.GetAsync($"/api/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(404);
        error.Message.Should().Be("Not Found");
    }

    private new class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }
}
