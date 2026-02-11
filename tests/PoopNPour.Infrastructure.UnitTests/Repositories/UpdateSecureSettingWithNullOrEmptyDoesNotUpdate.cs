using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateSecureSettingWithNullOrEmptyDoesNotUpdate
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateSecureSettingWithNullOrEmptyDoesNotUpdate()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateSecureSetting_WithNullOrEmpty_DoesNotUpdate()
    {
        // Arrange
        string currentValue = "current";
        string? newValue = null;
        string result = currentValue;

        // Act
        _sut.UpdateSecureSetting(newValue, currentValue, v => result = v, "Password");

        // Assert
        result.Should().Be("current");
        _sut.HasChanges.Should().BeFalse();
    }
}
