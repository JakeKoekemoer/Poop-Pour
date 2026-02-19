using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorMultipleErrorsReturnsAllErrors
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        var command = new CreateFamilyCommand(
            FamilyName: "",
            FamilyLastName: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyName);
        result.ShouldHaveValidationErrorFor(x => x.FamilyLastName);
    }
}
