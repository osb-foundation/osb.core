// List<T> for freestanding NativeAOT compatibility.
// Used by ExpressionEvaluator, Interpreter, Parser, NamespaceEvaluator, BigInteger, FreestandingUtf8Provider.

using System.Collections;

namespace System.Collections.Generic
{
    public class List<T> : IReadOnlyList<T>
    {
        private T[] _items;
        private int _size;

        public List()
        {
            _items = Array.Empty<T>();
            _size = 0;
        }

        public List(int capacity)
        {
            _items = new T[capacity];
            _size = 0;
        }

        public int Count => _size;

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public void Add(T item)
        {
            if (_size == _items.Length)
            {
                var newItems = new T[_items.Length == 0 ? 4 : _items.Length * 2];
                Array.Copy(_items, 0, newItems, 0, _size);
                _items = newItems;
            }
            _items[_size++] = item;
        }

        public void Reverse()
        {
            int i = 0, j = _size - 1;
            while (i < j)
            {
                T tmp = _items[i];
                _items[i] = _items[j];
                _items[j] = tmp;
                i++;
                j--;
            }
        }

        public T[] ToArray()
        {
            var result = new T[_size];
            Array.Copy(_items, 0, result, 0, _size);
            return result;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct Enumerator : IEnumerator<T>
        {
            private readonly List<T> _list;
            private int _index;
            private T _current;

            internal Enumerator(List<T> list) { _list = list; _index = -1; _current = default; }
            public bool MoveNext()
            {
                if (_index < _list._size - 1)
                {
                    _index++;
                    _current = _list._items[_index];
                    return true;
                }
                return false;
            }
            public T Current => _current;
            object IEnumerator.Current => _current;
            public void Reset() { _index = -1; _current = default; }
            public void Dispose() { }
        }
    }
}
