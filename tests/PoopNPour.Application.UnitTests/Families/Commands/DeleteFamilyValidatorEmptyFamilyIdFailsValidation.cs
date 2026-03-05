using FluentValidation.TestHelper;
using PoopNPour.Application.Families.Commands.DeleteFamily;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class DeleteFamilyValidatorEmptyFamilyIdFailsValidation
{
    private readonly DeleteFamilyCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyFamilyId_FailsValidation()
    {
        var command = new DeleteFamilyCommand(FamilyId: Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FamilyId);
    }
}
