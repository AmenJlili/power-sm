using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace PowerSM
{
    public class SheetMetalFolder2
    {
        private SheetMetalFolder swSheetMetalFolder;
        private Feature swFeat;
        public SheetMetalFolder2(SheetMetalFolder SheetMetalFolder)

        {
            if (SheetMetalFolder != null)
            {
                swSheetMetalFolder = SheetMetalFolder;
                swFeat = (Feature)swSheetMetalFolder.GetFeature();

            }
            else
            {
                throw new NullReferenceException("Constructor has a null parameter.");
            }
        }

        // Thickness is in mm
        public bool Thickness(double Thickness)

        {
            DisplayDimension swDisplayDimension = (DisplayDimension)swFeat.GetFirstDisplayDimension();
            while (swDisplayDimension != null)
            {
                // the display dimension of the thickness is not radial or diametric
                if (swDisplayDimension.GetType() != (int)swDimensionType_e.swRadialDimension && swDisplayDimension.GetType() != (int)swDimensionType_e.swDiameterDimension)
                {
                    Dimension swDimension = (Dimension)swDisplayDimension.GetDimension();
                    // thickness dimension has reference points and a direction
                    if (swDimension.ReferencePoints != null)
                    {

                        object[] swMathPointsObj = (object[])swDimension.ReferencePoints;
                        foreach (object swMathPointObj in swMathPointsObj)
                        {
                            MathPoint swMathPoint = (MathPoint)swMathPointObj;
                            if (!IsPointZero(swMathPoint))
                            {

                                int retval = swDimension.SetSystemValue2(Thickness / 1000.0, 0);
                                if (retval == 0)
                                    return true;
                                return false;



                            }
                        }
                    }
                }

                // Get next dimension
                swDisplayDimension = (DisplayDimension)swFeat.GetNextDisplayDimension(swDisplayDimension);
            }
            return false;
        }
        // BendRadius is in mm
        public bool BendRadius(double BendRadius)
        {
            DisplayDimension swDisplayDimension = (DisplayDimension)swFeat.GetFirstDisplayDimension();
            while (swDisplayDimension != null)
            {
                // the radius dimension is radial or diametric
                if (swDisplayDimension.GetType() == (int)swDimensionType_e.swRadialDimension || swDisplayDimension.GetType() == (int)swDimensionType_e.swDiameterDimension)
                {
                    Dimension swDimension = (Dimension)swDisplayDimension.GetDimension();

                    int retval = swDimension.SetSystemValue2(BendRadius / 1000.0, 0);
                    if (retval == 0)
                        return true;
                    return false;
                }

                // Get next dimension
                swDisplayDimension = (DisplayDimension)swFeat.GetNextDisplayDimension(swDisplayDimension);
            }
            return false;
        }

        private bool IsPointZero(MathPoint mathpoint)
        {
            double sum = 0;
            Double[] DoubleArray = (Double[])mathpoint.ArrayData;
            foreach (double d in DoubleArray)
            {
                sum = sum + d;
            }

            if (sum > 0.0001)
            {
                return false;
            }
            return true;
        }

    }
}
