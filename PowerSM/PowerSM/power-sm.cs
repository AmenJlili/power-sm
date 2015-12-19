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
            bool result = swApp.SetAddinCallbackInfo(0, this, AddInCookie);
            BuildMenu();
            return true;
        }

        public bool DisconnectFromSW()
        {
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
            int result;
            int DocType = (int)swDocTemplateTypes_e.swDocTemplateTypeNONE;
            result = swApp.AddMenu(DocType, "PowerSM", 1);
            result = swApp.AddMenuItem4(DocType, AddInCookie, "Power Radi Tool", 0, "RadiToolMethod", null, "Power Radi Tool", "");

        }
        private void DestroyMenu()
        {
            bool result;
            int DocType = (int)swDocTemplateTypes_e.swDocTemplateTypeNONE;
            result = swApp.RemoveMenu(DocType, "Power Radi Tool@PowerSM", "RadiToolMethod");
            if (result == false)
                ErrorEchoer.ShowErrorMessageBox((int)PowerSMEnums.Errors_e.UnloadMenuError);

        }
        #endregion

        #region Add-in Implementation
        private void RadiToolMethod()
        {
            Power_SM_Form form = new Power_SM_Form(swApp);
            form.ShowDialog();
        }
        #endregion

    } 
}

namespace PowerSMEnums
{
    enum Errors_e
    {
        UnloadMenuError=1,

    }
}
