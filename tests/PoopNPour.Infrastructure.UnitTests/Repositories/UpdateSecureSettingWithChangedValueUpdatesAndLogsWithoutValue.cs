using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateSecureSettingWithChangedValueUpdatesAndLogsWithoutValue
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateSecureSettingWithChangedValueUpdatesAndLogsWithoutValue()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateSecureSetting_WithChangedValue_UpdatesAndLogsWithoutValue()
    {
        // Arrange
        string currentValue = "old-secret";
        string newValue = "new-secret";
        string result = currentValue;

        // Act
        _sut.UpdateSecureSetting(newValue, currentValue, v => result = v, "Password");

        // Assert
        result.Should().Be("new-secret");
        _sut.HasChanges.Should().BeTrue();
        _sut.AuditMessages.Should().Contain("Password changed");
        _sut.AuditMessages.Should().NotContain(m => m.Contains("new-secret"));
    }
}
