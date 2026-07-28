using System;

namespace DataStructures.Trees.Heap.Heap
{
    // A Binary Heap is a complete binary tree that satisfies the HEAP-ORDER property. It is the
    // classical implementation of a Priority Queue and the workhorse behind Heap Sort.
    //
    // -----------------------------------------------------------------------
    // Two Defining Properties
    // -----------------------------------------------------------------------
    //   1. SHAPE property      — the tree is a COMPLETE binary tree: every level is fully filled
    //                            except possibly the last, which is filled from the left. This
    //                            lets the entire structure be stored in a contiguous array with
    //                            no wasted space and no explicit child pointers.
    //   2. HEAP-ORDER property — depends on the flavor:
    //                             • MAX-HEAP: every parent ≥ its children  → root holds the max.
    //                             • MIN-HEAP: every parent ≤ its children  → root holds the min.
    //                            This implementation is a MAX-HEAP.
    //
    // Note that a heap is only PARTIALLY ordered — sibling nodes have no defined relationship.
    // This is much cheaper to maintain than a full sort while still giving O(1) access to the
    // extreme (max or min) element.
    //
    // -----------------------------------------------------------------------
    // Array Representation
    // -----------------------------------------------------------------------
    // This implementation uses a 1-INDEXED array with a sentinel at index 0. That gives clean
    // index arithmetic without any off-by-one adjustments:
    //
    //     parent(i)     = i / 2
    //     leftChild(i)  = 2 * i
    //     rightChild(i) = 2 * i + 1
    //
    // (A 0-indexed variant uses (i-1)/2, 2i+1, 2i+2 instead.)
    //
    // -----------------------------------------------------------------------
    // Core Operations
    // -----------------------------------------------------------------------
    //   • INSERT      — append the new value at the end of the array, then "sift up" (swap with
    //                   the parent while it violates heap-order).                    → O(log n)
    //   • EXTRACT-MAX — take the root, move the last element to the root, then "sift down"
    //                   (swap with the larger child while it violates heap-order).   → O(log n)
    //   • PEEK        — read index 1.                                                → O(1)
    //
    // -----------------------------------------------------------------------
    // Common Uses
    // -----------------------------------------------------------------------
    //   • Priority Queues (task schedulers, Dijkstra's / Prim's algorithms, event simulators).
    //   • Heap Sort — repeatedly extract the max into the end of the array to produce a sorted
    //     sequence in O(n log n) time with O(1) extra space.
    //   • Streaming top-K selection — keep a heap of size K.
    // =============================================================================================
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
