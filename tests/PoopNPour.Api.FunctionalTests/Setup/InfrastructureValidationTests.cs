using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Setup;

/// <summary>
/// These tests verify the test infrastructure itself:
/// - Database was seeded correctly by the real DatabaseSeeder
/// - Admin, Web API, and Mobile API users exist
/// - Long-lived API tokens were generated and stored
/// - Those tokens are valid and can be used to authenticate via the login endpoint
///
/// These should be the FIRST tests that run.
/// If these fail, all other functional tests are meaningless.
/// </summary>
[TestCaseOrderer(
    "PoopNPour.Api.FunctionalTests.Infrastructure.PriorityOrderer",
    "PoopNPour.Api.FunctionalTests")]
public class InfrastructureValidationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public InfrastructureValidationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(0)]
    public void WebApiToken_WasRetrievedFromSeededData()
    {
        _factory.WebApiToken.Should().NotBeNullOrEmpty(
            "the DatabaseSeeder should have created the Web_Api client and stored its long-lived API token");
    }

    [Fact, TestPriority(1)]
    public void MobileApiToken_WasRetrievedFromSeededData()
    {
        _factory.MobileApiToken.Should().NotBeNullOrEmpty(
            "the DatabaseSeeder should have created the Mobile_Api client and stored its long-lived API token");
    }

    [Fact, TestPriority(2)]
    public async Task WebApiToken_CanAuthenticateAdminUser()
    {
        // Arrange - use the Web API long-lived token to call the login endpoint
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _factory.WebApiToken);

        // Act - login as admin using the real login endpoint
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
        result!.Token.Should().NotBeNullOrEmpty("login should return a valid JWT token");
        result.User.Should().NotBeNull();
        result.User.UserName.Should().Be(TestUsers.Admin.UserName);
        result.User.Roles.Should().Contain(Roles.Administrator);
    }

    [Fact, TestPriority(3)]
    public async Task MobileApiToken_CanAuthenticateAdminUser()
    {
        // Arrange - use the Mobile API long-lived token
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _factory.MobileApiToken);

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
        result!.Token.Should().NotBeNullOrEmpty("login should return a valid JWT token");
    }

    [Fact, TestPriority(4)]
    public async Task LoginWithoutApiToken_ReturnsUnauthorized()
    {
        // No bearer token at all - should fail at AuthorizationBehavior
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = TestUsers.Admin.UserName,
                Password = TestUsers.Admin.Password
            });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "calling login without an API client token should be rejected by the AuthorizationBehavior");
    }

    [Fact, TestPriority(5)]
    public async Task LoginWithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange - valid API token but wrong password
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _factory.WebApiToken);

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = TestUsers.Admin.UserName,
                Password = "WrongPassword123!"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "login with invalid credentials should return 401 from InvalidCredentialsException");
    }
}
