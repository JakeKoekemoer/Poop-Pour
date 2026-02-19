using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class UpdateFamilyValidatorNullFamilyNamePassesValidation
{
    private readonly UpdateFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_NullFamilyName_PassesValidation()
    {
        var command = new UpdateFamilyCommand(
            FamilyId: Guid.NewGuid(),
            FamilyName: null,
            FamilyLastName: "Updated");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
