using FluentAssertions;
using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorValidCommandPassesValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "Password1!",
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
