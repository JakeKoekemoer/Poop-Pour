using Microsoft.Extensions.Logging;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Domain.Interfaces.Settings;

namespace PoopNPour.Infrastructure.Repositories.SettingsRepository;

public class SettingsRepository(
    ISettingsService settingsService,
    IUser user,
    ILogger<SettingsRepository> logger,
    IIdentityService identityService) : ISettingsRepository
{
    private readonly List<string> _auditMessages = new List<string>();

    public T LoadSetting<T>(string extraIdentifier = "") where T : ISettings, new()
    {
        return settingsService.LoadSetting<T>(extraIdentifier);
    }

    public void UpdateSetting<T>(T? newValue, T currentValue, Action<T> setter, string settingName) where T : notnull
    {
        if (newValue != null && !newValue.Equals(currentValue))
        {
            setter(newValue);
            logger.LogInformation("User {UserId} updated {Setting} to {Value}", user.Id, settingName, newValue);
            _auditMessages.Add($"{settingName} changed to {newValue}");
        }
    }

    public void UpdateNullableSetting<T>(T? newValue, T? currentValue, Action<T?> setter, string settingName) where T : struct
    {
        if (newValue.HasValue && (!currentValue.HasValue || !newValue.Value.Equals(currentValue.Value)))
        {
            setter(newValue);
            logger.LogInformation("User {UserId} updated {Setting} to {Value}", user.Id, settingName, newValue.Value);
            _auditMessages.Add($"{settingName} changed to {newValue.Value}");
        }
    }

    public void UpdateSecureSetting(string? newValue, string? currentValue, Action<string> setter, string settingName)
    {
        if (!string.IsNullOrEmpty(newValue) && newValue != currentValue)
        {
            setter(newValue);
            logger.LogInformation("User {UserId} updated {Setting}", user.Id, settingName);
            _auditMessages.Add($"{settingName} changed");
        }
    }

    public void UpdateStringSetting(string? newValue, string? currentValue, Action<string> setter, string settingName)
    {
        if (!string.IsNullOrEmpty(newValue) && newValue != currentValue)
        {
            setter(newValue);
            logger.LogInformation("User {UserId} updated {Setting} to {Value}", user.Id, settingName, newValue);
            _auditMessages.Add($"{settingName} changed to {newValue}");
        }
    }

    public void UpdateBooleanSetting(bool? newValue, bool currentValue, Action<bool> setter, string settingName)
    {
        if (newValue.HasValue && newValue.Value != currentValue)
        {
            setter(newValue.Value);
            logger.LogInformation("User {UserId} updated {Setting} to {Value}", user.Id, settingName, newValue.Value);
            _auditMessages.Add($"{settingName} changed to {newValue.Value}");
        }
    }

    public async Task SaveSettingsAsync<T>(T settings, string extraIdentifier = "", CancellationToken cancellationToken = default) where T : ISettings, new()
    {
        if (_auditMessages.Any())
        {
            string? userName = null;
            if (!string.IsNullOrEmpty(user.Id))
            {
                var userEntity = await identityService.GetUserByIdAsync(user.Id, cancellationToken);
                if (userEntity != null && (!string.IsNullOrEmpty(userEntity.FirstName) || !string.IsNullOrEmpty(userEntity.LastName)))
                {
                    userName = $"{userEntity.FirstName} {userEntity.LastName}".Trim();
                }
            }

            logger.LogInformation(
                "User {UserId} ({UserName}) updated settings: {Changes}",
                user.Id,
                userName ?? user.Id ?? "Unknown",
                string.Join(", ", _auditMessages));

            settingsService.SaveSetting(settings, extraIdentifier);
        }
    }

    public IReadOnlyList<string> AuditMessages => _auditMessages.AsReadOnly();

    public bool HasChanges => _auditMessages.Any();
}
