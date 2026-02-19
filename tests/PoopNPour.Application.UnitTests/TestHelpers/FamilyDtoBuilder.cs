using PoopNPour.Abstractions.Family;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for FamilyDto in tests.
/// </summary>
public class FamilyDtoBuilder
{
    private Guid _familyId = Guid.NewGuid();
    private string _familyName = "Test Family";
    private string _familyLastName = "Smith";
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public FamilyDtoBuilder WithFamilyId(Guid familyId)
    {
        _familyId = familyId;
        return this;
    }

    public FamilyDtoBuilder WithFamilyName(string familyName)
    {
        _familyName = familyName;
        return this;
    }

    public FamilyDtoBuilder WithFamilyLastName(string familyLastName)
    {
        _familyLastName = familyLastName;
        return this;
    }

    public FamilyDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public FamilyDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public FamilyDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public FamilyDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public FamilyDto Build()
    {
        return new FamilyDto
        {
            FamilyId = _familyId,
            FamilyName = _familyName,
            FamilyLastName = _familyLastName,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
