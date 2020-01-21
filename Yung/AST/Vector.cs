using System;
using System.Collections;
using System.Collections.Generic;

namespace Yung.AST
{
    public class Vector : IValue, IEnumerable<IValue>
    {
        /// <summary>
        ///     The starting capacity of a Vector.
        /// </summary>
        public const int DefaultCapacity = 4;

        private static readonly IValue[] _emptyArray = new IValue[0];

        /// <summary>
        ///     The underlying array, that is the base for a Vector class.
        ///     Length of that array is the capacity of a Vector.
        /// </summary>
        private IValue[] _elements;

        /// <summary>
        ///     Construct a Vector that is empty and with capacity if zero. When the first element
        ///     is added, the capacity is extended to DefaultCapacity, and then increased by the
        ///     multiples of two.
        /// </summary>
        public Vector()
        {
            _elements = _emptyArray;
        }

        /// <summary>
        ///     Construct a Vector with the given initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of a Vector.</param>
        public Vector(int capacity)
        {
            _elements = capacity == 0 ? _emptyArray : new IValue[capacity];
        }

        /// <summary>
        ///     Get or set the capacity of the Vector.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when this.Count &lt; value.</exception>
        public int Capacity
        {
            get => _elements.Length;
            set
            {
                if (value == Capacity) return;
                if (value < Count) throw new ArgumentOutOfRangeException();
                if (value == 0)
                {
                    _elements = _emptyArray;
                    return;
                }

                var newElements = new IValue[value];
                if (Count > 0) Array.Copy(_elements, 0, newElements, 0, Count);
                _elements = newElements;
            }
        }

        /// <summary>
        ///     Get the number of elements in the Vector.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Get or set element at the given index.
        /// </summary>
        /// <param name="index"></param>
        public IValue this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        /// <summary>
        ///     Get an enumerator for the vector.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IValue> GetEnumerator()
        {
            return new VectorEnumerator(_elements, Count);
        }

        /// <summary>
        ///     Get an enumerator for the vector.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add a new element to the end of the Vector.
        /// </summary>
        /// <param name="value"></param>
        public void Add(IValue value)
        {
            if (Count >= Capacity)
            {
                if (Capacity == 0) Capacity = DefaultCapacity;
                else Capacity *= 2;
            }

            _elements[Count] = value;
            Count += 1;
        }

        /// <summary>
        ///     Clear the contents of the Vector.
        /// </summary>
        public void Clear()
        {
            if (Count == 0) return;
            // Clear the elements, so the garbage collector can reclaim memory.
            Array.Clear(_elements, 0, Count);
            Count = 0;
        }
    }
}
