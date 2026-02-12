using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorEmailTooLongFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_EmailTooLong_FailsValidation()
    {
        // Arrange
        var longEmail = new string('a', 90) + "@example.com"; // 102 chars total
        var command = new RegisterUserCommand(
            Email: longEmail,
            UserName: "testuser",
            Password: "Password1!",
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email cannot exceed 100 characters.");
    }
}
