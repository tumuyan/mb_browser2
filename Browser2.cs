using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Resources;
using MusicBeePlugin.Properties;

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
        private string defaultUrl;

        private Bitmap faviconImage;
        private bool currentIsFavourite;

        private List<Bookmark> favourites = new List<Bookmark>();
        private int settingsVersion = 3;
        private bool isSettingsDirty;

        private ToolStripMenuItem playNowMenuItem;
        private ToolStripMenuItem queueNextMenuItem;
        private ToolStripMenuItem queueLastMenuItem;

        private EventHandler openHandler;
        private EventHandler closeHandler;

        private struct Bookmark
        {
            public string Url;
            public string Name;
            public Bitmap Icon;
            public bool Custom;

            public Bookmark(string url, string name, Bitmap icon, bool custom = false)
            {
                Url = url;
                Name = name;
                Icon = icon;
                Custom = custom;
            }
        }

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            openHandler = OpenBrowser;
            closeHandler = CloseBrowser;
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Browser2";
            about.Description = "A modern web browser based on WebView2";
            about.Author = "tumuyan";
            about.TargetApplication = "";
            about.Type = PluginType.WebBrowser;
            about.VersionMajor = 1;
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;
            mbApiInterface.MB_Trace("Browser2: Initialise completed");
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "Default URL:";
                TextBox textBox = new TextBox();
                textBox.Bounds = new Rectangle(80, 0, 300, textBox.Height);
                textBox.Text = defaultUrl ?? "";
                textBox.TextChanged += (s, e) => { defaultUrl = textBox.Text; isSettingsDirty = true; };
                configPanel.Controls.AddRange(new Control[] { prompt, textBox });
            }
            return false;
        }

        public void SaveSettings()
        {
            string path = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.dat";
            try
            {
                using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                using (var writer = new System.IO.BinaryWriter(fs))
                {
                    writer.Write(settingsVersion);
                    writer.Write(defaultUrl ?? "");
                    writer.Write(favourites.Count);
                    foreach (var fav in favourites)
                    {
                        writer.Write(fav.Url);
                        writer.Write(string.IsNullOrEmpty(fav.Name) ? "No Title" : fav.Name);
                        if (fav.Icon != null)
                        {
                            using (var ms = new System.IO.MemoryStream())
                            {
                                fav.Icon.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var bytes = ms.ToArray();
                                writer.Write(bytes.Length);
                                writer.Write(bytes);
                            }
                        }
                        else
                        {
                            writer.Write(0);
                        }
                    }
                }
            }
            catch { }
            isSettingsDirty = false;
        }

        private string LoadSettings()
        {
            Debug.WriteLine("加载设置");
            string result = null;
            string path = mbApiInterface.Setting_GetPersistentStoragePath() + "Browser2Settings.dat";
            if (System.IO.File.Exists(path))
            {
                try
                {
                    using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    using (var reader = new System.IO.BinaryReader(fs))
                    {
                        settingsVersion = reader.ReadInt32();
                        result = defaultUrl = reader.ReadString();
                        int count = reader.ReadInt32();
                        favourites.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            var bookmark = new Bookmark
                            {
                                Url = reader.ReadString(),
                                Name = reader.ReadString()
                            };
                            int iconSize = reader.ReadInt32();
                            if (iconSize > 0)
                            {
                                byte[] iconData = reader.ReadBytes(iconSize);
                                using (var ms = new System.IO.MemoryStream(iconData))
                                {
                                    bookmark.Icon = new Bitmap(ms);
                                }
                            }
                            favourites.Add(bookmark);
                        }
                    }
                }
                catch { }
            }
            if (settingsVersion < 3)
            {
                InitializeV3Favorites();
                result = "http://en.wikipedia.org/wiki/Special:Search?search=<artist>";
            }
            return result;
        }

        private void InitializeV3Favorites()
        {
            if (settingsVersion < 3)
            {
                settingsVersion = 3;
            }
        }

        public void Close(PluginCloseReason reason)
        {
            if (isSettingsDirty)
            {
                SaveSettings();
            }
        }

        public void Uninstall()
        {
        }

        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.PluginStartup:
                    mbApiInterface.MB_Trace("Browser2: PluginStartup");
                    mbApiInterface.MB_Trace("Browser2: API Revision = " + mbApiInterface.ApiRevision);
                    //MessageBox.Show("Browser2: PluginStartup! API=" + mbApiInterface.ApiRevision, "Browser2");
                    try
                    {
                        mbApiInterface.MB_Trace("Browser2: Adding tree node");
                        using (var bitmap = Resources.icon_ie.ToBitmap())
                        {
                            mbApiInterface.MB_AddTreeNode("Services", "Browser2", bitmap, openHandler, closeHandler);
                        }
                        mbApiInterface.MB_AddMenuItem("mnuTools/Browser2", "Browser2", openHandler);
                    }
                    catch (Exception ex)
                    {
                        mbApiInterface.MB_Trace("Browser2: MenuItem exception: " + ex.Message);
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
            Debug.WriteLine("开始 OpenBrowser");
            mbApiInterface.MB_Trace("Browser2: OpenBrowser called");
            string text = null;
            if (browser == null || browser.CoreWebView2 == null)
            {
                text = LoadSettings();
                mbApiInterface.MB_Trace("Browser2: LoadSettings returned: " + (text ?? "null"));
                browser = null;
                panel = null;
                InitializeBrowser();
            }
            else if (panel != null && !panel.IsDisposed)
            {
                AddPanelToMusicBee();
            }
            if (string.IsNullOrEmpty(text))
            {
                text = activeUrl;
            }
            if (!string.IsNullOrEmpty(text))
            {
                mbApiInterface.MB_Trace("Browser2: Calling NavigateTo with: " + text);
                pendingUrl = text;
                TryNavigate();
            }
        }

        private string pendingUrl;

        private void InitializeBrowser()
        {
            mbApiInterface.MB_Trace("Browser2: InitializeBrowser started");
            Font font = mbApiInterface.Setting_GetDefaultFont();

            panel = new UserControl();
            panel.AutoScroll = false;
            mbApiInterface.MB_Trace("Browser2: UserControl panel created");

            if (string.IsNullOrEmpty(defaultUrl))
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
            locationBar.BorderStyle = BorderStyle.None;
            locationBar.BackColor = Color.White;
            locationBar.ForeColor = Color.Black;
            locationBar.Font = font;
            locationBar.TabStop = true;
            locationBar.KeyDown += LocationBar_KeyDown;

            header = new Control();
            header.Height = 43;
            header.Controls.Add(locationBarPrompt ?? new Control());
            header.Controls.Add(locationBar);
            header.Dock = DockStyle.Top;
            header.TabStop = false;
            header.Paint += Header_Paint;
            header.Resize += Header_Resize;
            header.MouseMove += Header_MouseMove;
            header.MouseClick += Header_MouseClick;

            browser = new WebView2();
            browser.Dock = DockStyle.Fill;
            browser.TabStop = false;
            browser.NavigationStarting += Browser_NavigationStarting;
            browser.NavigationCompleted += Browser_NavigationCompleted;
            browser.SourceChanged += Browser_SourceChanged;

            panel.Controls.Add(browser);
            panel.Controls.Add(header);
            panel.TabStop = false;

            ResizeHeader();
            mbApiInterface.MB_Trace("Browser2: InitializeBrowser completed, initializing WebView2");

            InitializeWebView2AndAddPanel();
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
                    webViewEnvironment = await CoreWebView2Environment.CreateAsync();
                    System.Diagnostics.Trace.WriteLine("WebView2 Environment created");
                }
                await browser.EnsureCoreWebView2Async(webViewEnvironment);
                System.Diagnostics.Trace.WriteLine("WebView2 Initialize completed");
                browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                mbApiInterface.MB_Trace("Browser2: WebView2 initialized");
                AddPanelToMusicBee();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("WebView2 init error: " + ex);
                mbApiInterface.MB_Trace("Browser2: WebView2 init error: " + ex.Message);
                MessageBox.Show("WebView2 init error: " + ex.Message, "Browser2 Error");
            }
        }

        private void AddPanelToMusicBee()
        {
            mbApiInterface.MB_AddPanel(panel, PluginPanelDock.MainPanel);
            mbApiInterface.MB_Trace("Browser2: Panel added to MainPanel");
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
                System.Diagnostics.Trace.WriteLine("WebView2 not ready, reinitializing");
                InitializeWebView2AndAddPanel();
                return;
            }
            System.Diagnostics.Trace.WriteLine("WebView2 ready, attempting navigation");
            if (string.IsNullOrEmpty(pendingUrl))
            {
                return;
            }
            System.Diagnostics.Trace.WriteLine("开始 NavigateTo " + pendingUrl);
            NavigateTo(pendingUrl);
            pendingUrl = null;
        }

        private Rectangle FavoritesHighlightBounds => new Rectangle(12, 9, 24, 24);
        private Rectangle BrowseBackButtonBounds => new Rectangle(54, 8, 28, 29);
        private Rectangle BrowseForwardButtonBounds => new Rectangle(81, 10, 25, 20);
        private Rectangle RefreshStopButtonBounds => new Rectangle(header.Width - 50 - 20, 13, 16, 16);
        private Rectangle BookmarkHighlightBounds => new Rectangle(header.Width - 36, 9, 24, 24);
        private Rectangle BookmarkButtonBounds => new Rectangle(header.Width - 32, 13, 16, 16);

        private bool isMouseOverBack;
        private bool isMouseOverForward;
        private bool isMouseOverBookmark;

        private void LoadBrowserImages()
        {
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(new Rectangle(0, 0, 1, header.Height - 1), Color.FromArgb(246, 248, 250), Color.FromArgb(235, 237, 239), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, header.Width, header.Height - 1));
            }
            using (var pen = new Pen(Color.FromArgb(187, 190, 193)))
            {
                e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            }

            if (browser.CanGoBack)
            {
                Rectangle backBounds = BrowseBackButtonBounds;
                e.Graphics.DrawString("<", new Font("Arial", 10), Brushes.DarkGray, backBounds.Location);
            }

            if (browser.CanGoForward)
            {
                Rectangle forwardBounds = BrowseForwardButtonBounds;
                e.Graphics.DrawString(">", new Font("Arial", 10), Brushes.DarkGray, forwardBounds.Location);
            }

            if (isMouseOverBookmark)
            {
                Rectangle bmBounds = BookmarkHighlightBounds;
                using (var brush = new LinearGradientBrush(new Rectangle(0, 0, 1, header.Height - 1), Color.FromArgb(251, 253, 255), Color.FromArgb(245, 247, 249), LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, bmBounds);
                }
                bmBounds.Width--;
                bmBounds.Height--;
                using (var pen = new Pen(Color.FromArgb(210, 210, 210)))
                {
                    e.Graphics.DrawRectangle(pen, bmBounds);
                }
            }

            string bookmarkSymbol = currentIsFavourite ? "*" : "";
            e.Graphics.DrawString(bookmarkSymbol, new Font("Arial", 12), Brushes.Blue, BookmarkButtonBounds.Location);

            e.Graphics.FillRectangle(Brushes.White, new Rectangle(107, 10, header.Width - 107 - 50, 22));

            if (faviconImage != null)
            {
                e.Graphics.DrawImage(faviconImage, new Rectangle(110, 13, 16, 16));
            }

            string refreshSymbol = isLoading ? "X" : "R";
            e.Graphics.DrawString(refreshSymbol, new Font("Arial", 10), Brushes.DarkGray, RefreshStopButtonBounds.Location);

            using (var pen = new Pen(Color.FromArgb(188, 190, 205)))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(106, 9, header.Width - 106 - 50, 23));
            }
        }

        private void Header_Resize(object sender, EventArgs e)
        {
            ResizeHeader();
        }

        private void ResizeHeader()
        {
            if (locationBar != null && header != null)
            {
                locationBar.Bounds = new Rectangle(129, (header.Height - locationBar.Font.Height) / 2, header.Width - 129 - 50 - 20 - 5, locationBar.Font.Height);
                if (locationBarPrompt != null)
                {
                    locationBarPrompt.Bounds = new Rectangle(locationBar.Left + 1, locationBar.Top, locationBar.Width - 1, locationBar.Height);
                }
            }
        }

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            bool needInvalidate = false;

            if (BrowseBackButtonBounds.Contains(e.Location) && browser.CanGoBack)
            {
                if (!isMouseOverBack)
                {
                    isMouseOverBack = true;
                    needInvalidate = true;
                }
            }
            else if (isMouseOverBack)
            {
                isMouseOverBack = false;
                needInvalidate = true;
            }

            if (BrowseForwardButtonBounds.Contains(e.Location) && browser.CanGoForward)
            {
                if (!isMouseOverForward)
                {
                    isMouseOverForward = true;
                    needInvalidate = true;
                }
            }
            else if (isMouseOverForward)
            {
                isMouseOverForward = false;
                needInvalidate = true;
            }

            if (BookmarkHighlightBounds.Contains(e.Location))
            {
                if (!isMouseOverBookmark)
                {
                    isMouseOverBookmark = true;
                    needInvalidate = true;
                }
            }
            else if (isMouseOverBookmark)
            {
                isMouseOverBookmark = false;
                needInvalidate = true;
            }

            if (needInvalidate)
            {
                header.Invalidate();
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
            else if (BookmarkHighlightBounds.Contains(e.Location))
            {
                if (!string.IsNullOrEmpty(locationBar.Text))
                {
                    bool found = false;
                    for (int i = 0; i < favourites.Count; i++)
                    {
                        if (string.Compare(favourites[i].Url, locationBar.Text, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            found = true;
                            RemoveFavourite(i);
                            break;
                        }
                    }
                    if (!found)
                    {
                        string title = browser.CoreWebView2?.DocumentTitle ?? "No title";
                        favourites.Add(new Bookmark(locationBar.Text, title, faviconImage != null ? (Bitmap)faviconImage.Clone() : null));
                        SaveSettings();
                        mbApiInterface.MB_SetBackgroundTaskMessage("Bookmark has been saved");
                        currentIsFavourite = true;
                        header.Invalidate(BookmarkButtonBounds);
                    }
                }
            }
        }

        private void RemoveFavourite(int index)
        {
            var bookmark = favourites[index];
            favourites.RemoveAt(index);
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

        public void Navigate(string url)
        {
            if (panel == null)
            {
                defaultUrl = url;
                return;
            }
            ShowNavigationTarget(url);
            browser.Focus();
            NavigateTo(locationBar.Text);
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

                    Debug.WriteLine("开始 NavigateTo " + url);
                    browser.Source = new Uri(url);
                }
            }
            catch (Exception ex)
            {
                mbApiInterface.MB_Trace("Navigation error: " + ex.Message);
            }
        }

        private void Browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            isLoading = true;
            ShowNavigationTarget(e.Uri);
            header.Invalidate();
            mbApiInterface.MB_Trace("Browser2: NavigationStarting - " + e.Uri);
        }

        private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            isLoading = false;
            activeUrl = browser.Source?.ToString();
            header.Invalidate();
            mbApiInterface.MB_Trace("Browser2: NavigationCompleted - " + (e.IsSuccess ? "Success" : "Failed: " + e.WebErrorStatus));
        }

        private void Browser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (browser.Source != null)
            {
                SetLocationBarText(browser.Source.ToString());
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

            foreach (var fav in favourites)
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
            SaveSettings();
            if (panel != null)
            {
                mbApiInterface.MB_RemovePanel(panel);
            }
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