using PoopNPour.Domain.Common;

namespace PoopNPour.Domain.Entities;

public class MedicineLog : BaseAuditableEntity
{
    // PK
    public Guid MedicineLogId { get; set; }

    // FKs
    public Guid DependentId { get; set; }

    // Properties
    public string MedicineName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public DateTimeOffset TimeAdministered { get; set; }
    public ICollection<string>? Notes { get; set; }

    // Navigation properties
    public Dependent Dependent { get; set; } = null!;
}
