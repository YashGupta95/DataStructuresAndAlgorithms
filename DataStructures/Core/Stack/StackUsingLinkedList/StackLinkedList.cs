using System;
using System.Collections.Generic;

namespace DataStructures.Core.Stack
{
    /// <summary>
    /// Linked List-based implementation of a generic stack (LIFO - Last In First Out).
    /// 
    /// Features:
    /// - Dynamic size (no resizing needed)
    /// - Efficient insert/remove at head
    /// 
    /// Time Complexity:
    /// Push   - O(1)
    /// Pop    - O(1)
    /// Peek   - O(1)
    /// 
    /// Space Complexity:
    /// O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements in the stack</typeparam>
    public class StackLinkedList<T>
    {
        private Node _head;
        private int _count;

        /// <summary>
        /// Internal node representation
        /// </summary>
        private class Node
        {
            public T Data;
            public Node Next;

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        /// <summary>
        /// Gets the number of elements in the stack.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Returns true if the stack is empty.
        /// </summary>
        public bool IsEmpty() => _count == 0;

        /// <summary>
        /// Pushes an element onto the stack.
        /// </summary>
        public void Push(T item)
        {
            var newNode = new Node(item)
            {
                Next = _head
            };

            _head = newNode;
            _count++;
        }

        /// <summary>
        /// Removes and returns the top element of the stack.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when stack is empty</exception>
        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty. Cannot perform Pop.");

            T value = _head.Data;
            _head = _head.Next;
            _count--;

            return value;
        }

        /// <summary>
        /// Returns the top element without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when stack is empty</exception>
        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty. Cannot perform Peek.");

            return _head.Data;
        }

        /// <summary>
        /// Returns elements from top to bottom.
        /// </summary>
        public IEnumerable<T> GetElements()
        {
            var current = _head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}