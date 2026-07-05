using System;

namespace DataStructures.Core.Array
{
    /// <summary>
    /// Represents a fixed-size array with manual control over size and capacity.
    /// 
    /// This class simulates real-world array behavior where:
    /// - Memory is pre-allocated (fixed capacity)
    /// - Insertions and deletions require shifting elements
    /// 
    /// Key Concepts Demonstrated:
    /// - Difference between Size and Capacity
    /// - In-place updates
    /// - Element shifting (core array limitation)
    /// 
    /// Time Complexity Summary:
    /// - Insert   : O(n) (due to shifting)
    /// - Delete   : O(n) (due to shifting)
    /// - Update   : O(1)
    /// - Get      : O(1)
    /// - Search   : O(n)
    /// - Reverse  : O(n)
    /// 
    /// Space Complexity:
    /// - O(n) (fixed array allocation)
    /// </summary>
    internal class ArrayManager
    {
        private readonly int[] _array;
        private int _size;

        /// <summary>
        /// Initializes a new instance of ArrayManager with given capacity.
        /// </summary>
        /// <param name="capacity">Maximum number of elements the array can hold.</param>
        public ArrayManager(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.");

            _array = new int[capacity];
            _size = 0;
        }

        /// <summary>
        /// Gets the current number of elements in the array.
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Gets the total capacity of the array.
        /// </summary>
        public int Capacity => _array.Length;

        /// <summary>
        /// Checks whether the array is full.
        /// </summary>
        /// <returns>True if array is full, otherwise false.</returns>
        public bool IsFull() => _size == _array.Length;

        /// <summary>
        /// Checks whether the array is empty.
        /// </summary>
        /// <returns>True if array is empty, otherwise false.</returns>
        public bool IsEmpty() => _size == 0;

        /// <summary>
        /// Inserts an element at the end of the array.
        /// 
        /// This is equivalent to appending an element.
        /// No shifting is required.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        /// <exception cref="InvalidOperationException">Thrown when array is full.</exception>
        /// 
        /// Time Complexity: O(1)
        /// Space Complexity: O(1)
        public void Insert(int value)
        {
            if (IsFull())
                throw new InvalidOperationException("Array is full.");

            _array[_size] = value;
            _size++;
        }

        /// <summary>
        /// Deletes an element at the specified index.
        /// 
        /// Shifts all elements after index to left by one position.
        /// </summary>
        /// <param name="index">Index of element to delete.</param>
        /// <returns>The deleted element.</returns>
        /// <exception cref="InvalidOperationException">Thrown when array is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Invalid index.</exception>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public int Delete(int index)
        {
            if (IsEmpty())
                throw new InvalidOperationException("Array is empty.");

            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index));

            var deletedElement = _array[index];

            // Shift elements to the left
            for (var i = index; i < _size - 1; i++)
            {
                _array[i] = _array[i + 1];
            }

            _size--;
            return deletedElement;
        }

        /// <summary>
        /// Updates the value at the specified index.
        /// </summary>
        /// <param name="index">Index to update.</param>
        /// <param name="value">New value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Invalid index.</exception>
        /// 
        /// Time Complexity: O(1)
        /// Space Complexity: O(1)
        public void Update(int index, int value)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index));

            _array[index] = value;
        }

        /// <summary>
        /// Retrieves the element at the specified index.
        /// </summary>
        /// <param name="index">Index of element.</param>
        /// <returns>Element at given index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Invalid index.</exception>
        /// 
        /// Time Complexity: O(1)
        /// Space Complexity: O(1)
        public int Get(int index)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _array[index];
        }

        /// <summary>
        /// Performs a linear search to find the given value.
        /// </summary>
        /// <param name="value">Value to search.</param>
        /// <returns>Index of value if found, otherwise -1.</returns>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public int Search(int value)
        {
            for (var i = 0; i < _size; i++)
            {
                if (_array[i] == value)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Reverses the array in-place.
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public void Reverse()
        {
            var left = 0;
            var right = _size - 1;

            while (left < right)
            {
                var temp = _array[left];
                _array[left] = _array[right];
                _array[right] = temp;

                left++;
                right--;
            }
        }

        /// <summary>
        /// Returns a copy of the current elements in the array.
        /// 
        /// This avoids exposing internal array directly.
        /// </summary>
        /// <returns>Array containing current elements.</returns>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(n)
        public int[] GetElements()
        {
            var result = new int[_size];
            System.Array.Copy(_array, result, _size);
            return result;
        }
    }
}