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
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(313, 80);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = Strings.Save;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(394, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = Strings.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormSetting
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(480, 120);
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
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
