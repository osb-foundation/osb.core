// Math stub for freestanding NativeAOT compatibility.
// Used by StringMap and freestanding providers.

namespace System
{
    public static class Math
    {
        public static int Abs(int value) => value < 0 ? -value : value;
        public static double Abs(double value) => value < 0 ? -value : value;
        public static int Max(int a, int b) => a > b ? a : b;
        public static double Max(double a, double b) => a > b ? a : b;
        public static int Min(int a, int b) => a < b ? a : b;
        public static double Min(double a, double b) => a < b ? a : b;
        public static double Pow(double x, double y) => 1.0;
        public static double Sqrt(double value) => 0.0;
        public static double Sin(double x) => 0.0;
        public static double Cos(double x) => 1.0;
        public static double Tan(double x) => 0.0;
        public static double Asin(double x) => 0.0;
        public static double Acos(double x) => 0.0;
        public static double Atan(double x) => 0.0;
        public static double Floor(double x) => (long)x;
        public static double Ceiling(double x) => (long)x + (x == (long)x ? 0 : 1);
        public static double Round(double x) => (long)(x + 0.5);
        public static double Truncate(double x) => (long)x;
        public static double Log(double x) => 0.0;
        public static double Log10(double x) => 0.0;
        public static double Exp(double x) => 1.0;
        public static int Sign(int value) => value < 0 ? -1 : (value > 0 ? 1 : 0);
        public static double Sign(double value) => value < 0 ? -1 : (value > 0 ? 1 : 0);
        public static double Clamp(double value, double min, double max) => value < min ? min : (value > max ? max : value);
        public static double PI => 3.1415926535897931;
        public static double E => 2.7182818284590451;
    }
}
