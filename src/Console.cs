// Console stub for freestanding NativeAOT compatibility.
// Used by ProbeMain for smoke-test output.

namespace System
{
    public static class Console
    {
        public static void WriteLine(string value) { }
        public static void WriteLine() { }
        public static void WriteLine(object value) { }
    }
}
