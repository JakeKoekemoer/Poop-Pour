using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class UpdateFamilyValidatorEmptyFamilyIdFailsValidation
{
    private readonly UpdateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyId_FailsValidation()
    {
        var command = new UpdateFamilyCommand(
            FamilyId: Guid.Empty,
            FamilyName: "Family",
            FamilyLastName: "Name");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyId);
    }
}
