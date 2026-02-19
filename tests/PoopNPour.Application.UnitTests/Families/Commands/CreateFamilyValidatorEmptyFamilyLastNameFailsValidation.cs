using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorEmptyFamilyLastNameFailsValidation
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyLastName_FailsValidation()
    {
        var command = new CreateFamilyCommand(
            FamilyName: "Smith Family",
            FamilyLastName: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyLastName);
    }
}
