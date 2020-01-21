using System.Collections;
using System.Collections.Generic;

namespace Yung.AST
{
    public class ListEnumerator : IEnumerator<IValue>
    {
        private readonly List.Node _head;
        private List.Node _current;

        public ListEnumerator(List.Node head)
        {
            _head = head;
            _current = null;
        }

        public void Dispose()
        {
        }

        public IValue Current => _current.Value;

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_head == null) return false;
            _current = _current == null ? _head : _current.Next;
            return _current != null;
        }

        public void Reset()
        {
            _current = null;
        }
    }
}
