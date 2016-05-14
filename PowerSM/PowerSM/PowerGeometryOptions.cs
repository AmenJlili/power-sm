using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           ForceDimensionalRespectCheckBox.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"]);
           ArchiveInZipFormatCheckBox.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"]);
           ForceMode.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["Force"]);


            if (ForceMode.Checked == true)
            {
                SetOverrideToFalse.Checked = false;
                System.Configuration.ConfigurationSettings.AppSettings["SetOverrideToFalse"] = "False";
                SetOverrideToFalse.Enabled = false;
            }
            else
            {
                SetOverrideToFalse.Enabled = true;
                SetOverrideToFalse.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["SetOverrideToFalse"]);
            }
        }

        private void SaveOptionsButton_Click(object sender, EventArgs e)
        {
            System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"] = ForceDimensionalRespectCheckBox.Checked.ToString();
            System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"] = ArchiveInZipFormatCheckBox.Checked.ToString();
            System.Configuration.ConfigurationSettings.AppSettings["Force"] = ForceMode.Checked.ToString();
            if ( ForceMode.Checked == true )
            {
                SetOverrideToFalse.Checked = false; 
                System.Configuration.ConfigurationSettings.AppSettings["SetOverrideToFalse"] = "False";
                SetOverrideToFalse.Enabled = false; 
            }
            else
            {
                SetOverrideToFalse.Enabled = true;
                System.Configuration.ConfigurationSettings.AppSettings["SetOverrideToFalse"] = SetOverrideToFalse.Checked.ToString();
            }
 

      
            this.Close();
        }

        private void ForceMode_CheckedChanged(object sender, EventArgs e)
        {
            if (ForceMode.Checked == true)
            {
                SetOverrideToFalse.Enabled = false;

            }
            else
            {
                SetOverrideToFalse.Enabled = true;
            }

        }
    }
}
