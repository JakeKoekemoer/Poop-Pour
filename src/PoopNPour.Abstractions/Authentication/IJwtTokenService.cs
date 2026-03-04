using PoopNPour.Abstractions.FamilyUser;

namespace PoopNPour.Abstractions.Authentication;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user with their roles and family memberships.
    /// Family memberships are embedded as family_member claims (one per family GUID).
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="email">The user email</param>
    /// <param name="userName">The user name</param>
    /// <param name="roles">The roles assigned to the user</param>
    /// <param name="familyMemberships">Optional family memberships to embed as claims</param>
    /// <returns>Tuple containing the token string and expiration DateTime</returns>
    Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(
        string userId,
        string email,
        string userName,
        IEnumerable<string> roles,
        IEnumerable<FamilyUserDto>? familyMemberships = null);

    /// <summary>
    /// Generates a long-lived API token for API clients (Web API, Mobile API, etc.)
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="roles">The roles assigned to the user</param>
    /// <returns>The API token string</returns>
    string GenerateApiToken(string userId, IEnumerable<string> roles);
}
