using System;

namespace DataStructures.Core.Array
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("========== Sliding Window Techniques ==========\n");

            // =====================================================
            // MaximumSumSubarrayOfSizeK()
            // =====================================================
            Console.WriteLine("Method: MaximumSumSubarrayOfSizeK()");
            Console.WriteLine("Category: Fixed Sliding Window");
            Console.WriteLine("Description: Finds the maximum sum of any contiguous subarray of size K.\n");

            var fixedWindowArray = new[] { 2, 1, 5, 1, 3, 2 };
            var windowSize = 3;

            Console.WriteLine("Input Array:");
            PrintArray(fixedWindowArray);

            Console.WriteLine($"\nWindow Size: {windowSize}");

            var maxSum = SlidingWindowOperations.MaximumSumSubarrayOfSizeK(fixedWindowArray, windowSize);

            Console.WriteLine($"\nResult: Maximum Window Sum = {maxSum}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // =====================================================
            // SmallestSubarrayWithGivenSum()
            // =====================================================
            Console.WriteLine("Method: SmallestSubarrayWithGivenSum()");
            Console.WriteLine("Category: Variable Sliding Window");
            Console.WriteLine("Description: Finds the length of the smallest contiguous subarray whose sum is greater than or equal to the target.\n");

            var variableWindowArray = new[] { 2, 3, 1, 2, 4, 3 };
            var target = 7;

            Console.WriteLine("Input Array:");
            PrintArray(variableWindowArray);

            Console.WriteLine($"\nTarget Sum: {target}");

            var minLength = SlidingWindowOperations.SmallestSubarrayWithGivenSum(variableWindowArray, target);

            Console.WriteLine($"\nResult: Smallest Subarray Length = {minLength}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // =====================================================
            // LongestSubstringWithoutRepeatingCharacters()
            // =====================================================
            Console.WriteLine("Method: LongestSubstringWithoutRepeatingCharacters()");
            Console.WriteLine("Category: Variable Sliding Window");
            Console.WriteLine("Description: Finds the length of the longest substring containing unique characters.\n");

            var input = "abcabcbb";

            Console.WriteLine($"Input String: \"{input}\"");

            var longestSubstringLength = SlidingWindowOperations.LongestSubstringWithoutRepeatingCharacters(input);

            Console.WriteLine($"\nResult: Longest Unique Substring Length = {longestSubstringLength}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // =====================================================
            // MaxConsecutiveOnesAfterFlippingKZeros()
            // =====================================================
            Console.WriteLine("Method: MaxConsecutiveOnesAfterFlippingKZeros()");
            Console.WriteLine("Category: Variable Sliding Window");
            Console.WriteLine("Description: Finds the maximum number of consecutive 1s after flipping at most K zeros.\n");

            var binaryArray = new[] { 1, 1, 0, 0, 1, 1, 1, 0, 1 };
            var allowedFlips = 2;

            Console.WriteLine("Input Array:");
            PrintArray(binaryArray);

            Console.WriteLine($"\nAllowed Zero Flips: {allowedFlips}");

            var maxConsecutiveOnes = SlidingWindowOperations.MaxConsecutiveOnesAfterFlippingKZeros(binaryArray, allowedFlips);

            Console.WriteLine($"\nResult: Maximum Consecutive Ones = {maxConsecutiveOnes}");
        }

        /// <summary>
        /// Displays the elements of an integer array.
        /// </summary>
        /// <param name="array">Array to be displayed.</param>
        private static void PrintArray(int[] array)
        {
            foreach (int value in array)
            {
                Console.Write($"{value} ");
            }

            Console.WriteLine();
        }
    }
}