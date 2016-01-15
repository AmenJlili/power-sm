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
        /* Notes to developers:
         
        -   This was developed under .NET Framework 4.5.2 and SolidWorks API 2015 
           The add-in is intend to be used with SolidWorks 2015 or superior
           
        -  For optimal results: 
           Select the target platform to 64 bit processor under the build tab
           Check Register for COM Interop under the build tab
        */

         int AddInCookie;
         SldWorks swApp;

        #region Connect To SolidWorks 
        public bool ConnectToSW(object swAppObj, int SessionCookie)
        {
            swApp = (SldWorks)swAppObj;
            AddInCookie = SessionCookie;
            swApp.SetAddinCallbackInfo(0, this, AddInCookie);
            BuildMenu();
            return true;
        }

        public bool DisconnectFromSW()
        {
            DestroyMenu();
            GC.Collect();
            return true;
        }
        #endregion

        #region COMRegistration

        [ComRegisterFunction()]
        private static void RegisterAssembly(Type t)
        {
            string KeyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
            RegistryKey rk = Registry.LocalMachine.CreateSubKey(KeyPath);
            rk.SetValue(null, 1); // 1: Add-in will load at start-up
            rk.SetValue("Title", "Power-SM"); // Title
            rk.SetValue("Description", "SolidWorks Add-in for Sheet Metal"); // Description
        }

        [ComUnregisterFunction()]
        private static void UnRegisterAssembly(Type t)
        {
            string KeyPath = string.Format(@"Software\SolidWorks\Addins\{0:b}", t.GUID);
            Registry.LocalMachine.DeleteSubKey(KeyPath);
        }
        
        #endregion      
    
        #region UIMethod
        private void BuildMenu()
        {
           
            int DocType = (int)swDocumentTypes_e.swDocNONE;
            swApp.AddMenu(DocType, "PowerSM", 1);
            swApp.AddMenuItem4(DocType, AddInCookie, "Power Geometry Tool@PowerSM", 1, "GeometryToolMethod", "3", "Power Geometry Tool", "");

        }
        private void DestroyMenu()
        {
          
            int DocType = (int)swDocumentTypes_e.swDocNONE;
            swApp.RemoveMenu(DocType, "PowerSM", "GeometryToolMethod");
            swApp.RemoveMenu(DocType, "Power Geometry Tool@PowerSM", "GeometryToolMethod");

        }
        #endregion

        #region Add-in Implementation
        // Callback methods must be public, otherwise call from menu item fails 
        public void GeometryToolMethod()
        {
            var  f = new PowerGeometryForm(swApp);
            f.ShowDialog();
        }
        #endregion

    } 
}


