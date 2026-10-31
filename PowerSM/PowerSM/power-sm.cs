using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;

namespace PowerSM
{
    [Guid("B2A0B714-0984-435A-A2DF-D3AA3E8E806D")]
    [ComVisible(true)]
    public class PowerSM : SwAddin
    {
        /* Notes to developers:
         * - Embed Interop Types must be true for SolidWorks.Interop.sldworks.
         * - Build for x64 (Release|x64 or the x64 platform target).
         * - "Register for COM interop" requires Visual Studio to run as administrator.
         *   The installer registers the add-in with RegAsm /codebase instead.
         */

        private const string MenuName = "PowerSM";
        private const string GeometryToolMenuItem = "Power Geometry Tool@PowerSM";
        private const string AddInTitle = "Power-SM";
        private const string AddInDescription = "SolidWorks Add-in for Sheet Metal";

        private int AddInCookie;
        private SldWorks swApp;

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
            swApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return true;
        }

        #endregion

        #region COM Registration

        private static string AddInKeyPath(Type t)
        {
            return string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
        }

        [ComRegisterFunction]
        private static void RegisterAssembly(Type t)
        {
            using (var rk = Registry.LocalMachine.CreateSubKey(AddInKeyPath(t)))
            {
                rk.SetValue(null, 1); // 1: add-in loads at start-up
                rk.SetValue("Title", AddInTitle);
                rk.SetValue("Description", AddInDescription);

                // Write the add-in icon next to the assembly and register its location
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources", assembly);
                    using (var icon = (Bitmap)rm.GetObject("add_icon"))
                    {
                        var iconPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "add_icon.bmp");
                        icon.Save(iconPath);
                        rk.SetValue("Icon Path", iconPath);
                    }
                }
                catch (Exception)
                {
                    // The icon is cosmetic; never fail registration because of it.
                }
            }
        }

        [ComUnregisterFunction]
        private static void UnRegisterAssembly(Type t)
        {
            Registry.LocalMachine.DeleteSubKey(AddInKeyPath(t), false);
        }

        #endregion

        #region UI Methods

        private void BuildMenu()
        {
            var docType = (int)swDocumentTypes_e.swDocNONE;
            swApp.AddMenu(docType, MenuName, 1);
            swApp.AddMenuItem4(docType, AddInCookie, GeometryToolMenuItem, 1, "GeometryToolMethod", "3", "Power Geometry Tool", "");
        }

        private void DestroyMenu()
        {
            var docType = (int)swDocumentTypes_e.swDocNONE;
            swApp.RemoveMenu(docType, GeometryToolMenuItem, "GeometryToolMethod");
            swApp.RemoveMenu(docType, MenuName, "");
        }

        #endregion

        #region Add-in Implementation

        // Callback methods must be public, otherwise the call from the menu item fails.
        public void GeometryToolMethod()
        {
            try
            {
                var owner = new swFrame(Process.GetProcessById(swApp.GetProcessID()).MainWindowHandle);
                using (var form = new PowerGeometryForm(swApp))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog(owner);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Power-SM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }

    /// <summary>Wraps the SOLIDWORKS main window handle so dialogs can be centered on it.</summary>
    public class swFrame : IWin32Window
    {
        public IntPtr Handle { get; set; }

        public swFrame(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
