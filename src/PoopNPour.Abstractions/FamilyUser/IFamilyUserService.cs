namespace PoopNPour.Abstractions.FamilyUser;

/// <summary>
/// Service interface for managing family users
/// </summary>
public interface IFamilyUserService
{
    /// <summary>
    /// Adds a user to a family
    /// </summary>
    Task<FamilyUserDto> AddUserToFamilyAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a user from a family
    /// </summary>
    Task<bool> RemoveUserFromFamilyAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a family user by composite key
    /// </summary>
    Task<FamilyUserDto?> GetFamilyUserByIdAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of family users
    /// </summary>
    Task<(IEnumerable<FamilyUserDto> familyUsers, int totalCount)> GetFamilyUsersAsync(
        int pageNumber,
        int pageSize,
        Guid? familyId = null,
        string? userId = null,
        CancellationToken cancellationToken = default);
}
