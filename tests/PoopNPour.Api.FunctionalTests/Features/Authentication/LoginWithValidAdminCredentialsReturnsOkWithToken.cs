using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class LoginWithValidAdminCredentialsReturnsOkWithToken : AuthenticatedTestBase
{
    public LoginWithValidAdminCredentialsReturnsOkWithToken(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithValidAdminCredentials_ReturnsOkWithToken()
    {
        // Arrange
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = TestUsers.Admin.UserName,
                Password = TestUsers.Admin.Password
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        result.User.UserName.Should().Be(TestUsers.Admin.UserName);
        result.User.Email.Should().Be(TestUsers.Admin.Email);
        result.User.Roles.Should().Contain("Administrator");
    }
}
