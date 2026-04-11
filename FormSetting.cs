using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class FormSetting : Form
    {
        private BrowserSettings settings;
        private string originalDefaultUrl;
        private bool originalAutoSaveZoom;
        private bool originalShowAddressBar;
        private DarkModeType originalDarkMode;
        private bool originalEnableExtensions;
        private string originalUrlDecodeChars;
        public bool SettingsChanged { get; private set; }

        private string extensionsFolderPath;
        public string ExtensionsFolderPath => extensionsFolderPath;
        private Microsoft.Web.WebView2.Core.CoreWebView2Profile profile;

        public FormSetting(BrowserSettings settings, string storagePath, Microsoft.Web.WebView2.Core.CoreWebView2Profile webviewProfile = null)
        {
            InitializeComponent();
            this.settings = settings;
            this.extensionsFolderPath = Path.Combine(storagePath, "Browser2Extensions");
            this.profile = webviewProfile;
            this.originalDefaultUrl = settings.DefaultUrl;
            this.originalAutoSaveZoom = settings.AutoSaveZoom;
            this.originalShowAddressBar = settings.ShowAddressBar;
            this.originalDarkMode = settings.DarkMode;
            this.originalEnableExtensions = settings.EnableExtensions;
            this.originalUrlDecodeChars = settings.UrlDecodeChars ?? "";
            this.SettingsChanged = false;

            txtDefaultUrl.Text = settings.DefaultUrl ?? "";
            chkAutoSaveZoom.Checked = settings.AutoSaveZoom;
            chkShowAddressBar.Checked = settings.ShowAddressBar;
            cmbDarkMode.SelectedIndex = (int)settings.DarkMode;
            chkEnableExtensions.Checked = settings.EnableExtensions;
            txtUrlDecodeChars.Text = settings.UrlDecodeChars ?? "";

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnOpenExtensionsFolder.Click += BtnOpenExtensionsFolder_Click;
            btnInstallUnpackedExtension.Click += BtnInstallUnpackedExtension_Click;
            btnUninstallExtension.Click += BtnUninstallExtension_Click;
            // btnToggleExtension.Click += BtnToggleExtension_Click;  // Hidden for now
            
            lstExtensions.SelectedIndexChanged += LstExtensions_SelectedIndexChanged;
            
            LoadInstalledExtensions();
        }

        private async void LoadInstalledExtensions()
        {
            lstExtensions.Items.Clear();
            
            if (!Directory.Exists(extensionsFolderPath))
            {
                return;
            }

            var extensions = await ExtensionManager.GetAllExtensionsAsync(profile, extensionsFolderPath);
            
            foreach (var ext in extensions)
            {
                var item = new ListViewItem(ext.Name);
                item.SubItems.Add(ext.Version);
                item.SubItems.Add(ext.IsEnabled ? Strings.Yes : Strings.No);
                item.Tag = ext.Path;
                lstExtensions.Items.Add(item);
            }
            
            UpdateButtonStates();
        }
        
        private void LstExtensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }
        
        private void UpdateButtonStates()
        {
            bool hasSelection = lstExtensions.SelectedItems.Count > 0;
            btnUninstallExtension.Enabled = hasSelection;
            // btnToggleExtension.Enabled = hasSelection;  // Hidden for now
        }
        
        private void BtnToggleExtension_Click(object sender, EventArgs e)
        {
            if (lstExtensions.SelectedItems.Count == 0)
            {
                return;
            }
            
            var selectedItem = lstExtensions.SelectedItems[0];
            string extensionPath = selectedItem.Tag as string;
            string enabledText = selectedItem.SubItems[2].Text;
            bool isEnabled = enabledText == Strings.Yes;
            
            try
            {
                bool success = ExtensionManager.ToggleExtension(extensionPath, !isEnabled);
                
                if (success)
                {
                    MessageBox.Show(
                        isEnabled ? Strings.ExtensionDisabled : Strings.ExtensionEnabledMessage, 
                        Strings.FormTitle, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    
                    MessageBox.Show(
                        Strings.RestartRequiredForExtensionChanges,
                        Strings.FormTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    
                    LoadInstalledExtensions();
                }
                else
                {
                    MessageBox.Show(Strings.ToggleExtensionFailed, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Toggle extension error: " + ex.Message);
                MessageBox.Show(Strings.ToggleExtensionFailed + " " + ex.Message, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private string GetExtensionLocalizedName(string extensionDir, string messageKey)
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            settings.DefaultUrl = txtDefaultUrl.Text.Trim();
            settings.AutoSaveZoom = chkAutoSaveZoom.Checked;
            settings.ShowAddressBar = chkShowAddressBar.Checked;
            settings.DarkMode = (DarkModeType)cmbDarkMode.SelectedIndex;
            settings.EnableExtensions = chkEnableExtensions.Checked;
            settings.UrlDecodeChars = txtUrlDecodeChars.Text.Trim();

            SettingsChanged = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnOpenExtensionsFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(extensionsFolderPath))
            {
                Directory.CreateDirectory(extensionsFolderPath);
            }
            Process.Start("explorer.exe", extensionsFolderPath);
        }

        private void BtnInstallUnpackedExtension_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = Strings.SelectExtensionFolder;
                folderDialog.ShowNewFolderButton = false;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = folderDialog.SelectedPath;

                    try
                    {
                        ExtensionManager.InstallExtension(sourcePath, extensionsFolderPath);
                        LoadInstalledExtensions();
                        MessageBox.Show(Strings.ExtensionInstalled, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Strings.ExtensionInstallFailed + " " + ex.Message, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnUninstallExtension_Click(object sender, EventArgs e)
        {
            if (lstExtensions.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = lstExtensions.SelectedItems[0];
            string extensionPath = selectedItem.Tag as string;
            string extensionName = selectedItem.Text;

            if (MessageBox.Show(Strings.ConfirmUninstall, Strings.FormTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    ExtensionManager.UninstallExtension(extensionPath);
                    LoadInstalledExtensions();
                    MessageBox.Show(Strings.ExtensionUninstalled, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Strings.ExtensionUninstallFailed + " " + ex.Message, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LinkProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = Strings.ProjectLinkUrl;
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                linkProject.LinkVisited = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Open project link error: " + ex.Message);
                MessageBox.Show("Failed to open project link: " + ex.Message, Strings.FormTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
