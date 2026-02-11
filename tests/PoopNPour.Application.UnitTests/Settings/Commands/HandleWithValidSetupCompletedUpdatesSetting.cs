using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using PoopNPour.Domain.Models.Settings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Commands;

public class HandleWithValidSetupCompletedUpdatesSetting
{
    private readonly ISettingsRepository _settingsRepository = Substitute.For<ISettingsRepository>();
    private readonly UpdateSystemSettingsCommandHandler _sut;

    public HandleWithValidSetupCompletedUpdatesSetting()
    {
        _sut = new UpdateSystemSettingsCommandHandler(_settingsRepository);
    }

    [Fact]
    public async Task Handle_WithValidSetupCompleted_UpdatesSetting()
    {
        // Arrange
        var systemSettings = new SystemSettings { SetupCompleted = false };
        _settingsRepository.LoadSetting<SystemSettings>().Returns(systemSettings);

        var command = new UpdateSystemSettingsCommand(true);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _settingsRepository.Received(1).LoadSetting<SystemSettings>();
        _settingsRepository.Received(1).UpdateBooleanSetting(
            true,
            false,
            Arg.Any<Action<bool>>(),
            "SetupCompleted");
        await _settingsRepository.Received(1).SaveSettingsAsync(
            systemSettings,
            extraIdentifier: "",
            Arg.Any<CancellationToken>());
    }
}
