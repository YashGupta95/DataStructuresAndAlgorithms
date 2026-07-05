using System;

namespace DataStructures.Core.Array
{
    /// <summary>
    /// Provides common array operations frequently asked in interviews.
    /// 
    /// Covers:
    /// - Rotation (Left/Right)
    /// - Max/Min
    /// - Second Largest
    /// - Remove Duplicates (Sorted Array)
    /// - Merge Arrays
    /// 
    /// Note:
    /// These methods operate directly on input arrays unless specified otherwise.
    /// </summary>
    internal static class ArrayOperations
    {
        /// <summary>
        /// Rotates the array to the left by k positions.
        /// 
        /// Example:
        /// Input:  [1,2,3,4,5], k=2
        /// Output: [3,4,5,1,2]
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="k">Number of positions</param>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static void RotateLeft(int[] arr, int k)
        {
            if (arr == null || arr.Length == 0)
                return;

            var n = arr.Length;
            k = k % n;

            Reverse(arr, 0, k - 1); // Reverse the first k elements
            Reverse(arr, k, n - 1); // Reverse the remaining (n-k) elements
            Reverse(arr, 0, n - 1); // Reverse the entire array
        }

        /// <summary>
        /// Rotates the array to the right by k positions.
        /// 
        /// Example:
        /// Input:  [1,2,3,4,5], k=2
        /// Output: [4,5,1,2,3]
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static void RotateRight(int[] arr, int k)
        {
            if (arr == null || arr.Length == 0)
                return;

            var n = arr.Length;
            k = k % n;

            Reverse(arr, n - k, n - 1); // Reverse the last k elements
            Reverse(arr, 0, n - k - 1); // Reverse the first (n-k) elements
            Reverse(arr, 0, n - 1); // Reverse the entire array
        }

        /// <summary>
        /// Finds the maximum element in the array.
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int FindMax(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                throw new ArgumentException("Array cannot be empty.");

            var max = arr[0];

            foreach (var num in arr)
            {
                if (num > max)
                    max = num;
            }

            return max;
        }

        /// <summary>
        /// Finds the minimum element in the array.
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int FindMin(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                throw new ArgumentException("Array cannot be empty.");

            var min = arr[0];

            foreach (var num in arr)
            {
                if (num < min)
                    min = num;
            }

            return min;
        }

        /// <summary>
        /// Finds the second largest element in the array.
        /// 
        /// Throws exception if not enough distinct elements exist.
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int SecondLargest(int[] arr)
        {
            if (arr == null || arr.Length < 2)
                throw new ArgumentException("Array must have at least 2 elements.");

            var first = int.MinValue;
            var second = int.MinValue;

            foreach (var num in arr)
            {
                if (num > first)
                {
                    second = first;
                    first = num;
                }
                else if (num > second && num != first)
                {
                    second = num;
                }
            }

            if (second == int.MinValue)
                throw new InvalidOperationException("No second largest element found.");

            return second;
        }

        /// <summary>
        /// Removes duplicates from a sorted array in-place.
        /// 
        /// Returns new logical size of array.
        /// 
        /// Example:
        /// Input:  [1,1,2,2,3]
        /// Output: [1,2,3,_,_], size=3
        /// </summary>
        /// 
        /// Time Complexity: O(n)
        /// Space Complexity: O(1)
        public static int RemoveDuplicatesSorted(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                return 0;

            var j = 0;

            for (var i = 1; i < arr.Length; i++)
            {
                if (arr[i] != arr[j])
                {
                    j++;
                    arr[j] = arr[i];
                }
            }

            return j + 1;
        }

        /// <summary>
        /// Merges two arrays into a single new array.
        /// 
        /// Note: This does NOT assume sorted arrays.
        /// </summary>
        /// 
        /// Time Complexity: O(n + m)
        /// Space Complexity: O(n + m)
        public static int[] Merge(int[] arr1, int[] arr2)
        {
            if (arr1 == null) arr1 = System.Array.Empty<int>();
            if (arr2 == null) arr2 = System.Array.Empty<int>();

            var result = new int[arr1.Length + arr2.Length];

            var i = 0;

            foreach (var num in arr1)
                result[i++] = num;

            foreach (var num in arr2)
                result[i++] = num;

            return result;
        }

        /// <summary>
        /// Helper method to reverse part of the array.
        /// </summary>
        private static void Reverse(int[] arr, int start, int end)
        {
            while (start < end)
            {
                var temp = arr[start];
                arr[start] = arr[end];
                arr[end] = temp;

                start++;
                end--;
            }
        }
    }
}