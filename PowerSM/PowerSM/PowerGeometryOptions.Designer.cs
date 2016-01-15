namespace PowerSM
{
    partial class PowerGeometryOptions
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
            this.ForceDimensionalRespectCheckBox = new System.Windows.Forms.CheckBox();
            this.ArchiveInZipFormatCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SaveOptionsButton = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ForceDimensionalRespectCheckBox
            // 
            this.ForceDimensionalRespectCheckBox.AutoSize = true;
            this.ForceDimensionalRespectCheckBox.Location = new System.Drawing.Point(17, 30);
            this.ForceDimensionalRespectCheckBox.Name = "ForceDimensionalRespectCheckBox";
            this.ForceDimensionalRespectCheckBox.Size = new System.Drawing.Size(233, 17);
            this.ForceDimensionalRespectCheckBox.TabIndex = 0;
            this.ForceDimensionalRespectCheckBox.Text = "Force PowerSM to respect parts dimensions";
            this.ForceDimensionalRespectCheckBox.UseVisualStyleBackColor = true;
            // 
            // ArchiveInZipFormatCheckBox
            // 
            this.ArchiveInZipFormatCheckBox.AutoSize = true;
            this.ArchiveInZipFormatCheckBox.Location = new System.Drawing.Point(17, 53);
            this.ArchiveInZipFormatCheckBox.Name = "ArchiveInZipFormatCheckBox";
            this.ArchiveInZipFormatCheckBox.Size = new System.Drawing.Size(144, 17);
            this.ArchiveInZipFormatCheckBox.TabIndex = 1;
            this.ArchiveInZipFormatCheckBox.Text = "Archive files in Zip format";
            this.ArchiveInZipFormatCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ForceDimensionalRespectCheckBox);
            this.groupBox1.Controls.Add(this.ArchiveInZipFormatCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 90);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // SaveOptionsButton
            // 
            this.SaveOptionsButton.Location = new System.Drawing.Point(361, 108);
            this.SaveOptionsButton.Name = "SaveOptionsButton";
            this.SaveOptionsButton.Size = new System.Drawing.Size(112, 27);
            this.SaveOptionsButton.TabIndex = 3;
            this.SaveOptionsButton.Text = "O.K";
            this.SaveOptionsButton.UseVisualStyleBackColor = true;
            this.SaveOptionsButton.Click += new System.EventHandler(this.SaveOptionsButton_Click);
            // 
            // PowerSMOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 141);
            this.Controls.Add(this.SaveOptionsButton);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PowerSMOptions";
            this.Text = "Power Geometry Options";
            this.Load += new System.EventHandler(this.powersmoptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox ForceDimensionalRespectCheckBox;
        private System.Windows.Forms.CheckBox ArchiveInZipFormatCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SaveOptionsButton;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}