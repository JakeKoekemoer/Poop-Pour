using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateUserValidatorFirstNameTooLongFailsValidation
{
    private readonly UpdateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_FirstNameTooLong_FailsValidation()
    {
        var command = new UpdateUserCommand("user-123", FirstName: new string('a', 51));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot exceed 50 characters.");
    }
}
