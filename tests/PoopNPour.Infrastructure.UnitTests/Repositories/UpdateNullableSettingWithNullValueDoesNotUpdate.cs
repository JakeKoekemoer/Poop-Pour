using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class UpdateNullableSettingWithNullValueDoesNotUpdate
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public UpdateNullableSettingWithNullValueDoesNotUpdate()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void UpdateNullableSetting_WithNullValue_DoesNotUpdate()
    {
        // Arrange
        int? currentValue = 5;
        int? newValue = null;
        int? result = currentValue;

        // Act
        _sut.UpdateNullableSetting(newValue, currentValue, v => result = v, "TestInt");

        // Assert
        result.Should().Be(5);
        _sut.HasChanges.Should().BeFalse();
    }
}
