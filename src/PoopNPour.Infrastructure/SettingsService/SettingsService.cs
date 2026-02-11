using System.ComponentModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Domain.Entities;
using PoopNPour.Domain.Interfaces.Settings;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.SettingsService;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;

    public SettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Loads a setting class from the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="extraIdentifier"></param>
    /// <returns></returns>
    public T LoadSetting<T>(string extraIdentifier = "") where T : ISettings, new()
    {
        //Create the setting object
        var settings = Activator.CreateInstance<T>();

        //Build the settings object
        foreach (var prop in typeof(T).GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            //The key to the property
            var key = typeof(T).Name + "." + prop.Name;
            if (!string.IsNullOrEmpty(extraIdentifier)) key += "." + extraIdentifier;

            //Get the setting
            var setting = GetSettingByKey(key);

            if (Attribute.IsDefined(prop, typeof(SettingComplexType)))
            {
                if (setting?.Value == null)
                {
                    var conStr = prop.PropertyType.GetConstructor(Type.EmptyTypes);
                    if (conStr != null)
                    {
                        var instance = conStr.Invoke(Array.Empty<object>());
                        prop.SetValue(settings, instance, null);
                    }
                    else prop.SetValue(settings, null, null);
                }
                else
                {
                    try
                    {
                        var value = JsonSerializer.Deserialize(setting.Value, prop.PropertyType);
                        if (value != null)
                        {
                            prop.SetValue(settings, value);
                        }
                        else
                        {
                            var instance = Activator.CreateInstance(prop.PropertyType);
                            prop.SetValue(settings, instance);
                        }
                    }
                    catch
                    {
                        var instance = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(settings, instance);
                    }
                }
            }
            else if (setting != null)
            {
                //Get the type converter
                var converter = TypeDescriptor.GetConverter(prop.PropertyType);

                //Check if the type can be converted from a string
                if (!converter.CanConvertFrom(typeof(string))) continue;

                //Check if the setting value can be converted into the target type
                if (!converter.IsValid(setting.Value)) continue;

                //Get the value of the property
                var value = converter.ConvertFromInvariantString(setting.Value);

                //Set the property
                prop.SetValue(settings, value);
            }
        }

        //Return the settings object
        return settings;
    }

    /// <summary>
    /// Save a setting to the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="settings"></param>
    /// <param name="extraIdentifier"></param>
    public void SaveSetting<T>(T settings, string extraIdentifier = "") where T : ISettings, new()
    {
        foreach (var prop in typeof(T).GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            //The key to the property
            var key = typeof(T).Name + "." + prop.Name;
            if (!string.IsNullOrEmpty(extraIdentifier)) key += "." + extraIdentifier;

            //Get the value
            var value = prop.GetValue(settings, null);

            //Set the setting
            if (Attribute.IsDefined(prop, typeof(SettingComplexType)))
            {
                if (value == null) SetSetting(key, string.Empty);
                else SetComplexSetting(key, value);
            }
            else
            {
                SetSetting(key, value?.ToString() ?? string.Empty);
            }
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes all settings from the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void DeleteSetting<T>(string extraIdentifier = "") where T : ISettings, new()
    {
        var toDelete = new List<string>();
        foreach (var prop in typeof(T).GetProperties())
        {
            var key = typeof(T).Name + "." + prop.Name;
            if (!string.IsNullOrEmpty(extraIdentifier)) key += "." + extraIdentifier;
            toDelete.Add(key);
        }

        var settings = _context.Settings.Where(s => toDelete.Contains(s.Key));
        _context.Settings.RemoveRange(settings);
        _context.SaveChanges();
    }

    #region Private Methods

    /// <summary>
    /// Get all settings from the repository
    /// </summary>
    /// <returns>A Dictionary of Setting objects</returns>
    private IDictionary<string, Setting> GetAllSettings()
    {
        var result = _context.Settings.ToList();
        var dict = new Dictionary<string, Setting>();
        foreach (var res in result)
        {
            var lKey = res.Key.ToLower();
            dict[lKey] = res;
        }

        return dict;
    }

    /// <summary>
    /// Get a setting by it key
    /// </summary>
    /// <param name="key">The key to match</param>
    /// <returns>A Setting object or null</returns>
    private Setting? GetSettingByKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

        var k = key.ToLower();
        var settings = GetAllSettings();

        return settings.TryGetValue(k, out var setting) ? setting : null;
    }

    /// <summary>
    /// Checks if a setting exists
    /// </summary>
    /// <param name="key">The key to match</param>
    /// <returns>A Setting object or null</returns>
    private bool SettingExists(string key) => GetSettingByKey(key) != null;

    /// <summary>
    /// Inserts a new setting in the database
    /// </summary>
    /// <param name="setting">A Setting object to insert</param>
    private void InsertSetting(Setting setting)
    {
        if (setting == null) throw new ArgumentNullException(nameof(setting));

        //Insert into database
        _context.Settings.Add(setting);
        _context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing setting
    /// </summary>
    /// <param name="setting"></param>
    private void UpdateSetting(Setting setting)
    {
        if (setting == null) throw new ArgumentNullException(nameof(setting));

        //Check if the setting belongs to this context
        if (!_context.Settings.Any(s => s.SettingId == setting.SettingId))
            return;

        //Update the database
        _context.Settings.Update(setting);
    }

    /// <summary>
    /// Insert or update a setting
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void SetSetting(string key, string value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        //Always store lowercase keys
        key = key.Trim().ToLowerInvariant();

        //Update an existing setting
        if (SettingExists(key))
        {
            var setting = GetSettingByKey(key);
            if (setting == null) return;
            setting.Value = value;
            setting.ModifiedOnUtc = DateTime.UtcNow;
            UpdateSetting(setting);
        }
        //Insert a new setting
        else
        {
            InsertSetting(new Setting
            {
                Key = key,
                Value = value
            });
        }
    }

    private void SetComplexSetting(string key, object value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        //Always store lowercase keys
        key = key.Trim().ToLowerInvariant();

        // Serialize the object
        var saveValue = JsonSerializer.Serialize(value);

        //Update an existing setting
        if (SettingExists(key))
        {
            var setting = GetSettingByKey(key);
            if (setting == null) return;
            setting.Value = saveValue;
            setting.ModifiedOnUtc = DateTime.UtcNow;
            UpdateSetting(setting);
        }
        //Insert a new setting
        else
        {
            InsertSetting(new Setting
            {
                Key = key,
                Value = saveValue
            });
        }
    }

    #endregion
}
