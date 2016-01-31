namespace PowerSM
{
    partial class PowerConvert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerConvert));
            this.FilesTreeGroupBox = new System.Windows.Forms.GroupBox();
            this.FilesTreeView = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutThisToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrowseForOutPutFolder = new System.Windows.Forms.Button();
            this.OpenFolder = new System.Windows.Forms.Button();
            this.LogGroupBox = new System.Windows.Forms.GroupBox();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this._ProgressBar = new System.Windows.Forms.ProgressBar();
            this.SaveLog = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.BrowseForFolderButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FilesTreeGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.LogGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilesTreeGroupBox
            // 
            this.FilesTreeGroupBox.Controls.Add(this.FilesTreeView);
            this.FilesTreeGroupBox.Location = new System.Drawing.Point(15, 27);
            this.FilesTreeGroupBox.Name = "FilesTreeGroupBox";
            this.FilesTreeGroupBox.Size = new System.Drawing.Size(481, 239);
            this.FilesTreeGroupBox.TabIndex = 2;
            this.FilesTreeGroupBox.TabStop = false;
            this.FilesTreeGroupBox.Text = "Files Tree";
            this.FilesTreeGroupBox.Enter += new System.EventHandler(this.FilesTreeGroupBox_Enter);
            // 
            // FilesTreeView
            // 
            this.FilesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilesTreeView.CheckBoxes = true;
            this.FilesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesTreeView.Location = new System.Drawing.Point(3, 16);
            this.FilesTreeView.Name = "FilesTreeView";
            this.FilesTreeView.Size = new System.Drawing.Size(475, 220);
            this.FilesTreeView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(505, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.unSelectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.selectAllToolStripMenuItem.Text = "Select All ";
            // 
            // unSelectAllToolStripMenuItem
            // 
            this.unSelectAllToolStripMenuItem.Name = "unSelectAllToolStripMenuItem";
            this.unSelectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.unSelectAllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.unSelectAllToolStripMenuItem.Text = "UnSelect All";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.optionsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(159, 22);
            this.optionsToolStripMenuItem1.Text = "Options";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click_1);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutThisToolToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.helpToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // aboutThisToolToolStripMenuItem
            // 
            this.aboutThisToolToolStripMenuItem.Name = "aboutThisToolToolStripMenuItem";
            this.aboutThisToolToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.aboutThisToolToolStripMenuItem.Text = "About PowerSM";
            this.aboutThisToolToolStripMenuItem.Click += new System.EventHandler(this.aboutThisToolToolStripMenuItem_Click_1);
            // 
            // BrowseForOutPutFolder
            // 
            this.BrowseForOutPutFolder.Location = new System.Drawing.Point(399, 269);
            this.BrowseForOutPutFolder.Name = "BrowseForOutPutFolder";
            this.BrowseForOutPutFolder.Size = new System.Drawing.Size(94, 38);
            this.BrowseForOutPutFolder.TabIndex = 18;
            this.BrowseForOutPutFolder.Text = "Output folder...";
            this.BrowseForOutPutFolder.UseVisualStyleBackColor = true;
            this.BrowseForOutPutFolder.Click += new System.EventHandler(this.BrowseForOutPutFolder_Click);
            // 
            // OpenFolder
            // 
            this.OpenFolder.Location = new System.Drawing.Point(289, 447);
            this.OpenFolder.Name = "OpenFolder";
            this.OpenFolder.Size = new System.Drawing.Size(103, 31);
            this.OpenFolder.TabIndex = 17;
            this.OpenFolder.Text = "Open folder";
            this.OpenFolder.UseVisualStyleBackColor = true;
            this.OpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
            // 
            // LogGroupBox
            // 
            this.LogGroupBox.Controls.Add(this.LogTextBox);
            this.LogGroupBox.Location = new System.Drawing.Point(15, 313);
            this.LogGroupBox.Name = "LogGroupBox";
            this.LogGroupBox.Size = new System.Drawing.Size(477, 99);
            this.LogGroupBox.TabIndex = 16;
            this.LogGroupBox.TabStop = false;
            this.LogGroupBox.Text = "Output";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTextBox.Location = new System.Drawing.Point(3, 16);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(471, 80);
            this.LogTextBox.TabIndex = 0;
            // 
            // _ProgressBar
            // 
            this._ProgressBar.Location = new System.Drawing.Point(12, 418);
            this._ProgressBar.Name = "_ProgressBar";
            this._ProgressBar.Size = new System.Drawing.Size(481, 23);
            this._ProgressBar.TabIndex = 13;
            // 
            // SaveLog
            // 
            this.SaveLog.Location = new System.Drawing.Point(180, 447);
            this.SaveLog.Name = "SaveLog";
            this.SaveLog.Size = new System.Drawing.Size(103, 31);
            this.SaveLog.TabIndex = 15;
            this.SaveLog.Text = "Save Log...";
            this.SaveLog.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(398, 447);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(94, 31);
            this.StartButton.TabIndex = 14;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BrowseForFolderButton
            // 
            this.BrowseForFolderButton.Location = new System.Drawing.Point(299, 269);
            this.BrowseForFolderButton.Name = "BrowseForFolderButton";
            this.BrowseForFolderButton.Size = new System.Drawing.Size(94, 38);
            this.BrowseForFolderButton.TabIndex = 10;
            this.BrowseForFolderButton.Text = "Source folder...";
            this.BrowseForFolderButton.UseVisualStyleBackColor = true;
            this.BrowseForFolderButton.Click += new System.EventHandler(this.BrowseForFolderButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 485);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(505, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 19;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // PowerConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 507);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.BrowseForOutPutFolder);
            this.Controls.Add(this.OpenFolder);
            this.Controls.Add(this.LogGroupBox);
            this.Controls.Add(this._ProgressBar);
            this.Controls.Add(this.SaveLog);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.BrowseForFolderButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.FilesTreeGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PowerConvert";
            this.Text = "PowerConvert";
            this.Load += new System.EventHandler(this.PowerConvert_Load);
            this.FilesTreeGroupBox.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.LogGroupBox.ResumeLayout(false);
            this.LogGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox FilesTreeGroupBox;
        private System.Windows.Forms.TreeView FilesTreeView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unSelectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutThisToolToolStripMenuItem;
        private System.Windows.Forms.Button BrowseForOutPutFolder;
        private System.Windows.Forms.Button OpenFolder;
        private System.Windows.Forms.GroupBox LogGroupBox;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.ProgressBar _ProgressBar;
        private System.Windows.Forms.Button SaveLog;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button BrowseForFolderButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
    }
}