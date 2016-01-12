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
    public partial class PowerSMOptions : Form
    {
        public PowerSMOptions()
        {
            InitializeComponent();
        }

        private void powersmoptions_Load(object sender, EventArgs e)
        {
           ForceDimensionalRespectCheckBox.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"]);
           ArchiveInZipFormatCheckBox.Checked = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"]);
        }

        private void SaveOptionsButton_Click(object sender, EventArgs e)
        {
            System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"] = ForceDimensionalRespectCheckBox.Checked.ToString();
            System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"] = ArchiveInZipFormatCheckBox.Checked.ToString();
            this.Close();
        }
    }
}
