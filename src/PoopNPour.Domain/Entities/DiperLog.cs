using PoopNPour.Domain.Common;
using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Domain.Entities;

public class DiperLog : BaseAuditableEntity
{
    // PK
    public Guid DiperLogId { get; set; }

    // FK
    public Guid DependentId { get; set; }

    // Properties
    public DateTimeOffset DiperDate { get; set; }
    public FecalDischargeColour FecalDischargeColour { get; set; }
    public UrinalDischargeColour UrinaryDischargeColour { get; set; }
    public ICollection<string> Notes { get; set; } = [];

    // Navigation properties
    public Dependent Dependent { get; set; } = null!;

}
