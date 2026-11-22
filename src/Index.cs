// System.Index for freestanding NativeAOT compatibility.
// Used by Parser, BigInteger, BigIntegerArithmetic.

namespace System
{
    public readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;
        public Index() { _value = 0; }
        public Index(int value) { _value = value; }
        public Index(int value, bool fromEnd) { _value = fromEnd ? ~value : value; }
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

    public readonly struct Range
    {
        public Index Start { get; }
        public Index End { get; }
        public Range(Index start, Index end) { Start = start; End = end; }
        public static Range All => new Range(Index.FromStart(0), Index.FromEnd(0));

        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start = Start.GetOffset(length);
            int end = End.GetOffset(length);
            if (start < 0) start = 0;
            if (end > length) end = length;
            if (start > end) start = end;
            return (start, end - start);
        }
    }
}
