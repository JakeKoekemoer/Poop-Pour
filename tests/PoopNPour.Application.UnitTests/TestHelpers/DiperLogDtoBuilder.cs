using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for DiperLogDto in tests.
/// </summary>
public class DiperLogDtoBuilder
{
    private Guid _diperLogId = Guid.NewGuid();
    private Guid _dependentId = Guid.NewGuid();
    private DateTimeOffset _diperDate = DateTimeOffset.UtcNow;
    private FecalDischargeColour _fecalDischargeColour = FecalDischargeColour.BROWN;
    private UrinalDischargeColour _urinaryDischargeColour = UrinalDischargeColour.YELLOW;
    private ICollection<string>? _notes = null;
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public DiperLogDtoBuilder WithDiperLogId(Guid diperLogId)
    {
        _diperLogId = diperLogId;
        return this;
    }

    public DiperLogDtoBuilder WithDependentId(Guid dependentId)
    {
        _dependentId = dependentId;
        return this;
    }

    public DiperLogDtoBuilder WithDiperDate(DateTimeOffset diperDate)
    {
        _diperDate = diperDate;
        return this;
    }

    public DiperLogDtoBuilder WithFecalDischargeColour(FecalDischargeColour fecalDischargeColour)
    {
        _fecalDischargeColour = fecalDischargeColour;
        return this;
    }

    public DiperLogDtoBuilder WithUrinaryDischargeColour(UrinalDischargeColour urinaryDischargeColour)
    {
        _urinaryDischargeColour = urinaryDischargeColour;
        return this;
    }

    public DiperLogDtoBuilder WithNotes(ICollection<string>? notes)
    {
        _notes = notes;
        return this;
    }

    public DiperLogDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public DiperLogDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public DiperLogDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public DiperLogDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public DiperLogDto Build()
    {
        return new DiperLogDto
        {
            DiperLogId = _diperLogId,
            DependentId = _dependentId,
            DiperDate = _diperDate,
            FecalDischargeColour = _fecalDischargeColour,
            UrinaryDischargeColour = _urinaryDischargeColour,
            Notes = _notes,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
