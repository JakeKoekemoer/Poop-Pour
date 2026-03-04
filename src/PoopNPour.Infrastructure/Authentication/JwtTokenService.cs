using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.FamilyUser;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;

namespace PoopNPour.Infrastructure.Authentication;

/// <summary>
/// Implementation of JWT token service
/// </summary>
public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(
        string userId,
        string email,
        string userName,
        IEnumerable<string> roles,
        IEnumerable<FamilyUserDto>? familyMemberships = null)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey not found in configuration.");
        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer not found in configuration.");
        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience not found in configuration.");
        var expirationMinutes = jwtSettings.GetValue<int>("ExpirationInMinutes");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId),
            new Claim(System.Security.Claims.ClaimTypes.Email, email ?? string.Empty),
            new Claim(System.Security.Claims.ClaimTypes.Name, userName ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, role));
        }

        foreach (var membership in familyMemberships ?? [])
        {
            claims.Add(new Claim(DomainClaimTypes.FamilyMember, membership.FamilyId.ToString()));
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult((tokenString, expiresAt));
    }

    public string GenerateApiToken(string userId, IEnumerable<string> roles)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey not found in configuration.");
        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer not found in configuration.");
        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience not found in configuration.");
        
        // API tokens have a much longer expiration (10 years)
        var apiTokenExpirationDays = jwtSettings.GetValue<int>("ApiTokenExpirationInDays", 3650);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId),
            new Claim("token_type", "api_token")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, role));
        }

        var expiresAt = DateTime.UtcNow.AddDays(apiTokenExpirationDays);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
