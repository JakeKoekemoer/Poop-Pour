using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.MedicineLogs.Models;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.MedicineLogService;

/// <summary>
/// Service implementation for managing medicine logs
/// </summary>
public class MedicineLogService(ApplicationDbContext context) : IMedicineLogService
{

    public async Task<MedicineLogDto> CreateMedicineLogAsync(
        Guid dependentId,
        string medicineName,
        string dosage,
        DateTimeOffset timeAdministered,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var medicineLog = new Domain.Entities.MedicineLog
        {
            MedicineLogId = Guid.NewGuid(),
            DependentId = dependentId,
            MedicineName = medicineName,
            Dosage = dosage,
            TimeAdministered = timeAdministered,
            Notes = notes
        };

        context.MedicineLogs.Add(medicineLog);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(medicineLog);
    }

    public async Task<MedicineLogDto?> UpdateMedicineLogAsync(
        Guid medicineLogId,
        string? medicineName = null,
        string? dosage = null,
        DateTimeOffset? timeAdministered = null,
        ICollection<string>? notes = null,
        CancellationToken cancellationToken = default)
    {
        var medicineLog = await context.MedicineLogs
            .FirstOrDefaultAsync(m => m.MedicineLogId == medicineLogId, cancellationToken);

        if (medicineLog == null)
        {
            return null;
        }

        if (medicineName != null)
            medicineLog.MedicineName = medicineName;

        if (dosage != null)
            medicineLog.Dosage = dosage;

        if (timeAdministered.HasValue)
            medicineLog.TimeAdministered = timeAdministered.Value;

        if (notes != null)
            medicineLog.Notes = notes;

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(medicineLog);
    }

    public async Task<MedicineLogDto?> GetMedicineLogByIdAsync(
        Guid medicineLogId,
        CancellationToken cancellationToken = default)
    {
        var medicineLog = await context.MedicineLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MedicineLogId == medicineLogId, cancellationToken);

        return medicineLog == null ? null : MapToDto(medicineLog);
    }

    public async Task<(IEnumerable<MedicineLogDto> medicineLogs, int totalCount)> GetMedicineLogsAsync(
        int pageNumber,
        int pageSize,
        Guid? dependentId = null,
        string? medicineName = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.MedicineLogs.AsNoTracking();

        // Apply filters
        if (dependentId.HasValue)
        {
            query = query.Where(m => m.DependentId == dependentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(medicineName))
        {
            query = query.Where(m => m.MedicineName.Contains(medicineName));
        }

        if (startDate.HasValue)
        {
            query = query.Where(m => m.TimeAdministered >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(m => m.TimeAdministered <= endDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var medicineLogs = await query
            .OrderByDescending(m => m.TimeAdministered)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => MapToDto(m))
            .ToListAsync(cancellationToken);

        return (medicineLogs, totalCount);
    }

    private static MedicineLogDto MapToDto(Domain.Entities.MedicineLog medicineLog)
    {
        return new MedicineLogDto
        {
            MedicineLogId = medicineLog.MedicineLogId,
            DependentId = medicineLog.DependentId,
            MedicineName = medicineLog.MedicineName,
            Dosage = medicineLog.Dosage,
            TimeAdministered = medicineLog.TimeAdministered,
            Notes = medicineLog.Notes,
            // Note: MedicineLog entity doesn't inherit from BaseAuditableEntity
            // so these fields will have default values
            CreatedOn = DateTimeOffset.MinValue,
            CreatedBy = null,
            LastModifiedOn = DateTimeOffset.MinValue,
            LastModifiedBy = null
        };
    }
}
