using Microsoft.AspNetCore.Identity;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Domain.Common.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<FamilyUser> FamilyUsers { get; set; } = [];
}
