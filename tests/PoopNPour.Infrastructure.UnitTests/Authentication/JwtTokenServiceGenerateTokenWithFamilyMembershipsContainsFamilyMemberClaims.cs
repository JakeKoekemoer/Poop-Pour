using FluentAssertions;
using Microsoft.Extensions.Configuration;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Xunit;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;
using FamilyRole = PoopNPour.Domain.Common.Auth.FamilyRole;

namespace PoopNPour.Infrastructure.UnitTests.Authentication;

public class JwtTokenServiceGenerateTokenWithFamilyMembershipsContainsFamilyMemberClaims
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
    public async Task GenerateTokenAsync_WithFamilyMemberships_ContainsOneFamilyMemberClaimPerFamily()
    {
        var sut = CreateSut();
        var familyId1 = Guid.NewGuid();
        var familyId2 = Guid.NewGuid();

        var memberships = new[]
        {
            new FamilyUserDto { FamilyId = familyId1, UserId = "user-1", Role = FamilyRole.Owner },
            new FamilyUserDto { FamilyId = familyId2, UserId = "user-1", Role = FamilyRole.Member }
        };

        var (token, _) = await sut.GenerateTokenAsync("user-1", "test@test.com", "user", [], memberships);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var familyMemberClaims = jsonToken.Claims
            .Where(c => c.Type == DomainClaimTypes.FamilyMember)
            .Select(c => c.Value)
            .ToList();

        familyMemberClaims.Should().HaveCount(2);
        familyMemberClaims.Should().Contain(familyId1.ToString());
        familyMemberClaims.Should().Contain(familyId2.ToString());
    }
}
