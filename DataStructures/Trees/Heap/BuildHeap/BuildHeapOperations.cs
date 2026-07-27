using System;

namespace DataStructures.Trees.Heap.BuildHeap
{
    /// <summary>
    /// Provides helper methods for building a max-heap from an array using different strategies.
    /// </summary>
    internal static class BuildHeapOperations
    {
        /// <summary>
        /// Builds a heap by inserting elements one by one and restoring the heap property upward.
        /// </summary>
        /// <param name="arr">The array to heapify. The implementation assumes a 1-based indexing scheme where index 1 is the root.</param>
        /// <param name="size">The size of the heap portion in the array.</param>
        public static void HeapifyTopDown(int[] arr, int size)
        {
            for (var i = 2; i <= size; i++)
            {
                RestoreUp(i, arr);
            }
        }

        /// <summary>
        /// Moves an element upward until the max-heap property is restored.
        /// </summary>
        /// <param name="index">The current index of the element being restored.</param>
        /// <param name="arr">The array containing the heap values.</param>
        private static void RestoreUp(int index, int[] arr)
        {
            var value = arr[index];
            var parentIndex = index / 2;

            while (arr[parentIndex] < value)
            {
                arr[index] = arr[parentIndex];
                index = parentIndex;
                parentIndex = index / 2;
            }

            arr[index] = value;
        }

        /// <summary>
        /// Builds a heap by starting at the last non-leaf node and restoring the heap property downward.
        /// </summary>
        /// <param name="arr">The array to heapify. The implementation assumes a 1-based indexing scheme where index 1 is the root.</param>
        /// <param name="size">The size of the heap portion in the array.</param>
        public static void HeapifyBottomUp(int[] arr, int size)
        {
            for (var i = size / 2; i >= 1; i--)
            {
                RestoreDown(i, arr, size);
            }
        }

        /// <summary>
        /// Moves an element downward until the max-heap property is restored.
        /// </summary>
        /// <param name="index">The current index of the element being restored.</param>
        /// <param name="arr">The array containing the heap values.</param>
        /// <param name="size">The size of the heap portion in the array.</param>
        private static void RestoreDown(int index, int[] arr, int size)
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

                if (arr[leftChildIndex] > arr[rightChildIndex])
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

            if (leftChildIndex == size && value < arr[leftChildIndex])
            {
                arr[index] = arr[leftChildIndex];
                index = leftChildIndex;
            }

            arr[index] = value;
        }
    }
}
