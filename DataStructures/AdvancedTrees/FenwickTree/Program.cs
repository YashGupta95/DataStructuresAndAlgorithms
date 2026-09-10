namespace DataStructures.AdvancedTrees.FenwickTree
{
    internal class Program
    {
        // Same demo input as SegmentTree/Program.cs so learners can diff the two projects
        // side-by-side and see how the two data structures answer the same questions.
        private static readonly int[] DemoInput = { 1, 3, 5, 7, 9, 11 };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                FENWICK TREE DEMONSTRATIONS");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Demo Input Array (size {DemoInput.Length}): [{string.Join(", ", DemoInput)}]");

            // ---- CORE BIT — the pedagogical anchor: full walkthrough. ----
            var bit = DemonstrateBuildAndVisualize();

            DemonstratePrefixSumQueries(bit);

            DemonstrateRangeSumFullRange(bit);

            DemonstrateRangeSumLeftHalf(bit);

            DemonstrateRangeSumRightHalf(bit);

            DemonstrateRangeSumMiddleOverlap(bit);

            DemonstrateRangeSumSingleElement(bit);

            DemonstratePointAdd(bit);

            DemonstrateRangeSumAfterUpdate(bit);

            DemonstratePointSet(bit);

            // ---- RANGE-UPDATE VARIANT — different bit[] interpretation, same loops. ----
            DemonstrateRangeUpdateVariant();

            // ---- HEAD-TO-HEAD with SegmentTree — pick-the-right-tool takeaway. ----
            DemonstrateSegmentTreeComparison();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Builds a Sum BIT over the demo input and prints two views of its structure.
        /// </summary>
        private static FenwickTreeOperations DemonstrateBuildAndVisualize()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build & Visualize (Sum BIT)");
            Console.WriteLine("==============================================================");

            var bit = new FenwickTreeOperations(DemoInput);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• A 1-indexed bit[] array of size n + 1 = 7 is allocated.");
            Console.WriteLine("• Each input value is seeded into its slot and pushed up to its");
            Console.WriteLine("  parent bit[i + lowbit(i)] in a single O(n) sweep.");

            Console.WriteLine("\nBIT Table — the diagnostic that makes lowbit click:");
            bit.PrintTable();

            Console.WriteLine("\nBIT Coverage Chart — each bar spans the arr[] indices its bit[i] slot aggregates:");
            bit.PrintTreeVisual();

            return bit;
        }

        /// <summary>
        /// Demonstrates a few PrefixSum walks — the fundamental BIT operation.
        /// </summary>
        private static void DemonstratePrefixSumQueries(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. PrefixSum Queries — The Query Loop (i -= i & -i)");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• PrefixSum(i) sums arr[0..i]. Starting at 1-indexed j = i + 1,");
            Console.WriteLine("  repeatedly subtract lowbit(j) and collect bit[j] until j == 0.");
            Console.WriteLine("• Example — PrefixSum(5): j starts at 6.");
            Console.WriteLine("      j = 6 (0b110): bit[6] added; j -= lowbit(6)=2 → j = 4");
            Console.WriteLine("      j = 4 (0b100): bit[4] added; j -= lowbit(4)=4 → j = 0  (stop)");
            Console.WriteLine("  Result: bit[6] + bit[4] — exactly the sum of arr[0..5], no double count.");

            Console.WriteLine($"\nResult: PrefixSum(0) = {bit.PrefixSum(0)}   (expected: 1)");
            Console.WriteLine($"Result: PrefixSum(2) = {bit.PrefixSum(2)}   (expected: 1 + 3 + 5 = 9)");
            Console.WriteLine($"Result: PrefixSum(5) = {bit.PrefixSum(5)}   (expected: 1 + 3 + 5 + 7 + 9 + 11 = 36)");
        }

        /// <summary>
        /// RangeSum over the entire array — the derivation from two PrefixSums.
        /// </summary>
        private static void DemonstrateRangeSumFullRange(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Range Sum: Full Range");
            Console.WriteLine("==============================================================");

            long result = bit.RangeSum(0, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(0, 5) = PrefixSum(5) - PrefixSum(-1) = 36 - 0 = 36.");
            Console.WriteLine("• PrefixSum(-1) is defined as 0 — the empty-prefix identity that makes");
            Console.WriteLine("  the subtraction formula well-defined at the left edge.");

            Console.WriteLine($"\nResult: RangeSum(0, 5) = {result}   (expected: 36)");
        }

        /// <summary>
        /// RangeSum aligned with the left half of the input.
        /// </summary>
        private static void DemonstrateRangeSumLeftHalf(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Range Sum: Left Half");
            Console.WriteLine("==============================================================");

            long result = bit.RangeSum(0, 2);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(0, 2) = PrefixSum(2) - PrefixSum(-1) = 9 - 0.");
            Console.WriteLine("• Two O(log n) walks; the tree structure is never mentioned by name.");

            Console.WriteLine($"\nResult: RangeSum(0, 2) = {result}   (expected: 1 + 3 + 5 = 9)");
        }

        /// <summary>
        /// RangeSum aligned with the right half of the input.
        /// </summary>
        private static void DemonstrateRangeSumRightHalf(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Range Sum: Right Half");
            Console.WriteLine("==============================================================");

            long result = bit.RangeSum(3, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(3, 5) = PrefixSum(5) - PrefixSum(2) = 36 - 9.");

            Console.WriteLine($"\nResult: RangeSum(3, 5) = {result}   (expected: 7 + 9 + 11 = 27)");
        }

        /// <summary>
        /// The "middle overlap" query — same one the SegmentTree demo highlights. In the SegTree
        /// this is the interesting case that recurses into both children; in the BIT it is
        /// pointedly UN-interesting, which is itself the lesson: subtraction hides the structure.
        /// </summary>
        private static void DemonstrateRangeSumMiddleOverlap(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Range Sum: Middle Overlap (SegTree's Hardest Case, BIT's Easiest)");
            Console.WriteLine("==============================================================");

            long result = bit.RangeSum(1, 4);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(1, 4) = PrefixSum(4) - PrefixSum(0) = 25 - 1.");
            Console.WriteLine("• Compare with the SegTree demo, section 5: the same query there triggers");
            Console.WriteLine("  the three-case dispatch and a partial-overlap recursion that visits both");
            Console.WriteLine("  halves. Here, the SUBTRACTION does that work invisibly — no dispatch,");
            Console.WriteLine("  no recursion, just two prefix walks. That's the whole point of the BIT.");

            Console.WriteLine($"\nResult: RangeSum(1, 4) = {result}   (expected: 3 + 5 + 7 + 9 = 24)");
        }

        /// <summary>
        /// Degenerate range: a single element.
        /// </summary>
        private static void DemonstrateRangeSumSingleElement(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Range Sum: Single Element (== GetValue)");
            Console.WriteLine("==============================================================");

            long viaRange = bit.RangeSum(2, 2);
            long viaGet = bit.GetValue(2);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(2, 2) and GetValue(2) are the SAME operation — GetValue is a");
            Console.WriteLine("  one-line convenience over RangeSum(i, i). This is because the BIT never");
            Console.WriteLine("  stores individual arr[] values; every read is derived from prefix sums.");

            Console.WriteLine($"\nResult: RangeSum(2, 2) = {viaRange}   GetValue(2) = {viaGet}   (expected: arr[2] = 5)");
        }

        /// <summary>
        /// Point update — the mirror-image walk of PrefixSum.
        /// </summary>
        private static void DemonstratePointAdd(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Point Update (PointAdd) — The Update Loop (i += i & -i)");
            Console.WriteLine("==============================================================");

            const int targetIndex = 3;
            const long delta = 93;
            int oldValue = DemoInput[targetIndex];
            long newValue = oldValue + delta;

            bit.PointAdd(targetIndex, delta);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• PointAdd({targetIndex}, +{delta}) — arr[{targetIndex}]: {oldValue} → {newValue}.");
            Console.WriteLine("• Starting at 1-indexed j = 4 (index+1), repeatedly ADD lowbit(j) and");
            Console.WriteLine("  update bit[j] until j > n. For n = 6:");
            Console.WriteLine("      j = 4 (0b100): bit[4] += 93; j += 4 → j = 8   (stop, 8 > 6)");
            Console.WriteLine("• Every slot whose covered range contains arr[3] gets updated. Any slot");
            Console.WriteLine("  whose range does NOT contain arr[3] is left untouched — that's the");
            Console.WriteLine("  guarantee we need to keep PrefixSum correct after the update.");

            Console.WriteLine("\nBIT Table After Update:");
            bit.PrintTable();
        }

        /// <summary>
        /// RangeSum after the update — proves the update propagated correctly.
        /// </summary>
        private static void DemonstrateRangeSumAfterUpdate(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Range Sum After Update");
            Console.WriteLine("==============================================================");

            long result = bit.RangeSum(3, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RangeSum(3, 5) — sum of the right half AFTER arr[3] became 100.");

            Console.WriteLine($"\nResult: RangeSum(3, 5) = {result}   (expected: 100 + 9 + 11 = 120)");
        }

        /// <summary>
        /// PointSet — overwrites an element and shows how it's built from the two primitives.
        /// </summary>
        private static void DemonstratePointSet(FenwickTreeOperations bit)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. PointSet — Composed From PointAdd + GetValue");
            Console.WriteLine("==============================================================");

            long oldValue = bit.GetValue(2);
            const long newValue = 50;

            bit.PointSet(2, newValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• PointSet(2, {newValue}) — arr[2] was {oldValue}, is now {newValue}.");
            Console.WriteLine("• Not a primitive: computed as PointAdd(2, newValue - GetValue(2)).");
            Console.WriteLine("• Two O(log n) walks — one to read the current value, one to apply the delta.");

            Console.WriteLine($"\nResult: GetValue(2) after set = {bit.GetValue(2)}   (expected: {newValue})");
        }

        /// <summary>
        /// Second class — Range Update + Point Query. Same loops, different bit[] semantics.
        /// </summary>
        private static void DemonstrateRangeUpdateVariant()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("11. Range-Update Variant — bit[] Now Holds a DIFFERENCE Sequence");
            Console.WriteLine("==============================================================");

            var ru = new FenwickTreeRangeUpdate(DemoInput);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• A fresh FenwickTreeRangeUpdate is built over the same input.");
            Console.WriteLine("• Internally, bit[] now represents the DIFFERENCE sequence");
            Console.WriteLine("  d[i] = arr[i] - arr[i - 1], so arr[i] = PrefixSum(i) on the diff BIT.");
            Console.WriteLine("• This class exposes RangeAdd + PointQuery — the OPPOSITE row of the");
            Console.WriteLine("  operation matrix from FenwickTreeOperations.");

            Console.WriteLine("\nReconstructed array before any updates:");
            ru.PrintArray();

            Console.WriteLine("\nApplying RangeAdd(1, 4, +10) — add 10 to every element in arr[1..4]:");
            ru.RangeAdd(1, 4, +10);
            Console.WriteLine("• Under the hood this is TWO point-adds on the diff BIT:");
            Console.WriteLine("      +10 at index 1  (arr[1] jumped up relative to arr[0])");
            Console.WriteLine("      -10 at index 5  (arr[5] fell back relative to arr[4])");
            Console.WriteLine("  Everything in between is unchanged in d[] because both sides grew equally.");

            Console.WriteLine("\nReconstructed array after RangeAdd:");
            ru.PrintArray();
            Console.WriteLine("  expected: [1, 13, 15, 17, 19, 11]");

            Console.WriteLine("\nSpot-check with PointQuery:");
            Console.WriteLine($"  PointQuery(0) = {ru.PointQuery(0)}   (expected: 1 — outside the range)");
            Console.WriteLine($"  PointQuery(3) = {ru.PointQuery(3)}   (expected: 17 — inside the range)");
            Console.WriteLine($"  PointQuery(5) = {ru.PointQuery(5)}   (expected: 11 — outside the range)");
        }

        /// <summary>
        /// A short text-only takeaway: when to reach for which tree. No code — the goal is to
        /// leave the reader with a decision rule.
        /// </summary>
        private static void DemonstrateSegmentTreeComparison()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("12. Fenwick Tree vs Segment Tree — Decision Guide");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("  Property               Fenwick Tree            Segment Tree");
            Console.WriteLine("  --------               ------------            ------------");
            Console.WriteLine("  Storage                n + 1                   ~4n");
            Console.WriteLine("  Core LOC               ~30                     ~150");
            Console.WriteLine("  Aggregations           sum, xor, count         any monoid");
            Console.WriteLine("                         (invertible only)       (min, max, gcd, ...)");
            Console.WriteLine("  Point Update           O(log n)                O(log n)");
            Console.WriteLine("  Range Query            O(log n) via subtract   O(log n) via 3-case walk");
            Console.WriteLine("  Range Update           needs a second class    natural with lazy prop");
            Console.WriteLine("  2D / higher            straightforward         doable but heavier");
            Console.WriteLine();
            Console.WriteLine("  Reach for the BIT when: prefix sums, order statistics, inversion counts,");
            Console.WriteLine("      or any invertible aggregate where every constant factor matters.");
            Console.WriteLine();
            Console.WriteLine("  Reach for the SegTree when: min / max / gcd range queries, lazy range");
            Console.WriteLine("      updates over associative-but-not-invertible operations, or when the");
            Console.WriteLine("      three-case pruning walk is easier to reason about than a diff BIT.");
        }
    }
}
