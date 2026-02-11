using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class LoginWithInvalidPasswordReturnsUnauthorized : AuthenticatedTestBase
{
    public LoginWithInvalidPasswordReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = TestUsers.Admin.UserName,
                Password = "CompletelyWrong123!"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
