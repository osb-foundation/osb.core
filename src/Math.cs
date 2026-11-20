// Math stub for freestanding NativeAOT compatibility.
// Used by StringMap.

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
    }
}
