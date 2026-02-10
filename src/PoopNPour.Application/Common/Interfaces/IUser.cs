namespace PoopNPour.Application.Common.Interfaces;

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
}
