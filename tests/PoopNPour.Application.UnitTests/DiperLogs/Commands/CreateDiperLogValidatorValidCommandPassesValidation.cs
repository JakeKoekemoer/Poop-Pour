using FluentValidation.TestHelper;
using PoopNPour.Application.DiperLogs.Commands.CreateDiperLog;
using PoopNPour.Domain.Enums.DiperLog;
using Xunit;

namespace PoopNPour.Application.UnitTests.DiperLogs.Commands;

public class CreateDiperLogValidatorValidCommandPassesValidation
{
    private readonly CreateDiperLogCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateDiperLogCommand(
            DependentId: Guid.NewGuid(),
            DiperDate: DateTimeOffset.UtcNow,
            FecalDischargeColour: FecalDischargeColour.BROWN,
            UrinaryDischargeColour: UrinalDischargeColour.YELLOW);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
