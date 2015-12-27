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
        public static void Err(int ErrorEnum)
        {
            switch (ErrorEnum)
            {
                case 0:
                    EchoMessage("Incorrect radius value.");
                    break;
                case 1: 
                    EchoMessage("Cannot delete Menu on Add-in unload.");
                    break;
                case 2:
                    EchoMessage("Cannot write log file.");
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

