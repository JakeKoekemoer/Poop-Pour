using PoopNPour.Domain.Common;

namespace PoopNPour.Domain.Entities;

public class Dependent : BaseAuditableEntity
{
    // PK
    public Guid DependentId { get; set; }

    // FK
    public Guid FamilyId { get; set; }

    // Properties
    public string DependentName { get; set; } = string.Empty;
    public string DependentSurname { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }

    // Computed property for age, not mapped to the database
    public int Age
    {
        get
        {
            if (DateOfBirth == default) return 0;
            var now = DateTimeOffset.UtcNow;
            var years = now.Year - DateOfBirth.Year;
            // adjust if birthday hasn't occurred yet this year
            if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            {
                years--;
            }
            return years;
        }
    }

    // Navigation properties
    public Family Family { get; set; } = null!;
    public ICollection<FeedLog> FeedLogs { get; set; } = [];
}
