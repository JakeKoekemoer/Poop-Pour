using PoopNPour.Abstractions.Dependent;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for DependentDto in tests.
/// </summary>
public class DependentDtoBuilder
{
    private Guid _dependentId = Guid.NewGuid();
    private Guid _familyId = Guid.NewGuid();
    private string _dependentName = "TestChild";
    private string _dependentSurname = "Doe";
    private DateTimeOffset _dateOfBirth = DateTimeOffset.UtcNow.AddYears(-2);
    private int _age = 2;
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public DependentDtoBuilder WithDependentId(Guid dependentId)
    {
        _dependentId = dependentId;
        return this;
    }

    public DependentDtoBuilder WithFamilyId(Guid familyId)
    {
        _familyId = familyId;
        return this;
    }

    public DependentDtoBuilder WithDependentName(string dependentName)
    {
        _dependentName = dependentName;
        return this;
    }

    public DependentDtoBuilder WithDependentSurname(string dependentSurname)
    {
        _dependentSurname = dependentSurname;
        return this;
    }

    public DependentDtoBuilder WithDateOfBirth(DateTimeOffset dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public DependentDtoBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }

    public DependentDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public DependentDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public DependentDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public DependentDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public DependentDto Build()
    {
        return new DependentDto
        {
            DependentId = _dependentId,
            FamilyId = _familyId,
            DependentName = _dependentName,
            DependentSurname = _dependentSurname,
            DateOfBirth = _dateOfBirth,
            Age = _age,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
