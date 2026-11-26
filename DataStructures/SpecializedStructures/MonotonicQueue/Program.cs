namespace DataStructures.SpecializedStructures.MonotonicQueue
{
    internal class Program
    {
        // The classic Sliding Window Maximum input.
        private static readonly int[] TraceInput = { 1, 3, -1, -3, 5, 3, 6, 7 };
        private const int TraceWindow = 3;

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              MONOTONIC QUEUE DEMONSTRATIONS");
            Console.WriteLine("   (evict from the back, expire from the front, read the front)");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Trace input: {Format(TraceInput)}   window size k = {TraceWindow}");

            DemonstrateMaxQueueTrace();
            DemonstrateMinQueueTrace();
            DemonstrateWhyAStackIsNotEnough();
            DemonstrateAmortizedCost();
            DemonstrateSlidingWindowExtremes();
            DemonstrateLongestSubarrayWithinLimit();
            DemonstrateConstrainedSubsequenceSum();
            DemonstrateDecisionGuide();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Section 1: walk the trace input through a max-queue, showing both kinds of removal.
        /// </summary>
        private static void DemonstrateMaxQueueTrace()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Trace — Max-Queue (decreasing front to back)");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Each step: push the new value at the BACK, evicting every smaller-or-equal");
            Console.WriteLine("  value there — they can never be a window maximum again.");
            Console.WriteLine("• Then remove values at the FRONT that have slid out of the window.");
            Console.WriteLine("• The front is now the maximum of the current window.");
            Console.WriteLine();

            TraceWindowRun(new MonotonicQueueOperations(MonotonicOrder.Decreasing), TraceInput, TraceWindow, expire: true, "max");
        }

        /// <summary>
        /// Section 2: same input, min-queue — only the order changes.
        /// </summary>
        private static void DemonstrateMinQueueTrace()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Trace — Min-Queue (increasing front to back)");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Same steps, but a new value evicts every LARGER-or-equal value at the back.");
            Console.WriteLine("• The front is now the minimum of the current window.");
            Console.WriteLine();

            TraceWindowRun(new MonotonicQueueOperations(MonotonicOrder.Increasing), TraceInput, TraceWindow, expire: true, "min");
        }

        /// <summary>
        /// Section 3: skip front expiry (which is all a monotonic STACK can do) and watch the
        /// front go stale.
        /// </summary>
        private static void DemonstrateWhyAStackIsNotEnough()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Why A Monotonic Stack Is Not Enough");
            Console.WriteLine("==============================================================");

            int[] input = { 9, 1, 2, 3 };
            const int k = 2;

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Input {Format(input)}, window size {k}. Correct window maxima: [9, 2, 3].");
            Console.WriteLine("• A monotonic stack only removes from ONE end (the top = the back).");
            Console.WriteLine("  Run the same process WITHOUT front expiry and the 9 is never removed,");
            Console.WriteLine("  even after it has left the window.");

            Console.WriteLine("\nWithout front expiry (stack behaviour):");
            TraceWindowRun(new MonotonicQueueOperations(MonotonicOrder.Decreasing), input, k, expire: false, "max");

            Console.WriteLine("\nWith front expiry (queue behaviour):");
            TraceWindowRun(new MonotonicQueueOperations(MonotonicOrder.Decreasing), input, k, expire: true, "max");

            Console.WriteLine("\nResult: without expiry every window reports 9 — wrong for the last two windows.");
        }

        /// <summary>
        /// Section 4: counters prove the O(n) bound against the O(n·k) brute force.
        /// </summary>
        private static void DemonstrateAmortizedCost()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Amortized O(n) — Queue vs Brute Force");
            Console.WriteLine("==============================================================");

            const int n = 30;
            const int k = 10;
            var input = new int[n];
            for (var i = 0; i < n; i++)
            {
                input[i] = (i * 7) % 11;
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Sliding window maximum over {n} values with window size {k}.");
            Console.WriteLine("• Every value is pushed once and removed at most once, so the queue does");
            Console.WriteLine("  at most 2n operations no matter how large k is.");
            Console.WriteLine("• The brute force rescans all k values of every window.");

            var queue = new MonotonicQueueOperations(MonotonicOrder.Decreasing);
            for (var i = 0; i < n; i++)
            {
                queue.PushBack(i, input[i]);
                queue.ExpireBefore(i - k + 1);
            }
            var bruteComparisons = (n - k + 1) * (k - 1);

            Console.WriteLine($"\nResult: queue pushes = {queue.TotalPushes}, removals = {queue.TotalPops}, total = {queue.TotalPushes + queue.TotalPops} (bound: 2n = {2 * n})");
            Console.WriteLine($"Result: brute-force comparisons = {bruteComparisons} (grows as n·k)");
        }

        private static void DemonstrateSlidingWindowExtremes()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Sliding Window Maximum & Minimum");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• The two problems share one algorithm — only the queue order differs.");

            Console.WriteLine($"\nInput: {Format(TraceInput)}, k = {TraceWindow}");
            PrintComparison(
                "Maximum",
                MonotonicQueueApplications.SlidingWindowMaximum(TraceInput, TraceWindow),
                MonotonicQueueApplications.SlidingWindowMaximumBruteForce(TraceInput, TraceWindow),
                "[3, 3, 5, 5, 6, 7]");
            PrintComparison(
                "Minimum",
                MonotonicQueueApplications.SlidingWindowMinimum(TraceInput, TraceWindow),
                MonotonicQueueApplications.SlidingWindowMinimumBruteForce(TraceInput, TraceWindow),
                "[-1, -3, -3, -3, 3, 3]");
        }

        private static void DemonstrateLongestSubarrayWithinLimit()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Longest Subarray With |max - min| <= limit");
            Console.WriteLine("==============================================================");

            int[] input = { 10, 1, 2, 4, 7, 2 };
            const int limit = 5;

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• The window has NO fixed size: it grows on the right every step and");
            Console.WriteLine("  shrinks from the left while max - min is too large.");
            Console.WriteLine("• Two queues — one for the max, one for the min — give both in O(1).");
            Console.WriteLine("  Shrinking the window is ExpireBefore(left) on both queues.");

            var fast = MonotonicQueueApplications.LongestSubarrayWithinLimit(input, limit);
            var brute = MonotonicQueueApplications.LongestSubarrayWithinLimitBruteForce(input, limit);

            Console.WriteLine($"\nInput: {Format(input)}, limit = {limit}");
            Console.WriteLine($"Result: monotonic queue = {fast}");
            Console.WriteLine($"Result: brute force     = {brute}");
            Console.WriteLine("        (expected: 4 — the subarray [2, 4, 7, 2], max 7 - min 2 = 5)");
            Console.WriteLine($"Result: match = {fast == brute}");
        }

        private static void DemonstrateConstrainedSubsequenceSum()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Constrained Subsequence Sum (queue-accelerated DP)");
            Console.WriteLine("==============================================================");

            int[] input = { 10, 2, -10, 5, 20 };
            const int k = 2;

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Pick a subsequence with the largest sum, where picked neighbours are at");
            Console.WriteLine($"  most {k} positions apart.");
            Console.WriteLine("• DP: best[i] = nums[i] + max(0, max of the previous k best values).");
            Console.WriteLine("• That inner max is a sliding window maximum over the DP values — a");
            Console.WriteLine("  max-queue answers it in O(1), so the DP drops from O(n·k) to O(n).");

            var fast = MonotonicQueueApplications.ConstrainedSubsequenceSum(input, k);
            var brute = MonotonicQueueApplications.ConstrainedSubsequenceSumBruteForce(input, k);

            Console.WriteLine($"\nInput: {Format(input)}, k = {k}");
            Console.WriteLine($"Result: monotonic queue = {fast}");
            Console.WriteLine($"Result: brute force     = {brute}");
            Console.WriteLine("        (expected: 37 — pick 10, 2, 5, 20 and skip -10)");
            Console.WriteLine($"Result: match = {fast == brute}");
        }

        private static void DemonstrateDecisionGuide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Decision Guide — Stack vs Queue vs Heap");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("   Question                                    Reach for          Cost");
            Console.WriteLine("   ------------------------------------------  -----------------  ----------");
            Console.WriteLine("   Nearest greater/smaller to the left/right   Monotonic Stack    O(n)");
            Console.WriteLine("   Max/min of every sliding window             Monotonic Queue    O(n)");
            Console.WriteLine("   Max/min where items leave in ANY order      Heap               O(n log n)");
            Console.WriteLine();
            Console.WriteLine("   • A heap also solves sliding window max, but it can't remove an expired");
            Console.WriteLine("     element cheaply — you leave it in and skip it when it reaches the top");
            Console.WriteLine("     (lazy deletion). That costs O(n log n).");
            Console.WriteLine("   • The queue is faster because elements expire in ARRIVAL order — always");
            Console.WriteLine("     from the front. If removals can happen in any order, use the heap.");
            Console.WriteLine();
            Console.WriteLine("   Spotting the pattern: \"max/min of every window\", or a DP that needs the");
            Console.WriteLine("   best of the last k results.");
        }

        private static void TraceWindowRun(MonotonicQueueOperations queue, int[] input, int k, bool expire, string label)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var evicted = queue.PushBack(i, input[i]);
                var expired = expire ? queue.ExpireBefore(i - k + 1) : 0;

                var evictedText = evicted.Count == 0 ? "-" : string.Join(", ", evicted);
                Console.WriteLine($"  i={i}  push {input[i],3}   evicted from back: {evictedText,-30} expired from front: {expired}");
                Console.Write("    ");
                queue.Print();
                if (i >= k - 1)
                {
                    Console.WriteLine($"    window {Format(input[(i - k + 1)..(i + 1)])} → {label} = {queue.PeekFront().Value}");
                }
            }
        }

        private static void PrintComparison(string label, int[] fast, int[] brute, string expected)
        {
            Console.WriteLine($"\n{label}:");
            Console.WriteLine($"Result: monotonic queue = {Format(fast)}");
            Console.WriteLine($"Result: brute force     = {Format(brute)}");
            Console.WriteLine($"        (expected: {expected})");
            Console.WriteLine($"Result: match = {fast.SequenceEqual(brute)}");
        }

        private static string Format(int[] values) => $"[{string.Join(", ", values)}]";
    }
}
