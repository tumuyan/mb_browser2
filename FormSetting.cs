using System;
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
        public bool SettingsChanged { get; private set; }

        public FormSetting(BrowserSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            this.originalDefaultUrl = settings.DefaultUrl;
            this.originalAutoSaveZoom = settings.AutoSaveZoom;
            this.originalShowAddressBar = settings.ShowAddressBar;
            this.originalDarkMode = settings.DarkMode;
            this.SettingsChanged = false;

            txtDefaultUrl.Text = settings.DefaultUrl ?? "";
            chkAutoSaveZoom.Checked = settings.AutoSaveZoom;
            chkShowAddressBar.Checked = settings.ShowAddressBar;
            cmbDarkMode.SelectedIndex = (int)settings.DarkMode;

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            settings.DefaultUrl = txtDefaultUrl.Text.Trim();
            settings.AutoSaveZoom = chkAutoSaveZoom.Checked;
            settings.ShowAddressBar = chkShowAddressBar.Checked;
            settings.DarkMode = (DarkModeType)cmbDarkMode.SelectedIndex;

            if (settings.DefaultUrl != originalDefaultUrl || 
                settings.AutoSaveZoom != originalAutoSaveZoom ||
                settings.ShowAddressBar != originalShowAddressBar ||
                settings.DarkMode != originalDarkMode)
            {
                SettingsChanged = true;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
