namespace PoopNPour.Application.Families.Models;

/// <summary>
/// Data transfer object for Family
/// </summary>
public class FamilyDto
{
    public Guid FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public string FamilyLastName { get; set; } = string.Empty;
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
