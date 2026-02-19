using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorEmptyFamilyNameFailsValidation
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyName_FailsValidation()
    {
        var command = new CreateFamilyCommand(
            FamilyName: "",
            FamilyLastName: "Smith");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyName);
    }
}
