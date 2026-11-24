// Minimal class library ("mini-CoreLib") for a freestanding NativeAOT kernel.
// M1-M2 additions in: CompilerSupport.cs, CollectionsInterfaces.cs, Delegates.cs, PrimitiveTypes.cs, TupleTypes.cs, StringComparer.cs, DateTime.cs, Exceptions.cs

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal.Runtime;

namespace System
{
    public unsafe class Object
    {
#pragma warning disable 649
        private MethodTable* m_pEEType;
#pragma warning restore 649
        public Object() { }
        ~Object() { }
        public virtual bool Equals(object o) => false;
        public virtual int GetHashCode() => 0;
        public virtual string ToString() => null;
        internal MethodTable* GetMethodTable() => m_pEEType;
        public virtual Type GetType() => null;
    }

    public struct Void { }

    // Primitives: the compiler special-cases these and their sizes; the private
    // self-typed field is the documented data contract with the runtime.
#pragma warning disable 0169
    public struct Boolean { private bool _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Char { private char _value; public static bool IsHighSurrogate(char c) => c >= 0xD800 && c <= 0xDBFF; public static bool IsLowSurrogate(char c) => c >= 0xDC00 && c <= 0xDFFF; public static int ConvertToUtf32(char highSurrogate, char lowSurrogate) => ((highSurrogate - 0xD800) << 10) + (lowSurrogate - 0xDC00) + 0x10000; public static bool IsDigit(char c) => c >= '0' && c <= '9'; public static bool IsLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'); public static bool IsLetterOrDigit(char c) => IsLetter(c) || IsDigit(c); public static bool IsWhiteSpace(char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r' || c == '\v' || c == '\f'; public override string ToString() => ""; }
    [StructLayout(LayoutKind.Sequential)]
    public struct SByte { private sbyte _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Byte { private byte _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int16 { private short _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct UInt16 { private ushort _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int32 { private int _value; public const int MaxValue = 0x7FFFFFFF; public static bool TryParse(string s, out int result) { result = 0; return false; } public static int Parse(string s) => Parse(s, null); public static int Parse(string s, IFormatProvider provider) => Parse(s, NumberStyles.Integer, provider); public static int Parse(string s, NumberStyles style, IFormatProvider provider) => ParseInt32(s); private static int ParseInt32(string s) { if (string.IsNullOrEmpty(s)) throw new FormatException(); int i = 0, len = s.Length; while (i < len && char.IsWhiteSpace(s[i])) i++; bool neg = false; if (i < len) { if (s[i] == '-') { neg = true; i++; } else if (s[i] == '+') i++; } int result = 0; bool hasDigits = false; while (i < len) { char c = s[i]; if (c >= '0' && c <= '9') { result = result * 10 + (c - '0'); hasDigits = true; i++; } else if (char.IsWhiteSpace(c)) { while (i < len && char.IsWhiteSpace(s[i])) i++; break; } else { throw new FormatException(); } } if (!hasDigits) throw new FormatException(); return neg ? -result : result; } public override string ToString() { if (_value == 0) return "0"; bool neg = _value < 0; uint v = neg ? (uint)(-(long)_value) : (uint)_value; int len = 0; uint t = v; while (t > 0) { len++; t /= 10; } char[] chars = new char[len + (neg ? 1 : 0)]; int pos = chars.Length - 1; while (v > 0) { chars[pos--] = (char)('0' + (int)(v % 10)); v /= 10; } if (neg) chars[0] = '-'; return new string(chars); } }

