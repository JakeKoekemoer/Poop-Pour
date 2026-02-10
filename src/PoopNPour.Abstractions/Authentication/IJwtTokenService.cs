using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Abstractions.Authentication;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user with their roles
    /// </summary>
    /// <param name="user">The user to generate token for</param>
    /// <param name="roles">The roles assigned to the user</param>
    /// <returns>Tuple containing the token string and expiration DateTime</returns>
    Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(
        ApplicationUser user,
        IEnumerable<string> roles);
}
