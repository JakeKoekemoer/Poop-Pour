using FluentAssertions;
using Microsoft.Extensions.Configuration;
using PoopNPour.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Xunit;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;

namespace PoopNPour.Infrastructure.UnitTests.Authentication;

public class JwtTokenServiceGenerateTokenWithNoFamilyMembershipsHasNoFamilyMemberClaims
{
    private static JwtTokenService CreateSut()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "TestSecretKey_MustBeAtLeast32CharactersLong",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();
        return new JwtTokenService(config);
    }

    [Fact]
    public async Task GenerateTokenAsync_WithNullFamilyMemberships_HasNoFamilyMemberClaims()
    {
        var sut = CreateSut();

        var (token, _) = await sut.GenerateTokenAsync("user-1", "test@test.com", "user", [], null);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        jsonToken.Claims.Where(c => c.Type == DomainClaimTypes.FamilyMember).Should().BeEmpty();
    }

    [Fact]
    public async Task GenerateTokenAsync_WithEmptyFamilyMemberships_HasNoFamilyMemberClaims()
    {
        var sut = CreateSut();

        var (token, _) = await sut.GenerateTokenAsync("user-1", "test@test.com", "user", [], []);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        jsonToken.Claims.Where(c => c.Type == DomainClaimTypes.FamilyMember).Should().BeEmpty();
    }
}
