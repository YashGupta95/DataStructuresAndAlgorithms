using System;

namespace DataStructures.Core.Array
{
    internal class Program
    {
        static void Main()
        {
            // =========================
            // Two Sum (Sorted)
            // =========================
            var arr = new[] { 1, 2, 3, 4, 6 };
            var target = 6;

            Console.Write("Array (Sorted): ");
            Print(arr);

            Console.WriteLine("\n===== Two Sum (Sorted) =====");
            var pair = TwoPointerOperations.TwoSumSorted(arr, target);
            Console.WriteLine($"Pair with sum {target}: ({pair.Item1}, {pair.Item2})");

            // =========================
            // Reverse
            // =========================
            Console.WriteLine("\n===== Array Reversal =====");
            Console.WriteLine("\nBefore Reverse:");
            Print(arr);

            TwoPointerOperations.Reverse(arr);

            Console.WriteLine("After Reverse:");
            Print(arr);

            // =========================
            // Palindrome
            // =========================
            Console.WriteLine("\n===== Palindrome Check =====");
            var str1 = "madam";
            var str2 = "hello";
            Console.WriteLine($"\nIs \"{str1}\" a palindrome? \n{TwoPointerOperations.IsPalindrome(str1)}");
            Console.WriteLine($"\nIs \"{str2}\" a palindrome? \n{TwoPointerOperations.IsPalindrome(str2)}");

            // =========================
            // Remove Duplicates
            // =========================
            Console.WriteLine("\n===== Duplicates Removal =====");
            var dupArr = new[] { 1, 1, 2, 2, 3 };

            Console.WriteLine("\nOriginal Array (with duplicates):");
            Print(dupArr);

            var newSize = TwoPointerOperations.RemoveDuplicates(dupArr);

            Console.WriteLine("After Removing Duplicates:");
            for (var i = 0; i < newSize; i++)
                Console.Write(dupArr[i] + " ");
            Console.WriteLine();

            // =========================
            // Container With Most Water
            // =========================
            Console.WriteLine("\n===== 'Container With Most Water' Problem =====");
            var height = new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 };

            Console.WriteLine("\nHeight Array:");
            Print(height);

            var maxArea = TwoPointerOperations.MaxArea(height);
            Console.WriteLine($"Max Water Container Area: {maxArea}");
        }

        private static void Print(int[] arr)
        {
            foreach (var num in arr)
                Console.Write(num + " ");
            Console.WriteLine();
        }
    }
}