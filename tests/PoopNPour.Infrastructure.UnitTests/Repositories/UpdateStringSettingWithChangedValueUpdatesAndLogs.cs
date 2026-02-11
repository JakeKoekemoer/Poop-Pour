using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateStringSettingWithChangedValueUpdatesAndLogs
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateStringSettingWithChangedValueUpdatesAndLogs()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateStringSetting_WithChangedValue_UpdatesAndLogs()
    {
        // Arrange
        string currentValue = "old";
        string newValue = "new";
        string result = currentValue;

        // Act
        _sut.UpdateStringSetting(newValue, currentValue, v => result = v, "TestSetting");

        // Assert
        result.Should().Be("new");
        _sut.HasChanges.Should().BeTrue();
        _sut.AuditMessages.Should().Contain("TestSetting changed to new");
    }
}
