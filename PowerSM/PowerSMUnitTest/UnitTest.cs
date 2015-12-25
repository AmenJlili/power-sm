using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using PowerSM;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
namespace PowerSMUnitTest
{
    [TestClass]
    public class UnitTest1
    {   
        [TestMethod]
        public void TestMethod1()
        {
            List<string> swSheetMetalFeatureTypes = new List<string>();
            swSheetMetalFeatureTypes.AddRange(new[] {"SMBaseFlange",
                                                    "EdgeFlange",
                                                    "SMBaseFlange",
                                                    "SketchedBend",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    "SMMiteredFlange",
                                                    "Jog",
                                                    "Bends"});
            var swApp = (SldWorks)Marshal.GetActiveObject("SolidWorks.Appliation");
            PowerSM.Power_SM_Form.TestArea.ChangeRadius(swApp,"", 1.5, swSheetMetalFeatureTypes);
            

        }
    }
}
