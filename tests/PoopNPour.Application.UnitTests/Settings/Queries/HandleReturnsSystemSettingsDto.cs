using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Settings.Models;
using PoopNPour.Application.Settings.Queries.GetSystemSettings;
using PoopNPour.Domain.Models.Settings;
using Xunit;

namespace PoopNPour.Application.UnitTests.Settings.Queries;

public class HandleReturnsSystemSettingsDto
{
    private readonly ISettingsRepository _settingsRepository = Substitute.For<ISettingsRepository>();
    private readonly GetSystemSettingsQueryHandler _sut;

    public HandleReturnsSystemSettingsDto()
    {
        _sut = new GetSystemSettingsQueryHandler(_settingsRepository);
    }

    [Fact]
    public async Task Handle_ReturnsSystemSettingsDto()
    {
        // Arrange
        var systemSettings = new SystemSettings { SetupCompleted = true };
        _settingsRepository.LoadSetting<SystemSettings>().Returns(systemSettings);

        var query = new GetSystemSettingsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SystemSettingsDto>();
        result.SetupCompleted.Should().Be(systemSettings.SetupCompleted);
        _settingsRepository.Received(1).LoadSetting<SystemSettings>();
    }
}
