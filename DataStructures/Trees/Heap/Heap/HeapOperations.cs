using System;

namespace DataStructures.Trees.Heap.Heap
{
    /// <summary>
    /// Represents a max heap for learning purposes.
    /// </summary>
    /// <remarks>
    /// This implementation uses a 1-based array and a sentinel value at index 0.
    /// The same logic can be reversed for a min heap implementation.
    /// </remarks>
    internal class HeapOperations
    {
        private readonly int[] arr;
        private int size;

        public HeapOperations()
        {
            arr = new int[10];
            size = 0;
            arr[0] = int.MaxValue; // Sentinel value
        }

        public HeapOperations(int maxSize)
        {
            arr = new int[maxSize];
            size = 0;
            arr[0] = int.MaxValue;
        }

        /// <summary>
        /// Inserts a value into the heap.
        /// </summary>
        /// <remarks>
        /// The value is appended at the end of the heap and then bubbled upward until the max-heap property is restored.
        /// </remarks>
        /// <param name="value">The value to insert.</param>
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal void Insert(int value)
        {
            size++;
            arr[size] = value;
            RestoreUp(size);
        }

        private void RestoreUp(int index)
        {
            var value = arr[index];
            var parentIndex = index / 2;

            // If there's no sentinel value modify it to: while(parentIndex >= 1 && arr[parentIndex] < value)
            while (arr[parentIndex] < value)
            {
                arr[index] = arr[parentIndex];
                index = parentIndex;
                parentIndex = index / 2;
            }

            arr[index] = value;
        }

        /// <summary>
        /// Removes and returns the maximum value from the heap.
        /// </summary>
        /// <remarks>
        /// The root is replaced by the last element and then moved down to restore the heap property.
        /// </remarks>
        /// <returns>The maximum value stored in the heap.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal int DeleteRoot()
        {
            if (size == 0)
                throw new InvalidOperationException("Heap is Empty");

            var maxValue = arr[1];
            arr[1] = arr[size];
            size--;
            RestoreDown(1);

            return maxValue;
        }

        private void RestoreDown(int index)
        {
            var value = arr[index];
            var leftChildIndex = 2 * index;
            var rightChildIndex = leftChildIndex + 1;

            while (rightChildIndex <= size)
            {
                if (value >= arr[leftChildIndex] && value >= arr[rightChildIndex])
                {
                    arr[index] = value;
                    return;
                }
                else if (arr[leftChildIndex] > arr[rightChildIndex])
                {
                    arr[index] = arr[leftChildIndex];
                    index = leftChildIndex;
                }
                else
                {
                    arr[index] = arr[rightChildIndex];
                    index = rightChildIndex;
                }

                leftChildIndex = 2 * index;
                rightChildIndex = leftChildIndex + 1;
            }

            // If the number of nodes is even, then there will always be a node that does not have a right children
            if (leftChildIndex == size && value < arr[leftChildIndex])
            {
                arr[index] = arr[leftChildIndex];
                index = leftChildIndex;
            }

            arr[index] = value;
        }

        /// <summary>
        /// Displays the current heap contents.
        /// </summary>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal void Display()
        {
            if (size == 0)
            {
                Console.WriteLine("Heap is empty");
                return;
            }

            Console.WriteLine($"Heap size: {size}");
            for (var i = 1; i <= size; i++)
                Console.Write($"{arr[i]} ");

            Console.WriteLine();
        }
    }
}