    [StructLayout(LayoutKind.Sequential)]
    public struct UInt64 { private ulong _value; public const ulong MaxValue = 0xFFFFFFFFFFFFFFFFUL; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int64 { private long _value; public const long MaxValue = 0x7FFFFFFFFFFFFFFFL; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Single { private float _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Double { private double _value; public const double NaN = (double)0x7FF8000000000000L; public const double PositiveInfinity = (double)0x7FF0000000000000L; public const double NegativeInfinity = (double)0xFFF0000000000000L; public static bool IsNaN(double d) => d != d; public static bool IsPositiveInfinity(double d) => PositiveInfinity == d; public static bool IsNegativeInfinity(double d) => NegativeInfinity == d; public static double Parse(string s) => Parse(s, null); public static double Parse(string s, IFormatProvider provider) => Parse(s, NumberStyles.Float, provider); public static double Parse(string s, NumberStyles style, IFormatProvider provider) => ParseDouble(s); private static double ParseDouble(string s) { if (s == null) return 0; int i = 0, len = s.Length; while (i < len && char.IsWhiteSpace(s[i])) i++; bool neg = false; if (i < len) { if (s[i] == '-') { neg = true; i++; } else if (s[i] == '+') i++; } double result = 0; while (i < len && s[i] >= '0' && s[i] <= '9') { result = result * 10 + (s[i] - '0'); i++; } if (i < len && s[i] == '.') { i++; double frac = 0; int fracDigits = 0; while (i < len && s[i] >= '0' && s[i] <= '9') { frac = frac * 10 + (s[i] - '0'); fracDigits++; i++; } if (fracDigits > 0) { double divisor = 1; for (int j = 0; j < fracDigits; j++) divisor *= 10; result += frac / divisor; } } return neg ? -result : result; } }
#pragma warning restore 0169

    public abstract class ValueType { }
    public abstract class Enum : ValueType
    {
        public override string ToString()
        {
            // Minimal implementation - in freestanding we can't use reflection
            // Derived enums should override this
            return "Enum";
        }
    }

    public interface IFormatProvider
    {
        object GetFormat(Type formatType);
    }

    [Flags]
    public enum NumberStyles
    {
        None = 0,
        AllowLeadingWhite = 0x0001,
        AllowTrailingWhite = 0x0002,
        AllowLeadingSign = 0x0004,
        AllowTrailingSign = 0x0008,
        AllowParentheses = 0x0010,
        AllowDecimalPoint = 0x0020,
        AllowThousands = 0x0040,
        AllowExponent = 0x0080,
        AllowCurrencySymbol = 0x0100,
        AllowHexSpecifier = 0x0200,
        Integer = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign,
        HexNumber = AllowLeadingWhite | AllowTrailingWhite | AllowHexSpecifier,
        Number = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowDecimalPoint | AllowThousands,
        Float = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowDecimalPoint | AllowExponent,
        Currency = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowDecimalPoint | AllowThousands | AllowCurrencySymbol,
        Any = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowDecimalPoint | AllowThousands | AllowCurrencySymbol | AllowExponent,
    }

