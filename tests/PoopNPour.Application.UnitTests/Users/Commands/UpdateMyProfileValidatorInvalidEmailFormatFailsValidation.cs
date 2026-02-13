using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorInvalidEmailFormatFailsValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test")]
    [InlineData("test.example.com")]
    public void Validate_InvalidEmailFormat_FailsValidation(string email)
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: email);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }
}
