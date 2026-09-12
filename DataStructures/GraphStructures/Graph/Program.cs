namespace DataStructures.GraphStructures.Graph
{
    internal class Program
    {
        // The demo graph, drawn:
        //
        //          0 ── 1 ── 3 ── 4
        //          │  ╱ │
        //          │ ╱  │
        //          2 ───┘
        //
        // Edges (undirected, unweighted): {0-1, 0-2, 1-2, 1-3, 3-4}
        //
        // Every section from 1 through 7 operates on this same 5-vertex layout — building
        // both representations from the same edge list makes the side-by-side comparison
        // exact. Section 8 (Clone) reuses the matrix built in section 6. Section 9 is a
        // text-only decision guide.
        private const int DemoVertexCount = 5;
        private static readonly (int u, int v)[] DemoEdges =
        {
            (0, 1), (0, 2), (1, 2), (1, 3), (3, 4),
        };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                   GRAPH DEMONSTRATIONS");
            Console.WriteLine("       (Adjacency Matrix  ⇔  Adjacency List)");
            Console.WriteLine("==============================================================");
            Console.WriteLine("Demo graph — undirected, unweighted, 5 vertices");
            Console.WriteLine($"Edges: {FormatEdgeList(DemoEdges)}");

            var matrix = DemonstrateBuildMatrix();

            var list = DemonstrateBuildList();

            DemonstrateEdgeQueries(matrix, list);

            DemonstrateDynamicUpdates(matrix, list);

            DemonstrateDirectedWeighted();

            DemonstrateAddRemoveVertex();

            DemonstrateGetAllEdges();

            DemonstrateClone();

            DemonstrateMatrixVsListDecisionGuide();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Section 1: build the demo graph in the adjacency-matrix representation and print
        /// the resulting V × V grid. The symmetry across the diagonal is the fingerprint of
        /// an undirected graph.
        /// </summary>
        private static GraphAdjacencyMatrix DemonstrateBuildMatrix()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build & Visualize — Adjacency Matrix");
            Console.WriteLine("==============================================================");

            var graph = new GraphAdjacencyMatrix(DemoVertexCount, isDirected: false, isWeighted: false);
            foreach (var (u, v) in DemoEdges)
            {
                graph.AddEdge(u, v);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Allocate a V × V grid of NoEdge sentinels — one cell per ORDERED");
            Console.WriteLine("  pair of vertices. Total cells: V² = 25.");
            Console.WriteLine("• For each undirected edge {u, v} write weight 1 into BOTH matrix[u, v]");
            Console.WriteLine("  and matrix[v, u]. This mirroring produces the diagonal symmetry that");
            Console.WriteLine("  visually identifies an undirected graph.");

            Console.WriteLine("\nAdjacency Matrix:");
            graph.PrintMatrix();

            Console.WriteLine($"\nResult: VertexCount = {graph.VertexCount} (expected: 5)");
            Console.WriteLine($"Result: EdgeCount   = {graph.EdgeCount} (expected: 5)");
            return graph;
        }

        /// <summary>
        /// Section 2: build the exact same graph with the adjacency-list representation. Each
        /// undirected edge shows up in two buckets — the list-side echo of the matrix's
        /// symmetry.
        /// </summary>
        private static GraphAdjacencyList DemonstrateBuildList()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Build & Visualize — Adjacency List");
            Console.WriteLine("==============================================================");

            var graph = new GraphAdjacencyList(DemoVertexCount, isDirected: false, isWeighted: false);
            foreach (var (u, v) in DemoEdges)
            {
                graph.AddEdge(u, v);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Allocate V empty buckets — one per vertex. Total storage now: O(V).");
            Console.WriteLine("• For each undirected edge {u, v} append v to u's bucket AND u to v's");
            Console.WriteLine("  bucket. Each edge appears TWICE, once from each endpoint's perspective.");
            Console.WriteLine("• The 25 zeroed matrix cells vanish — only the 10 half-edges are stored.");

            Console.WriteLine("\nAdjacency List:");
            graph.PrintList();

            Console.WriteLine($"\nResult: VertexCount = {graph.VertexCount} (expected: 5)");
            Console.WriteLine($"Result: EdgeCount   = {graph.EdgeCount} (expected: 5)");
            return graph;
        }

        /// <summary>
        /// Section 3: exercise HasEdge / GetWeight / Neighbors / OutDegree / InDegree on both
        /// representations to prove they agree externally while diverging internally.
        /// </summary>
        private static void DemonstrateEdgeQueries(GraphAdjacencyMatrix matrix, GraphAdjacencyList list)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Edge Queries — Same Answers, Different Cost Profiles");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Same query answered by both representations, run twice to highlight");
            Console.WriteLine("  agreement on the answer but disagreement on the underlying work:");
            Console.WriteLine("      HasEdge   — matrix: O(1) cell read      | list: O(deg(u)) bucket scan");
            Console.WriteLine("      Neighbors — matrix: O(V) row scan       | list: O(deg(u)) bucket walk");
            Console.WriteLine("      InDegree  — matrix: O(V) column scan    | list (undirected): O(deg(u))");

            Console.WriteLine("\nResult: HasEdge(1, 3)     — matrix: " + matrix.HasEdge(1, 3) + "   list: " + list.HasEdge(1, 3) + "   (expected: True)");
            Console.WriteLine("Result: HasEdge(0, 4)     — matrix: " + matrix.HasEdge(0, 4) + "  list: " + list.HasEdge(0, 4) + "  (expected: False)");
            Console.WriteLine("Result: Neighbors(1)      — matrix: [" + string.Join(", ", matrix.Neighbors(1)) + "]   list: [" + string.Join(", ", list.Neighbors(1)) + "]   (expected: [0, 2, 3])");
            Console.WriteLine("Result: OutDegree(1)      — matrix: " + matrix.OutDegree(1) + "        list: " + list.OutDegree(1) + "        (expected: 3)");
            Console.WriteLine("Result: InDegree(1)       — matrix: " + matrix.InDegree(1) + "        list: " + list.InDegree(1) + "        (expected: 3 — undirected, matches OutDegree)");
        }

        /// <summary>
        /// Section 4: mutate both graphs identically (add one edge, remove another) and
        /// re-print so the delta is visible in both formats.
        /// </summary>
        private static void DemonstrateDynamicUpdates(GraphAdjacencyMatrix matrix, GraphAdjacencyList list)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Dynamic Updates — AddEdge & RemoveEdge");
            Console.WriteLine("==============================================================");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• AddEdge(2, 4)    — link a previously disconnected pair.");
            Console.WriteLine("• RemoveEdge(0, 1) — drop the very first edge from the seed list.");
            Console.WriteLine("• Both operations apply to BOTH representations so the diff is the same");
            Console.WriteLine("  logical change viewed through two different windows.");

            matrix.AddEdge(2, 4);
            matrix.RemoveEdge(0, 1);
            list.AddEdge(2, 4);
            list.RemoveEdge(0, 1);

            Console.WriteLine("\nAdjacency Matrix (after updates):");
            matrix.PrintMatrix();

            Console.WriteLine("\nAdjacency List (after updates):");
            list.PrintList();

            Console.WriteLine($"\nResult: EdgeCount — matrix: {matrix.EdgeCount}   list: {list.EdgeCount}   (expected: 5 — one added, one removed)");
        }

        /// <summary>
        /// Section 5: build a directed weighted DAG. This is the case where matrix symmetry
        /// finally breaks and OutDegree ≠ InDegree in general.
        /// </summary>
        private static void DemonstrateDirectedWeighted()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Directed & Weighted Variant");
            Console.WriteLine("==============================================================");

            var edges = new (int u, int v, int w)[]
            {
                (0, 1, 5),
                (0, 2, 3),
                (1, 3, 2),
                (2, 3, 8),
                (3, 4, 1),
            };

            var directedMatrix = new GraphAdjacencyMatrix(5, isDirected: true, isWeighted: true);
            var directedList = new GraphAdjacencyList(5, isDirected: true, isWeighted: true);
            foreach (var (u, v, w) in edges)
            {
                directedMatrix.AddEdge(u, v, w);
                directedList.AddEdge(u, v, w);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Build a small weighted DAG with edges:");
            foreach (var (u, v, w) in edges)
            {
                Console.WriteLine($"      {u} -> {v}  (w = {w})");
            }
            Console.WriteLine("• Verify two properties that only surface in the directed case:");
            Console.WriteLine("      – the matrix is NO LONGER symmetric across the diagonal;");
            Console.WriteLine("      – InDegree(v) ≠ OutDegree(v) for interior vertices.");
            Console.WriteLine("• Compare InDegree cost: matrix is a cheap column scan; the adjacency");
            Console.WriteLine("  list has no reverse pointer, so it must walk EVERY bucket.");

            Console.WriteLine("\nAdjacency Matrix (weights shown; `.` = no edge):");
            directedMatrix.PrintMatrix();

            Console.WriteLine("\nAdjacency List (weights shown):");
            directedList.PrintList();

            Console.WriteLine($"\nResult: OutDegree(3) — matrix: {directedMatrix.OutDegree(3)}   list: {directedList.OutDegree(3)}   (expected: 1 — only 3 -> 4)");
            Console.WriteLine($"Result: InDegree(3)  — matrix: {directedMatrix.InDegree(3)}   list: {directedList.InDegree(3)}   (expected: 2 — from 1 and 2)");
            Console.WriteLine($"Result: GetWeight(2, 3) — matrix: {directedMatrix.GetWeight(2, 3)}   list: {directedList.GetWeight(2, 3)}   (expected: 8)");
        }

        /// <summary>
        /// Section 6: grow the graph by one vertex, then remove a vertex. The two operations
        /// have wildly different costs across the two representations — this is the section
        /// that motivates picking the right structure for a growing or shrinking graph.
        /// </summary>
        private static void DemonstrateAddRemoveVertex()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. AddVertex & RemoveVertex");
            Console.WriteLine("==============================================================");

            var matrix = new GraphAdjacencyMatrix(DemoVertexCount, isDirected: false, isWeighted: false);
            var list = new GraphAdjacencyList(DemoVertexCount, isDirected: false, isWeighted: false);
            foreach (var (u, v) in DemoEdges)
            {
                matrix.AddEdge(u, v);
                list.AddEdge(u, v);
            }

            Console.WriteLine("\nOperation Performed — Part A: AddVertex()");
            Console.WriteLine("• Grow both graphs by one vertex.");
            Console.WriteLine("      Adjacency matrix: allocate a (V+1)² grid and copy old cells — O(V²).");
            Console.WriteLine("      Adjacency list:   append one empty bucket to the outer list — O(1) amortized.");
            Console.WriteLine("• Connect the new vertex 5 to vertex 4 so it isn't isolated.");

            var newMatrixLabel = matrix.AddVertex();
            var newListLabel = list.AddVertex();
            matrix.AddEdge(newMatrixLabel, 4);
            list.AddEdge(newListLabel, 4);

            Console.WriteLine($"\nResult: new label (matrix) = {newMatrixLabel}   new label (list) = {newListLabel}   (expected: 5)");
            Console.WriteLine($"Result: Capacity — matrix: {matrix.Capacity}   list: {list.Capacity}   (expected: 6)");

            Console.WriteLine("\nOperation Performed — Part B: RemoveVertex(1)");
            Console.WriteLine("• Vertex 1 is the local hub of the demo graph — it has degree 3.");
            Console.WriteLine("      Adjacency matrix: clear row 1 AND column 1 — O(V).");
            Console.WriteLine("      Adjacency list:   clear bucket 1 AND filter every OTHER bucket to");
            Console.WriteLine("                        drop incoming edges — O(V + E).");
            Console.WriteLine("• Vertex label 1 is now RETIRED and will never be reused. This is the");
            Console.WriteLine("  \"stable labels\" invariant — see the header comment on either class.");

            matrix.RemoveVertex(1);
            list.RemoveVertex(1);

            Console.WriteLine("\nAdjacency Matrix (after add & remove):");
            matrix.PrintMatrix();

            Console.WriteLine("\nAdjacency List (after add & remove):");
            list.PrintList();

            Console.WriteLine($"\nResult: VertexCount — matrix: {matrix.VertexCount}   list: {list.VertexCount}   (expected: 5)");
            Console.WriteLine($"Result: Capacity    — matrix: {matrix.Capacity}   list: {list.Capacity}   (expected: 6 — label 1 retired, not shrunk)");
        }

        /// <summary>
        /// Section 7: enumerate all edges via GetAllEdges — the canonical feed for Kruskal's
        /// MST, Bellman-Ford, and edge-weight statistics. Emphasizes the "each undirected
        /// edge appears once" contract.
        /// </summary>
        private static void DemonstrateGetAllEdges()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Enumerate All Edges — GetAllEdges()");
            Console.WriteLine("==============================================================");

            var matrix = new GraphAdjacencyMatrix(DemoVertexCount, isDirected: false, isWeighted: true);
            var list = new GraphAdjacencyList(DemoVertexCount, isDirected: false, isWeighted: true);
            var seed = new (int u, int v, int w)[]
            {
                (0, 1, 4),
                (0, 2, 2),
                (1, 2, 5),
                (1, 3, 10),
                (3, 4, 3),
            };
            foreach (var (u, v, w) in seed)
            {
                matrix.AddEdge(u, v, w);
                list.AddEdge(u, v, w);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Enumerate every edge as a (source, destination, weight) triple.");
            Console.WriteLine("• Undirected contract: each edge is yielded ONCE — the pair with");
            Console.WriteLine("  source <= destination — so the count matches EdgeCount.");
            Console.WriteLine("• Cost comparison:");
            Console.WriteLine("      Adjacency matrix: O(V²) scan of the whole grid.");
            Console.WriteLine("      Adjacency list:   O(V + E) walk through the buckets — dramatically");
            Console.WriteLine("                        faster on sparse graphs.");

            Console.WriteLine("\nEdges from the adjacency MATRIX:");
            foreach (var (u, v, w) in matrix.GetAllEdges())
            {
                Console.WriteLine($"  {u} — {v}  (w = {w})");
            }

            Console.WriteLine("\nEdges from the adjacency LIST:");
            foreach (var (u, v, w) in list.GetAllEdges())
            {
                Console.WriteLine($"  {u} — {v}  (w = {w})");
            }

            Console.WriteLine($"\nResult: EdgeCount (matrix) = {matrix.EdgeCount}   EdgeCount (list) = {list.EdgeCount}   (expected: 5, matching the enumeration size)");
        }

        /// <summary>
        /// Section 8: build a graph, Clone() it, mutate the clone, and prove the original is
        /// untouched. This is the guardrail every algorithm that runs "speculative what-if"
        /// experiments relies on.
        /// </summary>
        private static void DemonstrateClone()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Clone() — Independent Deep Copy");
            Console.WriteLine("==============================================================");

            var original = new GraphAdjacencyList(DemoVertexCount, isDirected: false, isWeighted: false);
            foreach (var (u, v) in DemoEdges)
            {
                original.AddEdge(u, v);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Take the demo graph as the ORIGINAL.");
            Console.WriteLine("• Call Clone() to produce an independent deep copy.");
            Console.WriteLine("• Mutate ONLY the clone: RemoveEdge(1, 3), then AddEdge(0, 4).");
            Console.WriteLine("• Verify the original is untouched — this is the guarantee that lets");
            Console.WriteLine("  algorithms explore residual / speculative variants of a graph without");
            Console.WriteLine("  corrupting the input.");

            var clone = original.Clone();
            clone.RemoveEdge(1, 3);
            clone.AddEdge(0, 4);

            Console.WriteLine("\nOriginal (unchanged):");
            original.PrintList();

            Console.WriteLine("\nClone (mutated):");
            clone.PrintList();

            Console.WriteLine($"\nResult: original.HasEdge(1, 3) = {original.HasEdge(1, 3)}   (expected: True — original untouched)");
            Console.WriteLine($"Result: clone.HasEdge(1, 3)    = {clone.HasEdge(1, 3)}  (expected: False — removed on clone)");
            Console.WriteLine($"Result: original.HasEdge(0, 4) = {original.HasEdge(0, 4)}  (expected: False — clone-only)");
            Console.WriteLine($"Result: clone.HasEdge(0, 4)    = {clone.HasEdge(0, 4)}   (expected: True — added on clone)");
        }

        /// <summary>
        /// Section 9: text-only summary. No code — the goal is to leave the reader with a
        /// decision rule they can quote in an interview.
        /// </summary>
        private static void DemonstrateMatrixVsListDecisionGuide()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Matrix vs List — Which One, When");
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine("   Property / Op            Adjacency Matrix          Adjacency List");
            Console.WriteLine("   -----------------------  ------------------------  --------------------------");
            Console.WriteLine("   Space                    O(V²)                     O(V + E)");
            Console.WriteLine("   HasEdge / GetWeight      O(1)                      O(deg(u))");
            Console.WriteLine("   AddEdge / RemoveEdge     O(1)                      O(deg(u))");
            Console.WriteLine("   Neighbors(u)             O(V)  — one row scan      O(deg(u))");
            Console.WriteLine("   OutDegree(u)             O(V)                      O(1)");
            Console.WriteLine("   InDegree(u) — undirected O(V)                      O(deg(u))");
            Console.WriteLine("   InDegree(u) — directed   O(V)  — one column scan   O(V + E)  ← senior-interview trap");
            Console.WriteLine("   AddVertex                O(V²) — grid rebuild      O(1) amortized");
            Console.WriteLine("   RemoveVertex             O(V)                      O(V + E)");
            Console.WriteLine("   GetAllEdges              O(V²)                     O(V + E)");
            Console.WriteLine();
            Console.WriteLine("   Rule of thumb:");
            Console.WriteLine("   • DENSE graph (E close to V²) or you need O(1) HasEdge queries in a hot");
            Console.WriteLine("     loop → adjacency MATRIX.");
            Console.WriteLine("   • SPARSE graph (E far less than V², the common case for real-world data)");
            Console.WriteLine("     or you'll iterate neighbors far more than probe arbitrary pairs");
            Console.WriteLine("     → adjacency LIST.");
            Console.WriteLine("   • Algorithm is intrinsically matrix-shaped (Floyd–Warshall, PageRank)");
            Console.WriteLine("     → adjacency MATRIX regardless of density.");
        }

        private static string FormatEdgeList((int u, int v)[] edges)
        {
            var parts = new List<string>(edges.Length);
            foreach (var (u, v) in edges)
            {
                parts.Add($"{u}-{v}");
            }
            return "{" + string.Join(", ", parts) + "}";
        }
    }
}
