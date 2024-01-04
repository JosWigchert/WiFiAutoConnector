using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace WiFiAutoConnector
{
    public class Settings : INotifyPropertyChanged
    {
        private static Settings _instance;

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadSettingsFromFile();
                }
                return _instance;
            }
        }

        private Settings()
        {
            // Private constructor to prevent external instantiation
        }

        private bool _isAutoconnect;

        public bool IsAutoconnect
        {
            get { return _isAutoconnect; }
            set { _isAutoconnect = value; OnPropertyChanged(nameof(IsAutoconnect)); }
        }

        private string _target;

        public string Target
        {
            get { return _target; }
            set { _target = value; OnPropertyChanged(nameof(Target)); }
        }

        // Event to notify property changes
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Write settings to file on property change
            SaveSettingsToFile();
        }

        // Save settings to a JSON file
        private void SaveSettingsToFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                string filePath = GetSettingsFilePath();
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings to file: {ex.Message}");
            }
        }

        // Load settings from a JSON file
        private static Settings LoadSettingsFromFile()
        {
            try
            {
                string filePath = GetSettingsFilePath();
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings from file: {ex.Message}");
            }

            return new Settings(); // Return default settings if file doesn't exist or there's an error
        }

        // Get the file path for settings
        private static string GetSettingsFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        }
    }
}
