using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.DeleteFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class DeleteFamilyValidatorValidCommandPassesValidation
{
    private readonly DeleteFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new DeleteFamilyCommand(FamilyId: Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
