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
    public struct Char { private char _value; public static bool IsHighSurrogate(char c) => c >= 0xD800 && c <= 0xDBFF; public static bool IsLowSurrogate(char c) => c >= 0xDC00 && c <= 0xDFFF; public static int ConvertToUtf32(char highSurrogate, char lowSurrogate) => ((highSurrogate - 0xD800) << 10) + (lowSurrogate - 0xDC00) + 0x10000; public static bool IsDigit(char c) => c >= '0' && c <= '9'; public static bool IsLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'); public static bool IsLetterOrDigit(char c) => IsLetter(c) || IsDigit(c); }
    [StructLayout(LayoutKind.Sequential)]
    public struct SByte { private sbyte _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Byte { private byte _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int16 { private short _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct UInt16 { private ushort _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int32 { private int _value; public const int MaxValue = 0x7FFFFFFF; public static bool TryParse(string s, out int result) { result = 0; return false; } public static int Parse(string s) => 0; public static int Parse(string s, IFormatProvider provider) => 0; public static int Parse(string s, NumberStyles style, IFormatProvider provider) => 0; }
    [StructLayout(LayoutKind.Sequential)]
    public struct UInt64 { private ulong _value; public const ulong MaxValue = 0xFFFFFFFFFFFFFFFFUL; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Int64 { private long _value; public const long MaxValue = 0x7FFFFFFFFFFFFFFFL; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Single { private float _value; }
    [StructLayout(LayoutKind.Sequential)]
    public struct Double { private double _value; public const double NaN = (double)0x7FF8000000000000L; public const double PositiveInfinity = (double)0x7FF0000000000000L; public const double NegativeInfinity = (double)0xFFF0000000000000L; public static bool IsNaN(double d) => d != d; public static bool IsPositiveInfinity(double d) => PositiveInfinity == d; public static bool IsNegativeInfinity(double d) => NegativeInfinity == d; public static double Parse(string s) => 0; public static double Parse(string s, IFormatProvider provider) => 0; public static double Parse(string s, NumberStyles style, IFormatProvider provider) => 0; }
#pragma warning restore 0169

    public abstract class ValueType { }
    public abstract class Enum : ValueType
    {
        public override string ToString() => "";
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

    public struct Nullable<T> where T : struct { public bool HasValue; public T Value; }

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
        public static string Concat(string s1, string s2) => s1 != null ? s2 != null ? s1 + s2 : s1 : s2 != null ? s2 : Empty;
        public static string Concat(string s1, string s2, string s3) => s1 != null ? s2 != null ? s3 != null ? s1 + s2 + s3 : s1 + s2 : s3 != null ? s1 + s3 : s1 : s2 != null ? s3 != null ? s2 + s3 : s2 : s3 != null ? s3 : Empty;
        public static string Concat(params string[] values) => Empty;
        public static string Concat(object arg0) => arg0 != null ? arg0.ToString() : Empty;
        public static string Concat(object arg0, object arg1) => (arg0 != null ? arg0.ToString() : Empty) + (arg1 != null ? arg1.ToString() : Empty);
        public static string Concat(object arg0, object arg1, object arg2) => (arg0 != null ? arg0.ToString() : Empty) + (arg1 != null ? arg1.ToString() : Empty) + (arg2 != null ? arg2.ToString() : Empty);
        public static string Concat(string s1, string s2, string s3, string s4) => s1 != null ? s2 != null ? s3 != null ? s4 != null ? s1 + s2 + s3 + s4 : s1 + s2 + s3 : s4 != null ? s1 + s2 + s4 : s1 + s2 : s3 != null ? s4 != null ? s1 + s3 + s4 : s1 + s3 : s4 != null ? s1 + s4 : s1 : s2 != null ? s3 != null ? s4 != null ? s2 + s3 + s4 : s2 + s3 : s4 != null ? s2 + s4 : s2 : s3 != null ? s4 != null ? s3 + s4 : s3 : s4 != null ? s4 : Empty;
        public static bool operator ==(string a, string b) => throw null;
        public static bool operator !=(string a, string b) => throw null;
        public static bool Equals(string a, string b) => throw null;
        public static bool Equals(string a, string b, StringComparison comparison) => throw null;
        public bool Equals(string other) => throw null;
        public bool Equals(string other, StringComparison comparison) => throw null;
        public static string Format(string format, object arg0) => throw null;
        public static string Format(string format, object arg0, object arg1) => throw null;
        public static string Format(string format, object arg0, object arg1, object arg2) => throw null;
        public static string Format(string format, params object[] args) => throw null;
        public static readonly string Empty = null;
        public String(char[] value) { }
        public String(char[] value, int startIndex, int length) { }
        public static String Ctor(char[] value) => null;
        public static String Ctor(char[] value, int startIndex, int length) => null;
        [Intrinsic]
        public static String FastAllocateString(int length) => null;
        public string ToUpperInvariant() => this;
        public string Substring(int startIndex) => throw null;
        public string Substring(int startIndex, int length) => throw null;
        public string PadLeft(int totalWidth, char paddingChar) => throw null;
        public string TrimEnd(params char[] trimChars) => throw null;
        public string Trim() => throw null;
        public string Replace(string oldValue, string newValue) => throw null;
        public bool EndsWith(string value) => false;
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
            if (!_bufferInUse)
            {
                _bufferInUse = true;
                _chars = _sharedBuffer ??= new char[MaxBufferSize];
            }
            else
            {
                // Fallback: allocate small array (may fail in freestanding)
                _chars = new char[capacity];
            }
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
                Array.Copy(value, 0, _chars, _length, value.Length);
                _length += value.Length;
            }
            return this;
        }

        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            if (value != null)
            {
                EnsureCapacity(_length + charCount);
                Array.Copy(value, startIndex, _chars, _length, charCount);
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
            return new string(_chars, 0, _length);
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
        public static T[] Empty<T>() => throw null;
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
        internal static void ThrowAmbiguousMatchException(ExceptionStringID id) => Fail();         internal static void ThrowNotSupportedInlineArrayEqualsGetHashCode() => Fail(); internal static void ThrowIndexOutOfRangeException() => Fail();
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