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

            // =========================
            // Build Prefix Sum
            // =========================
            Console.WriteLine("\n===== Building Prefix Sum Array =====");
            var prefix = PrefixSumOperations.BuildPrefixSum(arr);

            Console.WriteLine("\nPrefix Sum Array:");
            Print(prefix);

            // =========================
            // Range Sum Query
            // =========================
            Console.WriteLine("\n===== Range Sum Query =====");
            int left = 1, right = 3;

            Console.WriteLine($"\nRange Sum [{left}, {right}]:");
            var sum = PrefixSumOperations.RangeSum(prefix, left, right);
            Console.WriteLine($"Result: {sum}");

            // =========================
            // Equilibrium Index
            // =========================
            Console.WriteLine("\n===== Finding Equilibrium Index =====");
            var eqArr = new[] { 1, 3, 5, 2, 2 };

            Console.WriteLine("\nArray for Equilibrium Index:");
            Print(eqArr);

            var eqIndex = PrefixSumOperations.FindEquilibriumIndex(eqArr);

            Console.WriteLine(eqIndex == -1
                ? "No Equilibrium Index found."
                : $"Equilibrium Index: {eqIndex}");
        }

        private static void Print(int[] arr)
        {
            foreach (var num in arr)
                Console.Write(num + " ");
            Console.WriteLine();
        }
    }
}