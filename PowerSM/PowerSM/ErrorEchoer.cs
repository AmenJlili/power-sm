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
                    EchoMessage("Incorrect radius value. Enter a positive number or leave it empty.");
                    break;
                case (int) PowerSMEnums.Errors.Cannot_delete_menu: 
                    EchoMessage("Cannot delete Menu on Add-in unload.");
                    break;
                case (int)PowerSMEnums.Errors.cannot_parse_kfacor:
                    EchoMessage("Incorrect bend zone value. A K-factor must be between 0 and 1; bend allowance and deduction must be positive lengths.");
                    break;
                case (int)PowerSMEnums.Errors.cannot_parse_thickness:
                    EchoMessage("Incorrect thickness value. Enter a positive number or leave it empty.");
                    break;
                case (int)PowerSMEnums.Errors.Cannot_Write_Log_File:
                    EchoMessage("Cannot write log file.");
                    break;
                case (int)PowerSMEnums.Errors.Empty_Tree:
                    EchoMessage("The files tree is empty. Browse for a source folder first.");
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

