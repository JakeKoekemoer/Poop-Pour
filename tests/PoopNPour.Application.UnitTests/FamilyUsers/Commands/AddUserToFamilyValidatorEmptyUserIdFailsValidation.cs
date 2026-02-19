using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyValidatorEmptyUserIdFailsValidation
{
    private readonly AddUserToFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyUserId_FailsValidation()
    {
        var command = new AddUserToFamilyCommand(
            FamilyId: Guid.NewGuid(),
            UserId: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}
