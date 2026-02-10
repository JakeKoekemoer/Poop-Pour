namespace PoopNPour.Application.Common.Interfaces;

/// <summary>
/// Service for identity and authorization operations
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Checks if a user is in a specific role
    /// </summary>
    Task<bool> IsInRoleAsync(string userId, string role);

    /// <summary>
    /// Checks if a user satisfies a specific policy
    /// </summary>
    Task<bool> AuthorizeAsync(string userId, string policyName);

    /// <summary>
    /// Gets all roles for a user
    /// </summary>
    Task<IList<string>> GetUserRolesAsync(string userId);
}
