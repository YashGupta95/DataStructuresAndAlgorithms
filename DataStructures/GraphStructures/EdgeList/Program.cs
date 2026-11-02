namespace DataStructures.GraphStructures.EdgeList
{
    internal class Program
    {
        // Shared undirected-unweighted demo graph used by sections 1, 3, 5-8:
        //
        //     0 ── 1 ── 3
        //     │  ╱ │    │
        //     │ ╱  │    │
        //     2    │    4
        //          └────┘
        //
        // Vertex count 5, five edges: (0,1) (0,2) (1,2) (1,3) (3,4).
        // Same shape as the Graph project's demo — so a reader who ran that project first
        // sees "same graph, different storage" side-by-side.
        private const int DemoVertexCount = 5;
        private static readonly (int src, int dst)[] DemoEdges =
        {
            (0, 1), (0, 2), (1, 2), (1, 3), (3, 4),
        };

        // A separate directed-weighted graph for sections 2 and 4 — the sort demo needs
        // distinct weights so ordering is visible, and directed semantics let us
        // demonstrate the OutgoingEdges / IncomingEdges asymmetry.
        private const int WeightedVertexCount = 5;
        private static readonly (int src, int dst, int weight)[] WeightedEdges =
        {
            (0, 1, 7), (0, 2, 3), (1, 2, 5), (2, 3, 2), (3, 4, 9), (1, 4, 6),
        };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              EDGE LIST DEMONSTRATIONS");
            Console.WriteLine("      (edge-centric storage — the sortable-pile view)");
            Console.WriteLine("==============================================================");

            DemonstrateBuildUndirected();
            DemonstrateBuildDirectedWeighted();
            DemonstrateEdgeQueries();
            DemonstrateSortByWeight();
            DemonstrateNeighborQueries();
            DemonstrateDegreeQueries();
            DemonstrateDynamicUpdates();
            DemonstrateCloneIndependence();
            DemonstrateDecisionGuide();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Section 1: build the undirected-unweighted demo graph and print it as a flat pile
        /// and as an adjacency-list-shaped regrouping side-by-side.
        /// </summary>
        private static void DemonstrateBuildUndirected()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build & Visualize — Undirected, Unweighted");
            Console.WriteLine("==============================================================");

            var el = BuildUndirectedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Create an EdgeList over {DemoVertexCount} vertices, undirected + unweighted.");
            Console.WriteLine($"• Add {DemoEdges.Length} edges: {FormatEdgePairs(DemoEdges)}.");
            Console.WriteLine("• Each undirected edge is stored ONCE with unordered semantics — the");
            Console.WriteLine("  storage is smaller than a symmetric matrix or a two-direction list.");

            Console.WriteLine("\nEdge list (the pile itself):");
            el.PrintList();

            Console.WriteLine("\nSame edges, regrouped by source (adjacency-list view):");
            el.PrintGroupedBySource();

            Console.WriteLine($"\nResult: EdgeCount = {el.EdgeCount} (expected: {DemoEdges.Length})");
            Console.WriteLine($"Result: VertexCount = {el.VertexCount} (expected: {DemoVertexCount})");
        }

        /// <summary>
        /// Section 2: build the directed weighted demo graph. Shows how the same class flips
        /// mode via the constructor flags.
        /// </summary>
        private static void DemonstrateBuildDirectedWeighted()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Build & Visualize — Directed, Weighted");
            Console.WriteLine("==============================================================");

            var el = BuildWeightedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Create an EdgeList over {WeightedVertexCount} vertices, directed + weighted.");
            Console.WriteLine($"• Add {WeightedEdges.Length} edges with distinct weights.");
            Console.WriteLine("• Directed mode means (u, v) does NOT imply (v, u) — every edge is a");
            Console.WriteLine("  one-way arrow, and the storage reflects that.");

            Console.WriteLine("\nEdge list (insertion order):");
            el.PrintList();

            Console.WriteLine($"\nResult: EdgeCount = {el.EdgeCount} (expected: {WeightedEdges.Length})");
            Console.WriteLine($"Result: IsDirected = {el.IsDirected} (expected: True)");
            Console.WriteLine($"Result: IsWeighted = {el.IsWeighted} (expected: True)");
        }

        /// <summary>
        /// Section 3: HasEdge / GetWeight. Each query is an O(E) linear scan — the trade-off
        /// EdgeList makes to keep the sort-and-sweep operations fast.
        /// </summary>
        private static void DemonstrateEdgeQueries()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Edge Queries — HasEdge, GetWeight");
            Console.WriteLine("==============================================================");

            var el = BuildUndirectedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Ask HasEdge for existing and non-existing pairs.");
            Console.WriteLine("• Because the graph is undirected, HasEdge(1, 0) and HasEdge(0, 1) both");
            Console.WriteLine("  return true — the class normalizes the lookup.");
            Console.WriteLine("• Each of these queries costs O(E) — worth noting: this is EDGE LIST'S");
            Console.WriteLine("  STRUCTURAL WEAKNESS. On any workload that leans on point queries,");
            Console.WriteLine("  reach for the adjacency list or matrix instead.");

            Console.WriteLine($"\nResult: HasEdge(0, 1) = {el.HasEdge(0, 1)} (expected: True)");
            Console.WriteLine($"Result: HasEdge(1, 0) = {el.HasEdge(1, 0)} (expected: True — undirected)");
            Console.WriteLine($"Result: HasEdge(3, 4) = {el.HasEdge(3, 4)} (expected: True)");
            Console.WriteLine($"Result: HasEdge(0, 4) = {el.HasEdge(0, 4)} (expected: False)");
            Console.WriteLine($"Result: HasEdge(2, 3) = {el.HasEdge(2, 3)} (expected: False)");

            var weighted = BuildWeightedDemo();
            Console.WriteLine($"\nResult: GetWeight(0, 2) on weighted demo = {weighted.GetWeight(0, 2)} (expected: 3)");
            Console.WriteLine($"Result: GetWeight(3, 4) on weighted demo = {weighted.GetWeight(3, 4)} (expected: 9)");
        }

        /// <summary>
        /// Section 4: THE POINT of the whole project. SortByWeight is the operation that
        /// makes EdgeList a first-class citizen among graph representations.
        /// </summary>
        private static void DemonstrateSortByWeight()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. SortByWeight — The Reason EdgeList Exists");
            Console.WriteLine("==============================================================");

            var el = BuildWeightedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Take the weighted demo (edges in insertion order).");
            Console.WriteLine("• Call SortByWeight() — an O(E log E) in-place sort. On a matrix this");
            Console.WriteLine("  would first cost O(V²) to extract; on an adjacency list, O(V + E).");
            Console.WriteLine("  On an EdgeList the sort is the ONLY step — the storage is already");
            Console.WriteLine("  what you want to sort.");
            Console.WriteLine("• This sorted order is exactly the input Kruskal's MST wants. The");
            Console.WriteLine("  algorithm itself lives in the future Algorithms/GraphAlgorithms/");
            Console.WriteLine("  Kruskal project — this project just proves EdgeList delivers the");
            Console.WriteLine("  right shape.");

            Console.WriteLine("\nBefore sort (insertion order):");
            el.PrintList();

            el.SortByWeight();
            Console.WriteLine("\nAfter SortByWeight() ascending:");
            el.PrintList();

            el.SortByWeight(ascending: false);
            Console.WriteLine("\nAfter SortByWeight(ascending: false):");
            el.PrintList();
        }

        /// <summary>
        /// Section 5: OutgoingEdges / IncomingEdges as deferred iterators. Shows the senior
        /// trap — these are O(E) on this representation and are shown here so the cost is
        /// tangible.
        /// </summary>
        private static void DemonstrateNeighborQueries()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Neighbor Queries — Where EdgeList Loses");
            Console.WriteLine("==============================================================");

            var el = BuildUndirectedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• OutgoingEdges(1) enumerates every edge touching vertex 1 (undirected");
            Console.WriteLine("  mode). Each returned Edge has Source normalized to 1.");
            Console.WriteLine("• The implementation must SCAN THE ENTIRE PILE to find matches — O(E).");
            Console.WriteLine("  An adjacency list would answer this in O(deg(1)) with no scan.");

            Console.WriteLine("\nOutgoingEdges(1):");
            foreach (var e in el.OutgoingEdges(1))
            {
                Console.WriteLine($"  {e.Source} -- {e.Destination}");
            }
            Console.WriteLine("  (expected: 1--0, 1--2, 1--3 — in scan order)");

            var directed = BuildWeightedDemo();
            Console.WriteLine("\nOutgoingEdges(1) on directed weighted demo:");
            foreach (var e in directed.OutgoingEdges(1))
            {
                Console.WriteLine($"  {e.Source} -> {e.Destination} (w={e.Weight})");
            }
            Console.WriteLine("  (expected: 1->2 w=5, 1->4 w=6 — only true outgoing arrows)");

            Console.WriteLine("\nIncomingEdges(2) on directed weighted demo:");
            foreach (var e in directed.IncomingEdges(2))
            {
                Console.WriteLine($"  {e.Source} -> {e.Destination} (w={e.Weight})");
            }
            Console.WriteLine("  (expected: 0->2 w=3, 1->2 w=5)");
        }

        /// <summary>
        /// Section 6: OutDegree / InDegree — both O(E). On an undirected graph, InDegree
        /// equals OutDegree.
        /// </summary>
        private static void DemonstrateDegreeQueries()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Degree Queries");
            Console.WriteLine("==============================================================");

            var undirected = BuildUndirectedDemo();
            var directed = BuildWeightedDemo();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Degree count reduces to filtered scan — O(E) each. Cheap in absolute");
            Console.WriteLine("  terms on small graphs, but on any workload that queries degrees");
            Console.WriteLine("  repeatedly this is where EdgeList falls behind an adjacency list.");

            Console.WriteLine("\nUndirected demo (InDegree == OutDegree for every vertex):");
            for (var v = 0; v < undirected.VertexCount; v++)
            {
                Console.WriteLine($"  vertex {v}: OutDegree = {undirected.OutDegree(v)}   InDegree = {undirected.InDegree(v)}");
            }

            Console.WriteLine("\nDirected weighted demo (asymmetric):");
            for (var v = 0; v < directed.VertexCount; v++)
            {
                Console.WriteLine($"  vertex {v}: OutDegree = {directed.OutDegree(v)}   InDegree = {directed.InDegree(v)}");
            }
        }

        /// <summary>
        /// Section 7: RemoveEdge and Clear — the mutation surface after construction.
        /// </summary>
        private static void DemonstrateDynamicUpdates()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Dynamic Updates — RemoveEdge, AddEdge Overwrite, Clear");
            Console.WriteLine("==============================================================");

            var el = BuildWeightedDemo();

            Console.WriteLine("\nStarting state:");
            el.PrintList();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• RemoveEdge(1, 2) — removes the (1 -> 2, w=5) edge, O(E).");
            Console.WriteLine("• AddEdge(0, 2, weight: 99) — the edge (0 -> 2) already exists, so this");
            Console.WriteLine("  OVERWRITES its weight in place rather than creating a duplicate.");
            Console.WriteLine("• RemoveEdge(9, 9) — actually skipped; that would out-of-range throw.");
            Console.WriteLine("  Instead we try RemoveEdge(1, 2) again to show the false return.");

            var removed = el.RemoveEdge(1, 2);
            Console.WriteLine($"\nRemoveEdge(1, 2) returned: {removed} (expected: True)");
            el.AddEdge(0, 2, weight: 99);
            Console.WriteLine("After overwrite of (0, 2) to weight 99:");
            el.PrintList();

            var removedAgain = el.RemoveEdge(1, 2);
            Console.WriteLine($"\nRemoveEdge(1, 2) again returned: {removedAgain} (expected: False — already gone)");

            el.Clear();
            Console.WriteLine($"\nAfter Clear(): EdgeCount = {el.EdgeCount} (expected: 0)");
            Console.WriteLine($"                 VertexCount = {el.VertexCount} (expected: {WeightedVertexCount} — vertices survive)");
        }

        /// <summary>
        /// Section 8: Clone gives an independent deep copy. Verify by mutating the clone and
        /// checking the original is untouched.
        /// </summary>
        private static void DemonstrateCloneIndependence()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Clone() — Independent Deep Copy");
            Console.WriteLine("==============================================================");

            var original = BuildUndirectedDemo();
            var clone = original.Clone();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Clone the undirected demo. Mutate ONLY the clone — remove one edge");
            Console.WriteLine("  and add a new one. Verify the original still holds the original 5.");

            clone.RemoveEdge(0, 1);
            clone.AddEdge(2, 4);

            Console.WriteLine("\nOriginal (unchanged):");
            original.PrintList();
            Console.WriteLine("\nClone (mutated):");
            clone.PrintList();

            Console.WriteLine($"\nResult: original.EdgeCount = {original.EdgeCount}   (expected: {DemoEdges.Length})");
            Console.WriteLine($"Result: clone.EdgeCount    = {clone.EdgeCount}   (expected: {DemoEdges.Length} — removed 1, added 1)");
            Console.WriteLine($"Result: original.HasEdge(0, 1) = {original.HasEdge(0, 1)}   (expected: True — untouched)");
            Console.WriteLine($"Result: clone.HasEdge(0, 1)    = {clone.HasEdge(0, 1)}  (expected: False — removed on clone)");
            Console.WriteLine($"Result: clone.HasEdge(2, 4)    = {clone.HasEdge(2, 4)}   (expected: True — added on clone)");
        }

        /// <summary>
        /// Section 9: text-only decision guide. When to reach for matrix vs list vs edge
        /// list, plus a Kruskal breadcrumb pointing at the future Algorithms hierarchy.
        /// </summary>
        private static void DemonstrateDecisionGuide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Matrix vs List vs EdgeList — Which One, When");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("   Operation                Matrix        Adj. List     EdgeList");
            Console.WriteLine("   ----------------------   -----------   -----------   -----------");
            Console.WriteLine("   HasEdge(u, v)            O(1)          O(deg(u))     O(E)");
            Console.WriteLine("   Neighbors(u)             O(V)          O(deg(u))     O(E)");
            Console.WriteLine("   Enumerate all edges      O(V²)         O(V + E)      O(E)");
            Console.WriteLine("   Sort edges by weight     O(V² + E lgE) O(V + E lgE)  O(E lg E)");
            Console.WriteLine("   Full-edge sweep pass     O(V²)         O(V + E)      O(E)  (cache-friendly)");
            Console.WriteLine("   Space                    O(V²)         O(V + E)      O(E)");
            Console.WriteLine();
            Console.WriteLine("   Rule of thumb:");
            Console.WriteLine("   • Frequent point queries and dense graph  →  MATRIX.");
            Console.WriteLine("   • Neighbor-walk algorithms (BFS, DFS, Dijkstra) on sparse graphs");
            Console.WriteLine("     →  ADJACENCY LIST.");
            Console.WriteLine("   • Whole-graph edge algorithms (Kruskal, Bellman-Ford, offline");
            Console.WriteLine("     connectivity, batch DSU problems)  →  EDGE LIST.");
            Console.WriteLine();
            Console.WriteLine("   The interview answer to \"how would you represent this graph?\" is");
            Console.WriteLine("   NOT \"adjacency list\" reflexively — it is \"it depends on what I'm");
            Console.WriteLine("   going to do with it\". That \"it depends\" is the senior-level answer.");
            Console.WriteLine();
            Console.WriteLine("   ── Breadcrumb ─────────────────────────────────────────────────");
            Console.WriteLine("   Kruskal's MST is the poster child of EdgeList + DSU working");
            Console.WriteLine("   together: sort the pile by weight, walk it front-to-back, and use");
            Console.WriteLine("   DSU.Union to reject edges that would close a cycle. The algorithm");
            Console.WriteLine("   itself will live under Algorithms/GraphAlgorithms/Kruskal when we");
            Console.WriteLine("   build that hierarchy. This project delivers the RIGHT SHAPE for");
            Console.WriteLine("   that algorithm — the sorted pile you saw in section 4.");
        }

        private static EdgeListOperations BuildUndirectedDemo()
        {
            var el = new EdgeListOperations(DemoVertexCount, isDirected: false, isWeighted: false);
            foreach (var (src, dst) in DemoEdges)
            {
                el.AddEdge(src, dst);
            }
            return el;
        }

        private static EdgeListOperations BuildWeightedDemo()
        {
            var el = new EdgeListOperations(WeightedVertexCount, isDirected: true, isWeighted: true);
            foreach (var (src, dst, weight) in WeightedEdges)
            {
                el.AddEdge(src, dst, weight);
            }
            return el;
        }

        private static string FormatEdgePairs((int src, int dst)[] pairs)
        {
            var parts = new List<string>(pairs.Length);
            foreach (var (src, dst) in pairs)
            {
                parts.Add($"({src},{dst})");
            }
            return string.Join(", ", parts);
        }
    }
}