    public struct Nullable<T> where T : struct
    {
        public bool HasValue;
        public T Value;
        public Nullable(T value) { HasValue = true; Value = value; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class String
    {
#if TARGET_64BIT
        internal const int FIRST_CHAR_OFFSET = 8 + sizeof(int);
#else
        internal const int FIRST_CHAR_OFFSET = 4 + sizeof(int);
#endif
#pragma warning disable 169, 649
        private int _stringLength;
#pragma warning restore
        public int Length => _stringLength;
        public unsafe char this[int index]
        {
            get
            {
                if (index < 0 || index >= _stringLength) return '\0';
                // NativeAOT string layout: [MethodTable:8][Length:4][Chars:2*Length]
                // _stringLength is at offset 8 from object base
                // Character data starts at offset 12 (FIRST_CHAR_OFFSET)
                byte* obj = (byte*)Unsafe.AsPointer(ref _stringLength);
                char* chars = (char*)(obj + sizeof(int));
                return chars[index];
            }
        }
        public static string Concat(string s1, string s2)
        {
            if (s1 == null) return s2 ?? Empty;
            if (s2 == null) return s1;
            int len1 = s1.Length;
            int len2 = s2.Length;
            if (len1 == 0) return s2;
            if (len2 == 0) return s1;
            char[] result = new char[len1 + len2];
            for (int i = 0; i < len1; i++) result[i] = s1[i];
            for (int i = 0; i < len2; i++) result[len1 + i] = s2[i];
            return new string(result, 0, len1 + len2);
        }

        public static string Concat(string s1, string s2, string s3)
        {
            if (s1 == null) return Concat(s2, s3);
            if (s2 == null) return Concat(s1, s3);
            if (s3 == null) return Concat(s1, s2);
            int len1 = s1.Length;
            int len2 = s2.Length;
            int len3 = s3.Length;
            char[] result = new char[len1 + len2 + len3];
            for (int i = 0; i < len1; i++) result[i] = s1[i];
            for (int i = 0; i < len2; i++) result[len1 + i] = s2[i];
            for (int i = 0; i < len3; i++) result[len1 + len2 + i] = s3[i];
            return new string(result, 0, len1 + len2 + len3);
        }

        public static string Concat(params string[] values)
        {
            if (values == null || values.Length == 0) return Empty;
            int totalLen = 0;
            foreach (var v in values) if (v != null) totalLen += v.Length;
            char[] result = new char[totalLen];
            int pos = 0;
            foreach (var v in values)
            {
                if (v != null)
                {
                    for (int i = 0; i < v.Length; i++) result[pos++] = v[i];
                }
            }
            return new string(result, 0, totalLen);
        }

        public static string Concat(object arg0) => arg0 != null ? arg0.ToString() : Empty;

        public static string Concat(object arg0, object arg1)
        {
            var s0 = arg0 != null ? arg0.ToString() : Empty;
            var s1 = arg1 != null ? arg1.ToString() : Empty;
            return Concat(s0, s1);
        }

        public static string Concat(object arg0, object arg1, object arg2)
        {
            var s0 = arg0 != null ? arg0.ToString() : Empty;
            var s1 = arg1 != null ? arg1.ToString() : Empty;
            var s2 = arg2 != null ? arg2.ToString() : Empty;
            return Concat(s0, s1, s2);
        }

        public static string Concat(string s1, string s2, string s3, string s4)
        {
            if (s1 == null) return Concat(s2, s3, s4);
            if (s2 == null) return Concat(s1, s3, s4);
            if (s3 == null) return Concat(s1, s2, s4);
            if (s4 == null) return Concat(s1, s2, s3);
            int len1 = s1.Length;
            int len2 = s2.Length;
            int len3 = s3.Length;
            int len4 = s4.Length;
            char[] result = new char[len1 + len2 + len3 + len4];
            for (int i = 0; i < len1; i++) result[i] = s1[i];
            for (int i = 0; i < len2; i++) result[len1 + i] = s2[i];
            for (int i = 0; i < len3; i++) result[len1 + len2 + i] = s3[i];
            for (int i = 0; i < len4; i++) result[len1 + len2 + len3 + i] = s4[i];
            return new string(result, 0, len1 + len2 + len3 + len4);
        }
        public static bool operator ==(String a, String b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
        public static bool operator !=(String a, String b) => !(a == b);
        public static bool Equals(string a, string b) => a == b;
        public static bool Equals(string a, string b, StringComparison comparison) => a == b;
        public bool Equals(string other) => this == other;
        public bool Equals(string other, StringComparison comparison) => this == other;
        public static string Format(string format, object arg0) => throw null;
        public static string Format(string format, object arg0, object arg1) => throw null;
        public static string Format(string format, object arg0, object arg1, object arg2) => throw null;
        public static string Format(string format, params object[] args) => throw null;
        public static readonly string Empty = null;
        public static bool IsNullOrEmpty(string value) => value == null || value.Length == 0;
        public String(char[] value)
        {
            if (value == null)
            {
                _stringLength = 0;
                return;
            }
            _stringLength = value.Length;
            // Copy data into this instance's buffer
            unsafe
            {
                fixed (char* src = value)
                {
                    byte* obj = (byte*)Unsafe.AsPointer(ref _stringLength);
                    byte* dst = (byte*)(obj + sizeof(int));
                    Buffer.Memmove(dst, (byte*)src, (nuint)value.Length * 2);
                }
            }
        }

        public String(char[] value, int startIndex, int length)
        {
            if (value == null || length == 0)
            {
                _stringLength = 0;
                return;
            }
            if (startIndex < 0 || length < 0 || startIndex + length > value.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            _stringLength = length;
            // Copy data into this instance's buffer
            unsafe
            {
                fixed (char* src = value)
                {
                    byte* obj = (byte*)Unsafe.AsPointer(ref _stringLength);
                    byte* dst = (byte*)(obj + sizeof(int));
                    Buffer.Memmove(dst, (byte*)(src + startIndex), (nuint)length * 2);
                }
            }
        }

        public String(char c, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException();
            _stringLength = count;
            // Fill buffer with the character
            unsafe
            {
                byte* obj = (byte*)Unsafe.AsPointer(ref _stringLength);
                byte* dst = (byte*)(obj + sizeof(int));
                for (int i = 0; i < count; i++)
                {
                    dst[i * 2] = (byte)c;
                    dst[i * 2 + 1] = 0;
                }
            }
        }

        public unsafe String(char* value, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException();
            _stringLength = length;
            if (length == 0) return;
            byte* obj = (byte*)Unsafe.AsPointer(ref _stringLength);
            byte* dst = (byte*)(obj + sizeof(int));
            Buffer.Memmove(dst, (byte*)value, (nuint)length * 2);
        }

        public static String Ctor(char[] value)
        {
            var result = FastAllocateString(value != null ? value.Length : 0);
            if (result != null && value != null && value.Length > 0)
            {
                CopyChars(value, 0, result, 0, value.Length);
            }
            return result;
        }

        public static String Ctor(char[] value, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || (value != null && startIndex + length > value.Length))
            {
                throw new ArgumentOutOfRangeException();
            }
            var result = FastAllocateString(length);
            if (result != null && length > 0 && value != null)
            {
                CopyChars(value, startIndex, result, 0, length);
            }
            return result;
        }

        private static void CopyChars(char[] src, int srcIndex, String dst, int dstIndex, int count)
        {
            unsafe
            {
                byte* obj = (byte*)Unsafe.AsPointer(ref dst._stringLength);
                byte* dstData = (byte*)(obj + sizeof(int));
                for (int i = 0; i < count; i++)
                {
                    dstData[(dstIndex + i) * 2] = (byte)src[srcIndex + i];
                    dstData[(dstIndex + i) * 2 + 1] = 0;
                }
            }
        }

        public static unsafe String Ctor(char* value, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException();
            var result = FastAllocateString(length);
            if (result != null && length > 0 && value != null)
            {
                byte* obj = (byte*)Unsafe.AsPointer(ref result._stringLength);
                byte* dst = (byte*)(obj + sizeof(int));
                Buffer.Memmove(dst, (byte*)value, (nuint)length * 2);
            }
            return result;
        }

        [DllImport("osbnative", EntryPoint = "RhNewString")]
        private static extern ulong RhNewStringRaw(IntPtr methodTable, int length);

        // Cached String MethodTable - initialized on first use
        private static IntPtr s_stringMethodTable = IntPtr.Zero;

        public static String FastAllocateString(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException();

            // Obtain a valid String MethodTable on first use.
            if (s_stringMethodTable == IntPtr.Zero)
            {
                s_stringMethodTable = GetStringMethodTable();
            }

            ulong ptr = RhNewStringRaw(s_stringMethodTable, length);
            if (ptr == 0) return null;
            String result = default!;
            ref ulong resultRef = ref Unsafe.As<String, ulong>(ref result);
            resultRef = ptr;
            result._stringLength = length;
            return result;
        }

        // Reads the MethodTable pointer (first 8 bytes) from a string object.
        private static unsafe IntPtr GetStringMethodTable()
        {
            string s = "\0"; // force a string object to exist
            byte* obj = (byte*)Unsafe.AsPointer(ref s._stringLength);
            obj -= sizeof(IntPtr); // _stringLength is at offset 8; MT is at offset 0
            return *(IntPtr*)obj;
        }

        public string ToUpperInvariant()
        {
            if (Length == 0) return this;
            var chars = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                char c = this[i];
                chars[i] = (c >= 'a' && c <= 'z') ? (char)(c - 'a' + 'A') : c;
            }
            return Ctor(chars);
        }

        public string Substring(int startIndex)
        {
            return Substring(startIndex, Length - startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex + length > Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (length == 0) return Empty;
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = this[startIndex + i];
            }
            return Ctor(chars);
        }

        public string PadLeft(int totalWidth, char paddingChar)
        {
            if (totalWidth <= Length) return this;
            int padCount = totalWidth - Length;
            var chars = new char[totalWidth];
            for (int i = 0; i < padCount; i++)
            {
                chars[i] = paddingChar;
            }
            for (int i = 0; i < Length; i++)
            {
                chars[padCount + i] = this[i];
            }
            return Ctor(chars);
        }

        public string TrimEnd(params char[] trimChars) => this;
        public string Trim() => this;
        public string Replace(string oldValue, string newValue) => this;
        public bool EndsWith(string value)
        {
            if (value == null) return false;
            if (value.Length > Length) return false;
            for (int i = 0; i < value.Length; i++)
            {
                if (this[Length - value.Length + i] != value[i]) return false;
            }
            return true;
        }

        public bool Contains(string value)
        {
            if (value == null) return false;
            if (value.Length == 0) return true;
            if (value.Length > Length) return false;
            for (int i = 0; i <= Length - value.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < value.Length; j++)
                {
                    if (this[i + j] != value[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return true;
            }
            return false;
        }
    }
}

namespace System.Text
{
    public class StringBuilder
    {
        // Fixed internal buffer - avoids managed array allocation issues in freestanding
        private const int MaxBufferSize = 256;
        private char[] _chars;
        private int _length;
        private static char[] _sharedBuffer;
        private static bool _bufferInUse;

        public StringBuilder() : this(16) { }

        public StringBuilder(int capacity)
        {
            if (capacity > MaxBufferSize) capacity = MaxBufferSize;
            // Always allocate a fresh array to avoid static field init issues
            _chars = new char[capacity];
            _length = 0;
        }

        public StringBuilder(string value) : this(value != null ? value.Length : 16)
        {
            if (value != null)
            {
                Append(value);
            }
        }

        public StringBuilder(string value, int capacity) : this(capacity)
        {
            if (value != null)
            {
                Append(value);
            }
        }

        public int Length
        {
            get => _length;
            set
            {
                if (value < 0 || value > MaxBufferSize) throw new ArgumentOutOfRangeException();
                _length = value;
            }
        }

        public int Capacity
        {
            get => MaxBufferSize;
            set
            {
                if (value < _length) throw new ArgumentOutOfRangeException();
                // Fixed capacity, ignore resize requests
            }
        }

        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= _length) throw new IndexOutOfRangeException();
                return _chars[index];
            }
            set
            {
                if (index < 0 || index >= _length) throw new IndexOutOfRangeException();
                _chars[index] = value;
            }
        }

        public StringBuilder Append(string value)
        {
            if (value != null)
            {
                EnsureCapacity(_length + value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    _chars[_length++] = value[i];
                }
            }
            return this;
        }

        public StringBuilder Append(char value)
        {
            EnsureCapacity(_length + 1);
            _chars[_length++] = value;
            return this;
        }

        public StringBuilder Append(char[] value)
        {
            if (value != null)
            {
                EnsureCapacity(_length + value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    _chars[_length + i] = value[i];
                }
                _length += value.Length;
            }
            return this;
        }

        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            if (value != null)
            {
                EnsureCapacity(_length + charCount);
                for (int i = 0; i < charCount; i++)
                {
                    _chars[_length + i] = value[startIndex + i];
                }
                _length += charCount;
            }
            return this;
        }

