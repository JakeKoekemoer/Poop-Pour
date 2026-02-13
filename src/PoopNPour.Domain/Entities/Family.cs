using PoopNPour.Domain.Common;

namespace PoopNPour.Domain.Entities;

public class Family : BaseAuditableEntity
{
    // PK
    public Guid FamilyId { get; set; }

    // Properties
    public string FamilyName { get; set; } // User friendly name for the family
    public string FamilyLastName { get; set; } // The last name of the family, used for sorting and display purposes
}
