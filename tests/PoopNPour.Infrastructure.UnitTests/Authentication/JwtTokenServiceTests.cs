using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Infrastructure.Authentication;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Authentication;

public class JwtTokenServiceTests
{
    private IConfiguration CreateConfiguration(Dictionary<string, string?>? jwtSettings = null)
    {
        var defaultSettings = new Dictionary<string, string?>
        {
            ["Jwt:SecretKey"] = "TestSecretKey_MustBeAtLeast32CharactersLong",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:ExpirationInMinutes"] = "60",
            ["Jwt:ApiTokenExpirationInDays"] = "3650"
        };

        if (jwtSettings != null)
        {
            foreach (var setting in jwtSettings)
            {
                if (setting.Value == null)
                {
                    defaultSettings.Remove($"Jwt:{setting.Key}");
                }
                else
                {
                    defaultSettings[$"Jwt:{setting.Key}"] = setting.Value;
                }
            }
        }

        return new ConfigurationBuilder()
            .AddInMemoryCollection(defaultSettings)
            .Build();
    }

    private JwtTokenService CreateSut(Dictionary<string, string?>? jwtSettings = null)
    {
        var configuration = CreateConfiguration(jwtSettings);
        return new JwtTokenService(configuration);
    }

    [Fact]
    public async Task GenerateTokenAsync_CreatesValidJwtWithCorrectClaims()
    {
        var sut = CreateSut();
        var userId = "user-123";
        var email = "test@example.com";
        var userName = "testuser";
        var roles = new[] { "Admin", "User" };

        var (token, expiresAt) = await sut.GenerateTokenAsync(userId, email, userName, roles);

        token.Should().NotBeNullOrEmpty();
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Should().NotBeNull();
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == email);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == userName);
    }

    [Fact]
    public async Task GenerateTokenAsync_TokenContainsRoleClaims()
    {
        var sut = CreateSut();
        var roles = new[] { "Admin", "Manager" };

        var (token, _) = await sut.GenerateTokenAsync("user-1", "test@test.com", "user", roles);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Claims.Where(c => c.Type == ClaimTypes.Role).Should().HaveCount(2);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Manager");
    }

    [Fact]
    public async Task GenerateTokenAsync_TokenExpirationIsSetCorrectly()
    {
        var expirationMinutes = 120;
        var sut = CreateSut(new Dictionary<string, string?> { ["ExpirationInMinutes"] = expirationMinutes.ToString() });

        var (token, expiresAt) = await sut.GenerateTokenAsync("user-1", "test@test.com", "user", Array.Empty<string>());

        expiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(expirationMinutes), TimeSpan.FromMinutes(1));
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(expirationMinutes), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task GenerateTokenAsync_MissingSecretKey_ThrowsInvalidOperationException()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();
        var sut = new JwtTokenService(config);

        var act = () => sut.GenerateTokenAsync("user-1", "test@test.com", "user", Array.Empty<string>());

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*SecretKey*");
    }

    [Fact]
    public async Task GenerateTokenAsync_MissingIssuer_ThrowsInvalidOperationException()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "TestSecretKey_MustBeAtLeast32CharactersLong",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();
        var sut = new JwtTokenService(config);

        var act = () => sut.GenerateTokenAsync("user-1", "test@test.com", "user", Array.Empty<string>());

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*Issuer*");
    }

    [Fact]
    public async Task GenerateTokenAsync_MissingAudience_ThrowsInvalidOperationException()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "TestSecretKey_MustBeAtLeast32CharactersLong",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();
        var sut = new JwtTokenService(config);

        var act = () => sut.GenerateTokenAsync("user-1", "test@test.com", "user", Array.Empty<string>());

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*Audience*");
    }

    [Fact]
    public void GenerateApiToken_CreatesValidToken()
    {
        var sut = CreateSut();
        var userId = "user-123";
        var roles = new[] { Roles.Web_Api };

        var token = sut.GenerateApiToken(userId, roles);

        token.Should().NotBeNullOrEmpty();
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
        jsonToken.Claims.Should().Contain(c => c.Type == "token_type" && c.Value == "api_token");
    }

    [Fact]
    public void GenerateApiToken_TokenHasLongExpiration()
    {
        var expirationDays = 3650;
        var sut = CreateSut(new Dictionary<string, string?> { ["ApiTokenExpirationInDays"] = expirationDays.ToString() });

        var token = sut.GenerateApiToken("user-1", Array.Empty<string>());

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddDays(expirationDays), TimeSpan.FromDays(1));
    }
}
