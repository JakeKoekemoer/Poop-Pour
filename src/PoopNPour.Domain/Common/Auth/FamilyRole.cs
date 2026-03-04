namespace PoopNPour.Domain.Common.Auth;

/// <summary>
/// Represents the role a user holds within a specific family.
/// Higher values indicate more permissions — comparisons use >= to check sufficiency.
/// </summary>
public enum FamilyRole
{
    Viewer = 0,
    Member = 10,
    Admin  = 20,
    Owner  = 30
}
