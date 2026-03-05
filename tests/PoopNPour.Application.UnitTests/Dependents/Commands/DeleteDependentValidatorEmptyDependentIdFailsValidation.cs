using FluentValidation.TestHelper;
using PoopNPour.Application.Dependents.Commands.DeleteDependent;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class DeleteDependentValidatorEmptyDependentIdFailsValidation
{
    private readonly DeleteDependentCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyDependentId_FailsValidation()
    {
        var command = new DeleteDependentCommand(DependentId: Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DependentId);
    }
}
