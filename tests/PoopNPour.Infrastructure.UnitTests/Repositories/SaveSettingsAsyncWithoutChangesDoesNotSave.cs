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

public class SaveSettingsAsyncWithoutChangesDoesNotSave
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public SaveSettingsAsyncWithoutChangesDoesNotSave()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public async Task SaveSettingsAsync_WithoutChanges_DoesNotSave()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = false };

        // Act
        await _sut.SaveSettingsAsync(settings, cancellationToken: CancellationToken.None);

        // Assert
        _settingsService.DidNotReceive().SaveSetting(Arg.Any<SystemSettings>(), Arg.Any<string>());
    }
}
