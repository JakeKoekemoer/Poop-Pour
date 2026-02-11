using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using PoopNPour.Domain.Models.Settings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Commands;

public class HandleWithNullSetupCompletedThrowsValidationException
{
    private readonly ISettingsRepository _settingsRepository = Substitute.For<ISettingsRepository>();
    private readonly UpdateSystemSettingsCommandHandler _sut;

    public HandleWithNullSetupCompletedThrowsValidationException()
    {
        _sut = new UpdateSystemSettingsCommandHandler(_settingsRepository);
    }

    [Fact]
    public async Task Handle_WithNullSetupCompleted_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSystemSettingsCommand(null);

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().ContainKey("SetupCompleted");
        exception.Which.Errors["SetupCompleted"].Should().Contain("SetupCompleted is required.");
        _settingsRepository.DidNotReceive().LoadSetting<SystemSettings>();
    }
}