        public StringBuilder AppendLine()
        {
            return Append("\n");
        }

        public StringBuilder AppendLine(string value)
        {
            Append(value);
            return Append("\n");
        }

        public StringBuilder AppendFormat(string format, object arg0) => throw null;
        public StringBuilder AppendFormat(string format, object arg0, object arg1) => throw null;
        public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2) => throw null;

        public StringBuilder Append(object value)
        {
            return Append(value != null ? value.ToString() : null);
        }

        public override string ToString()
        {
            return System.String.Ctor(_chars, 0, _length);
        }

        public void Clear()
        {
            _length = 0;
        }

        private void EnsureCapacity(int required)
        {
            if (required > MaxBufferSize)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (_chars == null || _chars.Length < required)
            {
                var newSize = _chars != null ? _chars.Length * 2 : 16;
                if (newSize < required) newSize = required;
                if (newSize > MaxBufferSize) newSize = MaxBufferSize;
                var newChars = new char[newSize];
                for (int i = 0; i < _length; i++)
                {
                    newChars[i] = _chars[i];
                }
                _chars = newChars;
            }
        }
    }
}

namespace System
{
    public class Array
    {
        public static void Ctor() => throw null;
        public static void CtorRare() => throw null;
#pragma warning disable 649
        // first field: the runtime/compiler depend on this data contract
        private int _numComponents;
#pragma warning restore 649
        public int Length => _numComponents;
        
