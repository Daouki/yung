using System;
using System.Collections;
using System.Collections.Generic;

namespace Yung.AST
{
    public class VectorEnumerator : IEnumerator<IValue>
    {
        private readonly int _count;
        private readonly IValue[] _elements;
        private int _index = -1;

        internal VectorEnumerator(IValue[] vector, int count)
        {
            _elements = vector;
            _count = count;
        }

        public void Dispose()
        {
        }

        public IValue Current
        {
            get
            {
                try
                {
                    return _elements[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool MoveNext()
        {
            _index += 1;
            return _index < _count;
        }

        public void Reset()
        {
            _index = -1;
        }

        object? IEnumerator.Current => Current;
    }
}
