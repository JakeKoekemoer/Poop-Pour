using PoopNPour.Application.Authentication.Models;

namespace PoopNPour.Api.FunctionalTests.TestHelpers;

/// <summary>
/// Fluent builder for RegisterRequestDto in functional tests.
/// </summary>
public class RegisterRequestDtoBuilder
{
    private string _email = $"test{Guid.NewGuid():N}@example.com";
    private string _userName = $"user{Guid.NewGuid():N}";
    private string _password = "ValidPassword123!";
    private string? _firstName = "Test";
    private string? _lastName = "User";

    public RegisterRequestDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public RegisterRequestDtoBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public RegisterRequestDtoBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public RegisterRequestDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public RegisterRequestDtoBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public RegisterRequestDto Build()
    {
        return new RegisterRequestDto
        {
            Email = _email,
            UserName = _userName,
            Password = _password,
            FirstName = _firstName,
            LastName = _lastName
        };
    }
}