        public unsafe static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null || destinationArray == null) throw new ArgumentNullException("array");
            if (sourceIndex < 0 || destinationIndex < 0 || length < 0) throw new ArgumentOutOfRangeException();
            if (sourceIndex + length > sourceArray.Length) throw new ArgumentException();
            if (destinationIndex + length > destinationArray.Length) throw new ArgumentException();

            // Array layout: [MethodTable:8][Length:4][Data...]
            // Data starts at offset 12 from object base
            // _numComponents is at offset 8, so data = &_numComponents + 4
            fixed (int* srcLen = &sourceArray._numComponents)
            fixed (int* dstLen = &destinationArray._numComponents)
            {
                byte* srcData = (byte*)(srcLen + 1); // +1 int = +4 bytes = offset 12
                byte* dstData = (byte*)(dstLen + 1);
                
                // For char arrays, element size is 2
                int elementSize = 2; // char
                
                byte* src = srcData + (nuint)sourceIndex * (nuint)elementSize;
                byte* dst = dstData + (nuint)destinationIndex * (nuint)elementSize;
                
                // Simple byte copy
                for (nuint i = 0; i < (nuint)length * (nuint)elementSize; i++)
                {
                    dst[i] = src[i];
                }
            }
        }
        
        public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length) =>
            Copy(sourceArray, (int)sourceIndex, destinationArray, (int)destinationIndex, (int)length);
        
