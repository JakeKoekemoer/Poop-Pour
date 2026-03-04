using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyValidatorInvalidEmailFormatFailsValidation
{
    private readonly AddUserToFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_InvalidEmailFormat_FailsValidation()
    {
        var command = new AddUserToFamilyCommand(
            FamilyId: Guid.NewGuid(),
            Email: "not-an-email");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
