using PoopNPour.Abstractions.User;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for UserDto in tests.
/// </summary>
public class UserDtoBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private string _email = "test@example.com";
    private string _userName = "testuser";
    private string? _firstName = "Test";
    private string? _lastName = "User";
    private DateTime _createdAt = new DateTime(2025, 1, 1);
    private DateTime? _updatedAt;
    private IEnumerable<string> _roles = new[] { "User" };

    public UserDtoBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public UserDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserDtoBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UserDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserDtoBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserDtoBuilder WithRoles(params string[] roles)
    {
        _roles = roles;
        return this;
    }

    public UserDtoBuilder WithUpdatedAt(DateTime? updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public UserDto Build()
    {
        return new UserDto
        {
            Id = _id,
            Email = _email,
            UserName = _userName,
            FirstName = _firstName,
            LastName = _lastName,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt,
            Roles = _roles
        };
    }
}
