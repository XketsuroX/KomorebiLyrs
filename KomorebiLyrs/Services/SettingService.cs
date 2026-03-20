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
            
            // Initialize storage
            _canSaveToDisk = EnsureDirectoryExists(appFolder);

            // Load settings (use defaults if first run or load failed)
            _settings = LoadSettings() ?? CreateDefaultSettings();
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
        
        private bool EnsureDirectoryExists(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to create settings directory: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }
        
        private AppSettings CreateDefaultSettings()
        {
            var defaults = new AppSettings();
        
            if (File.Exists(_settingsFilePath))
            {
                Console.WriteLine("[Warning] Settings file exists but failed to load. Using defaults without overwriting.");
            }
            else if (!_canSaveToDisk)
            {
                Console.WriteLine("[Warning] Cannot save to disk. Using memory-only defaults.");
            }
            else
            {
                SaveSettings(defaults);
            }

            return defaults;
        }
}
