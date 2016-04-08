namespace PowerSM
{
    public static class MeasuringUnitsConvertor
    {
        public static class Length 
       {
        private const double MillimetersPerInch = 25.4;
       
        
        public static double FromInchToMillimeters(double inchs)
            {
                return inchs * MillimetersPerInch;
            }

            public static double FromMillimetersToInchs(double millimeters)
            {
                return millimeters / MillimetersPerInch;
            }
        }
    }
}
