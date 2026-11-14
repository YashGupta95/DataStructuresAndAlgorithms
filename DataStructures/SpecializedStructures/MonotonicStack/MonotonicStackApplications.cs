namespace DataStructures.SpecializedStructures.MonotonicStack
{
    // The canonical problems a Monotonic Stack exists to solve. Each one is built on
    // <see cref="MonotonicStackOperations"/> and paired with an O(n²) brute force so the demo
    // can cross-check results — and so the reader sees exactly which nested loop the stack
    // eliminates.
    //
    // Every solution boils down to three choices:
    //   1. Which ORDER?   Greater → Decreasing,  Smaller → Increasing.
    //   2. Which ANSWER?  NEXT → read PushResult.Popped,  PREVIOUS → read PushResult.Previous.
    //   3. Which STRICTNESS?  Decides whether equal values count as an answer.
    //
    //   Problem                        Order        Strict  Answer read from
    //   -----------------------------  -----------  ------  --------------------------------
    //   Next Greater Element           Decreasing   false   Popped   (next strictly greater)
    //   Previous Smaller Element       Increasing   true    Previous (previous strictly smaller)
    //   Daily Temperatures             Decreasing   false   Popped   (index distance)
    //   Stock Span                     Decreasing   true    Previous (previous strictly greater)
    //   Next Greater Element Circular  Decreasing   false   Popped   (two passes)
    //   Largest Rectangle Histogram    Increasing   true    Popped + Previous (both bounds)
    //
    // ============================================================================================
    public static class MonotonicStackApplications
    {
        /// <summary>
        /// For each element, the first value to its RIGHT that is strictly greater, or -1.
        /// <para> Example: [2, 1, 2, 4, 3] → [4, 2, 4, -1, -1] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — each element pushed once, popped at most once. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] NextGreaterElement(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var result = Filled(values.Length, -1);
            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false);
            for (var i = 0; i < values.Length; i++)
            {
                // First we push the current element and get the list of popped entries
                // The Popped list contains entries whose NEXT greater element is the current value.
                foreach (var waiting in stack.Push(i, values[i]).Popped)
                {
                    result[waiting.Index] = values[i]; // Assign current value as the NEXT greater element for all popped entries.
                }
            }
            return result;
        }

        /// <summary> O(n²) reference for <see cref="NextGreaterElement"/>. </summary>
        public static int[] NextGreaterElementBruteForce(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var result = Filled(values.Length, -1);
            for (var i = 0; i < values.Length; i++)
            {
                for (var j = i + 1; j < values.Length; j++)
                {
                    if (values[j] > values[i])
                    {
                        result[i] = values[j];
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// For each element, the closest value to its LEFT that is strictly smaller, or -1.
        /// <para> Example: [4, 5, 2, 10, 8] → [-1, 4, -1, 2, 2] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] PreviousSmallerElement(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var result = new int[values.Length];
            var stack = new MonotonicStackOperations(MonotonicOrder.Increasing, strict: true);
            for (var i = 0; i < values.Length; i++)
            {
                var previous = stack.Push(i, values[i]).Previous;
                result[i] = previous?.Value ?? -1;
            }
            return result;
        }

        /// <summary> O(n²) reference for <see cref="PreviousSmallerElement"/>. </summary>
        public static int[] PreviousSmallerElementBruteForce(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var result = Filled(values.Length, -1);
            for (var i = 0; i < values.Length; i++)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (values[j] < values[i])
                    {
                        result[i] = values[j];
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// For each day, how many days you must wait for a strictly warmer temperature, or 0.
        /// Same shape as <see cref="NextGreaterElement"/>, but the answer is an index DISTANCE —
        /// the reason <see cref="StackEntry"/> carries the index.
        /// <para> Example: [73, 74, 75, 71, 69, 72, 76, 73] → [1, 1, 4, 2, 1, 1, 0, 0] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] DailyTemperatures(int[] temperatures)
        {
            ArgumentNullException.ThrowIfNull(temperatures);
            var result = new int[temperatures.Length];
            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false);
            for (var i = 0; i < temperatures.Length; i++)
            {
                foreach (var waiting in stack.Push(i, temperatures[i]).Popped)
                {
                    // Distance is calculated as the difference between current index and the index of the popped entry.
                    // Value for an index can be updated multiple times as we find the next warmer day and pop it from the stack.
                    result[waiting.Index] = i - waiting.Index; 
                }
            }
            return result;
        }

        /// <summary> O(n²) reference for <see cref="DailyTemperatures"/>. </summary>
        public static int[] DailyTemperaturesBruteForce(int[] temperatures)
        {
            ArgumentNullException.ThrowIfNull(temperatures);
            var result = new int[temperatures.Length];
            for (var i = 0; i < temperatures.Length; i++)
            {
                for (var j = i + 1; j < temperatures.Length; j++)
                {
                    if (temperatures[j] > temperatures[i])
                    {
                        result[i] = j - i;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// For each day, the number of consecutive days ending today whose price was
        /// less than or equal to today's price. Equivalent to "distance to the previous
        /// strictly greater price". Works ONLINE — each answer is final the moment the
        /// day's price arrives.
        /// <para> Example: [100, 80, 60, 70, 60, 75, 85] → [1, 1, 1, 2, 1, 4, 6] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] StockSpan(int[] prices)
        {
            ArgumentNullException.ThrowIfNull(prices);
            var result = new int[prices.Length];
            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: true);
            for (var i = 0; i < prices.Length; i++)
            {
                var previous = stack.Push(i, prices[i]).Previous;
                // If there is a previous greater price, the span is the distance to it.
                // Otherwise, the span is the entire range from the start to the current day.
                result[i] = previous is { } p ? i - p.Index : i + 1;
            }
            return result;
        }

        /// <summary> O(n²) reference for <see cref="StockSpan"/>. </summary>
        public static int[] StockSpanBruteForce(int[] prices)
        {
            ArgumentNullException.ThrowIfNull(prices);
            var result = new int[prices.Length];
            for (var i = 0; i < prices.Length; i++)
            {
                var span = 1;
                for (var j = i - 1; j >= 0 && prices[j] <= prices[i]; j--)
                {
                    span++;
                }
                result[i] = span;
            }
            return result;
        }

        /// <summary>
        /// <see cref="NextGreaterElement"/> on a CIRCULAR array — the search may wrap past the
        /// end back to the start. Trick: walk the array twice using virtual indices
        /// <c>0 .. 2n − 1</c> (value <c>values[i % n]</c>). Only virtual indices below n are
        /// real answer slots; second-pass entries exist purely to resolve first-pass waiters.
        /// <para> Example: [1, 2, 3, 4, 3] → [2, 3, 4, -1, 4] </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — 2n pushes, each popped at most once. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static int[] NextGreaterElementCircular(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var n = values.Length;
            var result = Filled(n, -1);
            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false);
            for (var i = 0; i < 2 * n; i++)
            {
                var value = values[i % n];
                foreach (var waiting in stack.Push(i, value).Popped)
                {
                    if (waiting.Index < n) // If the waiting index is within the original array bounds, update the result for that index
                    {
                        result[waiting.Index] = value;
                    }
                }
            }
            return result;
        }

        /// <summary> O(n²) reference for <see cref="NextGreaterElementCircular"/>. </summary>
        public static int[] NextGreaterElementCircularBruteForce(int[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var n = values.Length;
            var result = Filled(n, -1);
            for (var i = 0; i < n; i++)
            {
                for (var k = 1; k < n; k++)
                {
                    var candidate = values[(i + k) % n];
                    if (candidate > values[i])
                    {
                        result[i] = candidate;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Area of the largest rectangle that fits under a histogram of unit-width bars.
        ///
        /// <para>
        /// The flagship problem — it needs BOTH answers per bar. Every bar's best rectangle uses
        /// its own height and stretches until it hits a strictly shorter bar on each side. With
        /// an increasing stack, when bar <c>p</c> is evicted by bar <c>i</c>:
        /// </para>
        /// <list type="bullet">
        ///   <item> RIGHT bound = <c>i</c> — the evictor (a shorter-or-equal bar). </item>
        ///   <item> LEFT bound  = the entry directly below <c>p</c> — the next popped entry, or
        ///   <see cref="PushResult.Previous"/> for the last popped one, or −1 if none. </item>
        /// </list>
        /// <para>
        /// A height-0 sentinel pushed at index n flushes every bar still waiting. An equal-height
        /// evictor underestimates the evicted bar's width, but the evictor itself later covers that
        /// full span — so the maximum is still correct.
        /// </para>
        /// <para> Example: [2, 1, 5, 6, 2, 3] → 10 (bars 5 and 6, height 5, width 2) </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public static long LargestRectangleInHistogram(int[] heights)
        {
            ArgumentNullException.ThrowIfNull(heights);
            EnsureNonNegative(heights);

            long best = 0;
            var stack = new MonotonicStackOperations(MonotonicOrder.Increasing, strict: true);
            for (var i = 0; i <= heights.Length; i++)
            {
                var height = i < heights.Length ? heights[i] : 0;
                var result = stack.Push(i, height);
                var popped = result.Popped; // Popped bars have height >= the incoming bar (strict mode)
                // Iterate over each popped bar to calculate its maximum rectangle width
                // Since elements are popped in reverse order, the next popped element (k+1) is the nearest shorter bar to the left of the current one (k).
                for (var k = 0; k < popped.Count; k++)
                {
                    // Left wall = the nearest strictly shorter bar to popped[k]'s left, i.e. the entry just below it on the stack:
                    // the next popped entry if that was evicted too, else the survivor (Previous), else -1 (reaches index 0).
                    var left = k + 1 < popped.Count
                        ? popped[k + 1].Index
                        : result.Previous?.Index ?? -1;
                    var width = i - left - 1; // Rectangle spans columns left+1 .. i-1 at height popped[k].Value
                    best = Math.Max(best, (long)popped[k].Value * width);
                }
            }
            return best;
        }

        /// <summary> O(n²) reference for <see cref="LargestRectangleInHistogram"/>. </summary>
        public static long LargestRectangleInHistogramBruteForce(int[] heights)
        {
            ArgumentNullException.ThrowIfNull(heights);
            EnsureNonNegative(heights);

            long best = 0;
            for (var i = 0; i < heights.Length; i++)
            {
                var minHeight = int.MaxValue;
                for (var j = i; j < heights.Length; j++)
                {
                    minHeight = Math.Min(minHeight, heights[j]);
                    best = Math.Max(best, (long)minHeight * (j - i + 1));
                }
            }
            return best;
        }

        private static int[] Filled(int length, int value)
        {
            var array = new int[length];
            Array.Fill(array, value);
            return array;
        }

        private static void EnsureNonNegative(int[] heights)
        {
            foreach (var h in heights)
            {
                if (h < 0)
                {
                    throw new ArgumentException($"Bar heights must be non-negative; got {h}.", nameof(heights));
                }
            }
        }
    }
}
