using FluentValidation.TestHelper;
using PoopNPour.Application.Authentication.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class RegisterUserValidatorUserNameTooLongFailsValidation
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_UserNameTooLong_FailsValidation()
    {
        // Arrange
        var longUserName = new string('a', 51);
        var command = new RegisterUserCommand(
            Email: "test@example.com",
            UserName: longUserName,
            Password: "Password1!",
            FirstName: "Test",
            LastName: "User");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username cannot exceed 50 characters.");
    }
}
