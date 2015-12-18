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
            this.SourceFolderGroup = new System.Windows.Forms.GroupBox();
            this.BrowseForFolderButton = new System.Windows.Forms.Button();
            this.SourceFolderTextBox = new System.Windows.Forms.TextBox();
            this.SourceFolderGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SourceFolderGroup
            // 
            this.SourceFolderGroup.Controls.Add(this.SourceFolderTextBox);
            this.SourceFolderGroup.Controls.Add(this.BrowseForFolderButton);
            this.SourceFolderGroup.Location = new System.Drawing.Point(12, 28);
            this.SourceFolderGroup.Name = "SourceFolderGroup";
            this.SourceFolderGroup.Size = new System.Drawing.Size(480, 57);
            this.SourceFolderGroup.TabIndex = 0;
            this.SourceFolderGroup.TabStop = false;
            this.SourceFolderGroup.Text = "Source Folder";
            // 
            // BrowseForFolderButton
            // 
            this.BrowseForFolderButton.Location = new System.Drawing.Point(383, 12);
            this.BrowseForFolderButton.Name = "BrowseForFolderButton";
            this.BrowseForFolderButton.Size = new System.Drawing.Size(83, 32);
            this.BrowseForFolderButton.TabIndex = 0;
            this.BrowseForFolderButton.Text = "Browse...";
            this.BrowseForFolderButton.UseVisualStyleBackColor = true;
            this.BrowseForFolderButton.Click += new System.EventHandler(this.BrowseForFolderButton_Click);
            // 
            // SourceFolderTextBox
            // 
            this.SourceFolderTextBox.Location = new System.Drawing.Point(17, 19);
            this.SourceFolderTextBox.Name = "SourceFolderTextBox";
            this.SourceFolderTextBox.Size = new System.Drawing.Size(346, 20);
            this.SourceFolderTextBox.TabIndex = 1;
            // 
            // Power_SM_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 335);
            this.Controls.Add(this.SourceFolderGroup);
            this.Name = "Power_SM_Form";
            this.Text = "Power_SM_Form";
            this.Load += new System.EventHandler(this.Power_SM_Form_Load);
            this.SourceFolderGroup.ResumeLayout(false);
            this.SourceFolderGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SourceFolderGroup;
        private System.Windows.Forms.TextBox SourceFolderTextBox;
        private System.Windows.Forms.Button BrowseForFolderButton;
    }
}