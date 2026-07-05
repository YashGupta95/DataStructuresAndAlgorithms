using System;

namespace DataStructures.Core.Array
{
    /// <summary>
    /// Provides operations based on Prefix Sum technique.
    /// 
    /// Prefix Sum is a precomputation technique used to answer range queries efficiently.
    /// 
    /// Core Idea:
    /// prefix[i] = sum of elements from index 0 to i
    /// 
    /// Benefits:
    /// - Converts repeated O(n) range queries into O(1)
    /// - Widely used in range queries, subarray problems, and optimization scenarios
    /// </summary>
    internal static class PrefixSumOperations
    {
        /// <summary>
        /// Builds the prefix sum array for a given input array.
        /// 
        /// Example:
        /// Input:  [1, 2, 3, 4]
        /// Output: [1, 3, 6, 10]
        /// 
        /// prefix[i] = arr[0] + arr[1] + ... + arr[i]
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <returns>Prefix sum array</returns>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(n)
        public static int[] BuildPrefixSum(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                throw new ArgumentException("Array cannot be null or empty.");

            var prefix = new int[arr.Length];
            prefix[0] = arr[0];

            for (var i = 1; i < arr.Length; i++)
            {
                prefix[i] = prefix[i - 1] + arr[i];
            }

            return prefix;
        }

        /// <summary>
        /// Computes the sum of elements in the range [left, right] (inclusive) using the prefix sum array.
        /// 
        /// Formula:
        /// sum(left, right) = prefix[right] - prefix[left - 1]
        /// 
        /// Example:
        /// Original Array: [1, 2, 3, 4, 5]
        /// Prefix Array:   [1, 3, 6, 10, 15]
        /// 
        /// Query: left = 1, right = 3
        /// Calculation: prefix[3] - prefix[0] = 10 - 1 = 9
        /// Result: 2 + 3 + 4 = 9
        /// </summary>
        /// <param name="prefix">Prefix sum array</param>
        /// <param name="left">Start index</param>
        /// <param name="right">End index</param>
        /// <returns>Sum of range</returns>
        /// 
        /// Time Complexity: O(1)
        /// Space Complexity: O(1)
        public static int RangeSum(int[] prefix, int left, int right)
        {
            if (prefix == null || prefix.Length == 0)
                throw new ArgumentException("Prefix array cannot be null or empty.");

            if (left < 0 || right >= prefix.Length || left > right)
                throw new ArgumentOutOfRangeException("Invalid range.");

            if (left == 0)
                return prefix[right];

            return prefix[right] - prefix[left - 1];
        }

        /// <summary>
        /// Finds an equilibrium index in the array.
        /// 
        /// Equilibrium Index:
        /// Index where sum of elements to the left == sum of elements to the right
        /// 
        /// Example:
        /// Input:  [1, 3, 5, 2, 2]
        /// Output: 2 (since 1+3 = 2+2)
        /// 
        /// Returns -1 if no equilibrium index exists.
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <returns>Equilibrium index or -1</returns>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int FindEquilibriumIndex(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                return -1;

            var totalSum = 0;

            foreach (var num in arr)
                totalSum += num;

            var leftSum = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                var rightSum = totalSum - leftSum - arr[i];

                if (leftSum == rightSum)
                    return i;

                leftSum += arr[i];
            }

            return -1;
        }
    }
}