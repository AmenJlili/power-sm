namespace PowerSM
{
    /// <summary>
    /// Unit helpers. The SOLIDWORKS API always works in system units (meters),
    /// whatever the document's display units are.
    /// </summary>
    public static class MeasuringUnitsConvertor
    {
        public const string MMGS = "MMGS";
        public const string IPS = "IPS";

        public static class Length
        {
            private const double MillimetersPerInch = 25.4;
            private const double MetersPerInch = 0.0254;
            private const double MillimetersPerMeter = 1000.0;

            public static double FromInchToMillimeters(double inches)
            {
                return inches * MillimetersPerInch;
            }

            public static double FromMillimetersToInchs(double millimeters)
            {
                return millimeters / MillimetersPerInch;
            }

            /// <summary>Converts a length typed by the user in the given unit system to meters.</summary>
            public static double ToMeters(double value, string unitSystem)
            {
                return unitSystem == IPS ? value * MetersPerInch : value / MillimetersPerMeter;
            }

            /// <summary>Converts a length in meters to the given unit system (mm or inch).</summary>
            public static double FromMeters(double meters, string unitSystem)
            {
                return unitSystem == IPS ? meters / MetersPerInch : meters * MillimetersPerMeter;
            }

            /// <summary>Short unit label for the given unit system.</summary>
            public static string UnitLabel(string unitSystem)
            {
                return unitSystem == IPS ? "in" : "mm";
            }
        }
    }
}
