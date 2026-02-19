using FluentValidation.TestHelper;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyValidatorMultipleErrorsReturnsAllErrors
{
    private readonly AddUserToFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        var command = new AddUserToFamilyCommand(
            FamilyId: Guid.Empty,
            UserId: "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyId);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}
