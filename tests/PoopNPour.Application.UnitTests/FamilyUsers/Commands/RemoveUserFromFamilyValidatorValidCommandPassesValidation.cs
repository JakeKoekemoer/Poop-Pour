using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class RemoveUserFromFamilyValidatorValidCommandPassesValidation
{
    private readonly RemoveUserFromFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new RemoveUserFromFamilyCommand(
            FamilyId: Guid.NewGuid(),
            UserId: Guid.NewGuid().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
