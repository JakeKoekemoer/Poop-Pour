using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorFamilyNameTooLongFailsValidation
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_FamilyNameTooLong_FailsValidation()
    {
        var command = new CreateFamilyCommand(
            FamilyName: new string('A', 101),
            FamilyLastName: "Smith");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyName);
    }
}
