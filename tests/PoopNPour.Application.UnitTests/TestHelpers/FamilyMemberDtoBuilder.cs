using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for FamilyMemberDto in tests.
/// </summary>
public class FamilyMemberDtoBuilder
{
    private string _userId = Guid.NewGuid().ToString();
    private string _userName = "testuser";
    private string _email = "test@example.com";
    private string? _firstName = "Test";
    private string? _lastName = "User";
    private Guid _familyId = Guid.NewGuid();
    private string _familyName = "Smith";
    private string _familyLastName = "Smith";
    private FamilyRole _role = FamilyRole.Member;
    private DateTimeOffset _joinedOn = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public FamilyMemberDtoBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public FamilyMemberDtoBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public FamilyMemberDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public FamilyMemberDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public FamilyMemberDtoBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public FamilyMemberDtoBuilder WithFamilyId(Guid familyId)
    {
        _familyId = familyId;
        return this;
    }

    public FamilyMemberDtoBuilder WithFamilyName(string familyName)
    {
        _familyName = familyName;
        return this;
    }

    public FamilyMemberDtoBuilder WithFamilyLastName(string familyLastName)
    {
        _familyLastName = familyLastName;
        return this;
    }

    public FamilyMemberDtoBuilder WithRole(FamilyRole role)
    {
        _role = role;
        return this;
    }

    public FamilyMemberDtoBuilder WithJoinedOn(DateTimeOffset joinedOn)
    {
        _joinedOn = joinedOn;
        return this;
    }

    public FamilyMemberDto Build() => new()
    {
        UserId         = _userId,
        UserName       = _userName,
        Email          = _email,
        FirstName      = _firstName,
        LastName       = _lastName,
        FamilyId       = _familyId,
        FamilyName     = _familyName,
        FamilyLastName = _familyLastName,
        Role           = _role,
        JoinedOn       = _joinedOn,
    };
}
