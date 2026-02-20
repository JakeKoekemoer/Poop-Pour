using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorPasswordTooShortFailsValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("Pass1!")]
    [InlineData("Abc123!")]
    public void Validate_PasswordTooShort_FailsValidation(string password)
    {
        var command = new CreateUserCommand("newuser", "user@test.com", password);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters.");
    }
}
