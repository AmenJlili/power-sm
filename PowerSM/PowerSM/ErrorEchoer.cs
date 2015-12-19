using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerSM
{
   public static class ErrorEchoer
    { 
        public static void ShowErrorMessageBox(int ErrorEnum)
        {
            switch (ErrorEnum)
            {
                case 1: 
                    EchoMessage("Cannot deleted Menu on Add-in unload.");
                    break;
                default:
                    break;
            }
        }

        private static void EchoMessage(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}

