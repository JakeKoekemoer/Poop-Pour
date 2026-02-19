using FluentValidation.TestHelper;
using PoopNPour.Application.MedicineLogs.Commands.CreateMedicineLog;
using Xunit;

namespace PoopNPour.Application.UnitTests.MedicineLogs.Commands;

public class CreateMedicineLogValidatorValidCommandPassesValidation
{
    private readonly CreateMedicineLogCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateMedicineLogCommand(
            DependentId: Guid.NewGuid(),
            MedicineName: "Paracetamol",
            Dosage: "5ml",
            TimeAdministered: DateTimeOffset.UtcNow);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
