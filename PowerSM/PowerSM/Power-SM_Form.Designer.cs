namespace PowerSM
{
    partial class Power_SM_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BrowseForFolderButton = new System.Windows.Forms.Button();
            this.FilesTreeGroupBox = new System.Windows.Forms.GroupBox();
            this.FilesTreeView = new System.Windows.Forms.TreeView();
            this.SheetMetalDataGroupBox = new System.Windows.Forms.GroupBox();
            this.NewRadiusLabel = new System.Windows.Forms.Label();
            this.MeasurementSystemLabel = new System.Windows.Forms.Label();
            this.MeasurementSystemCombBox = new System.Windows.Forms.ComboBox();
            this.RadiusTextBox = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StartButton = new System.Windows.Forms.Button();
            this.SaveLog = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutThisToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseAgreementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.FilesTreeGroupBox.SuspendLayout();
            this.SheetMetalDataGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrowseForFolderButton
            // 
            this.BrowseForFolderButton.Location = new System.Drawing.Point(398, 248);
            this.BrowseForFolderButton.Name = "BrowseForFolderButton";
            this.BrowseForFolderButton.Size = new System.Drawing.Size(94, 38);
            this.BrowseForFolderButton.TabIndex = 0;
            this.BrowseForFolderButton.Text = "Browse...";
            this.BrowseForFolderButton.UseVisualStyleBackColor = true;
            this.BrowseForFolderButton.Click += new System.EventHandler(this.BrowseForFolderButton_Click);
            // 
            // FilesTreeGroupBox
            // 
            this.FilesTreeGroupBox.Controls.Add(this.FilesTreeView);
            this.FilesTreeGroupBox.Location = new System.Drawing.Point(12, 0);
            this.FilesTreeGroupBox.Name = "FilesTreeGroupBox";
            this.FilesTreeGroupBox.Size = new System.Drawing.Size(480, 239);
            this.FilesTreeGroupBox.TabIndex = 1;
            this.FilesTreeGroupBox.TabStop = false;
            this.FilesTreeGroupBox.Text = "Files Tree";
            // 
            // FilesTreeView
            // 
            this.FilesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesTreeView.Location = new System.Drawing.Point(3, 16);
            this.FilesTreeView.Name = "FilesTreeView";
            this.FilesTreeView.Size = new System.Drawing.Size(474, 220);
            this.FilesTreeView.TabIndex = 0;
            // 
            // SheetMetalDataGroupBox
            // 
            this.SheetMetalDataGroupBox.Controls.Add(this.NewRadiusLabel);
            this.SheetMetalDataGroupBox.Controls.Add(this.MeasurementSystemLabel);
            this.SheetMetalDataGroupBox.Controls.Add(this.MeasurementSystemCombBox);
            this.SheetMetalDataGroupBox.Controls.Add(this.RadiusTextBox);
            this.SheetMetalDataGroupBox.Location = new System.Drawing.Point(15, 245);
            this.SheetMetalDataGroupBox.Name = "SheetMetalDataGroupBox";
            this.SheetMetalDataGroupBox.Size = new System.Drawing.Size(377, 41);
            this.SheetMetalDataGroupBox.TabIndex = 2;
            this.SheetMetalDataGroupBox.TabStop = false;
            this.SheetMetalDataGroupBox.Text = "Sheet Metal Data";
            // 
            // NewRadiusLabel
            // 
            this.NewRadiusLabel.AutoSize = true;
            this.NewRadiusLabel.Location = new System.Drawing.Point(6, 18);
            this.NewRadiusLabel.Name = "NewRadiusLabel";
            this.NewRadiusLabel.Size = new System.Drawing.Size(68, 13);
            this.NewRadiusLabel.TabIndex = 4;
            this.NewRadiusLabel.Text = "New Radius:";
            // 
            // MeasurementSystemLabel
            // 
            this.MeasurementSystemLabel.AutoSize = true;
            this.MeasurementSystemLabel.Location = new System.Drawing.Point(148, 19);
            this.MeasurementSystemLabel.Name = "MeasurementSystemLabel";
            this.MeasurementSystemLabel.Size = new System.Drawing.Size(111, 13);
            this.MeasurementSystemLabel.TabIndex = 2;
            this.MeasurementSystemLabel.Text = "Measurement System:";
            // 
            // MeasurementSystemCombBox
            // 
            this.MeasurementSystemCombBox.FormattingEnabled = true;
            this.MeasurementSystemCombBox.Items.AddRange(new object[] {
            "MMGS",
            "IPS"});
            this.MeasurementSystemCombBox.Location = new System.Drawing.Point(274, 15);
            this.MeasurementSystemCombBox.Name = "MeasurementSystemCombBox";
            this.MeasurementSystemCombBox.Size = new System.Drawing.Size(97, 21);
            this.MeasurementSystemCombBox.TabIndex = 1;
            this.MeasurementSystemCombBox.Text = "MMGS";
            // 
            // RadiusTextBox
            // 
            this.RadiusTextBox.Location = new System.Drawing.Point(77, 16);
            this.RadiusTextBox.Name = "RadiusTextBox";
            this.RadiusTextBox.Size = new System.Drawing.Size(65, 20);
            this.RadiusTextBox.TabIndex = 0;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 292);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(477, 23);
            this.progressBar.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(496, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(398, 321);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(94, 31);
            this.StartButton.TabIndex = 5;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SaveLog
            // 
            this.SaveLog.Location = new System.Drawing.Point(289, 321);
            this.SaveLog.Name = "SaveLog";
            this.SaveLog.Size = new System.Drawing.Size(103, 31);
            this.SaveLog.TabIndex = 6;
            this.SaveLog.Text = "Save Log...";
            this.SaveLog.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(496, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutThisToolToolStripMenuItem,
            this.licenseAgreementToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // aboutThisToolToolStripMenuItem
            // 
            this.aboutThisToolToolStripMenuItem.Name = "aboutThisToolToolStripMenuItem";
            this.aboutThisToolToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.aboutThisToolToolStripMenuItem.Text = "About this tool";
            // 
            // licenseAgreementToolStripMenuItem
            // 
            this.licenseAgreementToolStripMenuItem.Name = "licenseAgreementToolStripMenuItem";
            this.licenseAgreementToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.licenseAgreementToolStripMenuItem.Text = "License agreement";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SaveLog);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.StartButton);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.progressBar);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SheetMetalDataGroupBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.BrowseForFolderButton);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.FilesTreeGroupBox);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(496, 357);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(496, 403);
            this.toolStripContainer1.TabIndex = 7;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // Power_SM_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 403);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Power_SM_Form";
            this.Text = "Power Radis Tool";
            this.Load += new System.EventHandler(this.Power_SM_Form_Load);
            this.FilesTreeGroupBox.ResumeLayout(false);
            this.SheetMetalDataGroupBox.ResumeLayout(false);
            this.SheetMetalDataGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BrowseForFolderButton;
        private System.Windows.Forms.GroupBox FilesTreeGroupBox;
        private System.Windows.Forms.TreeView FilesTreeView;
        private System.Windows.Forms.GroupBox SheetMetalDataGroupBox;
        private System.Windows.Forms.Label MeasurementSystemLabel;
        private System.Windows.Forms.ComboBox MeasurementSystemCombBox;
        private System.Windows.Forms.TextBox RadiusTextBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label NewRadiusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SaveLog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutThisToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseAgreementToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}