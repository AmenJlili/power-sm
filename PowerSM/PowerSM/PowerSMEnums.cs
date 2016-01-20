using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSM
{
    public class PowerSMEnums
    {
        public enum TreeViewIcons
        {
            directory = 1,
            file = 0
        }

        public enum Errors
        {
           
            Cannot_delete_menu = 0,
            Cannot_Write_Log_File = 1,
            Cannot_parse_radius_value = 2,
            cannot_parse_kfacor = 3,
            cannot_parse_thickness = 4,
            Empty_Tree = 5

        }
    }
}
