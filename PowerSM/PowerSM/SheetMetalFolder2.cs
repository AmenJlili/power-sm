using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace PowerSM
{
    /// <summary>
    /// Changes thickness / bend radius through the display dimensions of the sheet metal folder,
    /// so every feature inheriting from the folder picks up the new value.
    /// </summary>
    public class SheetMetalFolder2
    {
        private const double ZeroTolerance = 0.0001;
        private readonly Feature swFeat;

        public SheetMetalFolder2(SheetMetalFolder sheetMetalFolder)
        {
            if (sheetMetalFolder == null)
                throw new ArgumentNullException("sheetMetalFolder");

            swFeat = (Feature)sheetMetalFolder.GetFeature();
        }

        /// <summary>Sets the folder thickness. Value is in meters (SOLIDWORKS system units).</summary>
        public bool Thickness(double thicknessMeters)
        {
            var swDisplayDimension = (DisplayDimension)swFeat.GetFirstDisplayDimension();
            while (swDisplayDimension != null)
            {
                // The thickness dimension is linear (not radial / diametric) ...
                if (!IsRadialOrDiameter(swDisplayDimension))
                {
                    var swDimension = (Dimension)swDisplayDimension.GetDimension();
                    // ... and has non-zero reference points.
                    var referencePoints = swDimension.ReferencePoints as object[];
                    if (referencePoints != null)
                    {
                        foreach (var swMathPointObj in referencePoints)
                        {
                            if (!IsPointZero((MathPoint)swMathPointObj))
                                return SetValue(swDimension, thicknessMeters);
                        }
                    }
                }

                swDisplayDimension = (DisplayDimension)swFeat.GetNextDisplayDimension(swDisplayDimension);
            }
            return false;
        }

        /// <summary>Sets the folder bend radius. Value is in meters (SOLIDWORKS system units).</summary>
        public bool BendRadius(double bendRadiusMeters)
        {
            var swDisplayDimension = (DisplayDimension)swFeat.GetFirstDisplayDimension();
            while (swDisplayDimension != null)
            {
                // The radius dimension is radial or diametric
                if (IsRadialOrDiameter(swDisplayDimension))
                    return SetValue((Dimension)swDisplayDimension.GetDimension(), bendRadiusMeters);

                swDisplayDimension = (DisplayDimension)swFeat.GetNextDisplayDimension(swDisplayDimension);
            }
            return false;
        }

        private static bool IsRadialOrDiameter(DisplayDimension swDisplayDimension)
        {
            var type = swDisplayDimension.GetType();
            return type == (int)swDimensionType_e.swRadialDimension || type == (int)swDimensionType_e.swDiameterDimension;
        }

        private static bool SetValue(Dimension swDimension, double valueMeters)
        {
            // 0 = swSetValue_InThisConfiguration; return value 0 = success
            return swDimension.SetSystemValue2(valueMeters, 0) == 0;
        }

        private static bool IsPointZero(MathPoint mathpoint)
        {
            var coordinates = mathpoint.ArrayData as double[];
            if (coordinates == null)
                return true;

            double sum = 0;
            foreach (var d in coordinates)
                sum += Math.Abs(d); // absolute values so opposite coordinates don't cancel out
            return sum <= ZeroTolerance;
        }
    }
}
