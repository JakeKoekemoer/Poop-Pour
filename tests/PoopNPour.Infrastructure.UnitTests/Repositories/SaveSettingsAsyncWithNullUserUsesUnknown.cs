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

public class SaveSettingsAsyncWithNullUserUsesUnknown
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();

    [Fact]
    public async Task SaveSettingsAsync_WithNullUser_UsesUnknown()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = true };
        var unauthenticatedUser = UserMockBuilder.CreateUnauthenticated();
        var sutWithNoUser = new SettingsRepository(_settingsService, unauthenticatedUser, _logger, _identityService);
        
        sutWithNoUser.UpdateBooleanSetting(true, false, v => settings.SetupCompleted = v, "SetupCompleted");

        // Act
        await sutWithNoUser.SaveSettingsAsync(settings, cancellationToken: CancellationToken.None);

        // Assert
        _settingsService.Received(1).SaveSetting(settings, "");
    }
}
