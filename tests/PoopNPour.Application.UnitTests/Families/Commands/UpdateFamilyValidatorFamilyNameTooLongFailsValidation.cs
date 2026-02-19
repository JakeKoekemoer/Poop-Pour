using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class UpdateFamilyValidatorFamilyNameTooLongFailsValidation
{
    private readonly UpdateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_FamilyNameTooLong_FailsValidation()
    {
        var command = new UpdateFamilyCommand(
            FamilyId: Guid.NewGuid(),
            FamilyName: new string('A', 101),
            FamilyLastName: "Updated");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyName);
    }
}
