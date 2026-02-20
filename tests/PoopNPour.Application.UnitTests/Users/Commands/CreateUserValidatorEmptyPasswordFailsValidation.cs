using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorEmptyPasswordFailsValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyPassword_FailsValidation(string password)
    {
        var command = new CreateUserCommand("newuser", "user@test.com", password);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }
}
