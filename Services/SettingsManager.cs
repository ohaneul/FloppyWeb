using System;
using System.Collections.Generic;
using System.IO;

public class SettingsManager
{
    private Dictionary<string, object> settings;
    private readonly string settingsPath;

    public SettingsManager()
    {
        settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FloppyWeb",
            "settings.json"
        );
        LoadSettings();
    }

    public T GetSetting<T>(string key, T defaultValue)
    {
        if (settings.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        return defaultValue;
    }
} 