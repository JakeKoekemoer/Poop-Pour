using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PoopNPour.Application.Authentication;
using PoopNPour.Domain.Common.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PoopNPour.Infrastructure.Authentication;

/// <summary>
/// Implementation of JWT token service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(
        ApplicationUser user,
        IEnumerable<string> roles)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
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
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
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
}
