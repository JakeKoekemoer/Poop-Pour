using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class RemoveUserFromFamilyValidatorEmptyUserIdFailsValidation
{
    private readonly RemoveUserFromFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyUserId_FailsValidation()
    {
        var command = new RemoveUserFromFamilyCommand(
            FamilyId: Guid.NewGuid(),
            UserId: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}
