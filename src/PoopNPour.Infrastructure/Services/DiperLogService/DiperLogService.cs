using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.DiperLogs.Models;
using PoopNPour.Domain.Enums.DiperLog;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.DiperLogService;

/// <summary>
/// Service implementation for managing diper logs
/// </summary>
public class DiperLogService(ApplicationDbContext context) : IDiperLogService
{

    public async Task<DiperLogDto> CreateDiperLogAsync(
        Guid dependentId,
        DateTimeOffset diperDate,
        FecalDischargeColour fecalDischargeColour,
        UrinalDischargeColour urinaryDischargeColour,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var diperLog = new Domain.Entities.DiperLog
        {
            DiperLogId = Guid.NewGuid(),
            DependentId = dependentId,
            DiperDate = diperDate,
            FecalDischargeColour = fecalDischargeColour,
            UrinaryDischargeColour = urinaryDischargeColour,
            Notes = notes
        };

        context.DiperLogs.Add(diperLog);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(diperLog);
    }

    public async Task<DiperLogDto?> UpdateDiperLogAsync(
        Guid diperLogId,
        DateTimeOffset? diperDate = null,
        FecalDischargeColour? fecalDischargeColour = null,
        UrinalDischargeColour? urinaryDischargeColour = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var diperLog = await context.DiperLogs
            .FirstOrDefaultAsync(d => d.DiperLogId == diperLogId, cancellationToken);

        if (diperLog == null)
        {
            return null;
        }

        if (diperDate.HasValue)
            diperLog.DiperDate = diperDate.Value;

        if (fecalDischargeColour.HasValue)
            diperLog.FecalDischargeColour = fecalDischargeColour.Value;

        if (urinaryDischargeColour.HasValue)
            diperLog.UrinaryDischargeColour = urinaryDischargeColour.Value;

        if (notes != null)
            diperLog.Notes = notes;

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(diperLog);
    }

    public async Task<DiperLogDto?> GetDiperLogByIdAsync(
        Guid diperLogId,
        CancellationToken cancellationToken = default)
    {
        var diperLog = await context.DiperLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DiperLogId == diperLogId, cancellationToken);

        return diperLog == null ? null : MapToDto(diperLog);
    }

    public async Task<(IEnumerable<DiperLogDto> diperLogs, int totalCount)> GetDiperLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.DiperLogs.AsNoTracking();

        // Apply filters
        if (dependentId.HasValue)
        {
            query = query.Where(d => d.DependentId == dependentId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(d => d.DiperDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(d => d.DiperDate <= endDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var diperLogs = await query
            .OrderByDescending(d => d.DiperDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => MapToDto(d))
            .ToListAsync(cancellationToken);

        return (diperLogs, totalCount);
    }

    private static DiperLogDto MapToDto(Domain.Entities.DiperLog diperLog)
    {
        return new DiperLogDto
        {
            DiperLogId = diperLog.DiperLogId,
            DependentId = diperLog.DependentId,
            DiperDate = diperLog.DiperDate,
            FecalDischargeColour = diperLog.FecalDischargeColour,
            UrinaryDischargeColour = diperLog.UrinaryDischargeColour,
            Notes = diperLog.Notes,
            CreatedOn = diperLog.CreatedOn,
            CreatedBy = diperLog.CreatedBy,
            LastModifiedOn = diperLog.LastModifiedOn,
            LastModifiedBy = diperLog.LastModifiedBy
        };
    }
}
