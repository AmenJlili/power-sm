using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;

namespace PowerSM
{
    public class PowerSM: SwAddin
    {
        /* 
           Select the target platform to 64 bit processor under the build tab
           Check Register for COM Interop under the build tab
        */

        private int Cookie;
        public SldWorks swApp;

        public bool ConnectToSW(object swAppObj, int SessionCookie)
        {
            swApp = (SldWorks)swAppObj;
            Cookie = SessionCookie;
            bool result = swApp.SetAddinCallbackInfo(0, this, Cookie);
            BuildMenu();
            return true;
        }

        public bool DisconnectFromSW()
        {
            GC.Collect();
            return true;
        }

        [ComRegisterFunction()]
        private static void RegisterAssembly(Type t)
        {
            string KeyPath = string.Format(@"\Software\SolidWorks\Addins\{0:b}", t.GUID);
            RegistryKey rk = Registry.LocalMachine.CreateSubKey(KeyPath);
            rk.SetValue(null, 1); // 1: Add-in will load at start-up
            rk.SetValue("Title", "Power-SM"); // Title
            rk.SetValue("Description", "SolidWorks Add-in for Sheet Metal"); // Description
        }

        [ComUnregisterFunction()]
        private static void UnRegisterAssembly(Type t)
        {
            string KeyPath = string.Format(@"\Software\SolidWorks\Addins\{0:b}", t.GUID);
            Registry.LocalMachine.DeleteSubKey(KeyPath);
        }
       
       // Adds Menu to SolidWorks menu strip
       private void BuildMenu()
       {
       
       }
    }
}
