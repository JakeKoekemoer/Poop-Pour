using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateUserValidatorLastNameTooLongFailsValidation
{
    private readonly UpdateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_LastNameTooLong_FailsValidation()
    {
        var command = new UpdateUserCommand("user-123", LastName: new string('a', 51));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 50 characters.");
    }
}
