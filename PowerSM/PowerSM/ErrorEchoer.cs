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
                case (int) PowerSMEnums.Errors.Cannot_parse_radius_value:
                    EchoMessage("Incorrect radius value.");
                    break;
                case (int) PowerSMEnums.Errors.Cannot_delete_menu: 
                    EchoMessage("Cannot delete Menu on Add-in unload.");
                    break;
                case (int)PowerSMEnums.Errors.cannot_parse_kfacor:
                    EchoMessage("Incorrect kfactor value.");
                    break;
                case (int)PowerSMEnums.Errors.cannot_parse_thickness:
                    EchoMessage("Incorrect thickness value");
                    break;
                case (int)PowerSMEnums.Errors.Cannot_Write_Log_File:
                    EchoMessage("Cannot write log file.");
                    break;
                case (int)PowerSMEnums.Errors.Empty_Tree:
                    EchoMessage("Files tree is empty");
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

