using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorLastNameTooLongFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_LastNameTooLong_FailsValidation()
    {
        // Arrange
        var longLastName = new string('a', 51);
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "Password1!",
            FirstName: "Test",
            LastName: longLastName);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 50 characters.");
    }
}
