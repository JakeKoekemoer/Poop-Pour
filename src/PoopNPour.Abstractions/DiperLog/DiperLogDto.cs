using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Abstractions.DiperLog;

/// <summary>
/// Data transfer object for DiperLog
/// </summary>
public class DiperLogDto
{
    public Guid DiperLogId { get; set; }
    public Guid DependentId { get; set; }
    public DateTimeOffset DiperDate { get; set; }
    public FecalDischargeColour FecalDischargeColour { get; set; }
    public UrinalDischargeColour UrinaryDischargeColour { get; set; }
    public ICollection<string>? Notes { get; set; }
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
