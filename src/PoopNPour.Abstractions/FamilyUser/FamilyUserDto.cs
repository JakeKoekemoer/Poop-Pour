namespace PoopNPour.Abstractions.FamilyUser;

/// <summary>
/// Data transfer object for FamilyUser
/// </summary>
public class FamilyUserDto
{
    public Guid FamilyId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
