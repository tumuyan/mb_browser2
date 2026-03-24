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
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace MusicBeePlugin
{
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

        private BrowserSettings settings = new BrowserSettings();
        private bool isSettingsDirty;

        private ToolStripMenuItem playNowMenuItem;
        private ToolStripMenuItem queueNextMenuItem;
        private ToolStripMenuItem queueLastMenuItem;

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
            about.Revision = 2;
            about.Description = $"A modern web browser based on WebView2. (v{about.VersionMajor}.{about.VersionMinor}.{about.Revision})";
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            Debug.WriteLine("Browser2: Initialise completed");
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            using (var form = new FormSetting(settings))
            {
                if (form.ShowDialog() == DialogResult.OK && form.SettingsChanged)
                {
                    isSettingsDirty = true;
                    SaveSettings();
                }
            }
            return true;
        }

        public void SaveSettings()
        {
            string path = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.json";
            SettingsManager.Save(path, settings);
            isSettingsDirty = false;
        }

        private string LoadSettings()
        {
            Debug.WriteLine("加载设置");
            string jsonPath = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.json";
            settings = SettingsManager.Load<BrowserSettings>(jsonPath);
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
            
            // 释放 faviconImage
            faviconImage?.Dispose();
            faviconImage = null;
            
            // 清理书签列表
            settings.Bookmarks.Clear();
            
            // 释放上下文菜单
            bookmarkContextMenu?.Dispose();
            bookmarkContextMenu = null;
            
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
                    Debug.WriteLine("Browser2: PluginStartup");
                    Debug.WriteLine("Browser2: API Revision = " + mbApiInterface.ApiRevision);
                    //MessageBox.Show("Browser2: PluginStartup! API=" + mbApiInterface.ApiRevision, "Browser2");
                    
                    // Debug: 遍历所有 SkinElement 的配色
                    DebugSkinColors();
                    
                    try
                    {
                        Debug.WriteLine("Browser2: Adding tree node");
                        themeForegroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentForeground);
                        themeBackgroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentBackground);

                        // 加载主题图标
                        backIcon = CreateThemedIcon("iconmonstr-arrow-left-alt-filled-16.png", Color.Transparent, themeForegroundColor);
                        forwardIcon = CreateThemedIcon("iconmonstr-arrow-right-alt-filled-16.png", Color.Transparent, themeForegroundColor);
                        homeIcon = CreateThemedIcon("iconmonstr-home-7-24.png", Color.Transparent, themeForegroundColor);
                        refreshIcon = CreateThemedIcon("iconmonstr-synchronization-3-24.png", Color.Transparent, themeForegroundColor);
                        stopIcon = CreateThemedIcon("iconmonstr-x-mark-9-24.png", Color.Transparent, themeForegroundColor);
                        starFilledIcon = CreateThemedIcon("iconmonstr-star-3-24.png", Color.Transparent, themeForegroundColor);
                        starLinedIcon = CreateThemedIcon("iconmonstr-star-5-24.png", Color.Transparent, themeForegroundColor);
                        menuIcon = CreateThemedIcon("iconmonstr-menu-square-lined-24.png", Color.Transparent, themeForegroundColor);

                        var themedIcon = CreateThemedIcon("iconmonstr-networking-6-16.png", Color.Transparent, themeForegroundColor);
                        if (themedIcon != null)
                        {
                            mbApiInterface.MB_AddTreeNode("Services", "Browser2", themedIcon, openHandler, closeHandler);
                        }
                        else
                        {
                            Debug.WriteLine("Browser2: Failed to create themed icon");
                            mbApiInterface.MB_AddTreeNode("Services", "Browser2", null, openHandler, closeHandler);
                        }

                        //暂不添加到菜单中
                        //mbApiInterface.MB_AddMenuItem("mnuTools/Browser2", "Browser2", openHandler);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Browser2: MenuItem exception: " + ex.Message + ex.StackTrace);
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
            Debug.WriteLine("Browser2: OpenBrowser called, loadOnceUrl: "+loadOnceUrl);
            
            // 记录状态：浏览器应该可见
            shouldBrowserBeVisible = true;
            Debug.WriteLine($"Browser2: Set shouldBrowserBeVisible = true");
            
            string text = loadOnceUrl;
            loadOnceUrl = null;
            if (browser == null || browser.CoreWebView2 == null)
            {
                if (text == null)
                {
                    text = LoadSettings();
                }
                Debug.WriteLine("Browser2: LoadSettings returned: " + (text ?? "null"));
                Debug.WriteLine("Browser2: Before InitializeBrowser, pendingUrl=" + (pendingUrl ?? "null"));
                browser = null;
                panel = null;
                InitializeBrowser();
                Debug.WriteLine("Browser2: After InitializeBrowser, pendingUrl=" + (pendingUrl ?? "null"));
            }
            else
            {
                // WebView2 存在但 panel 已被移除，重新添加面板
                Debug.WriteLine("Browser2: WebView2 exists, reusing, text=" + (text ?? "null"));
                
                // 设置 WebView2 可见，恢复渲染
                if (browser != null)
                {
                    browser.Visible = true;
                    Debug.WriteLine("Browser2: Set WebView2 Visible = true");
                }
                
                AddPanelToMusicBee();
                
                // 如果有指定 URL，则加载指定 URL；否则不加载（因为可能已经在 Navigate 中加载过了）
                if (!string.IsNullOrEmpty(text))
                {
                    Debug.WriteLine("Browser2: Loading specified URL: " + text);
                    pendingUrl = text;
                    TryNavigate();
                }
                else
                {
                    Debug.WriteLine("Browser2: No URL specified, skipping navigation (may have been loaded in Navigate method)");
                    // 切换回来时，更新地址栏显示当前的 URL
                    if (!string.IsNullOrEmpty(activeUrl) && locationBar != null)
                    {
                        Debug.WriteLine("Browser2: Restoring location bar text to: " + activeUrl);
                        locationBar.Text = activeUrl;
                    }
                }
                return; // 已经处理完毕，直接返回
            }
            if (string.IsNullOrEmpty(text))
            {
                text = activeUrl;
            }
            if (!string.IsNullOrEmpty(text))
            {
                Debug.WriteLine("Browser2: Calling NavigateTo with: " + text);
                pendingUrl = text;
                TryNavigate();
            }
        }

        private string pendingUrl;

        private void InitializeBrowser()
        {
            Debug.WriteLine("Browser2: InitializeBrowser started");
            Font font = mbApiInterface.Setting_GetDefaultFont();

            panel = new UserControl();
            panel.AutoScroll = false;
            // 已删除：panel.MouseMove += Panel_MouseMove;
            // 已删除：panel.MouseLeave += Panel_MouseLeave;
            Debug.WriteLine("Browser2: UserControl panel created");

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
header.Dock = DockStyle.Top;  // 使用 Top Dock，自动填充宽度
header.Height = HEADER_FULL_HEIGHT;  // 固定高度 43 像素，始终显示
header.TabStop = false;
            header.Paint += Header_Paint;
            header.Resize += Header_Resize;
            header.MouseClick += Header_MouseClick;
            // 不设置 BackColor，让 Paint 事件负责绘制背景
            
            // 创建 browser 控件
            browser = new WebView2();
            browser.Dock = DockStyle.Fill;
            browser.TabStop = false;
            browser.NavigationStarting += Browser_NavigationStarting;
            browser.NavigationCompleted += Browser_NavigationCompleted;
            browser.SourceChanged += Browser_SourceChanged;
            
            // 注册 VisibleChanged 事件，自动处理可见性变化
            panel.VisibleChanged += Panel_VisibleChanged;
            // browser.VisibleChanged += Browser_VisibleChanged;
            
            // 先添加 browser（底层），再添加 header（上层）
            panel.Controls.Add(browser);
            panel.Controls.Add(header);
            panel.TabStop = false;

            ResizeHeader();
            Debug.WriteLine("Browser2: InitializeBrowser completed, initializing WebView2");

            InitializeWebView2AndAddPanel();
        }

        private void RegisterFormResizeEvent()
        {
            if (panel?.FindForm() != null)
            {
                panel.FindForm().Resize += MainForm_Resize;
                Debug.WriteLine("Browser2: Registered Form.Resize event");
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            Debug.WriteLine($"===== Form.Resize =====");
            Debug.WriteLine($"  WindowState = {form.WindowState}");
            Debug.WriteLine($"  shouldBrowserBeVisible = {shouldBrowserBeVisible}");
            Debug.WriteLine($"  panel.Visible = {panel?.Visible}");
            Debug.WriteLine($"  browser.Visible = {browser?.Visible}");
            Debug.WriteLine("========================");
            
            if (browser == null) return;
            
            if (form.WindowState == FormWindowState.Minimized)
            {
                // 最小化时，总是隐藏 browser
                if (browser.Visible)
                {
                    browser.Visible = false;
                    Debug.WriteLine("Browser2: MusicBee minimized, set browser.Visible = false");
                }
            }
            else if (form.WindowState == FormWindowState.Normal || form.WindowState == FormWindowState.Maximized)
            {
                Debug.WriteLine("Browser2: MusicBee restored");
                // 只有在 shouldBrowserBeVisible = true 时才恢复 browser.Visible
                if (shouldBrowserBeVisible && !browser.Visible)
                {
                    browser.Visible = true;
                    Debug.WriteLine("Browser2: shouldBrowserBeVisible = true, set browser.Visible = true");
                }
                else if (!shouldBrowserBeVisible)
                {
                    Debug.WriteLine("Browser2: shouldBrowserBeVisible = false, keep browser.Visible = false");
                }
                else
                {
                    Debug.WriteLine("Browser2: browser.Visible already true, no change needed");
                }
            }
        }

        private async void InitializeWebView2AndAddPanel()
        {
            if (browser?.CoreWebView2 != null)
            {
                System.Diagnostics.Trace.WriteLine("WebView2 already initialized, adding panel");
                AddPanelToMusicBee();
                return;
            }
            try
            {
                System.Diagnostics.Trace.WriteLine("开始 InitializeWebView2");
                if (webViewEnvironment == null)
                {
                    var envSettings = new CoreWebView2EnvironmentOptions();
                    webViewEnvironment = await CoreWebView2Environment.CreateAsync(null, null, envSettings);
                    System.Diagnostics.Trace.WriteLine("WebView2 Environment created");
                }
                await browser.EnsureCoreWebView2Async(webViewEnvironment);
                System.Diagnostics.Trace.WriteLine("WebView2 Initialize completed");
                
                browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                browser.ZoomFactorChanged += Browser_ZoomFactorChanged;
                
                if (settings.ZoomFactor > 0 && settings.AutoSaveZoom)
                {
                    browser.ZoomFactor = settings.ZoomFactor;
                }
                
                Debug.WriteLine("Browser2: WebView2 initialized");
                AddPanelToMusicBee();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("WebView2 init error: " + ex);
                Debug.WriteLine("Browser2: WebView2 init error: " + ex.Message);
                MessageBox.Show("WebView2 init error: " + ex.Message, "Browser2 Error");
            }
        }

        private void AddPanelToMusicBee()
        {
            mbApiInterface.MB_AddPanel(panel, PluginPanelDock.MainPanel);
            Debug.WriteLine("Browser2: Panel added to MainPanel");
            
            // 注册 Form Resize 事件
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
                Debug.WriteLine("Browser2: TryNavigate - WebView2 not ready");
                InitializeWebView2AndAddPanel();
                return;
            }
            Debug.WriteLine("Browser2: TryNavigate - WebView2 ready, pendingUrl=" + (pendingUrl ?? "null"));
            if (string.IsNullOrEmpty(pendingUrl))
            {
                Debug.WriteLine("Browser2: TryNavigate - pendingUrl is empty, returning");
                return;
            }
            Debug.WriteLine("Browser2: TryNavigate - Calling NavigateTo with: " + pendingUrl);
            NavigateTo(pendingUrl);
            pendingUrl = null;
        }

        private void Panel_VisibleChanged(object sender, EventArgs e)
        {
            Control panel = (Control)sender;
            Debug.WriteLine($"===== Panel_VisibleChanged =====");
            Debug.WriteLine($"  panel.Visible = {panel.Visible}");
            Debug.WriteLine($"  browser != null = {browser != null}");
            Debug.WriteLine($"  browser.Visible = {browser?.Visible}");
            Debug.WriteLine($"================================");
            
            // 当面板隐藏时，自动设置 WebView2 不可见
            if (!panel.Visible && browser != null)
            {
                browser.Visible = false;
                Debug.WriteLine("Browser2: Panel hidden, set browser.Visible = false");
            }
            // 当面板显示时，自动设置 WebView2 可见
            else if (panel.Visible && browser != null)
            {
                browser.Visible = true;
                Debug.WriteLine("Browser2: Panel shown, set browser.Visible = true");
            }
        }

        // 按钮布局常量
        private const int BUTTON_WIDTH = 32;  // 按钮位置计算宽度（包含间隙）
        private const int BUTTON_HEIGHT = 24; // 按钮实际宽度和高度
        private const int LEFT_MARGIN_BUTTONS = 1;  // 左侧保留 1 个按钮的间距
        private const int RIGHT_MARGIN_BUTTONS = 1; // 右侧保留 1 个按钮的间距
        
        private Rectangle FavoritesHighlightBounds => new Rectangle(LEFT_MARGIN_BUTTONS * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle BrowseBackButtonBounds => new Rectangle((LEFT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle BrowseForwardButtonBounds => new Rectangle((LEFT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle HomeButtonBounds => new Rectangle((LEFT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle RefreshStopButtonBounds => new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle BookmarkButtonBounds => new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);
        private Rectangle BookmarkListButtonBounds => new Rectangle(header.Width - (RIGHT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH - BUTTON_HEIGHT, (header.Height - BUTTON_HEIGHT) / 2, BUTTON_HEIGHT, BUTTON_HEIGHT);

        private bool isMouseOverBack;
        private bool isMouseOverForward;
        private bool isMouseOverHome;
        
        // Header 自动隐藏相关
// 已废弃：Header 自动隐藏相关字段
        // private bool isHeaderVisible = false;
        // private System.Windows.Forms.Timer headerHideTimer;
        // private const int HEADER_HIDE_DELAY = 2000;
        // private const int HEADER_SHOW_TRIGGER_Y = 16;
        // private const int HEADER_FULL_HEIGHT = 43;
        // private const int HEADER_HIDDEN_HEIGHT = 0;
        
        private const int HEADER_FULL_HEIGHT = 43; // header 固定高度

        private Color GetThemeColor(SkinElement element, ElementComponent component)
        {
            int colorInt = mbApiInterface.Setting_GetSkinElementColour(element, ElementState.ElementStateDefault, component);
            return ColorTranslator.FromWin32(colorInt);
        }

        private void DebugSkinColors()
        {
            Debug.WriteLine("========== Skin Element Colors Debug ==========");
            
            // 已知的 SkinElement ID
            HashSet<int> knownIds = new HashSet<int> { -1, 0, 7, 10, 14 };
            
            // 遍历 0-30 的所有整数值
            Debug.WriteLine("ID\tBG\tFG\tBDR");
            Debug.WriteLine("-------------------------------------------");
            
            for (int id = 0; id <= 30; id++)
            {
                // 跳过已知的 SkinElement
                // if (knownIds.Contains(id))
                // {
                //     continue;
                // }
                
                SkinElement testElement = (SkinElement)id;
                
                try
                {
                    Color bg = GetThemeColor(testElement, ElementComponent.ComponentBackground);
                    Color fg = GetThemeColor(testElement, ElementComponent.ComponentForeground);
                    Color bdr = GetThemeColor(testElement, ElementComponent.ComponentBorder);
                    
                    // 检查是否为有效配色（背景和前景不全是黑色）
                    if (bg.ToArgb() != Color.Black.ToArgb() || fg.ToArgb() != Color.Black.ToArgb())
                    {
                        Debug.WriteLine($"{id}\t#{bg.R:X2}{bg.G:X2}{bg.B:X2}\t#{fg.R:X2}{fg.G:X2}{fg.B:X2}\t#{bdr.R:X2}{bdr.G:X2}{bdr.B:X2}");
                    }
                }
                catch
                {
                    // 跳过无效的元素
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
                Debug.WriteLine("Browser2: Icon not found at " + iconPath);
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
                        
                        // 计算黑度（0-255，值越小越黑）
                        byte blackness = (byte)((255 - pixel.R + 255 - pixel.G + 255 - pixel.B) / 3);
                        
                        // 如果黑度大于 128（偏深色），则进行映射
                        if (blackness > 128)
                        {
                            // 按黑度比例调整前景色的亮度
                            byte newR = (byte)(fgColor.R * blackness / 255);
                            byte newG = (byte)(fgColor.G * blackness / 255);
                            byte newB = (byte)(fgColor.B * blackness / 255);
                            
                            // 保持原始 Alpha
                            themedIcon.SetPixel(x, y, Color.FromArgb(pixel.A, newR, newG, newB));
                        }
                        else
                        {
                            // 非深色像素保持不变
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
            // 重要：触发重绘，确保图标位置正确更新
            header.Invalidate();
        }

        private void ResizeHeader()
        {
            if (locationBar != null && header != null)
            {
                // 计算左右按钮区域的宽度
                int leftButtonWidth = (LEFT_MARGIN_BUTTONS + 3) * BUTTON_WIDTH;  // 左侧边距 + 3 个按钮
                int rightButtonWidth = (RIGHT_MARGIN_BUTTONS + 3) * BUTTON_WIDTH; // 右侧边距 + 3 个按钮
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
                if (string.IsNullOrEmpty(locationBar.Text))
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
                header.Invalidate(RefreshStopButtonBounds);
            }
            else if (BookmarkButtonBounds.Contains(e.Location))
            {
                if (!string.IsNullOrEmpty(locationBar.Text))
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
                    }
                }
            }
            else if (BookmarkListButtonBounds.Contains(e.Location))
            {
                ShowBookmarkMenu();
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

            var location = header.PointToScreen(new Point(header.Width - 50, header.Height));
            bookmarkContextMenu.Show(location);
        }

        private void RemoveFavourite(int index)
        {
            var bookmark = settings.Bookmarks[index];
            settings.Bookmarks.RemoveAt(index);
            SaveSettings();
            SetLocationBarText(locationBar.Text);
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

    if (e.KeyCode == Keys.Return && locationBar.Text.Length > 0)
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
            Debug.WriteLine("Browser2: Navigate called with URL: " + url);
            if (panel == null)
            {
                loadOnceUrl = url;
                return;
            }
            ShowNavigationTarget(url);
            browser.Focus();
            NavigateTo(url);
        }

        private void NavigateTo(string url)
        {
            try
            {
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
                    Debug.WriteLine("Browser2: NavigateTo - browser.CoreWebView2 is " + (browser?.CoreWebView2 == null ? "null" : "not null"));
                    if (browser?.CoreWebView2 != null)
                    {
                        Debug.WriteLine("Browser2: Calling CoreWebView2.Navigate(" + url + ")");
                        browser.CoreWebView2.Navigate(url);
                    }
                    else
                    {
                        Debug.WriteLine("Browser2: Setting browser.Source = " + url);
                        browser.Source = new Uri(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Navigation error: " + ex.Message);
                Debug.WriteLine("Stack trace: " + ex.StackTrace);
            }
        }

        private void Browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            isLoading = true;
            ShowNavigationTarget(e.Uri);
            header.Invalidate();
            Debug.WriteLine("Browser2: NavigationStarting - " + e.Uri);
        }

        private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            isLoading = false;
            activeUrl = browser.Source?.ToString();
            header.Invalidate();
            Debug.WriteLine("Browser2: NavigationCompleted - " + (e.IsSuccess ? "Success" : "Failed: " + e.WebErrorStatus));
        }

        private void Browser_ZoomFactorChanged(object sender, EventArgs e)
        {
            if (browser != null && settings.AutoSaveZoom)
            {
                settings.ZoomFactor = browser.ZoomFactor;
                isSettingsDirty = true;
                Debug.WriteLine("Browser2: ZoomFactor changed to " + browser.ZoomFactor);
            }
        }

        private void Browser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (browser.Source != null)
            {
                string newUrl = browser.Source.ToString();
                SetLocationBarText(newUrl);
                activeUrl = newUrl;
                Debug.WriteLine("Browser2: SourceChanged - " + newUrl);
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
            Debug.WriteLine("Browser2: CloseBrowser called");
            SaveSettings();
            
            // 记录状态：浏览器不应该可见
            shouldBrowserBeVisible = false;
            Debug.WriteLine($"Browser2: Set shouldBrowserBeVisible = false");
            
            // 设置 WebView2 不可见，停止渲染
            if (browser != null)
            {
                browser.Visible = false;
                Debug.WriteLine("Browser2: Set WebView2 Visible = false");
            }
            
            // 不调用 MB_RemovePanel，让 MusicBee 自己管理面板的显示/隐藏
            // 这样切换标签页时可以保持网页状态
            // 同时保留 activeUrl 和 locationBar.Text，以便切换回来时显示
            
            // 重置状态（但保留 activeUrl 和 locationBar.Text）
            isLoading = false;
            faviconImage?.Dispose();
            faviconImage = null;
            currentIsFavourite = false;
            

        }

        private void ScanStarting(object sender, EventArgs e)
        {
        }

        private void ItemScanned(MediaFile file)
        {
        }

        private void ScanCompleted(object sender, EventArgs e)
        {
        }

        private class MediaFile
        {
            public string Url;
            public string Status;
            public string Artist;
            public string Title;
            public string Album;
            public string Size;
            public string Duration;
            public string Origin;
        }
    }
}