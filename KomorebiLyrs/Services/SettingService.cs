using System;
using System.IO;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class SettingService
{
        private AppSettings _settings;

        private readonly string _settingsFilePath;
        
        private readonly bool _canSaveToDisk;
        
        public event EventHandler<AppSettings>? SettingsChanged;
    
        public SettingService()
        {
            // Path to the settings file in the user's AppData folder
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "KomorebiLyrs");
            _settingsFilePath = Path.Combine(appFolder, "settings.json");
            
            // Ensure the directory exists (create it if this is the first run)
            try
            {
                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }
                _canSaveToDisk = true;
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
            {
                Console.WriteLine($"[Error] Failed to create settings directory: {ex.Message}");
                _canSaveToDisk = false;
            }

            // Load settings (use defaults if first run or load failed)
            var loadedSettings = LoadSettings();
            if (loadedSettings != null)
            {
                _settings = loadedSettings;
            }
            else
            {
                _settings = new AppSettings();
                
                if (_canSaveToDisk && !File.Exists(_settingsFilePath))
                {
                    // If it's the first run, save a default settings file
                    SaveSettings(_settings);
                }
                else
                {
                    Console.WriteLine(File.Exists(_settingsFilePath)
                        ? "[Warning] Settings file exists but failed to load. Using defaults without overwriting."
                        : "[Warning] Cannot save to disk. Using memory-only defaults.");
                }
            }
        }
    
        public AppSettings GetSettings()
        {
            // Return a shallow copy to prevent external mutations
            return _settings with { };
        }
    
        public void UpdateSettings(AppSettings newSettings)
        {
            _settings = newSettings;
            SaveSettings(_settings);
            SettingsChanged?.Invoke(this, newSettings);
        }
    
        private AppSettings? LoadSettings()
        {
            // Implement loading logic (e.g., from JSON file)
            // Return null if no settings file exists
            return null;
        }
    
        private void SaveSettings(AppSettings settings)
        {
            // Implement saving logic (e.g., to JSON file)
        }
}
