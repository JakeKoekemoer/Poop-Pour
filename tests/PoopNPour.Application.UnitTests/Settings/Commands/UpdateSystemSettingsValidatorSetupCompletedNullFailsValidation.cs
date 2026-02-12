using FluentValidation.TestHelper;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Commands;

public class UpdateSystemSettingsValidatorSetupCompletedNullFailsValidation
{
    private readonly UpdateSystemSettingsCommandValidator _validator = new();

    [Fact]
    public void Validate_SetupCompletedNull_FailsValidation()
    {
        // Arrange
        var command = new UpdateSystemSettingsCommand(SetupCompleted: null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SetupCompleted)
            .WithErrorMessage("SetupCompleted is required.");
    }
}
