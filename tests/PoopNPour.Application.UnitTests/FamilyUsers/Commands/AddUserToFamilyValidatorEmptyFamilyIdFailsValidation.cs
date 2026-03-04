using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyValidatorEmptyFamilyIdFailsValidation
{
    private readonly AddUserToFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyId_FailsValidation()
    {
        var command = new AddUserToFamilyCommand(
            FamilyId: Guid.Empty,
            Email: "john.smith@example.com");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyId);
    }
}
