using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Application.Identity;

/// <summary>
/// Service for managing user identity operations
/// </summary>
public interface IIdentityService
{
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
    /// Adds a role to a user
    /// </summary>
    Task AddUserToRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user is in a role
    /// </summary>
    Task<bool> IsUserInRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ensures a role exists, creates it if it doesn't
    /// </summary>
    Task EnsureRoleExistsAsync(
        string role,
        CancellationToken cancellationToken = default);
}
