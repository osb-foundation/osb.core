// StringComparer for freestanding NativeAOT compatibility.
// Used by StringMap, Keywords, ExecutionContext, Interpreter for case-insensitive string keys.

using System.Collections;
using System.Collections.Generic;

namespace System
{
    public abstract class StringComparer : IEqualityComparer, IEqualityComparer<string>
    {
        public static StringComparer OrdinalIgnoreCase { get; } = new OrdinalIgnoreCaseComparer();

        public abstract bool Equals(string x, string y);
        public abstract int GetHashCode(string obj);

        bool IEqualityComparer.Equals(object x, object y) => Equals((string)x, (string)y);
        int IEqualityComparer.GetHashCode(object obj) => GetHashCode((string)obj);

        private sealed class OrdinalIgnoreCaseComparer : StringComparer
        {
            public override bool Equals(string x, string y)
            {
                if (x == y) return true;
                if (x == null || y == null) return false;
                if (x.Length != y.Length) return false;
                for (int i = 0; i < x.Length; i++)
                {
                    char cx = x[i];
                    char cy = y[i];
                    if (cx != cy)
                    {
                        // Simple ASCII case-insensitive comparison
                        if (cx >= 'a' && cx <= 'z') cx = (char)(cx - 'a' + 'A');
                        if (cy >= 'a' && cy <= 'z') cy = (char)(cy - 'a' + 'A');
                        if (cx != cy) return false;
                    }
                }
                return true;
            }
            public override int GetHashCode(string obj)
            {
                int h = 0;
                for (int i = 0; i < obj.Length; i++)
                {
                    char c = obj[i];
                    if (c >= 'a' && c <= 'z') c = (char)(c - 'a' + 'A');
                    h = h * 31 + c;
                }
                return h;
            }
        }
    }
}