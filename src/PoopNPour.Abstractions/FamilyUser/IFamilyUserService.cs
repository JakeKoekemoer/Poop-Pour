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

    /// <summary>
    /// Gets all family memberships for a given user, used to embed family_member claims in the JWT at login.
    /// </summary>
    Task<IEnumerable<FamilyUserDto>> GetUserFamilyMembershipsAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated, enriched list of members for a specific family.
    /// Includes user identity (name, email) and family name via EF navigation joins.
    /// </summary>
    Task<(IEnumerable<FamilyMemberDto> members, int totalCount)> GetFamilyMembersAsync(
        Guid familyId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single enriched family member record.
    /// Returns null if the user is not a member of the family.
    /// </summary>
    Task<FamilyMemberDto?> GetFamilyMemberAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default);
}
