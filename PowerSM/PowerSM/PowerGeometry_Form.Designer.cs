namespace PowerSM
{
    partial class PowerGeometryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerGeometryForm));
            this.BrowseForFolderButton = new System.Windows.Forms.Button();
            this.FilesTreeGroupBox = new System.Windows.Forms.GroupBox();
            this.FilesTreeView = new System.Windows.Forms.TreeView();
            this.SheetMetalDataGroupBox = new System.Windows.Forms.GroupBox();
            this.KFactorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ThicknessTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NewRadiusLabel = new System.Windows.Forms.Label();
            this.MeasurementSystemLabel = new System.Windows.Forms.Label();
            this.UnitSystemCombBox = new System.Windows.Forms.ComboBox();
            this.RadiusTextBox = new System.Windows.Forms.TextBox();
            this._ProgressBar = new System.Windows.Forms.ProgressBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StartButton = new System.Windows.Forms.Button();
            this.SaveLog = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.partViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutThisToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.BrowseForOutPutFolder = new System.Windows.Forms.Button();
            this.OpenFolder = new System.Windows.Forms.Button();
            this.LogGroupBox = new System.Windows.Forms.GroupBox();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.FilesTreeGroupBox.SuspendLayout();
            this.SheetMetalDataGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.LogGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrowseForFolderButton
            // 
            this.BrowseForFolderButton.Location = new System.Drawing.Point(398, 251);
            this.BrowseForFolderButton.Name = "BrowseForFolderButton";
            this.BrowseForFolderButton.Size = new System.Drawing.Size(94, 38);
            this.BrowseForFolderButton.TabIndex = 0;
            this.BrowseForFolderButton.Text = "Source folder...";
            this.BrowseForFolderButton.UseVisualStyleBackColor = true;
            this.BrowseForFolderButton.Click += new System.EventHandler(this.BrowseForFolderButton_Click);
            // 
            // FilesTreeGroupBox
            // 
            this.FilesTreeGroupBox.Controls.Add(this.FilesTreeView);
            this.FilesTreeGroupBox.Location = new System.Drawing.Point(12, 0);
            this.FilesTreeGroupBox.Name = "FilesTreeGroupBox";
            this.FilesTreeGroupBox.Size = new System.Drawing.Size(481, 239);
            this.FilesTreeGroupBox.TabIndex = 1;
            this.FilesTreeGroupBox.TabStop = false;
            this.FilesTreeGroupBox.Text = "Files Tree";
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
            this.FilesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.FilesTreeView_AfterCheck);
            this.FilesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FilesTreeView_AfterSelect);
            // 
            // SheetMetalDataGroupBox
            // 
            this.SheetMetalDataGroupBox.Controls.Add(this.KFactorTextBox);
            this.SheetMetalDataGroupBox.Controls.Add(this.label2);
            this.SheetMetalDataGroupBox.Controls.Add(this.ThicknessTextBox);
            this.SheetMetalDataGroupBox.Controls.Add(this.label1);
            this.SheetMetalDataGroupBox.Controls.Add(this.NewRadiusLabel);
            this.SheetMetalDataGroupBox.Controls.Add(this.MeasurementSystemLabel);
            this.SheetMetalDataGroupBox.Controls.Add(this.UnitSystemCombBox);
            this.SheetMetalDataGroupBox.Controls.Add(this.RadiusTextBox);
            this.SheetMetalDataGroupBox.Location = new System.Drawing.Point(15, 245);
            this.SheetMetalDataGroupBox.Name = "SheetMetalDataGroupBox";
            this.SheetMetalDataGroupBox.Size = new System.Drawing.Size(377, 100);
            this.SheetMetalDataGroupBox.TabIndex = 2;
            this.SheetMetalDataGroupBox.TabStop = false;
            this.SheetMetalDataGroupBox.Text = "Sheet Metal Data";
            // 
            // KFactorTextBox
            // 
            this.KFactorTextBox.Location = new System.Drawing.Point(110, 71);
            this.KFactorTextBox.Name = "KFactorTextBox";
            this.KFactorTextBox.Size = new System.Drawing.Size(65, 20);
            this.KFactorTextBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "K-Factor Value:";
            // 
            // ThicknessTextBox
            // 
            this.ThicknessTextBox.Location = new System.Drawing.Point(110, 42);
            this.ThicknessTextBox.Name = "ThicknessTextBox";
            this.ThicknessTextBox.Size = new System.Drawing.Size(65, 20);
            this.ThicknessTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Thickness Value:";
            // 
            // NewRadiusLabel
            // 
            this.NewRadiusLabel.AutoSize = true;
            this.NewRadiusLabel.Location = new System.Drawing.Point(22, 18);
            this.NewRadiusLabel.Name = "NewRadiusLabel";
            this.NewRadiusLabel.Size = new System.Drawing.Size(73, 13);
            this.NewRadiusLabel.TabIndex = 4;
            this.NewRadiusLabel.Text = "Radius Value:";
            // 
            // MeasurementSystemLabel
            // 
            this.MeasurementSystemLabel.AutoSize = true;
            this.MeasurementSystemLabel.Location = new System.Drawing.Point(186, 19);
            this.MeasurementSystemLabel.Name = "MeasurementSystemLabel";
            this.MeasurementSystemLabel.Size = new System.Drawing.Size(66, 13);
            this.MeasurementSystemLabel.TabIndex = 2;
            this.MeasurementSystemLabel.Text = "Unit System:";
            // 
            // UnitSystemCombBox
            // 
            this.UnitSystemCombBox.FormattingEnabled = true;
            this.UnitSystemCombBox.Items.AddRange(new object[] {
            "MMGS",
            "IPS"});
            this.UnitSystemCombBox.Location = new System.Drawing.Point(258, 15);
            this.UnitSystemCombBox.Name = "UnitSystemCombBox";
            this.UnitSystemCombBox.Size = new System.Drawing.Size(113, 21);
            this.UnitSystemCombBox.TabIndex = 1;
            this.UnitSystemCombBox.Text = "MMGS";
            // 
            // RadiusTextBox
            // 
            this.RadiusTextBox.Location = new System.Drawing.Point(110, 15);
            this.RadiusTextBox.Name = "RadiusTextBox";
            this.RadiusTextBox.Size = new System.Drawing.Size(65, 20);
            this.RadiusTextBox.TabIndex = 0;
            // 
            // _ProgressBar
            // 
            this._ProgressBar.Location = new System.Drawing.Point(12, 456);
            this._ProgressBar.Name = "_ProgressBar";
            this._ProgressBar.Size = new System.Drawing.Size(481, 23);
            this._ProgressBar.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(502, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(398, 485);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(94, 31);
            this.StartButton.TabIndex = 5;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SaveLog
            // 
            this.SaveLog.Location = new System.Drawing.Point(180, 485);
            this.SaveLog.Name = "SaveLog";
            this.SaveLog.Size = new System.Drawing.Size(103, 31);
            this.SaveLog.TabIndex = 6;
            this.SaveLog.Text = "Save Log...";
            this.SaveLog.UseVisualStyleBackColor = true;
            this.SaveLog.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(502, 24);
            this.menuStrip1.TabIndex = 1;
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
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
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
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // unSelectAllToolStripMenuItem
            // 
            this.unSelectAllToolStripMenuItem.Name = "unSelectAllToolStripMenuItem";
            this.unSelectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.unSelectAllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.unSelectAllToolStripMenuItem.Text = "UnSelect All";
            this.unSelectAllToolStripMenuItem.Click += new System.EventHandler(this.unSelectAllToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1,
            this.partViewerToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.optionsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.optionsToolStripMenuItem1.Text = "Options";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // partViewerToolStripMenuItem
            // 
            this.partViewerToolStripMenuItem.Enabled = false;
            this.partViewerToolStripMenuItem.Name = "partViewerToolStripMenuItem";
            this.partViewerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.partViewerToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.partViewerToolStripMenuItem.Text = "Part Viewer";
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
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutThisToolToolStripMenuItem
            // 
            this.aboutThisToolToolStripMenuItem.Name = "aboutThisToolToolStripMenuItem";
            this.aboutThisToolToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.aboutThisToolToolStripMenuItem.Text = "About PowerSM";
            this.aboutThisToolToolStripMenuItem.Click += new System.EventHandler(this.aboutThisToolToolStripMenuItem_Click);
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
            this.toolStripContainer1.ContentPanel.Controls.Add(this.BrowseForOutPutFolder);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.OpenFolder);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.LogGroupBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ProgressBar);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SaveLog);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.StartButton);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SheetMetalDataGroupBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.BrowseForFolderButton);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.FilesTreeGroupBox);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(502, 519);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(502, 565);
            this.toolStripContainer1.TabIndex = 7;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // BrowseForOutPutFolder
            // 
            this.BrowseForOutPutFolder.Location = new System.Drawing.Point(398, 290);
            this.BrowseForOutPutFolder.Name = "BrowseForOutPutFolder";
            this.BrowseForOutPutFolder.Size = new System.Drawing.Size(94, 38);
            this.BrowseForOutPutFolder.TabIndex = 9;
            this.BrowseForOutPutFolder.Text = "Output folder...";
            this.BrowseForOutPutFolder.UseVisualStyleBackColor = true;
            this.BrowseForOutPutFolder.Click += new System.EventHandler(this.BrowseForOutPutFolder_Click);
            // 
            // OpenFolder
            // 
            this.OpenFolder.Location = new System.Drawing.Point(289, 485);
            this.OpenFolder.Name = "OpenFolder";
            this.OpenFolder.Size = new System.Drawing.Size(103, 31);
            this.OpenFolder.TabIndex = 8;
            this.OpenFolder.Text = "Open folder";
            this.OpenFolder.UseVisualStyleBackColor = true;
            this.OpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
            // 
            // LogGroupBox
            // 
            this.LogGroupBox.Controls.Add(this.LogTextBox);
            this.LogGroupBox.Location = new System.Drawing.Point(15, 351);
            this.LogGroupBox.Name = "LogGroupBox";
            this.LogGroupBox.Size = new System.Drawing.Size(477, 99);
            this.LogGroupBox.TabIndex = 7;
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
            // PowerGeometryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 565);
            this.Controls.Add(this.toolStripContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PowerGeometryForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Power Geometry Tool";
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
            this.LogGroupBox.ResumeLayout(false);
            this.LogGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BrowseForFolderButton;
        private System.Windows.Forms.GroupBox FilesTreeGroupBox;
        private System.Windows.Forms.TreeView FilesTreeView;
        private System.Windows.Forms.GroupBox SheetMetalDataGroupBox;
        private System.Windows.Forms.Label MeasurementSystemLabel;
        private System.Windows.Forms.ComboBox UnitSystemCombBox;
        private System.Windows.Forms.TextBox RadiusTextBox;
        private System.Windows.Forms.ProgressBar _ProgressBar;
        private System.Windows.Forms.Label NewRadiusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SaveLog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutThisToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.GroupBox LogGroupBox;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unSelectAllToolStripMenuItem;
        private System.Windows.Forms.Button OpenFolder;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.TextBox KFactorTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ThicknessTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseForOutPutFolder;
        private System.Windows.Forms.ToolStripMenuItem partViewerToolStripMenuItem;
    }
}