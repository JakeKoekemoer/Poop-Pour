using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Abstractions.FamilyUser;

/// <summary>
/// Enriched DTO for displaying a family member in the UI.
/// Includes the user's identity fields and the family name, sourced via EF navigation properties.
/// Not used in the JWT/auth path — see FamilyUserDto for that.
/// </summary>
public class FamilyMemberDto
{
    // User identity
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Family membership
    public Guid FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public string FamilyLastName { get; set; } = string.Empty;
    public FamilyRole Role { get; set; }

    // When the user joined the family
    public DateTimeOffset JoinedOn { get; set; }
}
