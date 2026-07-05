using System;

namespace DataStructures.Core.Array
{
    /// <summary>
    /// Implements common problems using the Two Pointer technique.
    /// 
    /// Two Pointer Technique:
    /// - Uses two indices to traverse an array from different directions
    /// - Typically used for:
    ///     - Sorted arrays
    ///     - Pair problems
    ///     - In-place operations
    /// 
    /// Benefits:
    /// - Reduces time complexity from O(n²) → O(n)
    /// - Avoids extra space
    /// </summary>
    internal static class TwoPointerOperations
    {
        /// <summary>
        /// Finds two numbers in a sorted array that sum to target.
        /// 
        /// Example:
        /// Input: arr = [1,2,3,4,6], target = 6
        /// Output: (2,4)
        /// 
        /// Approach:
        /// - Start one pointer at beginning, one at end
        /// - Move pointers based on comparison with target
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static (int, int) TwoSumSorted(int[] arr, int target)
        {
            var left = 0;
            var right = arr.Length - 1;

            while (left < right)
            {
                var sum = arr[left] + arr[right];
                if (sum == target)
                    return (arr[left], arr[right]);

                if (sum < target)
                    left++;
                else
                    right--;
            }

            throw new InvalidOperationException("No pair found.");
        }

        /// <summary>
        /// Reverses the array in-place using two pointers.
        /// 
        /// Example:
        /// Input:  [1,2,3]
        /// Output: [3,2,1]
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static void Reverse(int[] arr)
        {
            var left = 0;
            var right = arr.Length - 1;

            while (left < right)
            {
                (arr[left], arr[right]) = (arr[right], arr[left]); // Swap using tuple deconstruction
                left++;
                right--;
            }
        }

        /// <summary>
        /// Checks if the given string is a palindrome.
        /// 
        /// Example:
        /// Input: "madam"
        /// Output: true
        /// 
        /// Approach:
        /// - Compare characters from both ends
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static bool IsPalindrome(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;

            var left = 0;
            var right = str.Length - 1;

            while (left < right)
            {
                if (str[left] != str[right])
                    return false;

                left++;
                right--;
            }

            return true;
        }

        /// <summary>
        /// Removes duplicates from a sorted array in-place.
        /// 
        /// Example:
        /// Input:  [1,1,2,2,3]
        /// Output: [1,2,3,_,_], size=3
        /// 
        /// Returns new logical size.
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int RemoveDuplicates(int[] arr)
        {
            if (arr.Length == 0)
                return 0;

            var left = 0;

            for (var right = 1; right < arr.Length; right++)
            {
                if (arr[right] != arr[left])
                {
                    left++;
                    arr[left] = arr[right];
                }
            }

            return left + 1;
        }

        /// <summary>
        /// Solves the "Container With Most Water" problem.
        /// 
        /// Given heights, find two lines that form the maximum area.
        /// 
        /// Example:
        /// Input:  [1,8,6,2,5,4,8,3,7]
        /// Output: 49
        /// Explanation: The lines at index 1 (height 8) and index 8 (height 7) form the container with the most water. 
        /// So, the area is min(8,7) * (8-1) = 7 * 7 = 49.
        /// 
        /// Approach:
        /// - Start from both ends
        /// - Move the smaller height pointer inward
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int MaxArea(int[] height)
        {
            var left = 0;
            var right = height.Length - 1;
            var maxArea = 0;

            while (left < right)
            {
                var width = right - left;
                var h = Math.Min(height[left], height[right]); // Height is determined by the shorter line
                var area = width * h; // Area = width * height

                maxArea = Math.Max(maxArea, area);

                if (height[left] < height[right])
                    left++;
                else
                    right--;
            }

            return maxArea;
        }
    }
}