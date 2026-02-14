namespace PoopNPour.Application.Dependents.Models;

/// <summary>
/// Data transfer object for Dependent
/// </summary>
public class DependentDto
{
    public Guid DependentId { get; set; }
    public Guid FamilyId { get; set; }
    public string DependentName { get; set; } = string.Empty;
    public string DependentSurname { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }
    public int Age { get; set; }
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
