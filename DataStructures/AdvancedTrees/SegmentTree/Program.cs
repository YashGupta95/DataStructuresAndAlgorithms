namespace DataStructures.AdvancedTrees.SegmentTree
{
    internal class Program
    {
        private static readonly int[] DemoInput = { 1, 3, 5, 7, 9, 11 };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                SEGMENT TREE DEMONSTRATIONS");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Demo Input Array (size {DemoInput.Length}): [{string.Join(", ", DemoInput)}]");

            // ---- SUM tree — the pedagogical anchor: full walkthrough. ----
            var sumTree = DemonstrateBuildAndVisualize();

            DemonstrateSumQueryFullRange(sumTree);

            DemonstrateSumQueryLeftHalf(sumTree);

            DemonstrateSumQueryRightHalf(sumTree);

            DemonstrateSumQueryMiddleOverlap(sumTree);

            DemonstrateSumQuerySingleElement(sumTree);

            DemonstratePointUpdate(sumTree);

            DemonstrateSumQueryAfterUpdate(sumTree);

            // ---- MIN & MAX trees — abbreviated: same code, different Combine/Identity. ----
            DemonstrateMinTree();

            DemonstrateMaxTree();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Builds a Sum Segment Tree over the demo input and prints its structure.
        /// </summary>
        private static SegmentTreeOperations DemonstrateBuildAndVisualize()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build & Visualize (Sum Tree)");
            Console.WriteLine("==============================================================");

            var sumTree = new SegmentTreeOperations(DemoInput, AggregationKind.Sum);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• A tree array of size 4 * n = 24 is allocated.");
            Console.WriteLine("• The input is split recursively; each internal node stores the SUM of its segment.");
            Console.WriteLine("• Leaves store the original input values.");

            Console.WriteLine("\nTree Structure — Visual (label format: nodeIndex=value):");
            sumTree.PrintTreeVisual();

            Console.WriteLine("\nTree Structure — Detailed (indent = depth, [start..end] = segment covered):");
            sumTree.PrintTree();

            return sumTree;
        }

        /// <summary>
        /// Demonstrates a range-sum query over the entire input.
        /// </summary>
        private static void DemonstrateSumQueryFullRange(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Range Sum: Full Range");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(0, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(0, 5) — sum of the entire input.");
            Console.WriteLine("• The root's segment [0..5] is fully inside the query range,");
            Console.WriteLine("  so its stored aggregate is returned immediately (Case 2).");
            Console.WriteLine($"\nResult: Query(0, 5) = {result}   (expected: 1 + 3 + 5 + 7 + 9 + 11 = 36)");
        }

        /// <summary>
        /// Demonstrates a range-sum query that aligns exactly with one internal node's segment.
        /// </summary>
        private static void DemonstrateSumQueryLeftHalf(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Range Sum: Left Half");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(0, 2);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(0, 2) — sum of the left half of the input.");
            Console.WriteLine("• The tree has an internal node whose segment is exactly [0..2],");
            Console.WriteLine("  so the walk stops there without descending further.");
            Console.WriteLine($"\nResult: Query(0, 2) = {result}   (expected: 1 + 3 + 5 = 9)");
        }

        /// <summary>
        /// Demonstrates a range-sum query that aligns with the right half's segment.
        /// </summary>
        private static void DemonstrateSumQueryRightHalf(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Range Sum: Right Half");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(3, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(3, 5) — sum of the right half of the input.");
            Console.WriteLine("• Symmetric to the previous case: the walk halts at node [3..5].");
            Console.WriteLine($"\nResult: Query(3, 5) = {result}   (expected: 7 + 9 + 11 = 27)");
        }

        /// <summary>
        /// Demonstrates a range-sum query that straddles the middle — exercising the "partial
        /// overlap" branch that recurses into both children.
        /// </summary>
        private static void DemonstrateSumQueryMiddleOverlap(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Range Sum: Middle Overlap (Most Interesting Case)");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(1, 4);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(1, 4) — no single stored segment matches this range exactly.");
            Console.WriteLine("• The walk descends into both halves, combining partial results:");
            Console.WriteLine("      arr[1]  from leaf [1..1] = 3");
            Console.WriteLine("      arr[2]  from leaf [2..2] = 5");
            Console.WriteLine("      arr[3..4] from node [3..4] = 16");
            Console.WriteLine("  giving 3 + 5 + 16 = 24 — no need to touch arr[0] or arr[5].");
            Console.WriteLine($"\nResult: Query(1, 4) = {result}   (expected: 3 + 5 + 7 + 9 = 24)");
        }

        /// <summary>
        /// Demonstrates a degenerate range-sum query over a single element.
        /// </summary>
        private static void DemonstrateSumQuerySingleElement(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Range Sum: Single Element");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(2, 2);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(2, 2) — a single-element range descends straight to leaf [2..2].");
            Console.WriteLine($"\nResult: Query(2, 2) = {result}   (expected: arr[2] = 5)");
        }

        /// <summary>
        /// Demonstrates a point update — changing one element and letting the change propagate.
        /// </summary>
        private static void DemonstratePointUpdate(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Point Update");
            Console.WriteLine("==============================================================");

            const int targetIndex = 3;
            const int newValue = 100;
            int oldValue = DemoInput[targetIndex];

            sumTree.Update(targetIndex, newValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Update index {targetIndex}: {oldValue} → {newValue}.");
            Console.WriteLine($"• The change propagates from leaf [3..3] up through nodes [3..4], [3..5], and [0..5].");
            Console.WriteLine("• Every other subtree is untouched.");

            Console.WriteLine("\nTree Structure After Update — Visual:");
            sumTree.PrintTreeVisual();

            Console.WriteLine("\nTree Structure After Update — Detailed:");
            sumTree.PrintTree();
        }

        /// <summary>
        /// Demonstrates a range-sum query after the previous point update, proving the tree
        /// aggregates were correctly recomputed on the path from leaf to root.
        /// </summary>
        private static void DemonstrateSumQueryAfterUpdate(SegmentTreeOperations sumTree)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Range Sum After Update");
            Console.WriteLine("==============================================================");

            int result = sumTree.Query(3, 5);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query(3, 5) — sum of the right half AFTER arr[3] became 100.");
            Console.WriteLine($"\nResult: Query(3, 5) = {result}   (expected: 100 + 9 + 11 = 120)");
        }

        /// <summary>
        /// Demonstrates a Min Segment Tree — same structure, different Combine / Identity.
        /// </summary>
        private static void DemonstrateMinTree()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Min Tree — Same Structure, Different Aggregate");
            Console.WriteLine("==============================================================");

            var minTree = new SegmentTreeOperations(DemoInput, AggregationKind.Min);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Built with AggregationKind.Min. Each node stores the MIN of its segment.");
            Console.WriteLine("• Build, Query, and Update code are IDENTICAL to the sum tree above —");
            Console.WriteLine("  only Combine() and Identity() branch on the aggregation kind.");

            Console.WriteLine("\nTree Structure — Visual:");
            minTree.PrintTreeVisual();

            Console.WriteLine("\nTree Structure — Detailed:");
            minTree.PrintTree();

            int result = minTree.Query(1, 4);
            Console.WriteLine($"\nResult: Query(1, 4) = {result}   (expected: min(3, 5, 7, 9) = 3)");
        }

        /// <summary>
        /// Demonstrates a Max Segment Tree — same structure, different Combine / Identity.
        /// </summary>
        private static void DemonstrateMaxTree()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. Max Tree — Same Structure, Different Aggregate");
            Console.WriteLine("==============================================================");

            var maxTree = new SegmentTreeOperations(DemoInput, AggregationKind.Max);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Built with AggregationKind.Max. Each node stores the MAX of its segment.");

            Console.WriteLine("\nTree Structure — Visual:");
            maxTree.PrintTreeVisual();

            Console.WriteLine("\nTree Structure — Detailed:");
            maxTree.PrintTree();

            int result = maxTree.Query(1, 4);
            Console.WriteLine($"\nResult: Query(1, 4) = {result}   (expected: max(3, 5, 7, 9) = 9)");
        }
    }
}
