using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorPasswordTooShortFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData("Pass1!")] // 6 chars, too short
    [InlineData("Pwd1!")]  // 5 chars
    public void Validate_PasswordTooShort_FailsValidation(string password)
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
            .WithErrorMessage("Password must be at least 8 characters long.");
    }
}
