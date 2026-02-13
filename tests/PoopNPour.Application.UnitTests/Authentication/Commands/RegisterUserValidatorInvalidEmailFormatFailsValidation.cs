using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorInvalidEmailFormatFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test")]
    [InlineData("test.example.com")]
    public void Validate_InvalidEmailFormat_FailsValidation(string email)
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: email,
            UserName: "testuser",
            Password: "Password1!",
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }
}