        public static void Copy(Array sourceArray, Array destinationArray, int length) =>
            Copy(sourceArray, 0, destinationArray, 0, length);
        public static void Clear(Array array, int index, int length) => throw null;
        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }

    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }
        public static void Resize<T>(ref T[] array, int newSize) => throw null;
        public static int IndexOf(Array array, object value) => -1;
    }

    // Generic arrays (T[]) are represented by this pseudo-type; the compiler
    // requires it to exist in the class library.
    internal class Array<T> : Array { }

    public struct TypedReference
    {
        public static TypedReference GetRefAny(RuntimeTypeHandle type, ref byte value) => throw null;
    }

    public struct RuntimeTypeHandle
    {
#pragma warning disable 649
        private IntPtr _value;
#pragma warning restore 649
        internal RuntimeTypeHandle(IntPtr value) { _value = value; }
        internal static unsafe RuntimeTypeHandle GetRuntimeTypeHandleFromMethodTable(MethodTable* pMT)
            => default;
    }
    public struct RuntimeMethodHandle { }
    public struct RuntimeFieldHandle { }

    public class Attribute { }

    public abstract class Type
    {
        internal static unsafe Type GetTypeFromMethodTable(MethodTable* pMT) => null;
        internal static unsafe Type GetTypeFromMethodTableMaybeNull(MethodTable* pMT) => null;
        public virtual string Name => null;
        public static Type GetTypeFromHandle(RuntimeTypeHandle handle) => null;
        public static bool operator ==(Type left, Type right) => left is not null && left.Equals(right);
        public static bool operator !=(Type left, Type right) => !(left == right);
    }

    public sealed class RuntimeType : Type { }

    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class FlagsAttribute : Attribute { }

    [Flags]
    public enum AttributeTargets
    {
        Assembly = 0x0001, Module = 0x0002, Class = 0x0004, Struct = 0x0008,
        Enum = 0x0010, Constructor = 0x0020, Method = 0x0040, Property = 0x0080,
        Field = 0x0100, Event = 0x0200, Interface = 0x0400, Parameter = 0x0800,
        Delegate = 0x1000, ReturnValue = 0x2000, GenericParameter = 0x4000,
        All = 0x7FFF,
    }

    public sealed class AttributeUsageAttribute : Attribute
    {
        public AttributeUsageAttribute(AttributeTargets validOn) { }
        public bool AllowMultiple { get; set; }
        public bool Inherited { get; set; }
    }

    public class AppContext
    {
        public static void SetData(string s, object o) { }
    }

    public static class GC
    {
        public static void Collect() { }
        public static void WaitForPendingFinalizers() { }
        public static int Generation(int maxGeneration) => 0;
    }
}

namespace System.Runtime.CompilerServices
{
    public class RuntimeHelpers
    {
        public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
    }

