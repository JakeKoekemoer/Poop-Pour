using FluentValidation.TestHelper;
using PoopNPour.Application.Dependents.Commands.CreateDependent;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class CreateDependentValidatorValidCommandPassesValidation
{
    private readonly CreateDependentCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateDependentCommand(
            FamilyId: Guid.NewGuid(),
            DependentName: "Child",
            DependentSurname: "Doe",
            DateOfBirth: DateTimeOffset.UtcNow.AddYears(-2));

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
