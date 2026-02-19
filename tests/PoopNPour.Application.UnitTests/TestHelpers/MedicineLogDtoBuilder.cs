using PoopNPour.Abstractions.MedicineLog;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for MedicineLogDto in tests.
/// </summary>
public class MedicineLogDtoBuilder
{
    private Guid _medicineLogId = Guid.NewGuid();
    private Guid _dependentId = Guid.NewGuid();
    private string _medicineName = "Paracetamol";
    private string _dosage = "5ml";
    private DateTimeOffset _timeAdministered = DateTimeOffset.UtcNow;
    private ICollection<string>? _notes = null;
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public MedicineLogDtoBuilder WithMedicineLogId(Guid medicineLogId)
    {
        _medicineLogId = medicineLogId;
        return this;
    }

    public MedicineLogDtoBuilder WithDependentId(Guid dependentId)
    {
        _dependentId = dependentId;
        return this;
    }

    public MedicineLogDtoBuilder WithMedicineName(string medicineName)
    {
        _medicineName = medicineName;
        return this;
    }

    public MedicineLogDtoBuilder WithDosage(string dosage)
    {
        _dosage = dosage;
        return this;
    }

    public MedicineLogDtoBuilder WithTimeAdministered(DateTimeOffset timeAdministered)
    {
        _timeAdministered = timeAdministered;
        return this;
    }

    public MedicineLogDtoBuilder WithNotes(ICollection<string>? notes)
    {
        _notes = notes;
        return this;
    }

    public MedicineLogDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public MedicineLogDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public MedicineLogDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public MedicineLogDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public MedicineLogDto Build()
    {
        return new MedicineLogDto
        {
            MedicineLogId = _medicineLogId,
            DependentId = _dependentId,
            MedicineName = _medicineName,
            Dosage = _dosage,
            TimeAdministered = _timeAdministered,
            Notes = _notes,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
