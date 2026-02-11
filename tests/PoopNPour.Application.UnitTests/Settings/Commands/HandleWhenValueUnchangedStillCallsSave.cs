using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using PoopNPour.Domain.Models.Settings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Commands;

public class HandleWhenValueUnchangedStillCallsSave
{
    private readonly ISettingsRepository _settingsRepository = Substitute.For<ISettingsRepository>();
    private readonly UpdateSystemSettingsCommandHandler _sut;

    public HandleWhenValueUnchangedStillCallsSave()
    {
        _sut = new UpdateSystemSettingsCommandHandler(_settingsRepository);
    }

    [Fact]
    public async Task Handle_WhenValueUnchanged_StillCallsSave()
    {
        // Arrange
        var systemSettings = new SystemSettings { SetupCompleted = true };
        _settingsRepository.LoadSetting<SystemSettings>().Returns(systemSettings);

        var command = new UpdateSystemSettingsCommand(true);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        // Even if value doesn't change, SaveSettingsAsync should be called
        // (the repository will handle whether to actually save based on HasChanges)
        await _settingsRepository.Received(1).SaveSettingsAsync(
            systemSettings,
            extraIdentifier: "",
            Arg.Any<CancellationToken>());
    }
}
