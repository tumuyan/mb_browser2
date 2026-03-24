using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MusicBeePlugin
{
    public class Bookmark
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public Bookmark() { }

        public Bookmark(string url, string name)
        {
            Url = url;
            Name = name;
        }
    }

    public enum DarkModeType
    {
        Default = 0,
        Dark = 1,
        MusicBeeTheme = 2
    }

    public class BrowserSettings
    {
        public int Version { get; set; } = 7;
        public string DefaultUrl { get; set; }
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public double ZoomFactor { get; set; } = 1.0;
        public bool AutoSaveZoom { get; set; } = true;
        public bool ShowAddressBar { get; set; } = true;
        public DarkModeType DarkMode { get; set; } = DarkModeType.Default;
    }

    public static class SettingsManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T Load<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath))
            {
                return new T();
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<T>(json, JsonOptions);
                return result != null ? result : new T();
            }
            catch
            {
                return new T();
            }
        }

        public static void Save<T>(string filePath, T settings)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(settings, JsonOptions);
                File.WriteAllText(filePath, json);
            }
            catch
            {
            }
        }

    }
}
