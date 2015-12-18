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
    public partial class Power_SM_Form : Form
    {
        public Power_SM_Form()
        {
            InitializeComponent();
        }

        private void Power_SM_Form_Load(object sender, EventArgs e)
        {

        }

        private void BrowseForFolderButton_Click(object sender, EventArgs e)
        {
            InitiateOpenFolder();
        }

        /*
         * modifier is public for testing purposes
         */

        public void InitiateOpenFolder() 
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(FolderBrowser.SelectedPath))
           SourceFolderTextBox.Text = FolderBrowser.SelectedPath;
        }
    }
}
