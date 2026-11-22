// Collection interfaces for freestanding NativeAOT compatibility.
// Implements System.Collections and System.Collections.Generic interfaces
// required by ValueArray, StringMap, and AST node types.

namespace System
{
    public interface IEquatable<T>
    {
        bool Equals(T other);
    }

    public interface IComparable<T>
    {
        int CompareTo(T other);
    }

    public interface IDisposable
    {
        void Dispose();
    }
}

namespace System.Collections
{
    public interface IEnumerable
    {
        IEnumerator GetEnumerator();
    }

    public interface IEnumerator
    {
        object Current { get; }
        bool MoveNext();
        void Reset();
    }

    public interface IEqualityComparer
    {
        bool Equals(object x, object y);
        int GetHashCode(object obj);
    }
}

namespace System.Collections.Generic
{
    public interface IEnumerable<out T> : IEnumerable
    {
        new IEnumerator<T> GetEnumerator();
    }

    public interface IEnumerator<out T> : IDisposable, IEnumerator
    {
        new T Current { get; }
    }

    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
    {
        int Count { get; }
    }

    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
    {
        T this[int index] { get; }
    }

    public interface IEqualityComparer<T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }

    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        public static EqualityComparer<T> Default { get; } = new EqualityComparer<T>();

        public virtual bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public virtual int GetHashCode(T obj)
        {
            if (obj == null) return 0;
            int h = obj.GetHashCode();
            return h;
        }
    }

    public struct KeyValuePair<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public KeyValuePair(TKey key, TValue value) { Key = key; Value = value; }
    }

    public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        TValue this[TKey key] { get; }
        bool TryGetValue(TKey key, out TValue value);
        int Count { get; }
    }
}