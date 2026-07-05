using System;
using System.Collections.Generic;

namespace DataStructures.Core.Array
{
    /// <summary>
    /// Provides common interview problems solved using the Sliding Window technique.
    ///
    /// Sliding Window is an optimization technique used to process contiguous sequences (subarrays/substrings) efficiently.
    ///
    /// Instead of recalculating results for every possible window, the window is expanded and/or shrunk while maintaining the required state.
    ///
    /// Sliding Window is broadly classified into:
    /// 1. Fixed-size Window
    /// 2. Variable-size Window
    ///
    /// Benefits:
    /// • Reduces many O(n²) brute-force problems to O(n)
    /// • Minimizes repeated computation
    /// • Widely used in string and array interview questions
    /// </summary>
    internal static class SlidingWindowOperations
    {
        /// <summary>
        /// Finds the maximum sum of any contiguous subarray of size k.
        ///
        /// Example:
        /// Input:
        /// Array = [2,1,5,1,3,2], k = 3
        ///
        /// Windows:
        /// [2,1,5] = 8
        /// [1,5,1] = 7
        /// [5,1,3] = 9
        /// [1,3,2] = 6
        ///
        /// Output: 9
        ///
        /// Approach:
        /// Instead of recomputing every window, subtract the outgoing element and add the incoming element.
        /// </summary>
        ///
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int MaximumSumSubarrayOfSizeK(int[] arr, int k)
        {
            if (arr == null || arr.Length < k || k <= 0)
                throw new ArgumentException("Invalid input.");

            int windowSum = 0;

            for (int i = 0; i < k; i++)
                windowSum += arr[i];

            int maxSum = windowSum;

            for (int i = k; i < arr.Length; i++)
            {
                windowSum += arr[i];
                windowSum -= arr[i - k];

                maxSum = Math.Max(maxSum, windowSum);
            }

            return maxSum;
        }

        /// <summary>
        /// Finds the length of the smallest contiguous subarray whose sum is greater than or equal to the given target.
        ///
        /// Example:
        /// Array = [2,3,1,2,4,3]
        /// Target = 7
        ///
        /// Output: 2
        ///
        /// Explanation:
        /// Subarray [4,3] has sum = 7 and length = 2. No valid subarray has smaller length.
        /// </summary>
        ///
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int SmallestSubarrayWithGivenSum(int[] arr, int target)
        {
            int left = 0;
            int windowSum = 0;
            int minLength = int.MaxValue;

            for (int right = 0; right < arr.Length; right++)
            {
                windowSum += arr[right];

                // If the current window sum meets or exceeds the target, try to shrink the window from the left
                // because we want the smallest length subarray
                while (windowSum >= target) 
                {
                    minLength = Math.Min(minLength, right - left + 1); // Update the minimum length
                    windowSum -= arr[left++]; // Shrink the window
                }
            }

            return minLength == int.MaxValue ? 0 : minLength;
        }

        /// <summary>
        /// Finds the length of the longest substring that contains no repeating characters.
        ///
        /// Example:
        /// Input: "abcabcbb"
        ///
        /// Output: 3
        ///
        /// Explanation:
        /// "abc" is the longest substring without duplicate characters.
        /// </summary>
        ///
        /// Time Complexity: O(n)
        /// Space Complexity: O(min(n, character set))
        public static int LongestSubstringWithoutRepeatingCharacters(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            var window = new HashSet<char>(); // To track unique characters in the current window

            int left = 0;
            int maxLength = 0;

            for (int right = 0; right < str.Length; right++)
            {
                while (window.Contains(str[right]))
                {
                    window.Remove(str[left]); // Remove characters from the left until the duplicate is removed
                    left++;
                }

                window.Add(str[right]); // Add the current character to the window

                maxLength = Math.Max(maxLength, right - left + 1); // Update the maximum length found so far
            }

            return maxLength;
        }

        /// <summary>
        /// Finds the maximum number of consecutive ones after flipping at most k zeros.
        ///
        /// Example:
        /// Array: [1,1,0,0,1,1,1,0,1], k = 2
        ///
        /// Output: 7
        ///
        /// Explanation: Flip the two zeros inside the optimal window.
        /// </summary>
        ///
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int MaxConsecutiveOnesAfterFlippingKZeros(int[] nums, int k)
        {
            int left = 0;
            int zeros = 0;
            int maxLength = 0;

            for (int right = 0; right < nums.Length; right++)
            {
                if (nums[right] == 0)
                    zeros++;

                while (zeros > k) // If the number of zeros exceeds k, shrink the window from the left
                {
                    if (nums[left] == 0)
                        zeros--; // Decrease the count of zeros as we move the left pointer

                    left++; // Move the left pointer to shrink the window
                }

                maxLength = Math.Max(maxLength, right - left + 1); // Update the maximum length of consecutive ones found so far
            }

            return maxLength;
        }
    }
}