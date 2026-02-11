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

public class UpdateBooleanSettingWithSameValueDoesNotUpdate
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user;
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateBooleanSettingWithSameValueDoesNotUpdate()
    {
        _user = UserMockBuilder.CreateAuthenticated();
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateBooleanSetting_WithSameValue_DoesNotUpdate()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = true };
        bool? newValue = true;

        // Act
        _sut.UpdateBooleanSetting(newValue, settings.SetupCompleted, v => settings.SetupCompleted = v, "SetupCompleted");

        // Assert
        _sut.HasChanges.Should().BeFalse();
        _sut.AuditMessages.Should().BeEmpty();
    }
}
