using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorUserNameTooShortFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData("ab")] // 2 chars
    [InlineData("a")]  // 1 char
    public void Validate_UserNameTooShort_FailsValidation(string userName)
    {
        // Arrange
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: userName,
            Password: "Password1!",
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username must be at least 3 characters long.");
    }
}
