// Primitive types for freestanding NativeAOT compatibility.
// IntPtr: Used extensively in EngineHeap, IMemoryProvider, GCHandle.
// UIntPtr: Used by RuntimeTypeHandle, TypeCast, RuntimeExports.
// ReadOnlySpan<T>: Used in IUtf8Provider, FreestandingUtf8Provider.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IntPtr
    {
        private nint _value;
        public const int Size = 8;
        public IntPtr(void* value) { _value = (nint)value; }
        public static IntPtr Zero => new IntPtr(null);
        public int ToInt32() => (int)_value;
        public long ToInt64() => (long)_value;
        public void* ToPointer() => (void*)_value;
        public static explicit operator IntPtr(void* value) => new IntPtr(value);
        public static explicit operator void*(IntPtr value) => (void*)value._value;
        public static bool operator ==(IntPtr a, IntPtr b) => a._value == b._value;
        public static bool operator !=(IntPtr a, IntPtr b) => a._value != b._value;
        public static IntPtr operator +(IntPtr a, int offset) => new IntPtr((void*)((byte*)a.ToPointer() + offset));
        public override bool Equals(object obj) => obj is IntPtr other && _value == other._value;
        public override int GetHashCode() => (int)_value;
        public override string ToString() => _value.ToString();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UIntPtr { private nuint _value; }

    [StructLayout(LayoutKind.Sequential)]
    public struct UInt32 { private uint _value; public const uint MaxValue = 0xFFFFFFFFu; }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ReadOnlySpan<T>
    {
        private readonly T[] _array;
        private readonly int _start;
        private readonly int _length;

        public ReadOnlySpan(T[] array)
        {
            _array = array;
            _start = 0;
            _length = array.Length;
        }

        public ReadOnlySpan(T[] array, int start, int length)
        {
            _array = array;
            _start = start;
            _length = length;
        }

        public int Length => _length;
        public T this[int index] => _array[_start + index];
        public ReadOnlySpan<T> Slice(int start) => Slice(start, _length - start);
        public ReadOnlySpan<T> Slice(int start, int length) => new ReadOnlySpan<T>(_array, _start + start, length);
        public T[] ToArray()
        {
            var result = new T[_length];
            Array.Copy(_array, _start, result, 0, _length);
            return result;
        }
    }

    public enum GCHandleType { Normal = 0, Weak = 1, WeakTrackResurrection = 2, Pinned = 3 }

    public struct GCHandle
    {
        private IntPtr _handle;
        public static GCHandle Alloc(object value) => default;
        public static GCHandle Alloc(object value, GCHandleType type) => default;
        public static GCHandle FromIntPtr(IntPtr value) => default;
        public static IntPtr ToIntPtr(GCHandle handle) => default;
        public object Target => null;
        public void Free() { }
        public IntPtr AddrOfPinnedObject() => default;
    }
}