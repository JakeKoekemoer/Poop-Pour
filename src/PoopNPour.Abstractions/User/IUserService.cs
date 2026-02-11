namespace PoopNPour.Abstractions.User;

/// <summary>
/// Service for user operations that returns UserDto.
/// Application layer uses this instead of interacting with Identity directly.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by ID
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by username
    /// </summary>
    Task<UserDto?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users with pagination support
    /// </summary>
    Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user with default role and returns the UserDto
    /// </summary>
    Task<UserDto> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user information and returns the updated UserDto
    /// </summary>
    Task<UserDto> UpdateUserAsync(
        string userId,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates password and returns UserDto if valid, null otherwise
    /// </summary>
    Task<UserDto?> ValidatePasswordAndGetUserAsync(
        string userNameOrEmail,
        string password,
        CancellationToken cancellationToken = default);
}
