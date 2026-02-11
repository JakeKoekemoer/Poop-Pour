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

public class LoadSettingDelegatesToSettingsService
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public LoadSettingDelegatesToSettingsService()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void LoadSetting_DelegatesToSettingsService()
    {
        // Arrange
        var expectedSettings = new SystemSettings { SetupCompleted = true };
        _settingsService.LoadSetting<SystemSettings>().Returns(expectedSettings);

        // Act
        var result = _sut.LoadSetting<SystemSettings>();

        // Assert
        result.Should().BeSameAs(expectedSettings);
        _settingsService.Received(1).LoadSetting<SystemSettings>();
    }
}
