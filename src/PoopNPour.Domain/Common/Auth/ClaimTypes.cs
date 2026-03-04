namespace PoopNPour.Domain.Common.Auth;

/// <summary>
/// Custom claim type constants
/// </summary>
public static class ClaimTypes
{
    public const string Policy = nameof(Policy);

    /// <summary>
    /// Claim type for family membership. One claim per family the user belongs to.
    /// Value is the family GUID as a string.
    /// </summary>
    public const string FamilyMember = "family_member";
}
