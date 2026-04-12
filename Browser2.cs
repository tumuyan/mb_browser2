using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    internal static class Log
    {
        public const bool EnableGeneralLog = true;
        public const bool EnableNavigationLog = true;
        public const bool EnableExtensionLog = true;
        public const bool EnableResizeLog = true;
        public const bool EnableSkinDebugLog = false;

        [Conditional("DEBUG")]
        public static void General(string message)
        {
            if (EnableGeneralLog) Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void Navigation(string message)
        {
            if (EnableNavigationLog) Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void Extension(string message)
        {
            if (EnableExtensionLog) Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void Resize(string message)
        {
            if (EnableResizeLog) Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void SkinDebug(string message)
        {
            if (EnableSkinDebugLog) Debug.WriteLine(message);
        }
    }

    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        private UserControl panel;
        private Control header;
        private WebView2 browser;
        private TextBox locationBar;
        private Control locationBarPrompt;

        private bool isLoading;
        private string activeUrl;
        
        private bool shouldBrowserBeVisible = false;

        private Bitmap faviconImage;
        private bool currentIsFavourite;

        private Bitmap backIcon;
        private Bitmap forwardIcon;
        private Bitmap refreshIcon;
        private Bitmap stopIcon;
        private Bitmap homeIcon;
        private Bitmap starFilledIcon;
        private Bitmap starLinedIcon;
        private Bitmap menuIcon;
        private Bitmap settingsIcon;

        private BrowserSettings settings = new BrowserSettings();
        private bool isSettingsDirty;

        private ContextMenuStrip bookmarkContextMenu;

        private EventHandler openHandler;
        private EventHandler closeHandler;

        private Color themeForegroundColor;
        private Color themeBackgroundColor;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            openHandler = OpenBrowser;
            closeHandler = CloseBrowser;
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Browser2";
            about.Author = "tumuyan";
            about.TargetApplication = "";
            about.Type = PluginType.WebBrowser;
            about.VersionMajor = 1;
            about.VersionMinor = 0;
            about.Revision = 6;
            about.Description = $"A modern web browser based on WebView2. (v{about.VersionMajor}.{about.VersionMinor}.{about.Revision})";
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;
            Log.General("Browser2: Initialise completed");
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            string storagePath = mbApiInterface.Setting_GetPersistentStoragePath();
            Microsoft.Web.WebView2.Core.CoreWebView2Profile profile = null;
            if (browser != null && browser.CoreWebView2 != null)
            {
                profile = browser.CoreWebView2.Profile;
            }
            using (var form = new FormSetting(settings, storagePath, profile))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.SettingsChanged)
                    {
                        isSettingsDirty = true;
                    }
                    SaveSettings();
                }
            }
            return true;
        }

        public void SaveSettings()
        {
            string path = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.json";
            try
            {
                SettingsManager.Save(path, settings);
            }
            catch (Exception ex)
            {
                Log.General("保存设置失败：" + ex.Message);
            }
            isSettingsDirty = false;
        }

        private string LoadSettings()
        {
            string jsonPath = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.json";
            Log.General("加载设置，路径：" + jsonPath);
            settings = SettingsManager.Load<BrowserSettings>(jsonPath);
            //  Log.General("加载设置完成，DefaultUrl: " + (settings.DefaultUrl ?? "null") + ", Bookmarks: " + settings.Bookmarks.Count);
            return settings.DefaultUrl;
        }

        public void Close(PluginCloseReason reason)
        {
            if (isSettingsDirty)
            {
                SaveSettings();
            }
            
            // 取消订阅 WebView2 事件
            if (browser != null)
            {
                browser.NavigationStarting -= Browser_NavigationStarting;
                browser.NavigationCompleted -= Browser_NavigationCompleted;
                browser.SourceChanged -= Browser_SourceChanged;
                browser.ZoomFactorChanged -= Browser_ZoomFactorChanged;
                
                if (browser.CoreWebView2 != null)
                {
                    browser.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;
                }
            }
            
            // 释放 WebView2 资源
            // 注意：CoreWebView2 本身没有 Dispose 方法，但调用 browser.Dispose() 会清理相关资源
            browser?.Dispose();
            browser = null;
            
            // 释放所有图标资源
            backIcon?.Dispose();
            backIcon = null;
            forwardIcon?.Dispose();
            forwardIcon = null;
            homeIcon?.Dispose();
            homeIcon = null;
            refreshIcon?.Dispose();
            refreshIcon = null;
            stopIcon?.Dispose();
            stopIcon = null;
            starFilledIcon?.Dispose();
            starFilledIcon = null;
            starLinedIcon?.Dispose();
            starLinedIcon = null;
            menuIcon?.Dispose();
            menuIcon = null;
            settingsIcon?.Dispose();
            settingsIcon = null;
            
            // 释放 faviconImage
            faviconImage?.Dispose();
            faviconImage = null;
            
            // 清理书签列表
            settings.Bookmarks.Clear();
            
            // 释放上下文菜单
            bookmarkContextMenu?.Dispose();
            bookmarkContextMenu = null;
            settingsContextMenu?.Dispose();
            settingsContextMenu = null;
            
            // 释放 locationBarPrompt
            locationBarPrompt?.Dispose();
            locationBarPrompt = null;
            
            // 释放 locationBar
            locationBar?.Dispose();
            locationBar = null;
            
            // 释放 header
            header?.Dispose();
            header = null;
            
            // 释放 panel
            panel?.Dispose();
            panel = null;
            
            // webViewEnvironment 不需要释放 (CoreWebView2Environment 未实现 IDisposable)
            webViewEnvironment = null;
        }

        public void Uninstall()
        {
        }

        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.PluginStartup:
                    Log.General("Browser2: PluginStartup");
                    Log.General("Browser2: API Revision = " + mbApiInterface.ApiRevision);
                    
                    LoadSettings();
                    DebugSkinColors();
                    try
                    {
                        Log.General("Browser2: Adding tree node");
                        themeForegroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentForeground);
                        themeBackgroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentBackground);

                        backIcon = CreateThemedIcon("iconmonstr-arrow-left-alt-filled-16.png", Color.Transparent, themeForegroundColor);
                        forwardIcon = CreateThemedIcon("iconmonstr-arrow-right-alt-filled-16.png", Color.Transparent, themeForegroundColor);
                        homeIcon = CreateThemedIcon("iconmonstr-home-7-24.png", Color.Transparent, themeForegroundColor);
                        refreshIcon = CreateThemedIcon("iconmonstr-synchronization-3-24.png", Color.Transparent, themeForegroundColor);
                        stopIcon = CreateThemedIcon("iconmonstr-x-mark-9-24.png", Color.Transparent, themeForegroundColor);
                        starFilledIcon = CreateThemedIcon("iconmonstr-star-3-24.png", Color.Transparent, themeForegroundColor);
                        starLinedIcon = CreateThemedIcon("iconmonstr-star-5-24.png", Color.Transparent, themeForegroundColor);
                        menuIcon = CreateThemedIcon("iconmonstr-menu-square-lined-24.png", Color.Transparent, themeForegroundColor);
                        settingsIcon = CreateThemedIcon("iconmonstr-tools-14-24.png", Color.Transparent, themeForegroundColor);

                        var themedIcon = CreateThemedIcon("iconmonstr-networking-6-16.png", Color.Transparent, themeForegroundColor);
                        if (themedIcon != null)
                        {
                            mbApiInterface.MB_AddTreeNode("Services", "Browser2", themedIcon, openHandler, closeHandler);
                        }
                        else
                        {
                            Log.General("Browser2: Failed to create themed icon");
                            mbApiInterface.MB_AddTreeNode("Services", "Browser2", null, openHandler, closeHandler);
                        }

                        //暂不添加到菜单中
                        //mbApiInterface.MB_AddMenuItem("mnuTools/Browser2", "Browser2", openHandler);
                    }
                    catch (Exception ex)
                    {
                        Log.General("Browser2: MenuItem exception: " + ex.Message + ex.StackTrace);
                        MessageBox.Show("Browser2: MenuItem error: " + ex.Message, "Browser2 Error");
                    }
                    break;
                case NotificationType.TrackChanged:
                    break;
                case NotificationType.PlayStateChanged:
                    break;
                case NotificationType.PlayingTracksChanged:
                    break;
                case NotificationType.TagsChanged:
                    break;
            }
        }

        public void OpenBrowser(object sender, EventArgs e)
        {
            Log.Navigation("Browser2: OpenBrowser called, loadOnceUrl: "+loadOnceUrl);
            
            shouldBrowserBeVisible = true;
            Log.Navigation("Browser2: Set shouldBrowserBeVisible = true");
            
            string text = loadOnceUrl;
            loadOnceUrl = null;
            if (browser == null || browser.CoreWebView2 == null)
            {
                if (text == null)
                {
                    text = settings.DefaultUrl;
                }
                Log.Navigation("Browser2: Using DefaultUrl: " + (text ?? "null"));
                Log.Navigation("Browser2: Before InitializeBrowser, pendingUrl=" + (pendingUrl ?? "null"));
                browser = null;
                panel = null;
                InitializeBrowser();
                Log.Navigation("Browser2: After InitializeBrowser, pendingUrl=" + (pendingUrl ?? "null"));
            }
            else
            {
                Log.Navigation("Browser2: WebView2 exists, reusing, text=" + (text ?? "null"));
                
                if (browser != null)
                {
                    browser.Visible = true;
                    Log.Navigation("Browser2: Set WebView2 Visible = true");
                }
                
                AddPanelToMusicBee();
                
                if (!string.IsNullOrEmpty(text))
                {
                    Log.Navigation("Browser2: Loading specified URL: " + text);
                    pendingUrl = text;
                    TryNavigate();
                }
                else
                {
                    Log.Navigation("Browser2: No URL specified, skipping navigation");
                    if (!string.IsNullOrEmpty(activeUrl) && locationBar != null)
                    {
                        Log.Navigation("Browser2: Restoring location bar text to: " + activeUrl);
                        locationBar.Text = activeUrl;
                    }
                }
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                text = activeUrl;
            }
            if (!string.IsNullOrEmpty(text))
            {
                Log.Navigation("Browser2: Calling NavigateTo with: " + text);
                pendingUrl = text;
                TryNavigate();
            }
        }

        private string pendingUrl;

        private void InitializeBrowser()
        {
            Log.General("Browser2: InitializeBrowser started");
            Font font = mbApiInterface.Setting_GetDefaultFont();

            panel = new UserControl();
            panel.AutoScroll = false;
            Log.General("Browser2: UserControl panel created");

            if (settings.ShowAddressBar)
            {
                if (string.IsNullOrEmpty(settings.DefaultUrl))
                {
                    locationBarPrompt = new Control();
                    locationBarPrompt.BackColor = Color.White;
                    locationBarPrompt.ForeColor = Color.FromArgb(115, 115, 115);
                    locationBarPrompt.Font = new Font(font, FontStyle.Italic);
                    locationBarPrompt.TabStop = false;
                    locationBarPrompt.Cursor = Cursors.IBeam;
                    locationBarPrompt.Text = "Enter address or select a bookmark";
                    locationBarPrompt.Paint += LocationBarPrompt_Paint;
                    locationBarPrompt.MouseDown += LocationBarPrompt_MouseDown;
                }

                locationBar = new TextBox();
                locationBar.BorderStyle = BorderStyle.FixedSingle;
                locationBar.BackColor = themeBackgroundColor;
                locationBar.ForeColor = themeForegroundColor;
                locationBar.Font = font;
                locationBar.TabStop = true;
                locationBar.KeyDown += LocationBar_KeyDown;

                header = new Control();
                header.Height = 43;
                header.Controls.Add(locationBarPrompt ?? new Control());
                header.Controls.Add(locationBar);
                header.Dock = DockStyle.Top;
                header.Height = HEADER_FULL_HEIGHT;
                header.TabStop = false;
                header.Paint += Header_Paint;
                header.Resize += Header_Resize;
                header.MouseClick += Header_MouseClick;
            }

            browser = new WebView2();
            browser.Dock = DockStyle.Fill;
            browser.TabStop = false;
            browser.NavigationStarting += Browser_NavigationStarting;
            browser.NavigationCompleted += Browser_NavigationCompleted;
            browser.SourceChanged += Browser_SourceChanged;

            panel.VisibleChanged += Panel_VisibleChanged;

            panel.Controls.Add(browser);
            if (settings.ShowAddressBar && header != null)
            {
                panel.Controls.Add(header);
            }
            panel.TabStop = false;

            if (settings.ShowAddressBar)
            {
                ResizeHeader();
            }
            Log.General("Browser2: InitializeBrowser completed, initializing WebView2");

            InitializeWebView2AndAddPanel();
        }

        private void RegisterFormResizeEvent()
        {
            if (panel?.FindForm() != null)
            {
                panel.FindForm().Resize += MainForm_Resize;
                Log.Resize("Browser2: Registered Form.Resize event");
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            Log.Resize($"===== Form.Resize =====");
            Log.Resize($"  WindowState = {form.WindowState}");
            Log.Resize($"  shouldBrowserBeVisible = {shouldBrowserBeVisible}");
            Log.Resize($"  panel.Visible = {panel?.Visible}");
            Log.Resize($"  browser.Visible = {browser?.Visible}");
            Log.Resize("========================");
            
            if (browser == null) return;
            
            if (form.WindowState == FormWindowState.Minimized)
            {
                if (browser.Visible)
                {
                    browser.Visible = false;
                    Log.Resize("Browser2: MusicBee minimized, set browser.Visible = false");
                }
            }
            else if (form.WindowState == FormWindowState.Normal || form.WindowState == FormWindowState.Maximized)
            {
                Log.Resize("Browser2: MusicBee restored");
                if (shouldBrowserBeVisible && !browser.Visible)
                {
                    browser.Visible = true;
                    Log.Resize("Browser2: shouldBrowserBeVisible = true, set browser.Visible = true");
                }
                else if (!shouldBrowserBeVisible)
                {
                    Log.Resize("Browser2: shouldBrowserBeVisible = false, keep browser.Visible = false");
                }
                else
                {
                    Log.Resize("Browser2: browser.Visible already true, no change needed");
                }
            }
        }

        private async void InitializeWebView2AndAddPanel()
        {
            if (browser?.CoreWebView2 != null)
            {
                Log.General("WebView2 already initialized, adding panel");
                AddPanelToMusicBee();
                return;
            }
            try
            {
                Log.General("开始 InitializeWebView2");
                if (webViewEnvironment == null)
                {
                    var envSettings = new CoreWebView2EnvironmentOptions();
                    var browserArgs = new System.Collections.Generic.List<string>();
                    
                    bool enableDarkMode = false;
                    
                    if (settings.DarkMode == DarkModeType.Dark)
                    {
                        enableDarkMode = true;
                    }
                    else if (settings.DarkMode == DarkModeType.MusicBeeTheme)
                    {
                        enableDarkMode =! IsColorDark(themeForegroundColor);
                        Log.General($"Dark mode {enableDarkMode}, themeForegroundColor: {themeForegroundColor.ToString()} ");
                    }
                    
                    if (enableDarkMode)
                    {
                        browserArgs.Add("--enable-features=ForceDarkModeFlag,MediaQueryEmulation");
                        browserArgs.Add("--force-dark-mode");
                        browserArgs.Add("--emulate-media-features=prefers-color-scheme:dark");
                        Log.General("Browser2: Dark mode enabled");
                    }
                    
                    if (settings.EnableExtensions)
                    {
                        Log.Extension("Browser2: Extensions enabled in settings");
                        try
                        {
                            envSettings.AreBrowserExtensionsEnabled = true;
                            Log.Extension("Browser2: AreBrowserExtensionsEnabled set to true");
                        }
                        catch
                        {
                            browserArgs.Add("--embedded-browser-webview-enable-extension");
                        }
                    }
                    
                    if (browserArgs.Count > 0)
                    {
                        envSettings.AdditionalBrowserArguments = string.Join(" ", browserArgs);
                        Log.Extension("Browser2: AdditionalBrowserArguments: " + envSettings.AdditionalBrowserArguments);
                    }
                    
                    webViewEnvironment = await CoreWebView2Environment.CreateAsync(null, null, envSettings);
                    Log.General("WebView2 Environment created");
                }
                await browser.EnsureCoreWebView2Async(webViewEnvironment);
                Log.General("WebView2 Initialize completed");
                
                browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                browser.ZoomFactorChanged += Browser_ZoomFactorChanged;
                
                if (settings.ZoomFactor > 0 && settings.AutoSaveZoom)
                {
                    browser.ZoomFactor = settings.ZoomFactor;
                }
                
                if (settings.EnableExtensions)
                {
                    await LoadExtensionsAsync();
                }
                
                Log.General("Browser2: WebView2 initialized");
                AddPanelToMusicBee();
            }
            catch (Exception ex)
            {
                Log.General("WebView2 init error: " + ex);
                Log.General("Browser2: WebView2 init error: " + ex.Message);
                MessageBox.Show("WebView2 init error: " + ex.Message, "Browser2 Error");
            }
        }

        private async System.Threading.Tasks.Task LoadExtensionsAsync()
        {
            string storagePath = mbApiInterface.Setting_GetPersistentStoragePath();
            
            Log.Extension("Browser2: LoadExtensionsAsync started");
            Log.Extension("Browser2: Storage path: " + storagePath);

            try
            {
                var profile = browser.CoreWebView2.Profile;
                Log.Extension("Browser2: Profile: " + (profile != null ? "available" : "null"));
                
                if (profile != null)
                {
                    await ExtensionManager.LoadExtensionsAsync(profile);
                    Log.Extension($"Browser2: Extensions loaded and cached: {ExtensionManager.CachedExtensions.Count}");
                }
            }
            catch (Exception ex)
            {
                Log.Extension("Browser2: LoadExtensions error: " + ex.Message);
                Log.Extension("Browser2: Exception type: " + ex.GetType().FullName);
                Log.Extension("Browser2: Stack trace: " + ex.StackTrace);
            }
        }

        private bool IsColorDark(Color color)
        {
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
            return luminance < 0.5;
        }

        private void AddPanelToMusicBee()
        {
            mbApiInterface.MB_AddPanel(panel, PluginPanelDock.MainPanel);
            Log.General("Browser2: Panel added to MainPanel");
            RegisterFormResizeEvent();
            TryNavigate();
        }
        
        private CoreWebView2Environment webViewEnvironment;

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            browser.Source = new Uri(e.Uri);
        }

        private void TryNavigate()
        {
            if (browser?.CoreWebView2 == null)
            {
                Log.Navigation("Browser2: TryNavigate - WebView2 not ready");
                InitializeWebView2AndAddPanel();
                return;
            }
            Log.Navigation("Browser2: TryNavigate - WebView2 ready, pendingUrl=" + (pendingUrl ?? "null"));
            if (string.IsNullOrEmpty(pendingUrl))
            {
                Log.Navigation("Browser2: TryNavigate - pendingUrl is empty, returning");
                return;
            }
            Log.Navigation("Browser2: TryNavigate - Calling NavigateTo with: " + pendingUrl);
            NavigateTo(pendingUrl);
            pendingUrl = null;
        }

        private void Panel_VisibleChanged(object sender, EventArgs e)
        {
            Control panel = (Control)sender;
            Log.General($"===== Panel_VisibleChanged =====");
            Log.General($"  panel.Visible = {panel.Visible}");
            Log.General($"  browser != null = {browser != null}");
            Log.General($"  browser.Visible = {browser?.Visible}");
            Log.General($"================================");
            
            if (!panel.Visible && browser != null)
            {
                browser.Visible = false;
                Log.General("Browser2: Panel hidden, set browser.Visible = false");
            }
            else if (panel.Visible && browser != null)
            {
                browser.Visible = true;
                Log.General("Browser2: Panel shown, set browser.Visible = true");
            }
        }

        // 按钮布局常量
        private const int BUTTON_WIDTH = 32;  // 按钮位置计算宽度（包含间隙）
        private const int BUTTON_HEIGHT = 24; // 按钮实际宽度和高度
        private const int LEFT_MARGIN_BUTTONS = 1;  // 左侧保留 1 个按钮的间距
        private const int RIGHT_MARGIN_BUTTONS = 1; // 右侧保留 1 个按钮的间距
        
        private Rectangle FavoritesHighlightBounds => header != null ? new Rectangle(LEFT_MARGIN_BUTTONS * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle BrowseBackButtonBounds => header != null ? new Rectangle((LEFT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle BrowseForwardButtonBounds => header != null ? new Rectangle((LEFT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle HomeButtonBounds => header != null ? new Rectangle((LEFT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle RefreshStopButtonBounds => header != null ? new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle BookmarkButtonBounds => header != null ? new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle BookmarkListButtonBounds => header != null ? new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        private Rectangle SettingsButtonBounds => header != null ? new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 3) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT) : Rectangle.Empty;
        
        private const int HEADER_FULL_HEIGHT = 43; // header 固定高度

        private Color GetThemeColor(SkinElement element, ElementComponent component)
        {
            int colorInt = mbApiInterface.Setting_GetSkinElementColour(element, ElementState.ElementStateDefault, component);
            return ColorTranslator.FromWin32(colorInt);
        }

        private void DebugSkinColors()
        {
            if (!Log.EnableSkinDebugLog) return;
            
            Debug.WriteLine("========== Skin Element Colors Debug ==========");
            
            Debug.WriteLine("ID\tBG\tFG\tBDR");
            Debug.WriteLine("-------------------------------------------");
            
            for (int id = 0; id <= 30; id++)
            {
                SkinElement testElement = (SkinElement)id;
                
                try
                {
                    Color bg = GetThemeColor(testElement, ElementComponent.ComponentBackground);
                    Color fg = GetThemeColor(testElement, ElementComponent.ComponentForeground);
                    Color bdr = GetThemeColor(testElement, ElementComponent.ComponentBorder);
                    
                    if (bg.ToArgb() != Color.Black.ToArgb() || fg.ToArgb() != Color.Black.ToArgb())
                    {
                        Debug.WriteLine($"{id}\t#{bg.R:X2}{bg.G:X2}{bg.B:X2}\t#{fg.R:X2}{fg.G:X2}{fg.B:X2}\t#{bdr.R:X2}{bdr.G:X2}{bdr.B:X2}");
                    }
                }
                catch
                {
                }
            }
            
            Debug.WriteLine("\n========== End Skin Element Colors Debug ==========");
        }

        private Bitmap CreateThemedIcon(string iconFileName, Color bgColor, Color fgColor)
        {
            string pluginPath = Path.GetDirectoryName(typeof(Plugin).Assembly.Location);
            string iconPath = Path.Combine(pluginPath, "Resources", iconFileName);
            
            if (!File.Exists(iconPath))
            {
                Log.General("Browser2: Icon not found at " + iconPath);
                return null;
            }

            using (var originalIcon = new Bitmap(iconPath))
            {
                Bitmap themedIcon = new Bitmap(originalIcon.Width, originalIcon.Height);
                
                for (int y = 0; y < originalIcon.Height; y++)
                {
                    for (int x = 0; x < originalIcon.Width; x++)
                    {
                        Color pixel = originalIcon.GetPixel(x, y);
                        
                        byte blackness = (byte)((255 - pixel.R + 255 - pixel.G + 255 - pixel.B) / 3);
                        
                        if (blackness > 128)
                        {
                            byte newR = (byte)(fgColor.R * blackness / 255);
                            byte newG = (byte)(fgColor.G * blackness / 255);
                            byte newB = (byte)(fgColor.B * blackness / 255);
                            
                            themedIcon.SetPixel(x, y, Color.FromArgb(pixel.A, newR, newG, newB));
                        }
                        else
                        {
                            themedIcon.SetPixel(x, y, pixel);
                        }
                    }
                }

                return themedIcon;
            }
        }

        private void LoadBrowserImages()
        {
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            Color borderColor = GetThemeColor(SkinElement.SkinInputPanel, ElementComponent.ComponentBorder);
            //Debug.WriteLine($"Browser2: Header_Paint - Height={header.Height}, Bg={themeBackgroundColor.ToArgb():X8}, Fg={themeForegroundColor.ToArgb():X8}, Border={borderColor.ToArgb():X8}");

            // 始终绘制背景，即使高度只有 10 像素
            using (var brush = new SolidBrush(themeBackgroundColor))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, header.Width, header.Height));
            }
            
            // 如果高度小于 20 像素，只绘制背景，不绘制按钮和地址栏
            if (header.Height < 20)
            {
                // 半隐藏状态，只绘制一条细线作为边框
                using (var pen = new Pen(borderColor))
                {
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
                return;  // 提前返回，不绘制其他内容
            }
            
            // 正常高度时绘制完整内容
            using (var pen = new Pen(borderColor))
            {
                e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            }

            if (browser.CanGoBack && backIcon != null)
            {
                Rectangle backBounds = BrowseBackButtonBounds;
                e.Graphics.DrawImage(backIcon, backBounds);
            }

            if (browser.CanGoForward && forwardIcon != null)
            {
                Rectangle forwardBounds = BrowseForwardButtonBounds;
                e.Graphics.DrawImage(forwardIcon, forwardBounds);
            }

            if (homeIcon != null)
            {
                Rectangle homeBounds = HomeButtonBounds;
                e.Graphics.DrawImage(homeIcon, homeBounds);
            }

            if (currentIsFavourite && starFilledIcon != null)
            {
                e.Graphics.DrawImage(starFilledIcon, BookmarkButtonBounds);
            }
            else if (starLinedIcon != null)
            {
                e.Graphics.DrawImage(starLinedIcon, BookmarkButtonBounds);
            }

            if (menuIcon != null)
            {
                e.Graphics.DrawImage(menuIcon, BookmarkListButtonBounds);
            }

            if (settingsIcon != null)
            {
                e.Graphics.DrawImage(settingsIcon, SettingsButtonBounds);
            }

            if (faviconImage != null)
            {
                e.Graphics.DrawImage(faviconImage, new Rectangle(110, 13, 16, 16));
            }

            if (isLoading && stopIcon != null)
            {
                e.Graphics.DrawImage(stopIcon, RefreshStopButtonBounds);
            }
            else if (refreshIcon != null)
            {
                e.Graphics.DrawImage(refreshIcon, RefreshStopButtonBounds);
            }
        }

        private void Header_Resize(object sender, EventArgs e)
        {
            ResizeHeader();
            header?.Invalidate();
        }

        private void ResizeHeader()
        {
            if (locationBar != null && header != null)
            {
                // 计算左右按钮区域的宽度
                int leftButtonWidth = (LEFT_MARGIN_BUTTONS + 3) * BUTTON_WIDTH;  // 左侧边距 + 3 个按钮
                int rightButtonWidth = (RIGHT_MARGIN_BUTTONS + 4) * BUTTON_WIDTH; // 右侧边距 + 4 个按钮
                int buttonSpacing = 4;     // 按钮与地址栏的间距
                
                // 地址栏从左侧按钮区域后开始，到右侧按钮区域前结束
                int locationBarX = leftButtonWidth + buttonSpacing;
                int locationBarWidth = header.Width - leftButtonWidth - rightButtonWidth - buttonSpacing * 2;
                
                // 确保地址栏宽度不小于 0
                if (locationBarWidth < 0)
                {
                    locationBarWidth = 0;
                }
                
                // 设置地址栏位置和大小（Y 坐标上移到 2）
                int locationBarY = 2;
                locationBar.Bounds = new Rectangle(locationBarX, locationBarY, locationBarWidth, locationBar.Font.Height + 6);
                
                if (locationBarPrompt != null)
                {
                    locationBarPrompt.Bounds = new Rectangle(locationBar.Left + 1, locationBar.Top, locationBar.Width - 1, locationBar.Height);
                }
            }
        }
        
        private void Header_MouseClick(object sender, MouseEventArgs e)
        {
            if (BrowseBackButtonBounds.Contains(e.Location))
            {
                if (browser.CanGoBack)
                {
                    browser.GoBack();
                }
            }
            else if (BrowseForwardButtonBounds.Contains(e.Location))
            {
                if (browser.CanGoForward)
                {
                    browser.GoForward();
                }
            }
            else if (HomeButtonBounds.Contains(e.Location))
            {
                if (!string.IsNullOrEmpty(settings.DefaultUrl))
                {
                    browser.CoreWebView2?.Navigate(settings.DefaultUrl);
                }
            }
            else if (RefreshStopButtonBounds.Contains(e.Location))
            {
                if (locationBar == null || string.IsNullOrEmpty(locationBar.Text))
                    return;

                if (isLoading)
                {
                    browser.Stop();
                    if (browser.Source != null)
                    {
                        ShowNavigationTarget(browser.Source.ToString());
                    }
                    isLoading = false;
                }
                else
                {
                    browser.Reload();
                    isLoading = true;
                }
                header?.Invalidate(RefreshStopButtonBounds);
            }
            else if (BookmarkButtonBounds.Contains(e.Location))
            {
                if (locationBar != null && !string.IsNullOrEmpty(locationBar.Text))
                {
                    bool found = false;
                    for (int i = 0; i < settings.Bookmarks.Count; i++)
                    {
                        if (string.Compare(settings.Bookmarks[i].Url, locationBar.Text, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            found = true;
                            RemoveFavourite(i);
                            break;
                        }
                    }
                    if (!found)
                    {
                        string title = browser.CoreWebView2?.DocumentTitle ?? "No title";
                        settings.Bookmarks.Add(new Bookmark(locationBar.Text, title));
                        SaveSettings();
                        mbApiInterface.MB_SetBackgroundTaskMessage("Bookmark has been saved");
                        currentIsFavourite = true;
                        header?.Invalidate(BookmarkButtonBounds);
                    }
                }
            }
            else if (BookmarkListButtonBounds.Contains(e.Location))
            {
                ShowBookmarkMenu();
            }
            else if (SettingsButtonBounds.Contains(e.Location))
            {
                ShowSettingsMenu();
            }
        }

        private void ShowBookmarkMenu()
        {
            if (bookmarkContextMenu == null)
            {
                bookmarkContextMenu = new ContextMenuStrip();
            }
            bookmarkContextMenu.Items.Clear();

            if (settings.Bookmarks.Count == 0)
            {
                bookmarkContextMenu.Items.Add("(No bookmarks)");
            }
            else
            {
                foreach (var fav in settings.Bookmarks)
                {
                    var item = new ToolStripMenuItem
                    {
                        Text = fav.Name,
                        ToolTipText = fav.Url
                    };
                    item.Click += (s, e) =>
                    {
                        NavigateTo(fav.Url);
                    };
                    bookmarkContextMenu.Items.Add(item);
                }
            }

            var location = header?.PointToScreen(new Point(header.Width - 50, header.Height)) ?? Cursor.Position;
            bookmarkContextMenu.Show(location);
        }

        private ContextMenuStrip settingsContextMenu;
        private async void ShowSettingsMenu()
        {
            if (settingsContextMenu == null)
            {
                settingsContextMenu = new ContextMenuStrip();
            }
            settingsContextMenu.Items.Clear();

            var settingsMenuItem = new ToolStripMenuItem
            {
                Text = "Settings"
            };
            settingsMenuItem.Click += (s, e) =>
            {
                Configure(IntPtr.Zero);
            };
            settingsContextMenu.Items.Add(settingsMenuItem);

            if (settings.EnableExtensions)
            {
                IReadOnlyList<MusicBeePlugin.ExtensionInfo> extensionsToUse;

                if (ExtensionManager.CachedExtensions.Count > 0)
                {
                    extensionsToUse = ExtensionManager.CachedExtensions;
                }
                else
                {
                    string storagePath = mbApiInterface.Setting_GetPersistentStoragePath();
                    string extPath = System.IO.Path.Combine(storagePath, "Browser2Extensions");
                    if (System.IO.Directory.Exists(extPath))
                    {
                        extensionsToUse = ExtensionManager.GetAllExtensions(extPath);
                        Log.General($"ShowSettingsMenu: Cache was empty, performed sync scan: {extensionsToUse.Count} extensions found");

                        if (browser?.CoreWebView2?.Profile != null)
                        {
                            try
                            {
                                foreach (var ext in extensionsToUse)
                                {
                                    if (!string.IsNullOrEmpty(ExtensionManager.GetRealExtensionId(ext.Path)))
                                    {
                                        continue;
                                    }

                                    var loadedExt = await browser.CoreWebView2.Profile.AddBrowserExtensionAsync(ext.Path);
                                    if (loadedExt != null)
                                    {
                                        ExtensionManager.SetRealExtensionId(ext.Path, loadedExt.Id);
                                        Log.General($"ShowSettingsMenu: Fetched real ID '{loadedExt.Id}' for '{ext.Name}'");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.General($"ShowSettingsMenu: Error fetching real IDs: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        extensionsToUse = new List<MusicBeePlugin.ExtensionInfo>();
                    }
                }

                var enabledExtensions = extensionsToUse.Where(ext => ext.IsEnabled).ToList();

                if (enabledExtensions.Count > 0)
                {
                    settingsContextMenu.Items.Add(new ToolStripSeparator());

                    foreach (var ext in enabledExtensions)
                    {
                        bool hasOptionsPage = !string.IsNullOrEmpty(ext.OptionsUrl);
                        bool hasPopupPage = !string.IsNullOrEmpty(ext.PopupUrl);
                        bool hasMainPage = !string.IsNullOrEmpty(ext.MainPageUrl);

                        if (!hasOptionsPage && !hasPopupPage && !hasMainPage)
                        {
                            continue;
                        }

                        string realExtensionId = ExtensionManager.GetRealExtensionId(ext.Path) ?? ext.Id;
                        string extensionPath = ext.Path;
                        string finalExtensionId = realExtensionId;

                        if (hasPopupPage)
                        {
                            var popupMenuItem = new ToolStripMenuItem
                            {
                                Text = $"{ext.Name} (Popup)"
                            };
                            string finalPopupUrl = ext.PopupUrl;
                            popupMenuItem.Click += async (s, e) =>
                            {
                                await OpenExtensionPage(finalExtensionId, extensionPath, finalPopupUrl, "Popup");
                            };
                            settingsContextMenu.Items.Add(popupMenuItem);
                        }

                        if (hasOptionsPage)
                        {
                            var optionsMenuItem = new ToolStripMenuItem
                            {
                                Text = $"{ext.Name} (Options)"
                            };
                            string finalOptionsUrl = ext.OptionsUrl;
                            optionsMenuItem.Click += async (s, e) =>
                            {
                                await OpenExtensionPage(finalExtensionId, extensionPath, finalOptionsUrl, "Options");
                            };
                            settingsContextMenu.Items.Add(optionsMenuItem);
                        }

                        if (!hasOptionsPage && !hasPopupPage && hasMainPage)
                        {
                            var mainPageMenuItem = new ToolStripMenuItem
                            {
                                Text = $"{ext.Name} (Main Page)"
                            };
                            string finalMainPageUrl = ext.MainPageUrl;
                            mainPageMenuItem.Click += async (s, e) =>
                            {
                                await OpenExtensionPage(finalExtensionId, extensionPath, finalMainPageUrl, "Main Page");
                            };
                            settingsContextMenu.Items.Add(mainPageMenuItem);
                        }
                    }
                }
            }

            var location = header?.PointToScreen(new Point(
                SettingsButtonBounds.X + SettingsButtonBounds.Width / 2,
                SettingsButtonBounds.Bottom)) ?? Cursor.Position;
            settingsContextMenu.Show(location);
        }

        private async System.Threading.Tasks.Task OpenExtensionPage(string extensionId, string extensionPath, string pageUrl, string pageType)
        {
            try
            {
                if (browser?.CoreWebView2 == null)
                {
                    return;
                }

                Log.General($"OpenExtensionPage: Opening {pageType} page for extension '{extensionId}', URL='{pageUrl}'");

                if (!string.IsNullOrEmpty(pageUrl))
                {
                    string fullUrl = $"extension://{extensionId}/{pageUrl.TrimStart('/')}";
                    Log.General($"OpenExtensionPage: Navigating to '{fullUrl}'");
                    browser.CoreWebView2.Navigate(fullUrl);
                }
                else
                {
                    mbApiInterface.MB_SetBackgroundTaskMessage($"Extension '{System.IO.Path.GetFileName(extensionPath)}' does not have a {pageType.ToLower()} page");
                }
            }
            catch (Exception ex)
            {
                Log.General($"OpenExtensionPage error ({pageType}): " + ex.Message);
                mbApiInterface.MB_SetBackgroundTaskMessage($"Failed to open extension {pageType.ToLower()} page: " + ex.Message);
            }
        }

        private void RemoveFavourite(int index)
        {
            var bookmark = settings.Bookmarks[index];
            settings.Bookmarks.RemoveAt(index);
            SaveSettings();
            SetLocationBarText(locationBar?.Text ?? "");
            mbApiInterface.MB_SetBackgroundTaskMessage("Bookmark '" + bookmark.Name + "' removed");
        }

        private void LocationBarPrompt_Paint(object sender, PaintEventArgs e)
        {
            if (locationBarPrompt != null)
            {
                TextRenderer.DrawText(e.Graphics, locationBarPrompt.Text, locationBarPrompt.Font, new Point(0, 0), locationBarPrompt.ForeColor, TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
            }
        }

        private void LocationBarPrompt_MouseDown(object sender, MouseEventArgs e)
        {
            if (locationBarPrompt != null)
            {
                locationBarPrompt.Dispose();
                locationBarPrompt = null;
                locationBar.Focus();
            }
        }

        // 已废弃：地址栏焦点事件处理（用于自动隐藏功能）
        // private void LocationBar_GotFocus(object sender, EventArgs e) { ... }
        // private void LocationBar_LostFocus(object sender, EventArgs e) { ... }
        
        private void LocationBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (locationBarPrompt != null)
            {
                locationBarPrompt.Dispose();
                locationBarPrompt = null;
            }

            if (e.KeyCode == Keys.Return && locationBar != null && locationBar.Text.Length > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                string url = locationBar.Text;
                if (url.IndexOf("://", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    url = "http://" + url;
                }

                NavigateTo(url);
                isSettingsDirty = true;
            }
        }

        private string loadOnceUrl;

        public void Navigate(string url)
        {
            Log.Navigation("Browser2: Navigate called with URL: " + url);
            if (panel == null)
            {
                loadOnceUrl = url;
                return;
            }
            ShowNavigationTarget(url);
            browser.Focus();
            NavigateTo(url);
        }

        private string DecodeUrlIfNeeded(string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(settings.UrlDecodeChars))
            {
                return url;
            }

            if (!url.Contains("%"))
            {
                return url;
            }

            string result = url;
            foreach (char c in settings.UrlDecodeChars)
            {
                string encoded = Uri.HexEscape(c);
                if (result.Contains(encoded))
                {
                    result = result.Replace(encoded, c.ToString());
                }
            }

            return result;
        }

        private void NavigateTo(string url)
        {
            try
            {
                url = DecodeUrlIfNeeded(url);

                int index = url.IndexOf("<Artist>", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    string artist = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist);
                    url = url.Remove(index, 8).Insert(index, Uri.EscapeDataString(artist ?? ""));
                }

                index = url.IndexOf("<Album Artist>", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    string albumArtist = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.AlbumArtist);
                    url = url.Remove(index, 14).Insert(index, Uri.EscapeDataString(albumArtist ?? ""));
                }

                index = url.IndexOf("<Title>", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    string title = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.TrackTitle);
                    url = url.Remove(index, 7).Insert(index, Uri.EscapeDataString(title ?? ""));
                }

                index = url.IndexOf("<Album>", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    string album = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Album);
                    url = url.Remove(index, 7).Insert(index, Uri.EscapeDataString(album ?? ""));
                }

                if (!string.IsNullOrEmpty(url))
                {
                    Log.Navigation("Browser2: NavigateTo - browser.CoreWebView2 is " + (browser?.CoreWebView2 == null ? "null" : "not null"));
                    if (browser?.CoreWebView2 != null)
                    {
                        Log.Navigation("Browser2: Calling CoreWebView2.Navigate(" + url + ")");
                        browser.CoreWebView2.Navigate(url);
                    }
                    else
                    {
                        Log.Navigation("Browser2: Setting browser.Source = " + url);
                        browser.Source = new Uri(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Navigation("Navigation error: " + ex.Message);
                Log.Navigation("Stack trace: " + ex.StackTrace);
            }
        }

        private void Browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            isLoading = true;
            ShowNavigationTarget(e.Uri);
            header?.Invalidate();
            Log.Navigation("Browser2: NavigationStarting - " + e.Uri);
        }

        private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            isLoading = false;
            activeUrl = browser.Source?.ToString();
            header?.Invalidate();
            Log.Navigation("Browser2: NavigationCompleted - " + (e.IsSuccess ? "Success" : "Failed: " + e.WebErrorStatus));
        }

        private void Browser_ZoomFactorChanged(object sender, EventArgs e)
        {
            if (browser != null && settings.AutoSaveZoom)
            {
                settings.ZoomFactor = browser.ZoomFactor;
                isSettingsDirty = true;
                Log.Navigation("Browser2: ZoomFactor changed to " + browser.ZoomFactor);
                SaveSettings();
            }
        }

        private void Browser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (browser.Source != null)
            {
                string newUrl = browser.Source.ToString();
                SetLocationBarText(newUrl);
                activeUrl = newUrl;
                Log.Navigation("Browser2: SourceChanged - " + newUrl);
            }
        }

        private void ShowNavigationTarget(string url)
        {
            SetLocationBarText(url);
            UpdateFavicon();
        }

        private void UpdateFavicon()
        {
        }

        private void SetLocationBarText(string value)
        {
            if (locationBar != null)
            {
                locationBar.Text = value;
            }

            bool wasFavourite = currentIsFavourite;
            currentIsFavourite = false;

            foreach (var fav in settings.Bookmarks)
            {
                if (string.Compare(fav.Url, value, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    currentIsFavourite = true;
                    break;
                }
            }

            if (currentIsFavourite != wasFavourite && header != null)
            {
                header.Invalidate(BookmarkButtonBounds);
            }
        }

        public void CloseBrowser(object sender, EventArgs e)
        {
            Log.General("Browser2: CloseBrowser called");
            SaveSettings();
            
            shouldBrowserBeVisible = false;
            Log.General("Browser2: Set shouldBrowserBeVisible = false");
            
            if (browser != null)
            {
                browser.Visible = false;
                Log.General("Browser2: Set WebView2 Visible = false");
            }
            
            isLoading = false;
            faviconImage?.Dispose();
            faviconImage = null;
            currentIsFavourite = false;
        }
    }
}