using PoopNPour.Abstractions.FamilyUser;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for FamilyUserDto in tests.
/// </summary>
public class FamilyUserDtoBuilder
{
    private Guid _familyId = Guid.NewGuid();
    private string _userId = Guid.NewGuid().ToString();
    private DateTimeOffset _createdOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _createdBy = "test-user";
    private DateTimeOffset _lastModifiedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private string? _lastModifiedBy = "test-user";

    public FamilyUserDtoBuilder WithFamilyId(Guid familyId)
    {
        _familyId = familyId;
        return this;
    }

    public FamilyUserDtoBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public FamilyUserDtoBuilder WithCreatedOn(DateTimeOffset createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public FamilyUserDtoBuilder WithCreatedBy(string? createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public FamilyUserDtoBuilder WithLastModifiedOn(DateTimeOffset lastModifiedOn)
    {
        _lastModifiedOn = lastModifiedOn;
        return this;
    }

    public FamilyUserDtoBuilder WithLastModifiedBy(string? lastModifiedBy)
    {
        _lastModifiedBy = lastModifiedBy;
        return this;
    }

    public FamilyUserDto Build()
    {
        return new FamilyUserDto
        {
            FamilyId = _familyId,
            UserId = _userId,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            LastModifiedOn = _lastModifiedOn,
            LastModifiedBy = _lastModifiedBy
        };
    }
}
