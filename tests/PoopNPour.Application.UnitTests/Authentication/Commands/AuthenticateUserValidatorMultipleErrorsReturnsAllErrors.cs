using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class AuthenticateUserValidatorMultipleErrorsReturnsAllErrors
{
    private readonly AuthenticateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var command = new AuthenticateUserCommand(
            Username: "",
            Password: "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
