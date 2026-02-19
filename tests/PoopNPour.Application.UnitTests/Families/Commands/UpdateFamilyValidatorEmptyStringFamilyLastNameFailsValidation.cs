using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class UpdateFamilyValidatorEmptyStringFamilyLastNameFailsValidation
{
    private readonly UpdateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyStringFamilyLastName_FailsValidation()
    {
        var command = new UpdateFamilyCommand(
            FamilyId: Guid.NewGuid(),
            FamilyName: "Updated",
            FamilyLastName: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyLastName);
    }
}
