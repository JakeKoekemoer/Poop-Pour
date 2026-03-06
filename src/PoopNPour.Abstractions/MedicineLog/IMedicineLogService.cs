namespace PoopNPour.Abstractions.MedicineLog;

/// <summary>
/// Service interface for managing medicine logs
/// </summary>
public interface IMedicineLogService
{
    /// <summary>
    /// Creates a new medicine log
    /// </summary>
    Task<MedicineLogDto> CreateMedicineLogAsync(
        Guid dependentId,
        string medicineName,
        string dosage,
        DateTimeOffset timeAdministered,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing medicine log
    /// </summary>
    Task<MedicineLogDto?> UpdateMedicineLogAsync(
        Guid medicineLogId,
        string? medicineName = null,
        string? dosage = null,
        DateTimeOffset? timeAdministered = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a medicine log by ID
    /// </summary>
    Task<MedicineLogDto?> GetMedicineLogByIdAsync(
        Guid medicineLogId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of medicine logs
    /// </summary>
    Task<(IEnumerable<MedicineLogDto> medicineLogs, int totalCount)> GetMedicineLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        string? medicineName = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        Guid? familyId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the same medicine has already been administered at the same time for a dependent
    /// </summary>
    Task<bool> IsMedicineLogDuplicateAsync(
        Guid dependentId,
        string medicineName,
        DateTimeOffset timeAdministered,
        Guid? excludeMedicineLogId = null,
        CancellationToken cancellationToken = default);
}
