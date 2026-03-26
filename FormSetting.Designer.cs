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
            this.btnUninstallExtension = new System.Windows.Forms.Button();
            this.lblRestartHint = new System.Windows.Forms.Label();
            this.linkProject = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHomePage
            // 
            this.lblHomePage.AutoSize = true;
            this.lblHomePage.Location = new System.Drawing.Point(15, 18);
            this.lblHomePage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHomePage.Name = "lblHomePage";
            this.lblHomePage.Size = new System.Drawing.Size(98, 18);
            this.lblHomePage.TabIndex = 16;
            this.lblHomePage.Text = global::MusicBeePlugin.Strings.HomePage;
            // 
            // txtDefaultUrl
            // 
            this.txtDefaultUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultUrl.Location = new System.Drawing.Point(113, 14);
            this.txtDefaultUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDefaultUrl.Name = "txtDefaultUrl";
            this.txtDefaultUrl.Size = new System.Drawing.Size(487, 28);
            this.txtDefaultUrl.TabIndex = 1;
            // 
            // chkAutoSaveZoom
            // 
            this.chkAutoSaveZoom.AutoSize = true;
            this.chkAutoSaveZoom.Location = new System.Drawing.Point(19, 54);
            this.chkAutoSaveZoom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoSaveZoom.Name = "chkAutoSaveZoom";
            this.chkAutoSaveZoom.Size = new System.Drawing.Size(223, 22);
            this.chkAutoSaveZoom.TabIndex = 15;
            this.chkAutoSaveZoom.Text = global::MusicBeePlugin.Strings.AutoSaveZoom;
            this.chkAutoSaveZoom.UseVisualStyleBackColor = true;
            // 
            // chkShowAddressBar
            // 
            this.chkShowAddressBar.AutoSize = true;
            this.chkShowAddressBar.Location = new System.Drawing.Point(19, 84);
            this.chkShowAddressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowAddressBar.Name = "chkShowAddressBar";
            this.chkShowAddressBar.Size = new System.Drawing.Size(178, 22);
            this.chkShowAddressBar.TabIndex = 14;
            this.chkShowAddressBar.Text = global::MusicBeePlugin.Strings.ShowAddressBar;
            this.chkShowAddressBar.UseVisualStyleBackColor = true;
            // 
            // lblDarkMode
            // 
            this.lblDarkMode.AutoSize = true;
            this.lblDarkMode.Location = new System.Drawing.Point(15, 120);
            this.lblDarkMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDarkMode.Name = "lblDarkMode";
            this.lblDarkMode.Size = new System.Drawing.Size(98, 18);
            this.lblDarkMode.TabIndex = 12;
            this.lblDarkMode.Text = global::MusicBeePlugin.Strings.DarkModeLabel;
            // 
            // cmbDarkMode
            // 
            this.cmbDarkMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDarkMode.FormattingEnabled = true;
            this.cmbDarkMode.Items.AddRange(new object[] {
            global::MusicBeePlugin.Strings.DarkModeDefault,
            global::MusicBeePlugin.Strings.DarkModeDark,
            global::MusicBeePlugin.Strings.DarkModeMusicBeeTheme});
            this.cmbDarkMode.Location = new System.Drawing.Point(113, 116);
            this.cmbDarkMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDarkMode.Name = "cmbDarkMode";
            this.cmbDarkMode.Size = new System.Drawing.Size(153, 26);
            this.cmbDarkMode.TabIndex = 4;
            // 
            // chkEnableExtensions
            // 
            this.chkEnableExtensions.AutoSize = true;
            this.chkEnableExtensions.Location = new System.Drawing.Point(19, 154);
            this.chkEnableExtensions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEnableExtensions.Name = "chkEnableExtensions";
            this.chkEnableExtensions.Size = new System.Drawing.Size(322, 22);
            this.chkEnableExtensions.TabIndex = 11;
            this.chkEnableExtensions.Text = global::MusicBeePlugin.Strings.EnableExtensions;
            this.chkEnableExtensions.UseVisualStyleBackColor = true;
            // 
            // btnOpenExtensionsFolder
            // 
            this.btnOpenExtensionsFolder.Location = new System.Drawing.Point(15, 186);
            this.btnOpenExtensionsFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenExtensionsFolder.Name = "btnOpenExtensionsFolder";
            this.btnOpenExtensionsFolder.Size = new System.Drawing.Size(141, 30);
            this.btnOpenExtensionsFolder.TabIndex = 5;
            this.btnOpenExtensionsFolder.Text = global::MusicBeePlugin.Strings.OpenExtensionsFolder;
            this.btnOpenExtensionsFolder.UseVisualStyleBackColor = true;
            // 
            // btnInstallUnpackedExtension
            // 
            this.btnInstallUnpackedExtension.Location = new System.Drawing.Point(165, 186);
            this.btnInstallUnpackedExtension.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInstallUnpackedExtension.Name = "btnInstallUnpackedExtension";
            this.btnInstallUnpackedExtension.Size = new System.Drawing.Size(167, 30);
            this.btnInstallUnpackedExtension.TabIndex = 6;
            this.btnInstallUnpackedExtension.Text = global::MusicBeePlugin.Strings.InstallUnpackedExtension;
            this.btnInstallUnpackedExtension.UseVisualStyleBackColor = true;
            // 
            // lblInstalledExtensions
            // 
            this.lblInstalledExtensions.AutoSize = true;
            this.lblInstalledExtensions.Location = new System.Drawing.Point(15, 226);
            this.lblInstalledExtensions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledExtensions.Name = "lblInstalledExtensions";
            this.lblInstalledExtensions.Size = new System.Drawing.Size(197, 18);
            this.lblInstalledExtensions.TabIndex = 10;
            this.lblInstalledExtensions.Text = global::MusicBeePlugin.Strings.InstalledExtensions;
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
            this.lstExtensions.Location = new System.Drawing.Point(15, 252);
            this.lstExtensions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstExtensions.MultiSelect = false;
            this.lstExtensions.Name = "lstExtensions";
            this.lstExtensions.Size = new System.Drawing.Size(487, 143);
            this.lstExtensions.TabIndex = 8;
            this.lstExtensions.UseCompatibleStateImageBehavior = false;
            this.lstExtensions.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = global::MusicBeePlugin.Strings.ExtensionName;
            this.colName.Width = 260;
            // 
            // colVersion
            // 
            this.colVersion.Text = global::MusicBeePlugin.Strings.ExtensionVersion;
            this.colVersion.Width = 80;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = global::MusicBeePlugin.Strings.ExtensionEnabled;
            // 
            // btnUninstallExtension
            // 
            this.btnUninstallExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUninstallExtension.Location = new System.Drawing.Point(512, 252);
            this.btnUninstallExtension.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUninstallExtension.Name = "btnUninstallExtension";
            this.btnUninstallExtension.Size = new System.Drawing.Size(90, 30);
            this.btnUninstallExtension.TabIndex = 9;
            this.btnUninstallExtension.Text = global::MusicBeePlugin.Strings.Uninstall;
            this.btnUninstallExtension.UseVisualStyleBackColor = true;
            // 
            // lblRestartHint
            // 
            this.lblRestartHint.AutoSize = true;
            this.lblRestartHint.ForeColor = System.Drawing.Color.Gray;
            this.lblRestartHint.Location = new System.Drawing.Point(15, 406);
            this.lblRestartHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRestartHint.Name = "lblRestartHint";
            this.lblRestartHint.Size = new System.Drawing.Size(521, 18);
            this.lblRestartHint.TabIndex = 13;
            this.lblRestartHint.Text = global::MusicBeePlugin.Strings.RestartHint;
            // 
            // linkProject
            // 
            this.linkProject.AutoSize = true;
            this.linkProject.LinkArea = new System.Windows.Forms.LinkArea(5, 51);
            this.linkProject.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkProject.Location = new System.Drawing.Point(15, 438);
            this.linkProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkProject.Name = "linkProject";
            this.linkProject.Size = new System.Drawing.Size(563, 26);
            this.linkProject.TabIndex = 12;
            this.linkProject.TabStop = true;
            this.linkProject.Text = global::MusicBeePlugin.Strings.ProjectLink + " " + global::MusicBeePlugin.Strings.ProjectLinkUrl;
            this.linkProject.UseCompatibleTextRendering = true;
            this.linkProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkProject_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(396, 478);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = global::MusicBeePlugin.Strings.Save;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(507, 478);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = global::MusicBeePlugin.Strings.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormSetting
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(617, 523);
            this.Controls.Add(this.btnUninstallExtension);
            this.Controls.Add(this.lstExtensions);
            this.Controls.Add(this.lblInstalledExtensions);
            this.Controls.Add(this.btnInstallUnpackedExtension);
            this.Controls.Add(this.btnOpenExtensionsFolder);
            this.Controls.Add(this.chkEnableExtensions);
            this.Controls.Add(this.cmbDarkMode);
            this.Controls.Add(this.lblDarkMode);
            this.Controls.Add(this.lblRestartHint);
            this.Controls.Add(this.linkProject);
            this.Controls.Add(this.chkShowAddressBar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkAutoSaveZoom);
            this.Controls.Add(this.txtDefaultUrl);
            this.Controls.Add(this.lblHomePage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = global::MusicBeePlugin.Strings.FormTitle;
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
        private System.Windows.Forms.LinkLabel linkProject;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
