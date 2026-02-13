using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorWeakPasswordFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData("password123!")] // no uppercase
    [InlineData("PASSWORD123!")] // no lowercase
    [InlineData("Password!!!")]  // no digit
    [InlineData("Password123")]  // no special char
    [InlineData("password")]     // no uppercase, digit, or special
    public void Validate_WeakPassword_FailsValidation(string password)
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: "testuser",
            Password: password,
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");
    }
}
