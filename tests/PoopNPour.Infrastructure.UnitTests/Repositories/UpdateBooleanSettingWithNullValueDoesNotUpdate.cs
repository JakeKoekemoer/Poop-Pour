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

public class UpdateBooleanSettingWithNullValueDoesNotUpdate
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateBooleanSettingWithNullValueDoesNotUpdate()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateBooleanSetting_WithNullValue_DoesNotUpdate()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = false };
        bool? newValue = null;

        // Act
        _sut.UpdateBooleanSetting(newValue, settings.SetupCompleted, v => settings.SetupCompleted = v, "SetupCompleted");

        // Assert
        settings.SetupCompleted.Should().BeFalse();
        _sut.HasChanges.Should().BeFalse();
    }
}
