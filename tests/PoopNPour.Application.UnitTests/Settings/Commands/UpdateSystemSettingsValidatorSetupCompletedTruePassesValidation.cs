using FluentAssertions;
using FluentValidation.TestHelper;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Commands;

public class UpdateSystemSettingsValidatorSetupCompletedTruePassesValidation
{
    private readonly UpdateSystemSettingsCommandValidator _validator = new();

    [Fact]
    public void Validate_SetupCompletedTrue_PassesValidation()
    {
        // Arrange
        var command = new UpdateSystemSettingsCommand(SetupCompleted: true);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
