using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Models.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class SaveSettingsAsyncWithUserWithoutNameUsesUserId
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user;
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public SaveSettingsAsyncWithUserWithoutNameUsesUserId()
    {
        _user = Substitute.For<IUser>();
        _user.Id.Returns("user-123");
        _user.UserName.Returns("testuser");
        _user.Email.Returns("test@test.com");
        _user.IsAuthenticated.Returns(true);
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public async Task SaveSettingsAsync_WithUserWithoutName_UsesUserId()
    {
        // Arrange
        var settings = new SystemSettings { SetupCompleted = true };
        _sut.UpdateBooleanSetting(true, false, v => settings.SetupCompleted = v, "SetupCompleted");
        
        var userEntity = new ApplicationUser { Id = "user-123" };
        _identityService.GetUserByIdAsync("user-123", Arg.Any<CancellationToken>())
            .Returns(userEntity);

        // Act
        await _sut.SaveSettingsAsync(settings, cancellationToken: CancellationToken.None);

        // Assert
        _settingsService.Received(1).SaveSetting(settings, "");
    }
}
