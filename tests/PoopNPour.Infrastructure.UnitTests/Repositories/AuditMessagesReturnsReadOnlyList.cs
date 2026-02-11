using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Infrastructure.Repositories.SettingsRepository;
using PoopNPour.Infrastructure.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Repositories;

public class AuditMessagesReturnsReadOnlyList
{
    private readonly ISettingsService _settingsService = Substitute.For<ISettingsService>();
    private readonly IUser _user = UserMockBuilder.CreateAuthenticated();
    private readonly ILogger<SettingsRepository> _logger = Substitute.For<ILogger<SettingsRepository>>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly SettingsRepository _sut;

    public AuditMessagesReturnsReadOnlyList()
    {
        _sut = new SettingsRepository(_settingsService, _user, _logger, _identityService);
    }

    [Fact]
    public void AuditMessages_ReturnsReadOnlyList()
    {
        // Arrange
        _sut.UpdateBooleanSetting(true, false, v => { }, "Test1");
        _sut.UpdateStringSetting("new", "old", v => { }, "Test2");

        // Act
        var messages = _sut.AuditMessages;

        // Assert
        messages.Should().HaveCount(2);
        messages.Should().Contain("Test1 changed to True");
        messages.Should().Contain("Test2 changed to new");
    }
}
