using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class RemoveUserFromFamilyValidatorEmptyFamilyIdFailsValidation
{
    private readonly RemoveUserFromFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyId_FailsValidation()
    {
        var command = new RemoveUserFromFamilyCommand(
            FamilyId: Guid.Empty,
            UserId: Guid.NewGuid().ToString());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyId);
    }
}
