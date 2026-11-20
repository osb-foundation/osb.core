// System.Index for freestanding NativeAOT compatibility.
// Used by Parser, BigInteger, BigIntegerArithmetic.

namespace System
{
    public readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;
        public Index() { _value = 0; }
        public Index(int value) { _value = value; }
        public static Index FromStart(int i) => new Index(i);
        public static Index FromEnd(int i) => new Index(~i);
        public int GetOffset(int length) => _value < 0 ? length + _value : _value;
        public override bool Equals(object obj) => obj is Index other && _value == other._value;
        public bool Equals(Index other) => _value == other._value;
        public override int GetHashCode() => _value;
        public override string ToString() => _value.ToString();
        public static bool operator ==(Index left, Index right) => left._value == right._value;
        public static bool operator !=(Index left, Index right) => left._value != right._value;
    }
}
