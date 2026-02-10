namespace PoopNPour.Abstractions.Authentication;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user with their roles
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="email">The user email</param>
    /// <param name="userName">The user name</param>
    /// <param name="roles">The roles assigned to the user</param>
    /// <returns>Tuple containing the token string and expiration DateTime</returns>
    Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(
        string userId,
        string email,
        string userName,
        IEnumerable<string> roles);
}
