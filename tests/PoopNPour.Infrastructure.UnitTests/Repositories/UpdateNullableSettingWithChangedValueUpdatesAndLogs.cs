using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateNullableSettingWithChangedValueUpdatesAndLogs
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateNullableSettingWithChangedValueUpdatesAndLogs()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateNullableSetting_WithChangedValue_UpdatesAndLogs()
    {
        // Arrange
        int? currentValue = 5;
        int? newValue = 10;
        int? result = currentValue;

        // Act
        _sut.UpdateNullableSetting(newValue, currentValue, v => result = v, "TestInt");

        // Assert
        result.Should().Be(10);
        _sut.HasChanges.Should().BeTrue();
        _sut.AuditMessages.Should().Contain("TestInt changed to 10");
    }
}
