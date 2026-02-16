using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Abstractions.FeedLog;

/// <summary>
/// Service interface for managing feed logs
/// </summary>
public interface IFeedLogService
{
    /// <summary>
    /// Creates a new feed log
    /// </summary>
    Task<FeedLogDto> CreateFeedLogAsync(
        Guid dependentId,
        FeedLogType feedType,
        DateTimeOffset timeFed,
        decimal? mililitersFed = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing feed log
    /// </summary>
    Task<FeedLogDto?> UpdateFeedLogAsync(
        Guid feedLogId,
        FeedLogType? feedType = null,
        DateTimeOffset? timeFed = null,
        decimal? mililitersFed = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a feed log by ID
    /// </summary>
    Task<FeedLogDto?> GetFeedLogByIdAsync(
        Guid feedLogId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of feed logs
    /// </summary>
    Task<(IEnumerable<FeedLogDto> feedLogs, int totalCount)> GetFeedLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default);
}
