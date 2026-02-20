using PoopNPour.Abstractions.User;
using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Abstractions.Identity;

/// <summary>
/// Service for managing user identity and authorization operations
/// Consolidated interface for all identity-related functionality
/// </summary>
public interface IIdentityService
{
    #region User Management

    /// <summary>
    /// Creates a new user with the specified password
    /// </summary>
    Task<ApplicationUser> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by username
    /// </summary>
    Task<ApplicationUser?> GetUserByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    Task<ApplicationUser?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    Task<ApplicationUser?> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user exists by username
    /// </summary>
    Task<bool> UserExistsByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user exists by email
    /// </summary>
    Task<bool> UserExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user information
    /// </summary>
    Task<ApplicationUser> UpdateUserAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user by ID
    /// </summary>
    Task DeleteUserAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users with pagination support
    /// </summary>
    Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates user password
    /// </summary>
    Task<ApplicationUser?> ValidatePasswordAsync(
        string userNameOrEmail,
        string password,
        CancellationToken cancellationToken = default);

    #endregion

    #region Role Management

    /// <summary>
    /// Adds a role to a user
    /// </summary>
    Task AddUserToRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user is in a role (by ApplicationUser object)
    /// </summary>
    Task<bool> IsUserInRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user is in a specific role (by user ID)
    /// Used by authorization pipeline
    /// </summary>
    Task<bool> IsInRoleAsync(string userId, string role);

    /// <summary>
    /// Ensures a role exists, creates it if it doesn't
    /// </summary>
    Task EnsureRoleExistsAsync(
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user roles (by ApplicationUser object)
    /// </summary>
    Task<IEnumerable<string>> GetUserRolesAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all roles for a user (by user ID)
    /// Used by authorization pipeline
    /// </summary>
    Task<IList<string>> GetUserRolesAsync(string userId);

    #endregion

    #region Authorization

    /// <summary>
    /// Checks if a user satisfies a specific policy
    /// Used by authorization pipeline
    /// </summary>
    Task<bool> AuthorizeAsync(string userId, string policyName);

    #endregion

    #region Claims Management

    /// <summary>
    /// Gets all claims for a user
    /// </summary>
    Task<IList<ClaimDto>> GetUserClaimsAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds claims to a user
    /// </summary>
    Task AddUserClaimsAsync(
        ApplicationUser user,
        IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes claims from a user
    /// </summary>
    Task RemoveUserClaimsAsync(
        ApplicationUser user,
        IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken = default);

    #endregion

    #region Authentication Tokens

    /// <summary>
    /// Sets an authentication token for a user (e.g., API token)
    /// </summary>
    Task SetAuthenticationTokenAsync(
        ApplicationUser user,
        string loginProvider,
        string tokenName,
        string tokenValue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an authentication token for a user
    /// </summary>
    Task<string?> GetAuthenticationTokenAsync(
        ApplicationUser user,
        string loginProvider,
        string tokenName,
        CancellationToken cancellationToken = default);

    #endregion
}
