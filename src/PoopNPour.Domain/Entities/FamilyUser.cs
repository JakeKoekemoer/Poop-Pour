using PoopNPour.Domain.Common;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Domain.Entities;

public class FamilyUser : BaseAuditableEntity
{
    // PK
    public Guid FamilyId { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Properties
    public FamilyRole Role { get; set; } = FamilyRole.Member;

    // Navigation properties
    public ApplicationUser? User { get; set; }
    public Family? Family { get; set; }
}
