using PoopNPour.Application.Users.Models;

namespace PoopNPour.Api.FunctionalTests.TestHelpers;

/// <summary>
/// Fluent builder for UpdateProfileRequestDto in functional tests.
/// </summary>
public class UpdateProfileRequestDtoBuilder
{
    private string? _firstName;
    private string? _lastName;
    private string? _email;

    public UpdateProfileRequestDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UpdateProfileRequestDtoBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UpdateProfileRequestDtoBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public UpdateProfileRequestDto Build()
    {
        return new UpdateProfileRequestDto
        {
            FirstName = _firstName,
            LastName = _lastName,
            Email = _email
        };
    }
}
