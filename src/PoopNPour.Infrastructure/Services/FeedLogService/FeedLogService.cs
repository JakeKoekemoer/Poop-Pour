using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Domain.Enums.FeedLog;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.FeedLogService;

/// <summary>
/// Service implementation for managing feed logs
/// </summary>
public class FeedLogService(ApplicationDbContext context) : IFeedLogService
{

    public async Task<FeedLogDto> CreateFeedLogAsync(
        Guid dependentId,
        FeedLogType feedType,
        DateTimeOffset timeFed,
        decimal? mililitersFed = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var feedLog = new Domain.Entities.FeedLog
        {
            DependentId = dependentId,
            FeedType = feedType,
            TimeFed = timeFed,
            MililitersFed = mililitersFed,
            Notes = notes
        };

        context.FeedLogs.Add(feedLog);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(feedLog);
    }

    public async Task<FeedLogDto?> UpdateFeedLogAsync(
        Guid feedLogId,
        FeedLogType? feedType = null,
        DateTimeOffset? timeFed = null,
        decimal? mililitersFed = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var feedLog = await context.FeedLogs
            .FirstOrDefaultAsync(f => f.FeedLogId == feedLogId, cancellationToken);

        if (feedLog == null)
        {
            return null;
        }

        if (feedType.HasValue)
            feedLog.FeedType = feedType.Value;

        if (timeFed.HasValue)
            feedLog.TimeFed = timeFed.Value;

        if (mililitersFed.HasValue)
            feedLog.MililitersFed = mililitersFed;

        if (notes != null)
            feedLog.Notes = notes;

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(feedLog);
    }

    public async Task<FeedLogDto?> GetFeedLogByIdAsync(
        Guid feedLogId,
        CancellationToken cancellationToken = default)
    {
        var feedLog = await context.FeedLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FeedLogId == feedLogId, cancellationToken);

        return feedLog == null ? null : MapToDto(feedLog);
    }

    public async Task<(IEnumerable<FeedLogDto> feedLogs, int totalCount)> GetFeedLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        Guid? familyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FeedLogs.AsNoTracking();

        // Apply filters
        if (dependentId.HasValue)
        {
            query = query.Where(f => f.DependentId == dependentId.Value);
        }

        if (familyId.HasValue)
        {
            query = query.Where(f => f.Dependent!.FamilyId == familyId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(f => f.TimeFed >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(f => f.TimeFed <= endDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(f => f.TimeFed)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (entities.Select(MapToDto), totalCount);
    }

    public async Task<bool> IsFeedLogTimestampDuplicateAsync(
        Guid dependentId,
        DateTimeOffset timeFed,
        Guid? excludeFeedLogId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FeedLogs
            .Where(fl => fl.DependentId == dependentId && fl.TimeFed == timeFed);

        if (excludeFeedLogId.HasValue)
        {
            query = query.Where(fl => fl.FeedLogId != excludeFeedLogId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    private static FeedLogDto MapToDto(Domain.Entities.FeedLog feedLog)
    {
        return new FeedLogDto
        {
            FeedLogId = feedLog.FeedLogId,
            DependentId = feedLog.DependentId,
            FeedType = feedLog.FeedType,
            TimeFed = feedLog.TimeFed,
            MililitersFed = feedLog.MililitersFed,
            Notes = feedLog.Notes,
            CreatedOn = feedLog.CreatedOn,
            CreatedBy = feedLog.CreatedBy,
            LastModifiedOn = feedLog.LastModifiedOn,
            LastModifiedBy = feedLog.LastModifiedBy
        };
    }
}
