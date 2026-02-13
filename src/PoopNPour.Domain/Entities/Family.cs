using PoopNPour.Domain.Common;

namespace PoopNPour.Domain.Entities;

public class Family : BaseAuditableEntity
{
    // PK
    public Guid FamilyId { get; set; }

    // Properties
    public string FamilyName { get; set; } = string.Empty; // User friendly name for the family
    public string FamilyLastName { get; set; } = string.Empty; // The last name of the family, used for sorting and display purposes

    // Navigation properties
    public ICollection<FamilyUser> Users { get; set; } = [];
}
