﻿﻿﻿﻿using System;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class FormSetting : Form
    {
        private BrowserSettings settings;
        private string originalDefaultUrl;
        private bool originalAutoSaveZoom;
        public bool SettingsChanged { get; private set; }

        public FormSetting(BrowserSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            this.originalDefaultUrl = settings.DefaultUrl;
            this.originalAutoSaveZoom = settings.AutoSaveZoom;
            this.SettingsChanged = false;

            txtDefaultUrl.Text = settings.DefaultUrl ?? "";
            chkAutoSaveZoom.Checked = settings.AutoSaveZoom;

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            settings.DefaultUrl = txtDefaultUrl.Text.Trim();
            settings.AutoSaveZoom = chkAutoSaveZoom.Checked;

            if (settings.DefaultUrl != originalDefaultUrl || 
                settings.AutoSaveZoom != originalAutoSaveZoom)
            {
                SettingsChanged = true;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // settings.DefaultUrl = originalDefaultUrl;
            // settings.AutoSaveZoom = originalAutoSaveZoom;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
