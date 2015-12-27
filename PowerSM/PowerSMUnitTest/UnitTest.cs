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
            var swApp = (SldWorks)Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application"));
            PowerSM.TestArea.ChangeRadius(swApp, @"C:\Users\JLILI\Desktop\good moning christina\Part1.SLDPRT", 6.0, swSheetMetalFeatureTypes);
            

        }
    }
}
