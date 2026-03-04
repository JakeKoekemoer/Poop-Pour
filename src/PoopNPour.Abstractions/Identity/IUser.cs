namespace PoopNPour.Abstractions.Identity;

/// <summary>
/// Represents the current authenticated user
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the unique identifier of the current user
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Gets the username of the current user
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Gets the email of the current user
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Indicates whether the user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Indicates whether the user is an administrator
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// The IDs of all families the user is a member of, parsed from JWT family_member claims.
    /// Used as a fast in-memory gate before any DB lookup.
    /// </summary>
    IEnumerable<Guid> FamilyIds { get; }
}
