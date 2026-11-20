// DateTime stub for freestanding NativeAOT compatibility.
// Used by ITimeProvider interface (UtcNow, CreateDate, CreateTime).
// Minimal ticks-based implementation.

using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct DateTime : IEquatable<DateTime>, IComparable<DateTime>
    {
        private readonly long _ticks;
        public DateTime(long ticks) { _ticks = ticks; }
        public long Ticks => _ticks;
        public static DateTime UtcNow => new DateTime(0);
        public static DateTime MinValue => new DateTime(0);
        public static DateTime MaxValue => new DateTime(long.MaxValue);
        public bool Equals(DateTime other) => _ticks == other._ticks;
        public int CompareTo(DateTime other)
        {
            if (_ticks < other._ticks) return -1;
            if (_ticks > other._ticks) return 1;
            return 0;
        }
        public override string ToString() => _ticks.ToString();
    }
}