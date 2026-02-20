using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateUserValidatorInvalidEmailFormatFailsValidation
{
    private readonly UpdateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test.example.com")]
    public void Validate_InvalidEmailFormat_FailsValidation(string email)
    {
        var command = new UpdateUserCommand("user-123", Email: email);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }
}
