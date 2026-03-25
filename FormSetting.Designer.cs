namespace MusicBeePlugin
{
    partial class FormSetting
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblHomePage = new System.Windows.Forms.Label();
            this.txtDefaultUrl = new System.Windows.Forms.TextBox();
            this.chkAutoSaveZoom = new System.Windows.Forms.CheckBox();
            this.chkShowAddressBar = new System.Windows.Forms.CheckBox();
            this.lblDarkMode = new System.Windows.Forms.Label();
            this.cmbDarkMode = new System.Windows.Forms.ComboBox();
            this.chkEnableExtensions = new System.Windows.Forms.CheckBox();
            this.btnOpenExtensionsFolder = new System.Windows.Forms.Button();
            this.btnInstallUnpackedExtension = new System.Windows.Forms.Button();
            this.lblInstalledExtensions = new System.Windows.Forms.Label();
            this.lstExtensions = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            // this.btnToggleExtension = new System.Windows.Forms.Button();  // Hidden for now
            this.btnUninstallExtension = new System.Windows.Forms.Button();
            this.lblRestartHint = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHomePage
            // 
            this.lblHomePage.AutoSize = true;
            this.lblHomePage.Location = new System.Drawing.Point(12, 15);
            this.lblHomePage.Name = "lblHomePage";
            this.lblHomePage.Size = new System.Drawing.Size(70, 15);
            this.lblHomePage.Text = Strings.HomePage;
            // 
            // txtDefaultUrl
            // 
            this.txtDefaultUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultUrl.Location = new System.Drawing.Point(88, 12);
            this.txtDefaultUrl.Name = "txtDefaultUrl";
            this.txtDefaultUrl.Size = new System.Drawing.Size(380, 23);
            this.txtDefaultUrl.TabIndex = 1;
            // 
            // chkAutoSaveZoom
            // 
            this.chkAutoSaveZoom.AutoSize = true;
            this.chkAutoSaveZoom.Location = new System.Drawing.Point(15, 45);
            this.chkAutoSaveZoom.Name = "chkAutoSaveZoom";
            this.chkAutoSaveZoom.Size = new System.Drawing.Size(130, 19);
            this.chkAutoSaveZoom.Text = Strings.AutoSaveZoom;
            this.chkAutoSaveZoom.UseVisualStyleBackColor = true;
            // 
            // chkShowAddressBar
            // 
            this.chkShowAddressBar.AutoSize = true;
            this.chkShowAddressBar.Location = new System.Drawing.Point(15, 70);
            this.chkShowAddressBar.Name = "chkShowAddressBar";
            this.chkShowAddressBar.Size = new System.Drawing.Size(100, 19);
            this.chkShowAddressBar.Text = Strings.ShowAddressBar;
            this.chkShowAddressBar.UseVisualStyleBackColor = true;
            // 
            // lblDarkMode
            // 
            this.lblDarkMode.AutoSize = true;
            this.lblDarkMode.Location = new System.Drawing.Point(12, 100);
            this.lblDarkMode.Name = "lblDarkMode";
            this.lblDarkMode.Size = new System.Drawing.Size(60, 15);
            this.lblDarkMode.Text = Strings.DarkModeLabel;
            // 
            // cmbDarkMode
            // 
            this.cmbDarkMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDarkMode.FormattingEnabled = true;
            this.cmbDarkMode.Items.AddRange(new object[] {
                Strings.DarkModeDefault,
                Strings.DarkModeDark,
                Strings.DarkModeMusicBeeTheme});
            this.cmbDarkMode.Location = new System.Drawing.Point(88, 97);
            this.cmbDarkMode.Name = "cmbDarkMode";
            this.cmbDarkMode.Size = new System.Drawing.Size(120, 23);
            this.cmbDarkMode.TabIndex = 4;
            // 
            // chkEnableExtensions
            // 
            this.chkEnableExtensions.AutoSize = true;
            this.chkEnableExtensions.Location = new System.Drawing.Point(15, 128);
            this.chkEnableExtensions.Name = "chkEnableExtensions";
            this.chkEnableExtensions.Size = new System.Drawing.Size(150, 19);
            this.chkEnableExtensions.Text = Strings.EnableExtensions;
            this.chkEnableExtensions.UseVisualStyleBackColor = true;
            // 
            // btnOpenExtensionsFolder
            // 
            this.btnOpenExtensionsFolder.Location = new System.Drawing.Point(12, 155);
            this.btnOpenExtensionsFolder.Name = "btnOpenExtensionsFolder";
            this.btnOpenExtensionsFolder.Size = new System.Drawing.Size(110, 25);
            this.btnOpenExtensionsFolder.TabIndex = 5;
            this.btnOpenExtensionsFolder.Text = Strings.OpenExtensionsFolder;
            this.btnOpenExtensionsFolder.UseVisualStyleBackColor = true;
            // 
            // btnInstallUnpackedExtension
            // 
            this.btnInstallUnpackedExtension.Location = new System.Drawing.Point(128, 155);
            this.btnInstallUnpackedExtension.Name = "btnInstallUnpackedExtension";
            this.btnInstallUnpackedExtension.Size = new System.Drawing.Size(130, 25);
            this.btnInstallUnpackedExtension.TabIndex = 6;
            this.btnInstallUnpackedExtension.Text = Strings.InstallUnpackedExtension;
            this.btnInstallUnpackedExtension.UseVisualStyleBackColor = true;
            // 
            // lblInstalledExtensions
            // 
            this.lblInstalledExtensions.AutoSize = true;
            this.lblInstalledExtensions.Location = new System.Drawing.Point(12, 188);
            this.lblInstalledExtensions.Name = "lblInstalledExtensions";
            this.lblInstalledExtensions.Size = new System.Drawing.Size(100, 15);
            this.lblInstalledExtensions.Text = Strings.InstalledExtensions;
            // 
            // lstExtensions
            // 
            this.lstExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstExtensions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colName,
                this.colVersion,
                this.colEnabled});
            this.lstExtensions.FullRowSelect = true;
            this.lstExtensions.HideSelection = false;
            this.lstExtensions.Location = new System.Drawing.Point(12, 210);
            this.lstExtensions.MultiSelect = false;
            this.lstExtensions.Name = "lstExtensions";
            this.lstExtensions.Size = new System.Drawing.Size(380, 120);
            this.lstExtensions.TabIndex = 8;
            this.lstExtensions.UseCompatibleStateImageBehavior = false;
            this.lstExtensions.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = Strings.ExtensionName;
            this.colName.Width = 260;
            // 
            // colVersion
            // 
            this.colVersion.Text = Strings.ExtensionVersion;
            this.colVersion.Width = 80;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = Strings.ExtensionEnabled;
            this.colEnabled.Width = 60;
            // 
            // btnToggleExtension (Hidden for now)
            // 
            // this.btnToggleExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // this.btnToggleExtension.Location = new System.Drawing.Point(398, 241);
            // this.btnToggleExtension.Name = "btnToggleExtension";
            // this.btnToggleExtension.Size = new System.Drawing.Size(70, 25);
            // this.btnToggleExtension.TabIndex = 10;
            // this.btnToggleExtension.Text = Strings.DisableExtension;
            // this.btnToggleExtension.UseVisualStyleBackColor = true;
            // 
            // btnUninstallExtension
            // 
            this.btnUninstallExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUninstallExtension.Location = new System.Drawing.Point(398, 210);
            this.btnUninstallExtension.Name = "btnUninstallExtension";
            this.btnUninstallExtension.Size = new System.Drawing.Size(70, 25);
            this.btnUninstallExtension.TabIndex = 9;
            this.btnUninstallExtension.Text = Strings.Uninstall;
            this.btnUninstallExtension.UseVisualStyleBackColor = true;
            // 
            // lblRestartHint
            // 
            this.lblRestartHint.AutoSize = true;
            this.lblRestartHint.ForeColor = System.Drawing.Color.Gray;
            this.lblRestartHint.Location = new System.Drawing.Point(12, 338);
            this.lblRestartHint.Name = "lblRestartHint";
            this.lblRestartHint.Size = new System.Drawing.Size(200, 15);
            this.lblRestartHint.Text = Strings.RestartHint;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(308, 358);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 25);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = Strings.Save;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(394, 358);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = Strings.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormSetting
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(480, 395);
            // this.Controls.Add(this.btnToggleExtension);  // Hidden for now
            this.Controls.Add(this.btnUninstallExtension);
            this.Controls.Add(this.lstExtensions);
            this.Controls.Add(this.lblInstalledExtensions);
            this.Controls.Add(this.btnInstallUnpackedExtension);
            this.Controls.Add(this.btnOpenExtensionsFolder);
            this.Controls.Add(this.chkEnableExtensions);
            this.Controls.Add(this.cmbDarkMode);
            this.Controls.Add(this.lblDarkMode);
            this.Controls.Add(this.lblRestartHint);
            this.Controls.Add(this.chkShowAddressBar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkAutoSaveZoom);
            this.Controls.Add(this.txtDefaultUrl);
            this.Controls.Add(this.lblHomePage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = Strings.FormTitle;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHomePage;
        private System.Windows.Forms.TextBox txtDefaultUrl;
        private System.Windows.Forms.CheckBox chkAutoSaveZoom;
        private System.Windows.Forms.CheckBox chkShowAddressBar;
        private System.Windows.Forms.Label lblDarkMode;
        private System.Windows.Forms.ComboBox cmbDarkMode;
        private System.Windows.Forms.CheckBox chkEnableExtensions;
        private System.Windows.Forms.Button btnOpenExtensionsFolder;
        private System.Windows.Forms.Button btnInstallUnpackedExtension;
        private System.Windows.Forms.Label lblInstalledExtensions;
        private System.Windows.Forms.ListView lstExtensions;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.Button btnToggleExtension;
        private System.Windows.Forms.Button btnUninstallExtension;
        private System.Windows.Forms.Label lblRestartHint;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
