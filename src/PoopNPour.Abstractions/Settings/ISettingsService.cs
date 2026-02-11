namespace PoopNPour.Abstractions.Settings;

public interface ISettingsService
{
    /// <summary>
    /// Loads a setting class from the database
    /// </summary>
    /// <typeparam name="T">The type of settings to load (must implement ISettings)</typeparam>
    /// <param name="extraIdentifier">Optional extra identifier for the setting key</param>
    /// <returns>The settings object with values loaded from the database</returns>
    T LoadSetting<T>(string extraIdentifier = "") where T : Domain.Interfaces.Settings.ISettings, new();

    /// <summary>
    /// Saves a setting class to the database
    /// </summary>
    /// <typeparam name="T">The type of settings to save (must implement ISettings)</typeparam>
    /// <param name="settings">The settings object to save</param>
    /// <param name="extraIdentifier">Optional extra identifier for the setting key</param>
    void SaveSetting<T>(T settings, string extraIdentifier = "") where T : Domain.Interfaces.Settings.ISettings, new();

    /// <summary>
    /// Deletes all settings of a given type from the database
    /// </summary>
    /// <typeparam name="T">The type of settings to delete (must implement ISettings)</typeparam>
    /// <param name="extraIdentifier">Optional extra identifier for the setting key</param>
    void DeleteSetting<T>(string extraIdentifier = "") where T : Domain.Interfaces.Settings.ISettings, new();
}
