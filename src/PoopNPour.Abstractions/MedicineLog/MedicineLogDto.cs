namespace PoopNPour.Abstractions.MedicineLog;

/// <summary>
/// Data transfer object for MedicineLog
/// </summary>
public class MedicineLogDto
{
    public Guid MedicineLogId { get; set; }
    public Guid DependentId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTimeOffset TimeAdministered { get; set; }
    public ICollection<string>? Notes { get; set; }
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
