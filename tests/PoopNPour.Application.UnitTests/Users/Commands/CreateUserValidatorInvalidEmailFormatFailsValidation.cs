using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorInvalidEmailFormatFailsValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test")]
    [InlineData("test.example.com")]
    public void Validate_InvalidEmailFormat_FailsValidation(string email)
    {
        var command = new CreateUserCommand("newuser", email, "Password1!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }
}
