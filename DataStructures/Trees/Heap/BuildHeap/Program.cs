using System;

namespace DataStructures.Trees.Heap.BuildHeap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var originalValues = new[] { int.MaxValue, 1, 4, 5, 7, 9, 10 };
            var topDownValues = (int[])originalValues.Clone();
            var bottomUpValues = (int[])originalValues.Clone();

            Console.WriteLine("==============================================================");
            Console.WriteLine("        BUILD-HEAP DEMONSTRATION (TOP-DOWN vs BOTTOM-UP)");
            Console.WriteLine("==============================================================");
            Console.WriteLine("Original array (using index 1 as the root):");
            PrintArray(topDownValues, 1);

            BuildHeapOperations.HeapifyTopDown(topDownValues, topDownValues.Length - 1);
            Console.WriteLine("\nAfter Top-Down Heapify:");
            PrintArray(topDownValues, 1);

            BuildHeapOperations.HeapifyBottomUp(bottomUpValues, bottomUpValues.Length - 1);
            Console.WriteLine("\nAfter Bottom-Up Heapify:");
            PrintArray(bottomUpValues, 1);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void PrintArray(int[] arr, int startIndex)
        {
            for (var i = startIndex; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }

            Console.WriteLine();
        }
    }
}
