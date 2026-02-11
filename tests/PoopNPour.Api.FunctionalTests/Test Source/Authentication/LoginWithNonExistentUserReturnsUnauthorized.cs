using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class LoginWithNonExistentUserReturnsUnauthorized : AuthenticatedTestBase
{
    public LoginWithNonExistentUserReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = "nonexistent_user",
                Password = "SomePassword@123"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
