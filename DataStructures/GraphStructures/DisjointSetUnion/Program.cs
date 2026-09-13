namespace DataStructures.GraphStructures.DisjointSetUnion
{
    internal class Program
    {
        // Shared demo state: 8 singleton elements labelled 0..7. Sections 3 through 8 apply
        // the same union sequence to both variants so learners can diff them side-by-side.
        //
        // Union sequence:
        //   Union(0, 1), Union(2, 3), Union(4, 5), Union(6, 7)   — four pairs, four roots left
        //   Union(0, 2)                                           — merges left half
        //   Union(4, 6)                                           — merges right half
        //
        // End state after the six unions:
        //   • QuickFind — every element parent[i] == its root ID directly.
        //   • Optimized — trees of rank 2, root 0 for the left set, root 4 for the right set.
        //
        // Section 8 then does one more mega-union across the two size-4 sets to demonstrate
        // a rank-tying grow event on the optimized side.
        private const int DemoSize = 8;
        private static readonly (int x, int y)[] DemoUnions =
        {
            (0, 1), (2, 3), (4, 5), (6, 7), (0, 2), (4, 6),
        };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              DISJOINT SET UNION DEMONSTRATIONS");
            Console.WriteLine("       (Quick-Find  ⇔  Union-By-Rank + Path Compression)");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Demo elements: 0 .. {DemoSize - 1}");
            Console.WriteLine($"Union sequence: {FormatUnions(DemoUnions)}");

            DemonstrateBuildQuickFind();
            DemonstrateBuildOptimized();

            var (quickFind, optimized) = DemonstrateUnionSideBySide();

            DemonstrateFindAndPathCompression(quickFind.Clone(), optimized.Clone());

            DemonstrateConnectedQueries(quickFind, optimized);

            DemonstrateSetQueries(quickFind, optimized);

            DemonstrateReset(optimized.Clone());

            DemonstrateClone();

            DemonstrateDecisionGuide();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Section 1: build the Quick-Find variant with 8 singletons and print its parent
        /// table. Every parent[i] == i — the flat forest that Quick-Find upholds by
        /// construction.
        /// </summary>
        private static void DemonstrateBuildQuickFind()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build & Visualize — Quick-Find Variant");
            Console.WriteLine("==============================================================");

            var dsu = new DisjointSetUnionQuickFind(DemoSize);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Allocate a parent[] array of size 8 and set parent[i] = i for every i.");
            Console.WriteLine("• Each element points DIRECTLY at its own root — this is the Quick-Find");
            Console.WriteLine("  invariant that makes Find an O(1) array read forever.");

            Console.WriteLine("\nParent table (initial state):");
            dsu.PrintParents();

            Console.WriteLine($"\nResult: Count = {dsu.Count} (expected: 8)");
            Console.WriteLine($"Result: SetCount = {dsu.SetCount} (expected: 8 — every element is its own set)");
        }

        /// <summary>
        /// Section 2: build the optimized variant with 8 singletons. Same 8 elements, but the
        /// parent[] and rank[] arrays now support the tree semantics.
        /// </summary>
        private static void DemonstrateBuildOptimized()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Build & Visualize — Union-By-Rank + Path Compression");
            Console.WriteLine("==============================================================");

            var dsu = new DisjointSetUnionOperations(DemoSize);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Allocate parent[], rank[], and setSize[] arrays of size 8.");
            Console.WriteLine("• Every element starts as its own root (parent[i] = i), rank 0, size 1.");
            Console.WriteLine("• The array is IDENTICAL to Quick-Find at this point — the semantics only");
            Console.WriteLine("  diverge once the first Union links two roots together.");

            Console.WriteLine("\nParent / root / rank table (initial state):");
            dsu.PrintParents();

            Console.WriteLine($"\nResult: Count = {dsu.Count} (expected: 8)");
            Console.WriteLine($"Result: SetCount = {dsu.SetCount} (expected: 8)");
        }

        /// <summary>
        /// Section 3: apply the six-union seed sequence to both variants side-by-side. Prints
        /// the parent arrays afterward so the divergence is stark.
        /// </summary>
        private static (DisjointSetUnionQuickFind, DisjointSetUnionOperations) DemonstrateUnionSideBySide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Union Operations — Same Sequence, Two Different Costs");
            Console.WriteLine("==============================================================");

            var qf = new DisjointSetUnionQuickFind(DemoSize);
            var op = new DisjointSetUnionOperations(DemoSize);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Apply the six-union seed sequence to both variants.");
            Console.WriteLine("• Quick-Find performs a FULL O(n) array scan per union to relabel every");
            Console.WriteLine("  member of the losing set. Six unions on n = 8 = 48 cell writes worst case.");
            Console.WriteLine("• The optimized variant does two O(α(n)) Finds + one parent-pointer write");
            Console.WriteLine("  per union. Six unions ≈ six pointer writes total.");

            foreach (var (x, y) in DemoUnions)
            {
                qf.Union(x, y);
                op.Union(x, y);
            }

            Console.WriteLine("\nQuick-Find — parent table after all six unions:");
            qf.PrintParents();
            Console.WriteLine("\nQuick-Find — set forest (every set is a depth-1 star by construction):");
            qf.PrintForest();

            Console.WriteLine("Optimized — parent / root / rank table after all six unions:");
            op.PrintParents();
            Console.WriteLine("\nOptimized — set forest (real trees, up to rank 2):");
            op.PrintForest();

            Console.WriteLine($"Result: SetCount — QuickFind: {qf.SetCount}   Optimized: {op.SetCount}   (expected: 2)");
            return (qf, op);
        }

        /// <summary>
        /// Section 4: the pedagogical peak. Find on the optimized DSU rewires every visited
        /// node to point directly at the root — a side-effect visible by printing the array
        /// before and after. On Quick-Find nothing changes because parent[] already stores
        /// the root directly.
        /// </summary>
        private static void DemonstrateFindAndPathCompression(DisjointSetUnionQuickFind qf, DisjointSetUnionOperations op)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Find & Full Path Compression");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Call Find(3) on both variants. Element 3 sits at depth 2 in the");
            Console.WriteLine("  optimized tree (3 → 2 → 0), so Find has to walk two hops.");
            Console.WriteLine("• Full path compression rewires EVERY node on that walk (3 and 2) to");
            Console.WriteLine("  point directly at the root 0 as a side-effect. The tree becomes flatter.");
            Console.WriteLine("• Quick-Find's parent[] already stores the root directly, so Find(3)");
            Console.WriteLine("  changes nothing — it's a single O(1) array read.");

            Console.WriteLine("\nOptimized — parent table BEFORE Find(3):");
            op.PrintParents();

            var opRoot = op.Find(3);

            Console.WriteLine($"\nFind(3) on optimized returned: {opRoot} (expected: 0)");
            Console.WriteLine("\nOptimized — parent table AFTER Find(3):");
            op.PrintParents();
            Console.WriteLine("  ↑ Notice parent[3] now points straight at 0 — one fewer hop next time.");

            Console.WriteLine("\nOptimized — forest AFTER Find(3):");
            op.PrintForest();

            var qfRoot = qf.Find(3);
            Console.WriteLine($"Quick-Find — Find(3) returned: {qfRoot} (expected: 0)");
            Console.WriteLine("Quick-Find — parent[] unchanged (nothing to compress, already flat).");
        }

        /// <summary>
        /// Section 5: verify Connected(a, b) — sugar over Find(a) == Find(b) on both variants.
        /// </summary>
        private static void DemonstrateConnectedQueries(DisjointSetUnionQuickFind qf, DisjointSetUnionOperations op)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Connected Queries");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Connected(x, y) is defined as Find(x) == Find(y). Both variants agree");
            Console.WriteLine("  on every answer — the two DSU instances represent the same partition,");
            Console.WriteLine("  they just STORE it differently.");

            Console.WriteLine($"\nResult: Connected(0, 3) — QuickFind: {qf.Connected(0, 3)}   Optimized: {op.Connected(0, 3)}   (expected: True)");
            Console.WriteLine($"Result: Connected(4, 7) — QuickFind: {qf.Connected(4, 7)}   Optimized: {op.Connected(4, 7)}   (expected: True)");
            Console.WriteLine($"Result: Connected(0, 5) — QuickFind: {qf.Connected(0, 5)}  Optimized: {op.Connected(0, 5)}  (expected: False — different sets)");
            Console.WriteLine($"Result: Connected(3, 7) — QuickFind: {qf.Connected(3, 7)}  Optimized: {op.Connected(3, 7)}  (expected: False — different sets)");
        }

        /// <summary>
        /// Section 6: exercise SetSize, SetCount, GetSet, GetAllSets on both variants.
        /// </summary>
        private static void DemonstrateSetQueries(DisjointSetUnionQuickFind qf, DisjointSetUnionOperations op)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. SetSize, SetCount, GetSet, GetAllSets");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Query the current partition — both variants agree on the answers.");
            Console.WriteLine("• GetSet(x) enumerates x's set; GetAllSets() enumerates every set.");

            Console.WriteLine($"\nResult: SetSize(0)  — QuickFind: {qf.SetSize(0)}   Optimized: {op.SetSize(0)}   (expected: 4)");
            Console.WriteLine($"Result: SetSize(7)  — QuickFind: {qf.SetSize(7)}   Optimized: {op.SetSize(7)}   (expected: 4)");
            Console.WriteLine($"Result: SetCount    — QuickFind: {qf.SetCount}   Optimized: {op.SetCount}   (expected: 2)");

            Console.WriteLine($"\nGetSet(0) — QuickFind: [{string.Join(", ", qf.GetSet(0))}]");
            Console.WriteLine($"GetSet(0) — Optimized: [{string.Join(", ", op.GetSet(0))}]");
            Console.WriteLine("           (expected: [0, 1, 2, 3])");

            Console.WriteLine("\nGetAllSets — Optimized:");
            foreach (var bucket in op.GetAllSets())
            {
                Console.WriteLine($"  {{{string.Join(", ", bucket)}}}");
            }
        }

        /// <summary>
        /// Section 7: Reset() drops the DSU back to n singleton sets — cheap way to reuse
        /// the instance across multiple algorithm phases without reallocating.
        /// </summary>
        private static void DemonstrateReset(DisjointSetUnionOperations op)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Reset — Back To N Singletons");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Call Reset() on the optimized DSU (currently 2 sets of size 4).");
            Console.WriteLine("• Every element becomes its own root; ranks and sizes zero out; setCount");
            Console.WriteLine("  snaps back to n. No allocation — this is a cheap O(n) rewind.");

            op.Reset();

            Console.WriteLine("\nOptimized — parent / rank table after Reset:");
            op.PrintParents();

            Console.WriteLine($"\nResult: SetCount after Reset = {op.SetCount} (expected: 8)");
            Console.WriteLine($"Result: SetSize(0) after Reset = {op.SetSize(0)} (expected: 1)");
        }

        /// <summary>
        /// Section 8: Clone() gives you a fully independent DSU. Same guarantee as Graph.Clone
        /// — enables algorithms that need to try mutations speculatively (residual DSUs in
        /// offline Kruskal variants, backtracking union sequences).
        /// </summary>
        private static void DemonstrateClone()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Clone() — Independent Deep Copy + Rank-Bump Demo");
            Console.WriteLine("==============================================================");

            var original = new DisjointSetUnionOperations(DemoSize);
            foreach (var (x, y) in DemoUnions)
            {
                original.Union(x, y);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Rebuild the original DSU in its post-seed state (two size-4 sets, both");
            Console.WriteLine("  rank 2). Clone it. Mutate ONLY the clone by unioning the two sets.");
            Console.WriteLine("• On the clone, Union(0, 4) has rx = 0, ry = 4, both rank 2 — a tied");
            Console.WriteLine("  merge, so ry attaches under rx and rx's rank BUMPS to 3. That's the");
            Console.WriteLine("  only code path in the whole class that ever raises a rank.");
            Console.WriteLine("• Verify the original still holds its two-set partition.");

            var clone = original.Clone();
            clone.Union(0, 4);

            Console.WriteLine("\nOriginal (unchanged):");
            original.PrintForest();
            Console.WriteLine("Clone (mutated):");
            clone.PrintForest();

            Console.WriteLine($"Result: original.SetCount = {original.SetCount}   (expected: 2 — original untouched)");
            Console.WriteLine($"Result: clone.SetCount    = {clone.SetCount}   (expected: 1 — merged on clone)");
            Console.WriteLine($"Result: original.GetRank(0) = {original.GetRank(0)}  (expected: 2)");
            Console.WriteLine($"Result: clone.GetRank(0)    = {clone.GetRank(0)}  (expected: 3 — tied-merge rank bump)");
        }

        /// <summary>
        /// Section 9: text-only takeaway. No code — the goal is to leave the reader with a
        /// decision rule they can quote in an interview.
        /// </summary>
        private static void DemonstrateDecisionGuide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Quick-Find vs Optimized — Which One, When");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("   Operation           Quick-Find        Optimized (union-by-rank + full path compression)");
            Console.WriteLine("   ------------------  ----------------  -------------------------------------------------");
            Console.WriteLine("   Find                O(1)              O(α(n)) amortized");
            Console.WriteLine("   Union               O(n)              O(α(n)) amortized");
            Console.WriteLine("   Connected           O(1)              O(α(n)) amortized");
            Console.WriteLine("   SetSize             O(1)              O(α(n)) amortized");
            Console.WriteLine("   SetCount            O(1)              O(1)");
            Console.WriteLine("   GetSet              O(n)              O(n)");
            Console.WriteLine("   Space               O(n)              O(n)  (parent + rank + setSize arrays)");
            Console.WriteLine();
            Console.WriteLine("   Rule of thumb:");
            Console.WriteLine("   • Always reach for the OPTIMIZED variant in production code. α(n) is");
            Console.WriteLine("     ≤ 4 for any n a computer will ever hold, so both operations behave as");
            Console.WriteLine("     constant-time in practice.");
            Console.WriteLine("   • Quick-Find's O(1) Find never survives contact with any workload that");
            Console.WriteLine("     also unions — one call to Union costs as much as n calls to Find.");
            Console.WriteLine("     Kruskal-shaped algorithms (E unions on V elements) would run in O(E · V)");
            Console.WriteLine("     with Quick-Find and O(E · α(V)) with the optimized variant.");
            Console.WriteLine();
            Console.WriteLine("   What DSU does NOT do:");
            Console.WriteLine("   • Split a set. DSU is one-way — merges only. If you need to remove an");
            Console.WriteLine("     edge and re-query connectivity, reach for a Link-Cut Tree instead.");
        }

        private static string FormatUnions((int x, int y)[] unions)
        {
            var parts = new List<string>(unions.Length);
            foreach (var (x, y) in unions)
            {
                parts.Add($"Union({x},{y})");
            }
            return string.Join(", ", parts);
        }
    }
}
