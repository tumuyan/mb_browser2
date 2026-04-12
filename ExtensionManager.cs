using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class ExtensionInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public bool IsEnabled { get; set; }
        public string Path { get; set; }
        public string Id { get; set; }
        public string OptionsUrl { get; set; }
        public string PopupUrl { get; set; }
        public string MainPageUrl { get; set; }
    }

    public static class ExtensionManager
    {
        private static List<ExtensionInfo> _cachedExtensions = new List<ExtensionInfo>();
        private static Dictionary<string, string> _extensionIdMap = new Dictionary<string, string>();
        private static string _extensionsPath;

        public static string ExtensionsPath => _extensionsPath;
        public static IReadOnlyList<ExtensionInfo> CachedExtensions => _cachedExtensions.AsReadOnly();

        public static string GetExtensionsPath(string storagePath)
        {
            return Path.Combine(storagePath, "Browser2Extensions");
        }

        public static void SetExtensionsPath(string path)
        {
            _extensionsPath = path;
        }

        public static string GetRealExtensionId(string extensionPath)
        {
            if (_extensionIdMap != null && _extensionIdMap.ContainsKey(extensionPath))
            {
                return _extensionIdMap[extensionPath];
            }
            return null;
        }

        public static void SetRealExtensionId(string extensionPath, string realId)
        {
            if (_extensionIdMap == null)
            {
                _extensionIdMap = new Dictionary<string, string>();
            }
            _extensionIdMap[extensionPath] = realId;
        }

        public static async Task LoadExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile)
        {
            if (profile == null)
            {
                Debug.WriteLine("ExtensionManager: Profile is null");
                return;
            }

            string extensionsPath = GetExtensionsPath(Path.GetDirectoryName(profile.ProfilePath));
            _extensionsPath = extensionsPath;
            Debug.WriteLine("ExtensionManager: LoadExtensionsAsync started");
            Debug.WriteLine("ExtensionManager: Extensions path: " + extensionsPath);

            if (!Directory.Exists(extensionsPath))
            {
                Debug.WriteLine("ExtensionManager: Extensions directory does not exist");
                _cachedExtensions.Clear();
                _extensionIdMap.Clear();
                return;
            }

            try
            {
                _cachedExtensions.Clear();
                _extensionIdMap.Clear();

                var extensionDirs = Directory.GetDirectories(extensionsPath);
                Debug.WriteLine("ExtensionManager: Found " + extensionDirs.Length + " extension directories");

                foreach (string extensionDir in extensionDirs)
                {
                    Debug.WriteLine("ExtensionManager: Processing extension directory: " + extensionDir);

                    string manifestPath = Path.Combine(extensionDir, "manifest.json");
                    bool isDisabled = false;
                    string name = Path.GetFileName(extensionDir);
                    string version = "Unknown";
                    string optionsUrl = null;
                    string popupUrl = null;

                    if (File.Exists(manifestPath))
                    {
                        try
                        {
                            string manifestContent = File.ReadAllText(manifestPath);
                            using (var doc = JsonDocument.Parse(manifestContent))
                            {
                                var root = doc.RootElement;

                                isDisabled = root.TryGetProperty("manifest_v2_disable", out _);

                                name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : name;
                                version = root.TryGetProperty("version", out var versionProp) ? versionProp.GetString() : "Unknown";

                                if (!string.IsNullOrEmpty(name) && name.StartsWith("__MSG_") && name.EndsWith("__"))
                                {
                                    string messageKey = name.Substring(6, name.Length - 8);
                                    string localeName = GetExtensionLocalizedName(extensionDir, messageKey);
                                    if (!string.IsNullOrEmpty(localeName))
                                    {
                                        name = localeName;
                                    }
                                }

                                if (root.TryGetProperty("options_page", out var optionsPageProp))
                                {
                                    optionsUrl = optionsPageProp.GetString();
                                }
                                else if (root.TryGetProperty("options_ui", out var optionsUiProp))
                                {
                                    if (optionsUiProp.TryGetProperty("page", out var pageProp))
                                    {
                                        optionsUrl = pageProp.GetString();
                                    }
                                }

                                if (root.TryGetProperty("action", out var actionProp))
                                {
                                    if (actionProp.TryGetProperty("default_popup", out var popupProp))
                                        popupUrl = popupProp.GetString();
                                }
                                else if (root.TryGetProperty("browser_action", out var browserActionProp))
                                {
                                    if (browserActionProp.TryGetProperty("default_popup", out var popupProp))
                                        popupUrl = popupProp.GetString();
                                }
                                else if (root.TryGetProperty("page_action", out var pageActionProp))
                                {
                                    if (pageActionProp.TryGetProperty("default_popup", out var popupProp))
                                        popupUrl = popupProp.GetString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("ExtensionManager: Failed to read manifest: " + ex.Message);
                        }
                    }

                    string realId = null;
                    try
                    {
                        var extension = await profile.AddBrowserExtensionAsync(extensionDir);
                        if (extension != null)
                        {
                            await extension.EnableAsync(!isDisabled);
                            realId = extension.Id;
                            _extensionIdMap[extensionDir] = realId;
                            Debug.WriteLine($"ExtensionManager: Extension loaded - Path: {extensionDir}, RealId: {realId}, IsEnabled: {extension.IsEnabled}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ExtensionManager: Failed to load extension: " + extensionDir + " - " + ex.Message);
                    }

                    string mainPageUrl = null;
                    if (string.IsNullOrEmpty(optionsUrl) && string.IsNullOrEmpty(popupUrl))
                    {
                        mainPageUrl = FindExtensionMainPage(extensionDir);
                    }

                    _cachedExtensions.Add(new ExtensionInfo
                    {
                        Name = name,
                        Version = version,
                        IsEnabled = !isDisabled,
                        Path = extensionDir,
                        Id = realId ?? Path.GetFileName(extensionDir),
                        OptionsUrl = optionsUrl,
                        PopupUrl = popupUrl,
                        MainPageUrl = mainPageUrl
                    });
                }

                Debug.WriteLine($"ExtensionManager: Total extensions cached: {_cachedExtensions.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtensionManager: LoadExtensions error: " + ex.Message);
            }
        }

        public static async Task<ExtensionInfo[]> GetAllExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile, string extensionsPath)
        {
            if (_cachedExtensions.Count > 0)
            {
                Debug.WriteLine("ExtensionManager: Returning cached extensions");
                return _cachedExtensions.ToArray();
            }

            if (!Directory.Exists(extensionsPath))
            {
                return new ExtensionInfo[0];
            }

            _extensionsPath = extensionsPath;
            var extensions = new List<ExtensionInfo>();

            foreach (string extensionDir in Directory.GetDirectories(extensionsPath))
            {
                string manifestPath = Path.Combine(extensionDir, "manifest.json");
                if (File.Exists(manifestPath))
                {
                    try
                    {
                        string manifestJson = File.ReadAllText(manifestPath);
                        using (var doc = JsonDocument.Parse(manifestJson))
                        {
                            var root = doc.RootElement;

                            string name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : Path.GetFileName(extensionDir);
                            string version = root.TryGetProperty("version", out var versionProp) ? versionProp.GetString() : "Unknown";

                            if (!string.IsNullOrEmpty(name) && name.StartsWith("__MSG_") && name.EndsWith("__"))
                            {
                                string messageKey = name.Substring(6, name.Length - 8);
                                string localeName = GetExtensionLocalizedName(extensionDir, messageKey);
                                if (!string.IsNullOrEmpty(localeName))
                                {
                                    name = localeName;
                                }
                            }

                            bool isEnabled = !root.TryGetProperty("manifest_v2_disable", out _);

                            string optionsUrl = null;
                            string popupUrl = null;

                            if (root.TryGetProperty("options_page", out var optionsPageProp))
                            {
                                optionsUrl = optionsPageProp.GetString();
                            }
                            else if (root.TryGetProperty("options_ui", out var optionsUiProp))
                            {
                                if (optionsUiProp.TryGetProperty("page", out var pageProp))
                                {
                                    optionsUrl = pageProp.GetString();
                                }
                            }

                            if (root.TryGetProperty("action", out var actionProp))
                            {
                                if (actionProp.TryGetProperty("default_popup", out var popupProp))
                                    popupUrl = popupProp.GetString();
                            }
                            else if (root.TryGetProperty("browser_action", out var browserActionProp))
                            {
                                if (browserActionProp.TryGetProperty("default_popup", out var popupProp))
                                    popupUrl = popupProp.GetString();
                            }
                            else if (root.TryGetProperty("page_action", out var pageActionProp))
                            {
                                if (pageActionProp.TryGetProperty("default_popup", out var popupProp))
                                    popupUrl = popupProp.GetString();
                            }

                            string mainPageUrl = null;
                            if (string.IsNullOrEmpty(optionsUrl) && string.IsNullOrEmpty(popupUrl))
                            {
                                mainPageUrl = FindExtensionMainPage(extensionDir);
                            }

                            extensions.Add(new ExtensionInfo
                            {
                                Name = name,
                                Version = version,
                                IsEnabled = isEnabled,
                                Path = extensionDir,
                                Id = Path.GetFileName(extensionDir),
                                OptionsUrl = optionsUrl,
                                PopupUrl = popupUrl,
                                MainPageUrl = mainPageUrl
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ExtensionManager: Failed to load extension " + extensionDir + ": " + ex.Message);
                        extensions.Add(new ExtensionInfo
                        {
                            Name = Path.GetFileName(extensionDir),
                            Version = "Unknown",
                            IsEnabled = true,
                            Path = extensionDir,
                            Id = Path.GetFileName(extensionDir)
                        });
                    }
                }
            }

            _cachedExtensions = extensions;
            return extensions.ToArray();
        }

        public static ExtensionInfo[] GetAllExtensions(string extensionsPath)
        {
            if (_cachedExtensions.Count > 0)
            {
                return _cachedExtensions.ToArray();
            }
            return GetAllExtensionsAsync(null, extensionsPath).Result;
        }

        public static void InstallExtension(string sourcePath, string extensionsPath)
        {
            string extensionName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(extensionsPath, extensionName);

            if (Directory.Exists(destPath))
            {
                Directory.Delete(destPath, true);
            }

            CopyDirectory(sourcePath, destPath);
            InvalidateCache();
        }

        public static void UninstallExtension(string extensionPath)
        {
            if (Directory.Exists(extensionPath))
            {
                Directory.Delete(extensionPath, true);
            }
            InvalidateCache();
        }

        public static bool ToggleExtension(string extensionPath, bool enable)
        {
            try
            {
                string manifestPath = Path.Combine(extensionPath, "manifest.json");
                if (!File.Exists(manifestPath))
                {
                    Debug.WriteLine("ExtensionManager: Manifest not found: " + manifestPath);
                    return false;
                }

                string manifestJson = File.ReadAllText(manifestPath);
                string manifestCopy;

                if (enable)
                {
                    if (manifestJson.Contains("\"manifest_v2_disable\""))
                    {
                        manifestCopy = Regex.Replace(manifestJson, @",\s*""manifest_v2_disable"":\s*true\s*\}", "}");
                        if (!manifestCopy.Contains("\"manifest_v2_disable\""))
                        {
                            manifestCopy = Regex.Replace(manifestCopy, @"""manifest_v2_disable"":\s*true\s*,?\s*", "");
                            manifestCopy = Regex.Replace(manifestCopy, @",\s*}", "}");
                        }
                        else
                        {
                            manifestCopy = manifestJson;
                        }
                    }
                    else
                    {
                        manifestCopy = manifestJson;
                    }
                }
                else
                {
                    if (!manifestJson.Contains("\"manifest_v2_disable\""))
                    {
                        manifestJson = manifestJson.TrimEnd(' ', '\r', '\n');
                        if (manifestJson.EndsWith("}"))
                        {
                            manifestCopy = manifestJson.Substring(0, manifestJson.Length - 1).TrimEnd(' ', '\r', '\n') + ",\n  \"manifest_v2_disable\": true\n}";
                        }
                        else
                        {
                            manifestCopy = manifestJson + "\n}";
                        }
                    }
                    else
                    {
                        manifestCopy = manifestJson;
                    }
                }

                File.WriteAllText(manifestPath, manifestCopy);
                InvalidateCache();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtensionManager: Toggle extension error: " + ex.Message);
                return false;
            }
        }

        public static void InvalidateCache()
        {
            _cachedExtensions.Clear();
            Debug.WriteLine("ExtensionManager: Cache invalidated");
        }

        public static async Task RefreshExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile)
        {
            Debug.WriteLine("ExtensionManager: Refreshing all extensions...");
            InvalidateCache();
            await LoadExtensionsAsync(profile);
        }

        public static string FindExtensionMainPage(string extensionPath)
        {
            try
            {
                var priorityFiles = new[] {
                    "index.html",
                    "main.html",
                    "app.html",
                    "popup.html",
                    "options.html",
                    "home.html"
                };

                foreach (string fileName in priorityFiles)
                {
                    string filePath = Path.Combine(extensionPath, fileName);
                    if (File.Exists(filePath))
                    {
                        Debug.WriteLine($"ExtensionManager: Found priority file '{fileName}'");
                        return fileName;
                    }
                }

                string[] htmlFiles = Directory.GetFiles(extensionPath, "*.html", SearchOption.TopDirectoryOnly);

                if (htmlFiles.Length > 0)
                {
                    Array.Sort(htmlFiles, (a, b) =>
                    {
                        string nameA = Path.GetFileName(a).ToLower();
                        string nameB = Path.GetFileName(b).ToLower();

                        if (nameA.Contains("main") || nameA.Contains("index")) return -1;
                        if (nameB.Contains("main") || nameB.Contains("index")) return 1;

                        return string.Compare(nameA, nameB, StringComparison.OrdinalIgnoreCase);
                    });

                    string mainPage = Path.GetFileName(htmlFiles[0]);
                    Debug.WriteLine($"ExtensionManager: Found main page '{mainPage}' from {htmlFiles.Length} HTML files");
                    return mainPage;
                }

                Debug.WriteLine($"ExtensionManager: No HTML files found in {extensionPath}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ExtensionManager: FindExtensionMainPage error: {ex.Message}");
                return null;
            }
        }

        public static string GetExtensionLocalizedName(string extensionDir, string messageKey)
        {
            try
            {
                string defaultLocale = "en";
                string manifestPath = Path.Combine(extensionDir, "manifest.json");
                if (File.Exists(manifestPath))
                {
                    string manifestJson = File.ReadAllText(manifestPath);
                    using (var doc = JsonDocument.Parse(manifestJson))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("default_locale", out var localeProp))
                        {
                            defaultLocale = localeProp.GetString() ?? "en";
                        }
                    }
                }

                string[] possibleLocales = { defaultLocale, "en", "zh_CN", "zh" };

                foreach (string locale in possibleLocales)
                {
                    string localesDir = Path.Combine(extensionDir, "_locales", locale);
                    if (Directory.Exists(localesDir))
                    {
                        string messagesPath = Path.Combine(localesDir, "messages.json");
                        if (File.Exists(messagesPath))
                        {
                            string messagesJson = File.ReadAllText(messagesPath);
                            using (var doc = JsonDocument.Parse(messagesJson))
                            {
                                var root = doc.RootElement;
                                if (root.TryGetProperty(messageKey, out var messageProp))
                                {
                                    if (messageProp.TryGetProperty("message", out var messageValue))
                                    {
                                        return messageValue.GetString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return messageKey;
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destDir, fileName);
                File.Copy(file, destFile, true);
            }

            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(dir);
                string destSubDir = Path.Combine(destDir, dirName);
                CopyDirectory(dir, destSubDir);
            }
        }
    }
}
