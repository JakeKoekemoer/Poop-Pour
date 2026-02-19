using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Abstractions.DiperLog;

/// <summary>
/// Service interface for managing diper logs
/// </summary>
public interface IDiperLogService
{
    /// <summary>
    /// Creates a new diper log
    /// </summary>
    Task<DiperLogDto> CreateDiperLogAsync(
        Guid dependentId,
        DateTimeOffset diperDate,
        FecalDischargeColour fecalDischargeColour,
        UrinalDischargeColour urinaryDischargeColour,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing diper log
    /// </summary>
    Task<DiperLogDto?> UpdateDiperLogAsync(
        Guid diperLogId,
        DateTimeOffset? diperDate = null,
        FecalDischargeColour? fecalDischargeColour = null,
        UrinalDischargeColour? urinaryDischargeColour = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a diper log by ID
    /// </summary>
    Task<DiperLogDto?> GetDiperLogByIdAsync(
        Guid diperLogId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of diper logs
    /// </summary>
    Task<(IEnumerable<DiperLogDto> diperLogs, int totalCount)> GetDiperLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a diaper log with the same timestamp already exists for a dependent
    /// </summary>
    Task<bool> IsDiperLogTimestampDuplicateAsync(
        Guid dependentId,
        DateTimeOffset diperDate,
        Guid? excludeDiperLogId = null,
        CancellationToken cancellationToken = default);
}
