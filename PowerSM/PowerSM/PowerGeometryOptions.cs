using System;
using System.Windows.Forms;

namespace PowerSM
{
    public partial class PowerGeometryOptions : Form
    {
        public PowerGeometryOptions()
        {
            InitializeComponent();
        }

        private void powersmoptions_Load(object sender, EventArgs e)
        {
            ForceDimensionalRespectCheckBox.Checked = PowerSMSettings.ForceDimensionalRespect;
            ArchiveInZipFormatCheckBox.Checked = PowerSMSettings.ArchiveInZipFormat;
            ForceMode.Checked = PowerSMSettings.ForceMode;
            SetOverrideToFalse.Checked = !ForceMode.Checked && PowerSMSettings.SetOverrideToFalse;
            UpdateOverrideCheckBox();
        }

        private void SaveOptionsButton_Click(object sender, EventArgs e)
        {
            PowerSMSettings.ForceDimensionalRespect = ForceDimensionalRespectCheckBox.Checked;
            PowerSMSettings.ArchiveInZipFormat = ArchiveInZipFormatCheckBox.Checked;
            PowerSMSettings.ForceMode = ForceMode.Checked;
            // "Inherit from parent" only applies in normal mode.
            PowerSMSettings.SetOverrideToFalse = !ForceMode.Checked && SetOverrideToFalse.Checked;
            PowerSMSettings.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ForceMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOverrideCheckBox();
        }

        private void UpdateOverrideCheckBox()
        {
            SetOverrideToFalse.Enabled = !ForceMode.Checked;
            if (ForceMode.Checked)
                SetOverrideToFalse.Checked = false;
        }
    }
}
