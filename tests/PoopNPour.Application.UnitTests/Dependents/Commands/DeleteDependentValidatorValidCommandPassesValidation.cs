using FluentValidation.TestHelper;
using PoopNPour.Application.Dependents.Commands.DeleteDependent;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class DeleteDependentValidatorValidCommandPassesValidation
{
    private readonly DeleteDependentCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new DeleteDependentCommand(DependentId: Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
