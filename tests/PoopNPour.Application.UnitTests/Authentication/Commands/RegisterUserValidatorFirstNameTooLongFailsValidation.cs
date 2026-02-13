using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorFirstNameTooLongFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_FirstNameTooLong_FailsValidation()
    {
        // Arrange
        var longFirstName = new string('a', 51);
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "Password1!",
            FirstName: longFirstName,
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot exceed 50 characters.");
    }
}
