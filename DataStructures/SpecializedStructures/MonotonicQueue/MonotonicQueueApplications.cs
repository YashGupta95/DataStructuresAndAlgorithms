namespace DataStructures.SpecializedStructures.MonotonicQueue
{
    // The canonical problems a Monotonic Queue exists to solve, each paired with a brute force
    // so the demo can cross-check results.
    //
    //   Problem                                   Queues used                    Window
    //   ---------------------------------------   ----------------------------   ---------
    //   Sliding Window Maximum                    1 decreasing (max)             fixed k
    //   Sliding Window Minimum                    1 increasing (min)             fixed k
    //   Longest Subarray, |max - min| <= limit    1 max + 1 min                  variable
    //   Constrained Subsequence Sum               1 decreasing over DP values    fixed k
    //
    // ============================================================================================
    public static class MonotonicQueueApplications
    {
        /// <summary>
        /// The maximum of every contiguous window of size <paramref name="k"/>.
        /// <para> Example: [1, 3, -1, -3, 5, 3, 6, 7], k = 3 → [3, 3, 5, 5, 6, 7] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — each element pushed once, removed at most once. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(k) for the queue. </para>
        /// </summary>
        public static int[] SlidingWindowMaximum(int[] values, int k) =>
            SlidingWindowExtreme(values, k, MonotonicOrder.Decreasing);

        /// <summary>
        /// The minimum of every contiguous window of size <paramref name="k"/>. Identical to
        /// <see cref="SlidingWindowMaximum"/> except for the queue order.
        /// <para> Example: [1, 3, -1, -3, 5, 3, 6, 7], k = 3 → [-1, -3, -3, -3, 3, 3] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] SlidingWindowMinimum(int[] values, int k) =>
            SlidingWindowExtreme(values, k, MonotonicOrder.Increasing);

        /// <summary> O(n·k) reference for <see cref="SlidingWindowMaximum"/>. </summary>
        public static int[] SlidingWindowMaximumBruteForce(int[] values, int k) =>
            SlidingWindowExtremeBruteForce(values, k, Math.Max);

        /// <summary> O(n·k) reference for <see cref="SlidingWindowMinimum"/>. </summary>
        public static int[] SlidingWindowMinimumBruteForce(int[] values, int k) =>
            SlidingWindowExtremeBruteForce(values, k, Math.Min);

        /// <summary>
        /// Length of the longest contiguous subarray whose max and min differ by at most
        /// <paramref name="limit"/>.
        ///
        /// <para>
        /// The window GROWS on the right every step and SHRINKS from the left while it is
        /// invalid. Two queues track the window's max and min; shrinking the window is just
        /// <see cref="MonotonicQueueOperations.ExpireBefore"/> on both.
        /// </para>
        /// <para> Example: [10, 1, 2, 4, 7, 2], limit = 5 → 4 (the subarray [2, 4, 7, 2]) </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — both window edges only move forward. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int LongestSubarrayWithinLimit(int[] values, int limit)
        {
            ArgumentNullException.ThrowIfNull(values);
            ArgumentOutOfRangeException.ThrowIfNegative(limit);

            var maxQueue = new MonotonicQueueOperations(MonotonicOrder.Decreasing);
            var minQueue = new MonotonicQueueOperations(MonotonicOrder.Increasing);
            var left = 0;
            var best = 0;
            for (var right = 0; right < values.Length; right++)
            {
                maxQueue.PushBack(right, values[right]); // largest in the current window
                minQueue.PushBack(right, values[right]); // smallest in the current window

                while (maxQueue.PeekFront().Value - minQueue.PeekFront().Value > limit)
                {
                    left++;
                    // remove elements outside the current window
                    maxQueue.ExpireBefore(left);
                    minQueue.ExpireBefore(left);
                }
                best = Math.Max(best, right - left + 1);
            }
            return best;
        }

        /// <summary> O(n²) reference for <see cref="LongestSubarrayWithinLimit"/>. </summary>
        public static int LongestSubarrayWithinLimitBruteForce(int[] values, int limit)
        {
            ArgumentNullException.ThrowIfNull(values);
            ArgumentOutOfRangeException.ThrowIfNegative(limit);

            var best = 0;
            for (var i = 0; i < values.Length; i++)
            {
                int max = values[i], min = values[i];
                for (var j = i; j < values.Length; j++)
                {
                    max = Math.Max(max, values[j]);
                    min = Math.Min(min, values[j]);
                    if (max - min > limit)
                    {
                        break;
                    }
                    best = Math.Max(best, j - i + 1);
                }
            }
            return best;
        }

        /// <summary>
        /// Maximum sum of a non-empty subsequence where every two consecutive picked elements i and j
        /// are at most <paramref name="k"/> positions apart in the original array (j - i ≤ k).
        ///
        /// <para>
        /// A DP problem the queue makes fast:
        /// <c>best[i] = nums[i] + max(0, max of best[i-k .. i-1])</c>.
        /// The inner "max of the last k results" is a sliding window maximum over the DP values,
        /// so a max-queue answers it in O(1) instead of an O(k) scan.
        /// </para>
        /// <para> Example: [10, 2, -10, 5, 20], k = 2 → 37 (pick 10, 2, 5, 20) </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) for the DP values. </para>
        /// </summary>
        public static int ConstrainedSubsequenceSum(int[] nums, int k)
        {
            EnsureNonEmptyWithPositiveK(nums, k);

            var best = new int[nums.Length]; // DP array to store the best sum ending at each index
            var maxQueue = new MonotonicQueueOperations(MonotonicOrder.Decreasing);
            var answer = int.MinValue;
            for (var i = 0; i < nums.Length; i++)
            {
                maxQueue.ExpireBefore(i - k); // Remove indices that are out of the allowed range for the current element.
                var bestPrevious = maxQueue.TryPeekFront(out var front) ? Math.Max(0, front.Value) : 0;
                best[i] = nums[i] + bestPrevious; // Update the DP value for the current index based on the best previous sum within the allowed range.
                maxQueue.PushBack(i, best[i]); // Add the current index and its DP value to max-queue.
                answer = Math.Max(answer, best[i]); // Update overall best answer considering the current DP value.
            }
            return answer;
        }

        /// <summary> O(n·k) reference for <see cref="ConstrainedSubsequenceSum"/> — same DP, scanning the last k values. </summary>
        public static int ConstrainedSubsequenceSumBruteForce(int[] nums, int k)
        {
            EnsureNonEmptyWithPositiveK(nums, k);

            var best = new int[nums.Length];
            var answer = int.MinValue;
            for (var i = 0; i < nums.Length; i++)
            {
                var bestPrevious = 0;
                for (var j = Math.Max(0, i - k); j < i; j++)
                {
                    bestPrevious = Math.Max(bestPrevious, best[j]);
                }
                best[i] = nums[i] + bestPrevious;
                answer = Math.Max(answer, best[i]);
            }
            return answer;
        }

        private static int[] SlidingWindowExtreme(int[] values, int k, MonotonicOrder order)
        {
            EnsureValidWindow(values, k);

            var result = new int[values.Length - k + 1];
            var queue = new MonotonicQueueOperations(order);
            for (var i = 0; i < values.Length; i++)
            {
                queue.PushBack(i, values[i]);
                queue.ExpireBefore(i - k + 1); // Remove entries that have slid out of the window.
                if (i >= k - 1) // Only start recording results once the first full window is formed.
                {
                    result[i - k + 1] = queue.PeekFront().Value;
                }
            }
            return result;
        }

        private static int[] SlidingWindowExtremeBruteForce(int[] values, int k, Func<int, int, int> pick)
        {
            EnsureValidWindow(values, k);

            var result = new int[values.Length - k + 1];
            for (var start = 0; start < result.Length; start++)
            {
                var extreme = values[start];
                for (var j = start + 1; j < start + k; j++)
                {
                    extreme = pick(extreme, values[j]);
                }
                result[start] = extreme;
            }
            return result;
        }

        private static void EnsureValidWindow(int[] values, int k)
        {
            ArgumentNullException.ThrowIfNull(values);
            if (k < 1 || k > values.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(k), $"Window size must be in [1, {values.Length}]; got {k}.");
            }
        }

        private static void EnsureNonEmptyWithPositiveK(int[] nums, int k)
        {
            ArgumentNullException.ThrowIfNull(nums);
            if (nums.Length == 0)
            {
                throw new ArgumentException("Input must contain at least one element.", nameof(nums));
            }
            ArgumentOutOfRangeException.ThrowIfLessThan(k, 1);
        }
    }
}
