using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class AuthenticateUserValidatorEmptyPasswordFailsValidation
{
    private readonly AuthenticateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_EmptyPassword_FailsValidation(string password)
    {
        // Arrange
        var command = new AuthenticateUserCommand(
            Username: "testuser",
            Password: password);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }
}
