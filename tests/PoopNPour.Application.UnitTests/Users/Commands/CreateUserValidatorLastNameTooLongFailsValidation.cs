using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorLastNameTooLongFailsValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_LastNameTooLong_FailsValidation()
    {
        var command = new CreateUserCommand("newuser", "user@test.com", "Password1!", "John", new string('a', 51));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 50 characters.");
    }
}