    public static class RuntimeFeature
    {
        public const string UnmanagedSignatureCallingConvention = nameof(UnmanagedSignatureCallingConvention);
        public const string ByRefLikeGenerics = nameof(ByRefLikeGenerics);
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class IntrinsicAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
    public sealed class IsByRefLikeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class IsVolatile : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FixedBufferAttribute : Attribute
    {
        public FixedBufferAttribute(Type elementType, int length) { }
    }

    public enum MethodImplOptions
    {
        Unmanaged = 0x0004,
        NoInlining = 0x0008,
        AggressiveInlining = 0x0100,
        InternalCall = 0x1000,
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MethodImplAttribute : Attribute
    {
        public MethodImplAttribute(MethodImplOptions methodImplOptions) { }
        public MethodImplAttribute(short value) { }
    }
}

namespace System.Runtime.InteropServices
{
    public enum LayoutKind { Sequential = 0, Explicit = 2, Auto = 3 }
    public enum CharSet { None = 1, Ansi = 2, Unicode = 3, Auto = 4 }
    public enum CallingConvention { Winapi = 1, Cdecl = 2, StdCall = 3 }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class StructLayoutAttribute : Attribute { public StructLayoutAttribute(LayoutKind layoutKind) { } public StructLayoutAttribute(short layoutKind) { } public CharSet CharSet; public int Pack; public int Size; }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldOffsetAttribute : Attribute { public FieldOffsetAttribute(int offset) { } }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DllImportAttribute : Attribute { public DllImportAttribute(string dllName) { } public string EntryPoint; public CharSet CharSet; }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UnmanagedCallersOnlyAttribute : Attribute { public Type[] CallConvs; public string EntryPoint; }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UnmanagedCallConvAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class OutAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InAttribute : Attribute { }

