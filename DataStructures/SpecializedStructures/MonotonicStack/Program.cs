namespace DataStructures.SpecializedStructures.MonotonicStack
{
    internal class Program
    {
        // Digits of π — small, irregular, and contains a duplicate (two 1s) for the traces.
        private static readonly int[] TraceInput = { 3, 1, 4, 1, 5, 9, 2, 6 };

        // A strictly descending run followed by one large value: the final push evicts
        // everything at once, the worst case for a single push but still O(n) in total.
        private static readonly int[] AmortizedInput = { 8, 7, 6, 5, 4, 3, 2, 1, 9 };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              MONOTONIC STACK DEMONSTRATIONS");
            Console.WriteLine("     (one push answers two questions: NEXT and PREVIOUS)");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Trace input: {Format(TraceInput)}");

            DemonstrateDecreasingTrace();
            DemonstrateIncreasingTrace();
            DemonstrateStrictVsNonStrict();
            DemonstrateAmortizedCost();
            DemonstrateNextGreaterElement();
            DemonstratePreviousSmallerElement();
            DemonstrateDailyTemperatures();
            DemonstrateStockSpan();
            DemonstrateNextGreaterElementCircular();
            DemonstrateLargestRectangleInHistogram();
            DemonstrateDecisionGuide();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Section 1: push the trace input onto a DECREASING stack and show, per push, what was
        /// evicted (they just found their next greater) and what remained below (previous greater).
        /// </summary>
        private static void DemonstrateDecreasingTrace()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Trace — Decreasing Stack (answers GREATER questions)");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Push every value as value@index onto a decreasing, non-strict stack.");
            Console.WriteLine("• Before each push, every smaller value on top is evicted. The incoming");
            Console.WriteLine("  value is the NEXT GREATER of everything it evicts.");
            Console.WriteLine("• Whatever is left on top right before the push is the incoming value's");
            Console.WriteLine("  PREVIOUS GREATER.");

            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false);
            Console.WriteLine();
            TracePushes(stack, TraceInput);

            Console.WriteLine("\nResult: values left on the stack never found a next greater element.");
        }

        /// <summary>
        /// Section 2: same input, INCREASING stack. Evictions now answer "next smaller".
        /// </summary>
        private static void DemonstrateIncreasingTrace()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Trace — Increasing Stack (answers SMALLER questions)");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Same input, but the stack now keeps values increasing bottom to top.");
            Console.WriteLine("• The incoming value evicts every LARGER-or-equal value (strict mode),");
            Console.WriteLine("  so it is the NEXT SMALLER-or-equal of each evicted value, and the value");
            Console.WriteLine("  left below it is its PREVIOUS strictly SMALLER.");
            Console.WriteLine("• Note the counter-intuitive mapping: INCREASING stack → SMALLER answers.");

            var stack = new MonotonicStackOperations(MonotonicOrder.Increasing, strict: true);
            Console.WriteLine();
            TracePushes(stack, TraceInput);
        }

        /// <summary>
        /// Section 3: the duplicate-value trap. The same input on a strict and a non-strict
        /// decreasing stack produces different NEXT answers for the equal values.
        /// </summary>
        private static void DemonstrateStrictVsNonStrict()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Strict vs Non-Strict — Where Do Duplicates Go?");
            Console.WriteLine("==============================================================");

            int[] input = { 5, 3, 3, 4 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Push {Format(input)} onto two decreasing stacks that differ only in strictness.");
            Console.WriteLine("• STRICT pops while top <= x, so the second 3 evicts the first 3 — the");
            Console.WriteLine("  first 3's NEXT answer becomes 3 (greater-OR-EQUAL).");
            Console.WriteLine("• NON-STRICT pops while top < x, so both 3s wait for the 4 — their NEXT");
            Console.WriteLine("  answer is strictly greater. That is what \"Next Greater Element\" wants.");

            Console.WriteLine("\nStrict decreasing stack:");
            TracePushes(new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: true), input);

            Console.WriteLine("\nNon-strict decreasing stack:");
            TracePushes(new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false), input);

            Console.WriteLine("\nRule of thumb:");
            Console.WriteLine("   Order        Strict  Popped element's NEXT   Incoming's PREVIOUS");
            Console.WriteLine("   ----------   ------  ----------------------  -------------------");
            Console.WriteLine("   Increasing   true    smaller-or-equal        strictly smaller");
            Console.WriteLine("   Increasing   false   strictly smaller        smaller-or-equal");
            Console.WriteLine("   Decreasing   true    greater-or-equal        strictly greater");
            Console.WriteLine("   Decreasing   false   strictly greater        greater-or-equal");
        }

        /// <summary>
        /// Section 4: prove the amortized bound with real counters — one push evicts eight
        /// entries, yet total pushes + pops stays within 2n.
        /// </summary>
        private static void DemonstrateAmortizedCost()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Amortized O(n) — Why One Expensive Push Doesn't Matter");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Push {Format(AmortizedInput)} onto a decreasing stack.");
            Console.WriteLine("• The final push (9) evicts all eight earlier values in one go — O(n) for");
            Console.WriteLine("  that single push. But every value is pushed once and popped at most once,");
            Console.WriteLine("  so the TOTAL work over the whole input is bounded by 2n.");
            Console.WriteLine("• Compare with the brute-force Next Greater Element, which rescans to the");
            Console.WriteLine("  right from every position.");

            var stack = new MonotonicStackOperations(MonotonicOrder.Decreasing, strict: false);
            for (var i = 0; i < AmortizedInput.Length; i++)
            {
                stack.Push(i, AmortizedInput[i]);
            }

            var n = AmortizedInput.Length;
            var bruteComparisons = CountBruteForceNextGreaterComparisons(AmortizedInput);

            Console.WriteLine($"\nResult: pushes = {stack.TotalPushes}, pops = {stack.TotalPops}, total = {stack.TotalPushes + stack.TotalPops} (bound: 2n = {2 * n})");
            Console.WriteLine($"Result: brute-force comparisons = {bruteComparisons} (grows as n²/2)");
        }

        private static void DemonstrateNextGreaterElement()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Next Greater Element");
            Console.WriteLine("==============================================================");

            int[] input = { 2, 1, 2, 4, 3 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Decreasing, NON-strict stack. Each evicted value's answer is the evictor.");
            Console.WriteLine("• The duplicate 2 checks the strictness choice: the first 2's answer must");
            Console.WriteLine("  be 4, not the second 2.");

            PrintComparison(
                input,
                MonotonicStackApplications.NextGreaterElement(input),
                MonotonicStackApplications.NextGreaterElementBruteForce(input),
                "[4, 2, 4, -1, -1]");
        }

        private static void DemonstratePreviousSmallerElement()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Previous Smaller Element");
            Console.WriteLine("==============================================================");

            int[] input = { 4, 5, 2, 10, 8 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Increasing, STRICT stack. No eviction is read here — the answer is the");
            Console.WriteLine("  entry left on top right before each push (PushResult.Previous).");

            PrintComparison(
                input,
                MonotonicStackApplications.PreviousSmallerElement(input),
                MonotonicStackApplications.PreviousSmallerElementBruteForce(input),
                "[-1, 4, -1, 2, 2]");
        }

        private static void DemonstrateDailyTemperatures()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Daily Temperatures");
            Console.WriteLine("==============================================================");

            int[] input = { 73, 74, 75, 71, 69, 72, 76, 73 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Same shape as Next Greater Element, but the answer is \"how many days");
            Console.WriteLine("  until warmer\" — an index DISTANCE. That is why entries carry their index.");

            PrintComparison(
                input,
                MonotonicStackApplications.DailyTemperatures(input),
                MonotonicStackApplications.DailyTemperaturesBruteForce(input),
                "[1, 1, 4, 2, 1, 1, 0, 0]");
        }

        private static void DemonstrateStockSpan()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Stock Span");
            Console.WriteLine("==============================================================");

            int[] input = { 100, 80, 60, 70, 60, 75, 85 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Span = consecutive days ending today with price <= today's price.");
            Console.WriteLine("• Equivalent to the distance to the PREVIOUS STRICTLY GREATER price —");
            Console.WriteLine("  a decreasing, strict stack's PushResult.Previous.");
            Console.WriteLine("• Works ONLINE: each day's span is final as soon as its price arrives.");

            PrintComparison(
                input,
                MonotonicStackApplications.StockSpan(input),
                MonotonicStackApplications.StockSpanBruteForce(input),
                "[1, 1, 1, 2, 1, 4, 6]");
        }

        private static void DemonstrateNextGreaterElementCircular()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Next Greater Element — Circular Array");
            Console.WriteLine("==============================================================");

            int[] input = { 1, 2, 3, 4, 3 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• The search may wrap past the end. Trick: walk the array TWICE with");
            Console.WriteLine("  virtual indices 0 .. 2n-1, reading values[i % n].");
            Console.WriteLine("• Only virtual indices < n are answer slots; the second pass exists purely");
            Console.WriteLine("  to resolve values still waiting from the first pass.");
            Console.WriteLine("• The last 3 wraps around and finds the 4.");

            PrintComparison(
                input,
                MonotonicStackApplications.NextGreaterElementCircular(input),
                MonotonicStackApplications.NextGreaterElementCircularBruteForce(input),
                "[2, 3, 4, -1, 4]");
        }

        private static void DemonstrateLargestRectangleInHistogram()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. Largest Rectangle in Histogram");
            Console.WriteLine("==============================================================");

            int[] input = { 2, 1, 5, 6, 2, 3 };

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Each bar's best rectangle uses its own height and stretches until a");
            Console.WriteLine("  strictly shorter bar on BOTH sides — so it needs both answers at once.");
            Console.WriteLine("• Increasing, strict stack. When a bar is evicted: RIGHT bound = evictor,");
            Console.WriteLine("  LEFT bound = the entry directly below it on the stack.");
            Console.WriteLine("• A height-0 sentinel at the end flushes the bars still waiting.");

            Console.WriteLine($"\nHistogram {Format(input)}:");
            PrintHistogram(input);

            var fast = MonotonicStackApplications.LargestRectangleInHistogram(input);
            var brute = MonotonicStackApplications.LargestRectangleInHistogramBruteForce(input);

            Console.WriteLine($"\nResult: monotonic stack = {fast}");
            Console.WriteLine($"Result: brute force     = {brute}");
            Console.WriteLine("        (expected: 10 — bars of height 5 and 6, height 5 x width 2)");
            Console.WriteLine($"Result: match = {fast == brute}");
        }

        private static void DemonstrateDecisionGuide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("11. Decision Guide — Which Stack For Which Question");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("   Three choices solve every problem in this family:");
            Console.WriteLine();
            Console.WriteLine("   1. ORDER       \"greater\" question  ->  DECREASING stack");
            Console.WriteLine("                  \"smaller\" question  ->  INCREASING stack");
            Console.WriteLine("   2. ANSWER      NEXT      ->  read the entries evicted by each push");
            Console.WriteLine("                  PREVIOUS  ->  read the entry left below each push");
            Console.WriteLine("   3. STRICTNESS  decides which side counts equal values as an answer");
            Console.WriteLine();
            Console.WriteLine("   Spotting the pattern in an interview:");
            Console.WriteLine("   • \"For each element, find the nearest X to its left/right\" where X is");
            Console.WriteLine("     greater or smaller — that phrasing is the monotonic stack's signature.");
            Console.WriteLine("   • A brute force with an inner loop that scans outward from each index");
            Console.WriteLine("     and stops at the first larger/smaller value is O(n^2) begging to");
            Console.WriteLine("     become O(n).");
            Console.WriteLine();
            Console.WriteLine("   What it is NOT for:");
            Console.WriteLine("   • Max/min over a SLIDING WINDOW — elements must also expire from the");
            Console.WriteLine("     old end, which a stack can't do. That's the Monotonic Queue, next.");
        }

        private static void TracePushes(MonotonicStackOperations stack, int[] input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var result = stack.Push(i, input[i]);
                var popped = result.Popped.Count == 0 ? "-" : string.Join(", ", result.Popped);
                var previous = result.Previous?.ToString() ?? "none";
                Console.WriteLine($"  push {input[i]}@{i}   evicted: {popped,-12} previous: {previous}");
                Console.Write("    ");
                stack.Print();
            }
        }

        private static int CountBruteForceNextGreaterComparisons(int[] values)
        {
            var comparisons = 0;
            for (var i = 0; i < values.Length; i++)
            {
                for (var j = i + 1; j < values.Length; j++)
                {
                    comparisons++;
                    if (values[j] > values[i])
                    {
                        break;
                    }
                }
            }
            return comparisons;
        }

        private static void PrintComparison(int[] input, int[] fast, int[] brute, string expected)
        {
            Console.WriteLine($"\nInput:                  {Format(input)}");
            Console.WriteLine($"Result: monotonic stack = {Format(fast)}");
            Console.WriteLine($"Result: brute force     = {Format(brute)}");
            Console.WriteLine($"        (expected: {expected})");
            Console.WriteLine($"Result: match = {fast.SequenceEqual(brute)}");
        }

        private static void PrintHistogram(int[] heights)
        {
            var max = heights.Length == 0 ? 0 : heights.Max();
            for (var level = max; level >= 1; level--)
            {
                Console.Write("    ");
                foreach (var h in heights)
                {
                    Console.Write(h >= level ? "## " : "   ");
                }
                Console.WriteLine();
            }
            Console.Write("    ");
            foreach (var h in heights)
            {
                Console.Write($"{h,-3}");
            }
            Console.WriteLine();
        }

        private static string Format(int[] values) => $"[{string.Join(", ", values)}]";
    }
}
