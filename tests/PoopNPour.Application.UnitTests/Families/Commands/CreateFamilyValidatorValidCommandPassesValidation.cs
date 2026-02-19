using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyValidatorValidCommandPassesValidation
{
    private readonly CreateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateFamilyCommand(
            FamilyName: "Smith Family",
            FamilyLastName: "Smith");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
