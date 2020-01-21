using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Yung.AST
{
    public class List : IValue, IEnumerable<IValue>
    {
        /// <summary>
        ///     Construct an empty list.
        /// </summary>
        public List()
        {
        }

        /// <summary>
        ///     Construct a list from the given collection.
        /// </summary>
        /// <param name="collection"></param>
        public List(IEnumerable<IValue> collection)
        {
            foreach (var value in collection.Reverse()) Add(value);
        }

        private List(Node node, int count)
        {
            Head = node;
            Count = count;
        }

        /// <summary>
        ///     Get the first element in the list.
        /// </summary>
        [AllowNull]
        public Node Head { get; private set; }

        /// <summary>
        ///     Get all elements from the list, except the first.
        /// </summary>
        public List Tail => new List(Head?.Next, Count - 1);

        /// <summary>
        ///     Get the number of elements in the list.
        /// </summary>
        public int Count { get; private set; }

        public IEnumerator<IValue> GetEnumerator()
        {
            return new ListEnumerator(Head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add a new element to the beginning of the list.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(IValue value)
        {
            var node = new Node(value, Head);
            Head = node;
            Count += 1;
        }

        public class Node
        {
            public readonly Node Next;
            public readonly IValue Value;

            internal Node(IValue value, Node next)
            {
                Value = value;
                Next = next;
            }
        }
    }
}
