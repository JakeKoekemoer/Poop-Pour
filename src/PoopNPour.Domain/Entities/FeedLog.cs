using PoopNPour.Domain.Common;
using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Domain.Entities;

public class FeedLog : BaseAuditableEntity
{
    // PK
    public Guid FeedLogId { get; set; }

    // FK
    public Guid DependentId { get;set; }

    // Properties
    public FeedLogType FeedType { get; set; }
    public DateTimeOffset TimeFed { get; set; }
    public decimal? MililitersFed { get; set; }
    public ICollection<string>? Notes { get; set; } = [];

    // Navigation properties
    public Dependent? Dependent { get; set; }
}
