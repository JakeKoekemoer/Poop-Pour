using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class AuthenticateUserValidatorPasswordTooShortFailsValidation
{
    private readonly AuthenticateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("Pass1")]  // 5 chars
    [InlineData("Pwd")]    // 3 chars
    [InlineData("12345")]  // 5 chars
    public void Validate_PasswordTooShort_FailsValidation(string password)
    {
        // Arrange
        var command = new AuthenticateUserCommand(
            Username: "testuser",
            Password: password);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 6 characters long.");
    }
}
