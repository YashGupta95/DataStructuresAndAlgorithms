using System;

namespace DataStructures.Core.Array
{
    internal class Program
    {
        static void Main()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            Console.WriteLine("Original Array:");
            Print(arr);

            Console.WriteLine("\n===== ARRAY ROTATION =====");
            ArrayOperations.RotateLeft(arr, 2);
            Console.WriteLine("\nAfter Left Rotate (k=2):");
            Print(arr);

            ArrayOperations.RotateRight(arr, 2);
            Console.WriteLine("\nAfter Right Rotate (k=2):");
            Print(arr);

            Console.WriteLine("\n===== MIN/MAX ELEMENTS =====");
            Console.WriteLine($"\nMax: {ArrayOperations.FindMax(arr)}");
            Console.WriteLine($"Min: {ArrayOperations.FindMin(arr)}");

            // =========================
            // Remove Duplicates Section
            // =========================
            Console.WriteLine("\n===== REMOVE DUPLICATES =====");
            var arrWithDuplicates = new[] { 1, 1, 2, 2, 3, 3 };

            Console.WriteLine("\nOriginal Sorted Array (with duplicates):");
            Print(arrWithDuplicates);

            var newSize = ArrayOperations.RemoveDuplicatesSorted(arrWithDuplicates);

            Console.WriteLine("After Removing Duplicates:");
            for (var i = 0; i < newSize; i++)
                Console.Write(arrWithDuplicates[i] + " ");
            Console.WriteLine();

            // =========================
            // Merge Arrays Section
            // =========================
            Console.WriteLine("\n===== MERGE ARRAYS =====");
            var arr1 = new[] { 1, 2, 3 };
            var arr2 = new[] { 4, 5, 6 };

            Console.WriteLine("\nFirst Array:");
            Print(arr1);

            Console.WriteLine("Second Array:");
            Print(arr2);

            var merged = ArrayOperations.Merge(arr1, arr2);

            Console.WriteLine("Merged Array:");
            Print(merged);
        }

        private static void Print(int[] arr)
        {
            foreach (var num in arr)
                Console.Write(num + " ");
            Console.WriteLine();
        }
    }
}