    public interface IDynamicInterfaceCastable
    {
        RuntimeTypeHandle GetDynamicInterfaceImplementation(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented);
        bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented);
    }
}

namespace System.Runtime
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class RuntimeExportAttribute : Attribute
    {
        public RuntimeExportAttribute(string entry) { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class RuntimeImportAttribute : Attribute
    {
        public RuntimeImportAttribute(string dllName, string entry) { }
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class AsyncHelpers
    {
        public static void AllocContinuation() => throw null;
    }

    public static unsafe class Unsafe
    {
        [Intrinsic] public static IntPtr ByteOffset<T>(ref readonly T origin, ref readonly T target) => throw null;
        [Intrinsic] public static void* AsPointer<T>(ref readonly T value) => throw null;
        [Intrinsic] public static int SizeOf<T>() => throw null;
        [Intrinsic] public static T As<T>(object value) where T : class => throw null;
        [Intrinsic] public static ref TTo As<TFrom, TTo>(ref TFrom source) => throw null;
        [Intrinsic] public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset) => throw null;
        [Intrinsic] public static ref T AsRef<T>(in T source) => throw null;
        [Intrinsic] public static T ReadUnaligned<T>(void* source) => throw null;
        [Intrinsic] public static T ReadUnaligned<T>(ref readonly byte source) => throw null;
        [Intrinsic] public static void CopyBlock(void* destination, void* source, ulong byteCount) => throw null;
        [Intrinsic] public static void CopyBlock(ref byte destination, ref readonly byte source, ulong byteCount) => throw null;

        public static ref T Add<T>(ref T source, int elementOffset) =>
            ref AddByteOffset(ref source, (IntPtr)(elementOffset * (nint)SizeOf<T>()));
        public static ref T Add<T>(ref T source, IntPtr elementOffset) =>
            ref AddByteOffset(ref source, (IntPtr)((nint)elementOffset * (nint)SizeOf<T>()));
    }
}

namespace System
{
    internal static partial class SpanHelpers
    {
        [Intrinsic]
        public static unsafe void ClearWithoutReferences(ref byte dest, nuint len) => Fill(ref dest, 0, len);

        [Intrinsic]
        internal static unsafe void Memmove(ref byte dest, ref byte src, nuint len)
        {
            if ((nuint)(nint)Unsafe.ByteOffset(ref src, ref dest) >= len)
                for (nuint i = 0; i < len; i++)
                    Unsafe.Add(ref dest, (nint)i) = Unsafe.Add(ref src, (nint)i);
            else
                for (nuint i = len; i > 0; i--)
                    Unsafe.Add(ref dest, (nint)(i - 1)) = Unsafe.Add(ref src, (nint)(i - 1));
        }

        internal static void Fill(ref byte dest, byte value, nuint len)
        {
            for (nuint i = 0; i < len; i++)
                Unsafe.Add(ref dest, (nint)i) = value;
        }
    }

    public static unsafe partial class Buffer
    {
        public static unsafe void Memmove(byte* dest, byte* src, nuint len)
        {
            if (dest == src || len == 0) return;
            if (dest < src) { for (nuint i = 0; i < len; i++) dest[i] = src[i]; }
            else { for (nuint i = len; i > 0; i--) dest[i - 1] = src[i - 1]; }
        }
        public static void Memcpy(byte* dest, byte* src, nuint len) => Memmove(dest, src, len);
        public static unsafe void MemoryCopy(byte* source, byte* destination, nuint byteCount) => Memmove(destination, source, byteCount);
        public static unsafe void MemoryCopy(byte* source, byte* destination, int sourceBytes, int destinationBytes) => Memmove(destination, source, (nuint)sourceBytes);
        internal static void BulkMoveWithWriteBarrier(ref byte destination, ref byte source, nuint byteCount)
            => SpanHelpers.Memmove(ref destination, ref source, byteCount);
    }
}

namespace Internal.Runtime
{
    // Opaque, compiler-known type. We only ever use MethodTable*.
    internal unsafe struct MethodTable { }

    internal enum ExceptionStringID { }
}

namespace Internal.Runtime.CompilerHelpers
{
    internal static class StartupCodeHelpers
    {
        [System.Runtime.RuntimeExport("RhpReversePInvoke")]
        static void RhpReversePInvoke(IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpReversePInvokeReturn")]
        static void RhpReversePInvokeReturn(IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpPInvoke")]
        static void RhpPInvoke(IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpPInvokeReturn")]
        static void RhpPInvokeReturn(IntPtr frame) { }
        [System.Runtime.RuntimeExport("RhpFallbackFailFast")]
        static void RhpFallbackFailFast() { while (true) { } }
    }

internal static class ThrowHelpers
    {
        static void Fail() { while (true) { } }
        internal static void ThrowBodyRemoved() => Fail(); internal static void ThrowFeatureBodyRemoved() => Fail();
        internal static void ThrowInstanceBodyRemoved() => Fail(); internal static void ThrowUnavailableType() => Fail();
        internal static void ThrowBadImageFormatException(ExceptionStringID id) => Fail(); internal static void ThrowTypeLoadException(ExceptionStringID id, string className, string typeName) => Fail();
        internal static void ThrowTypeLoadExceptionWithArgument(ExceptionStringID id, string className, string typeName, string messageArg) => Fail();
        internal static void ThrowMissingMethodException(ExceptionStringID id, string methodName) => Fail(); internal static void ThrowMissingFieldException(ExceptionStringID id, string fieldName) => Fail();
        internal static void ThrowFileNotFoundException(ExceptionStringID id, string fileName) => Fail(); internal static void ThrowInvalidProgramException(ExceptionStringID id) => Fail();
        internal static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName) => throw null; internal static void ThrowMarshalDirectiveException(ExceptionStringID id) => Fail();
        internal static void ThrowAmbiguousMatchException(ExceptionStringID id) => Fail(); internal static void ThrowNotSupportedInlineArrayEqualsGetHashCode() => Fail(); internal static void ThrowIndexOutOfRangeException() => Fail();
        internal static void ThrowOverflowException() => Fail();
        internal static void ThrowPlatformNotSupportedException() => Fail();
    }
}

namespace System.Runtime.Versioning
{
    // Stub for AssemblyAttributes.cs generation under NoStdLib
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    internal sealed class TargetFrameworkAttribute : Attribute
    {
        public TargetFrameworkAttribute(string frameworkName) { }
        public string FrameworkName { get; }
        public string FrameworkDisplayName { get; set; }
    }
}

namespace System.Runtime.CompilerServices
{
    // Required by NativeAOT for generic unboxing thunks (e.g., Boxed_ValueTuple)
    internal unsafe struct RawData
    {
        public void* Data;
        public int Length;
    }
}