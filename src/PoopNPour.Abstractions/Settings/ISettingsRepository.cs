using PoopNPour.Domain.Interfaces.Settings;

namespace PoopNPour.Abstractions.Settings;

/// <summary>
/// Repository for managing settings with audit logging and change tracking
/// </summary>
public interface ISettingsRepository
{
    /// <summary>
    /// Loads a setting class from the database
    /// </summary>
    /// <typeparam name="T">The type of settings to load (must implement ISettings)</typeparam>
    /// <param name="extraIdentifier">Optional extra identifier for the setting key</param>
    /// <returns>The settings object with values loaded from the database</returns>
    T LoadSetting<T>(string extraIdentifier = "") where T : ISettings, new();

    /// <summary>
    /// Updates a setting value if the new value is different from the current value.
    /// Logs the change and adds it to the audit trail.
    /// </summary>
    /// <typeparam name="T">The type of the setting value</typeparam>
    /// <param name="newValue">The new value to set</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="setter">Action to set the new value</param>
    /// <param name="settingName">Name of the setting for logging and audit</param>
    void UpdateSetting<T>(T? newValue, T currentValue, Action<T> setter, string settingName) where T : notnull;

    /// <summary>
    /// Updates a nullable setting value if the new value is different from the current value.
    /// Logs the change and adds it to the audit trail.
    /// </summary>
    /// <typeparam name="T">The type of the setting value</typeparam>
    /// <param name="newValue">The new value to set</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="setter">Action to set the new value</param>
    /// <param name="settingName">Name of the setting for logging and audit</param>
    void UpdateNullableSetting<T>(T? newValue, T? currentValue, Action<T?> setter, string settingName) where T : struct;

    /// <summary>
    /// Updates a secure setting (like passwords, API keys) if the new value is different from the current value.
    /// Logs the change but doesn't log the actual value for security.
    /// </summary>
    /// <param name="newValue">The new value to set</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="setter">Action to set the new value</param>
    /// <param name="settingName">Name of the setting for logging and audit</param>
    void UpdateSecureSetting(string? newValue, string? currentValue, Action<string> setter, string settingName);

    /// <summary>
    /// Updates a string setting if the new value is different from the current value.
    /// Logs the change and adds it to the audit trail.
    /// </summary>
    /// <param name="newValue">The new value to set</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="setter">Action to set the new value</param>
    /// <param name="settingName">Name of the setting for logging and audit</param>
    void UpdateStringSetting(string? newValue, string? currentValue, Action<string> setter, string settingName);

    /// <summary>
    /// Updates a boolean setting if the new value is different from the current value.
    /// Logs the change and adds it to the audit trail.
    /// </summary>
    /// <param name="newValue">The new value to set</param>
    /// <param name="currentValue">The current value</param>
    /// <param name="setter">Action to set the new value</param>
    /// <param name="settingName">Name of the setting for logging and audit</param>
    void UpdateBooleanSetting(bool? newValue, bool currentValue, Action<bool> setter, string settingName);

    /// <summary>
    /// Saves the settings and logs audit messages if any changes were made.
    /// </summary>
    /// <typeparam name="T">The type of settings to save</typeparam>
    /// <param name="settings">The settings object to save</param>
    /// <param name="extraIdentifier">Optional extra identifier for the setting key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    Task SaveSettingsAsync<T>(T settings, string extraIdentifier = "", CancellationToken cancellationToken = default) where T : ISettings, new();

    /// <summary>
    /// Gets the current audit messages for this update session.
    /// </summary>
    IReadOnlyList<string> AuditMessages { get; }

    /// <summary>
    /// Checks if any changes were made during this update session.
    /// </summary>
    bool HasChanges { get; }
}
