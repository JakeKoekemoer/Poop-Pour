using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Application.FeedLogs.Models;

/// <summary>
/// Data transfer object for FeedLog
/// </summary>
public class FeedLogDto
{
    public Guid FeedLogId { get; set; }
    public Guid DependentId { get; set; }
    public FeedLogType FeedType { get; set; }
    public DateTimeOffset TimeFed { get; set; }
    public decimal? MililitersFed { get; set; }
    public ICollection<string>? Notes { get; set; }
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
