using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Infrastructure;

/// <summary>
/// Base class for functional tests that need authenticated HTTP clients.
/// Uses the seeded long-lived API tokens to authenticate via the real login endpoint.
/// All authentication goes through the full production pipeline.
/// </summary>
public abstract class AuthenticatedTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory;

    /// <summary>
    /// Cache of user tokens so we don't re-login for every test method.
    /// Key = username, Value = JWT token.
    /// </summary>
    private readonly Dictionary<string, string> _tokenCache = new();

    protected AuthenticatedTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
    }

    /// <summary>
    /// Logs in via POST /api/authentication/login using the Web API long-lived token,
    /// then returns the user's JWT token from the response.
    /// Tokens are cached per username so login only happens once per user per test class.
    /// </summary>
    protected async Task<string> LoginAsAsync(string username, string password)
    {
        if (_tokenCache.TryGetValue(username, out var cached))
            return cached;

        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Factory.WebApiToken);

        var response = await client.PostAsJsonAsync("/api/authentication/login",
            new LoginRequestDto
            {
                Username = username,
                Password = password
            });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        var token = result!.Token;

        _tokenCache[username] = token;
        return token;
    }

    /// <summary>
    /// Creates an HttpClient authenticated as the specified user.
    /// Goes through the full login pipeline to obtain a real JWT token.
    /// </summary>
    protected async Task<HttpClient> CreateAuthenticatedClientAsync(string username, string password)
    {
        var token = await LoginAsAsync(username, password);
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    // ---- Convenience methods for common test personas ----

    /// <summary>
    /// Creates an HttpClient authenticated as the Admin user.
    /// </summary>
    protected Task<HttpClient> CreateAdminClientAsync()
        => CreateAuthenticatedClientAsync(TestUsers.Admin.UserName, TestUsers.Admin.Password);

    /// <summary>
    /// Creates an HttpClient with the Web API long-lived token.
    /// Use for testing login/register endpoints themselves.
    /// </summary>
    protected HttpClient CreateWebApiClient()
    {
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Factory.WebApiToken);
        return client;
    }

    /// <summary>
    /// Creates an HttpClient with the Mobile API long-lived token.
    /// </summary>
    protected HttpClient CreateMobileApiClient()
    {
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Factory.MobileApiToken);
        return client;
    }

    /// <summary>
    /// Verifies that an HTTP response contains a standard error response format.
    /// </summary>
    protected async Task<ErrorResponse> VerifyErrorResponseAsync(HttpResponseMessage response, int expectedStatusCode)
    {
        response.StatusCode.Should().Be((System.Net.HttpStatusCode)expectedStatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(expectedStatusCode);
        return error!;
    }

    /// <summary>
    /// Standard error response format for HTTP responses.
    /// </summary>
    protected class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
