using System;
using System.Collections.Generic;

namespace DataStructures.Core.Stack
{
    /// <summary>
    /// Array-based implementation of a generic stack (LIFO - Last In First Out).
    /// 
    /// Features:
    /// - Dynamic resizing
    /// - Generic type support
    /// 
    /// Time Complexity:
    /// Push   - O(1) amortized
    /// Pop    - O(1)
    /// Peek   - O(1)
    /// 
    /// Space Complexity:
    /// O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements in the stack</typeparam>
    public class StackArray<T>
    {
        private T[] _items;
        private int _top;

        private const int DefaultCapacity = 4;

        /// <summary>
        /// Initializes a new instance of the stack with default capacity.
        /// </summary>
        public StackArray()
        {
            _items = new T[DefaultCapacity];
            _top = -1;
        }

        /// <summary>
        /// Gets the number of elements in the stack.
        /// </summary>
        public int Count => _top + 1;

        /// <summary>
        /// Returns true if the stack is empty.
        /// </summary>
        public bool IsEmpty() => _top == -1;

        /// <summary>
        /// Pushes an element onto the stack.
        /// </summary>
        /// <param name="item">Item to be pushed</param>
        public void Push(T item)
        {
            if (_top == _items.Length - 1)
            {
                Resize();
            }

            _items[++_top] = item;
        }

        /// <summary>
        /// Removes and returns the top element of the stack.
        /// </summary>
        /// <returns>Top element</returns>
        /// <exception cref="InvalidOperationException">Thrown when stack is empty</exception>
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty. Cannot perform Pop.");
            }

            T item = _items[_top];
            _items[_top] = default!; // Clear reference
            _top--;

            return item;
        }

        /// <summary>
        /// Returns the top element without removing it.
        /// </summary>
        /// <returns>Top element</returns>
        /// <exception cref="InvalidOperationException">Thrown when stack is empty</exception>
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty. Cannot perform Peek.");
            }

            return _items[_top];
        }

        /// <summary>
        /// Returns elements from top to bottom.
        /// </summary>
        public IEnumerable<T> GetElements()
        {
            for (int i = _top; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        /// <summary>
        /// Doubles the capacity of the internal array.
        /// </summary>
        private void Resize()
        {
            int newCapacity = _items.Length * 2;
            T[] newArray = new T[newCapacity];

            Array.Copy(_items, newArray, _items.Length);
            _items = newArray;
        }
    }
}