using System;

namespace DataStructures.Trees.Heap.BuildHeap
{
    // "Build heap" is the problem of converting an arbitrary array of n values into a valid
    // Binary Heap (see the Heap project for the full definition). There are two standard ways to
    // do it, and the difference in running time is a classic result in algorithm analysis.
    //
    // -----------------------------------------------------------------------
    // Strategy 1: Top-Down (Repeated Insertion)
    // -----------------------------------------------------------------------
    // Start with an empty heap. Insert each element one at a time using the normal heap Insert
    // operation, which "sifts up" the new value to its correct position.
    //
    //     for i from 1 to n:  insert(a[i])
    //
    // Cost: each insert is O(log n) in the worst case, giving O(n log n) overall.
    //
    // -----------------------------------------------------------------------
    // Strategy 2: Bottom-Up (Floyd's Heapify)
    // -----------------------------------------------------------------------
    // Treat the input array as if it were already a complete binary tree (which it is, by shape).
    // Starting from the LAST INTERNAL node (index n/2) and walking backward toward the root,
    // "sift down" each node so that its subtree becomes a valid heap.
    //
    //     for i from n/2 down to 1:  siftDown(i)
    //
    // Cost: although each individual siftDown is O(log n), most of the work happens near the
    // leaves where subtrees are shallow. A tight analysis shows the total cost is O(n) — LINEAR
    // in the number of elements.
    //
    // -----------------------------------------------------------------------
    // When to Use Which
    // -----------------------------------------------------------------------
    //   • If elements arrive one at a time (streaming), you have no choice — use INSERT.
    //   • If all n elements are known up front (e.g., inside heap sort), use Floyd's heapify to
    //     get the O(n) win.
    // =============================================================================================
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
