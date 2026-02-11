using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Domain.Models.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateBooleanSettingWithChangedValueUpdatesAndLogs
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateBooleanSettingWithChangedValueUpdatesAndLogs()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateBooleanSetting_WithChangedValue_UpdatesAndLogs()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = false };
        bool newValue = true;

        // Act
        _sut.UpdateBooleanSetting(newValue, settings.SetupCompleted, v => settings.SetupCompleted = v, "SetupCompleted");

        // Assert
        settings.SetupCompleted.Should().BeTrue();
        _sut.HasChanges.Should().BeTrue();
        _sut.AuditMessages.Should().Contain("SetupCompleted changed to True");
    }
}
