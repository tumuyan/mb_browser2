using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    /// <summary>
    /// 扩展信息类
    /// </summary>
    public class ExtensionInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public bool IsEnabled { get; set; }
        public string Path { get; set; }
        public string Id { get; set; }
    }

    /// <summary>
    /// 扩展管理器 - 提供扩展安装、卸载、启用、禁用等功能
    /// </summary>
    public static class ExtensionManager
    {
        /// <summary>
        /// 获取扩展目录路径
        /// </summary>
        public static string GetExtensionsPath(string storagePath)
        {
            return Path.Combine(storagePath, "Browser2Extensions");
        }

        /// <summary>
        /// 加载所有扩展
        /// </summary>
        public static async Task LoadExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile)
        {
            if (profile == null)
            {
                Debug.WriteLine("ExtensionManager: Profile is null");
                return;
            }

            string extensionsPath = GetExtensionsPath(Path.GetDirectoryName(profile.ProfilePath));
            Debug.WriteLine("ExtensionManager: LoadExtensionsAsync started");
            Debug.WriteLine("ExtensionManager: Extensions path: " + extensionsPath);

            if (!Directory.Exists(extensionsPath))
            {
                Debug.WriteLine("ExtensionManager: Extensions directory does not exist");
                return;
            }

            try
            {
                var extensionDirs = Directory.GetDirectories(extensionsPath);
                Debug.WriteLine("ExtensionManager: Found " + extensionDirs.Length + " extension directories");

                foreach (string extensionDir in extensionDirs)
                {
                    Debug.WriteLine("ExtensionManager: Processing extension directory: " + extensionDir);

                    string manifestPath = Path.Combine(extensionDir, "manifest.json");
                    Debug.WriteLine("ExtensionManager: Manifest path: " + manifestPath);
                    Debug.WriteLine("ExtensionManager: Manifest exists: " + File.Exists(manifestPath));

                    bool isDisabled = false;
                    if (File.Exists(manifestPath))
                    {
                        try
                        {
                            string manifestContent = File.ReadAllText(manifestPath);
                            Debug.WriteLine("ExtensionManager: Loading extension for WebView2: " + extensionDir);
                            Debug.WriteLine("ExtensionManager: Manifest content (first 500 chars): " + manifestContent.Substring(0, Math.Min(500, manifestContent.Length)));

                            using (var doc = JsonDocument.Parse(manifestContent))
                            {
                                var root = doc.RootElement;
                                isDisabled = root.TryGetProperty("manifest_v2_disable", out var disableProp);
                                Debug.WriteLine("ExtensionManager: Has manifest_v2_disable: " + isDisabled);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("ExtensionManager: Failed to read manifest: " + ex.Message);
                        }
                    }

                    try
                    {
                        // 使用 WebView2 官方 API 加载扩展，返回 CoreWebView2Extension 对象
                        var extension = await profile.AddBrowserExtensionAsync(extensionDir);
                        Debug.WriteLine("ExtensionManager: Extension loaded successfully: " + extensionDir);
                        Debug.WriteLine("ExtensionManager: Extension Id: " + extension?.Id);
                        Debug.WriteLine("ExtensionManager: Extension Name: " + extension?.Name);
                        
                        // 根据 manifest 中的禁用状态调用 EnableAsync 方法
                        if (extension != null)
                        {
                            await extension.EnableAsync(!isDisabled);
                            Debug.WriteLine("ExtensionManager: Extension " + (isDisabled ? "disabled" : "enabled") + " via EnableAsync: " + extension.Id);
                            Debug.WriteLine("ExtensionManager: Extension IsEnabled property: " + extension.IsEnabled);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ExtensionManager: Failed to load extension: " + extensionDir + " - " + ex.Message);
                        Debug.WriteLine("ExtensionManager: Exception type: " + ex.GetType().FullName);
                        Debug.WriteLine("ExtensionManager: Stack trace: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtensionManager: LoadExtensions error: " + ex.Message);
                Debug.WriteLine("ExtensionManager: Exception type: " + ex.GetType().FullName);
                Debug.WriteLine("ExtensionManager: Stack trace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取所有扩展信息列表（从 WebView2 获取实时状态）
        /// </summary>
        public static async Task<ExtensionInfo[]> GetAllExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile, string extensionsPath)
        {
            if (!Directory.Exists(extensionsPath))
            {
                return new ExtensionInfo[0];
            }

            var extensions = new System.Collections.Generic.List<ExtensionInfo>();

            // 首先获取 WebView2 已加载的扩展
            var webViewExtensions = new System.Collections.Generic.Dictionary<string, Microsoft.Web.WebView2.Core.CoreWebView2BrowserExtension>();
            
            if (profile != null)
            {
                try
                {
                    // 注意：WebView2 没有直接获取所有扩展列表的 API
                    // 我们需要通过加载扩展来获取 IsEnabled 状态
                    Debug.WriteLine("ExtensionManager: Getting extension status from WebView2...");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExtensionManager: Failed to get WebView2 extensions: " + ex.Message);
                }
            }

            foreach (string extensionDir in Directory.GetDirectories(extensionsPath))
            {
                string manifestPath = Path.Combine(extensionDir, "manifest.json");
                if (File.Exists(manifestPath))
                {
                    try
                    {
                        string manifestJson = File.ReadAllText(manifestPath);
                        Debug.WriteLine("ExtensionManager: Loading extension: " + extensionDir);
                        Debug.WriteLine("ExtensionManager: Manifest content: " + manifestJson.Substring(0, Math.Min(200, manifestJson.Length)));

                        using (var doc = JsonDocument.Parse(manifestJson))
                        {
                            var root = doc.RootElement;

                            string name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : Path.GetFileName(extensionDir);
                            string version = root.TryGetProperty("version", out var versionProp) ? versionProp.GetString() : "Unknown";
                            string id = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : Path.GetFileName(extensionDir);

                            Debug.WriteLine("ExtensionManager: Extension name: " + name);
                            Debug.WriteLine("ExtensionManager: Has manifest_v2_disable: " + root.TryGetProperty("manifest_v2_disable", out _));

                            if (!string.IsNullOrEmpty(name) && name.StartsWith("__MSG_") && name.EndsWith("__"))
                            {
                                string messageKey = name.Substring(6, name.Length - 8);
                                string localeName = GetExtensionLocalizedName(extensionDir, messageKey);
                                if (!string.IsNullOrEmpty(localeName))
                                {
                                    name = localeName;
                                }
                            }

                            bool isEnabled = !root.TryGetProperty("manifest_v2_disable", out var disableProp);

                            Debug.WriteLine("ExtensionManager: Final name: " + name);
                            Debug.WriteLine("ExtensionManager: Is enabled (from manifest): " + isEnabled);

                            extensions.Add(new ExtensionInfo
                            {
                                Name = name,
                                Version = version,
                                IsEnabled = isEnabled,
                                Path = extensionDir,
                                Id = id
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

            return extensions.ToArray();
        }

        /// <summary>
        /// 获取所有扩展信息列表（从文件系统读取，不保证实时状态）
        /// </summary>
        public static ExtensionInfo[] GetAllExtensions(string extensionsPath)
        {
            return GetAllExtensionsAsync(null, extensionsPath).Result;
        }

        /// <summary>
        /// 安装扩展（从文件夹复制）
        /// </summary>
        public static void InstallExtension(string sourcePath, string extensionsPath)
        {
            string extensionName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(extensionsPath, extensionName);

            if (Directory.Exists(destPath))
            {
                Directory.Delete(destPath, true);
            }

            CopyDirectory(sourcePath, destPath);
        }

        /// <summary>
        /// 卸载扩展
        /// </summary>
        public static void UninstallExtension(string extensionPath)
        {
            if (Directory.Exists(extensionPath))
            {
                Directory.Delete(extensionPath, true);
            }
        }

        /// <summary>
        /// 切换扩展启用/禁用状态
        /// </summary>
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
                Debug.WriteLine("ExtensionManager: Original manifest: " + manifestJson.Substring(0, Math.Min(200, manifestJson.Length)));

                string manifestCopy;

                if (enable)
                {
                    Debug.WriteLine("ExtensionManager: Enabling extension...");
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
                    Debug.WriteLine("ExtensionManager: Disabling extension...");
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

                Debug.WriteLine("ExtensionManager: New manifest: " + manifestCopy.Substring(0, Math.Min(200, manifestCopy.Length)));
                File.WriteAllText(manifestPath, manifestCopy);

                string newContent = File.ReadAllText(manifestPath);
                Debug.WriteLine("ExtensionManager: Verified manifest after write: " + newContent.Substring(0, Math.Min(200, newContent.Length)));

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtensionManager: Toggle extension error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 刷新扩展状态（重新加载所有扩展）
        /// </summary>
        public static async Task RefreshExtensionsAsync(Microsoft.Web.WebView2.Core.CoreWebView2Profile profile)
        {
            Debug.WriteLine("ExtensionManager: Refreshing all extensions...");
            await LoadExtensionsAsync(profile);
        }

        /// <summary>
        /// 获取扩展本地化名称
        /// </summary>
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

        /// <summary>
        /// 复制目录
        /// </summary>
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
