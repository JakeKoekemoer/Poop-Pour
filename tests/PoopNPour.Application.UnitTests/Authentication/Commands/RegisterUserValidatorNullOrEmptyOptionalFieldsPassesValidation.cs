using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorNullOrEmptyOptionalFieldsPassesValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_NullOrEmptyOptionalFields_PassesValidation(string optionalValue)
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "Password1!",
            FirstName: optionalValue,
            LastName: optionalValue);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }
}
