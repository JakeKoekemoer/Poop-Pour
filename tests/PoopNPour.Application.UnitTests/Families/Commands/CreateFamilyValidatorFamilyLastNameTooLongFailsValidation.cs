using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorFamilyLastNameTooLongFailsValidation
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_FamilyLastNameTooLong_FailsValidation()
    {
        var command = new CreateFamilyCommand(
            FamilyName: "Smith Family",
            FamilyLastName: new string('A', 101));

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyLastName);
    }
}
