namespace DataStructures.GraphStructures.EdgeList
{
    // The EdgeList is the third representation of a graph in this repo and the simplest of
    // the three by a wide margin. Where the adjacency-matrix and adjacency-list projects
    // organize their storage AROUND vertices ("what does vertex 5 connect to?"), an EdgeList
    // organizes its storage AROUND edges ("here are all the edges, in an undifferentiated
    // pile"). There is no per-vertex bucket, no matrix cell — just a flat list of Edge
    // records, each carrying its own source, destination, and weight.
    //
    // -----------------------------------------------------------------------
    // Same Problem, Different Filing System
    // -----------------------------------------------------------------------
    // Think of the three graph representations as three ways to file the same information:
    //
    //   • Adjacency Matrix — a giant grid on the wall. Instantly see any pair (row, col),
    //     but wastes O(V²) space on empty cells. Wins on DENSE graphs where you constantly
    //     ask "is there an edge between u and v?".
    //
    //   • Adjacency List — a filing cabinet with one folder per vertex. Great for
    //     "everything about vertex 5", O(V + E) space. Wins on SPARSE graphs and on any
    //     algorithm that walks by neighbor (BFS, DFS, Dijkstra).
    //
    //   • Edge List (this class) — an unsorted pile of edges on your desk. Terrible for
    //     "everything about vertex 5" (must scan the whole pile), but you can SORT the pile
    //     or SCAN it end-to-end in a single sweep. Wins on GLOBAL edge operations.
    //
    // The takeaway is not "which representation is best" — it's that every graph problem
    // comes with an access pattern, and the representation should match that pattern.
    //
    // -----------------------------------------------------------------------
    // What EdgeList Is Good At (And Nothing Else Is)
    // -----------------------------------------------------------------------
    // Three categories of workload prefer EdgeList over the vertex-centric representations:
    //
    //   1. SORTING EDGES BY WEIGHT. On a matrix you'd extract all edges first (O(V²)), on
    //      a list you'd walk every bucket first (O(V + E)). On an EdgeList the sort is
    //      literally what the storage already looks like — O(E log E), and the sorted order
    //      persists so future scans stay linear. This is exactly what Kruskal's MST wants.
    //
    //   2. FULL-EDGE SWEEPS. Bellman-Ford relaxes every edge V − 1 times. On an EdgeList
    //      that's V − 1 tight linear scans over a flat contiguous list — the CPU cache
    //      loves this. On an adjacency list you nest a vertex loop inside the relaxation
    //      loop and lose locality.
    //
    //   3. OFFLINE / BATCH PROBLEMS. When the problem hands you all edges up front and asks
    //      global questions ("how many connected components?", "does adding edges in this
    //      order create a cycle at step k?"), the flat pile IS the input. Combined with a
    //      DSU, most connectivity-flavored offline problems collapse to a single sweep of
    //      the EdgeList.
    //
    // (Breadcrumb for the future <c>Algorithms/GraphAlgorithms/Kruskal</c> project:
    // Kruskal is the poster child of EdgeList + DSU working together — sort the pile by
    // weight, walk it front-to-back, and use DSU.Union to decide "does this edge close a
    // cycle?". This project intentionally does NOT implement Kruskal itself — algorithms
    // live in the future Algorithms hierarchy. What EdgeList owes here is the RIGHT SHAPE
    // for that algorithm to consume: a sortable list of edges. That is exactly what the
    // <c>SortByWeight</c> demo section will show.)
    //
    // -----------------------------------------------------------------------
    // What EdgeList Is Bad At
    // -----------------------------------------------------------------------
    //   • "Neighbors of vertex v" is O(E) — must scan the whole pile because edges aren't
    //     bucketed by source. On any real BFS/DFS/Dijkstra workload this is a dealbreaker.
    //   • "Does edge (u, v) exist?" is O(E) — same reason.
    //   • Degree of a vertex is O(E).
    //
    // If your algorithm's inner loop is "for each neighbor of the current vertex, ...",
    // reach for the adjacency list. EdgeList is for when the inner loop is "for each edge
    // in the graph, ...".
    //
    // -----------------------------------------------------------------------
    // Directed vs Undirected — Where The Extra Row Goes
    // -----------------------------------------------------------------------
    // Two design choices for undirected edges in an edge list:
    //
    //   (a) Store each undirected edge ONCE and treat it as unordered — the reader has to
    //       remember that "source" and "destination" are just labels, and Edge(u, v) and
    //       Edge(v, u) represent the same edge.
    //   (b) Store each undirected edge TWICE, once in each direction, so directed algorithms
    //       just work.
    //
    // This class picks (a). Every undirected edge lives in the list exactly once. The
    // trade-off:
    //   • Sorting and scanning are cheaper (fewer rows).
    //   • Neighbor queries have to check BOTH endpoints per row to see whether v participates.
    //     <c>OutgoingEdges(v)</c> on an undirected graph therefore returns every edge that
    //     TOUCHES v — with the endpoints normalized so the caller sees v as the "source" of
    //     each returned edge. This keeps the API consistent between the two modes.
    //
    // -----------------------------------------------------------------------
    // Simple-Graph Semantics
    // -----------------------------------------------------------------------
    // Matching the Graph project: no parallel edges, no self-loops. <c>AddEdge(u, v, w)</c>
    // on an existing edge overwrites the weight in place; on a self-loop it throws. This is
    // deliberate — parallel edges and self-loops are useful in some workloads (multigraphs,
    // flow networks), but they change the meaning of every downstream query and are best
    // handled by a purpose-built class rather than a "sometimes on, sometimes off" toggle.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary  (V = vertex count, E = current edge count)
    // -----------------------------------------------------------------------
    //   Operation             Time             Space
    //   --------------------  ---------------  ------
    //   Constructor           O(1)             O(1)
    //   AddEdge               O(E)             O(1)   (linear scan to reject duplicates)
    //   RemoveEdge            O(E)             O(1)
    //   HasEdge               O(E)             O(1)
    //   GetWeight             O(E)             O(1)
    //   OutgoingEdges         O(E)             O(1)   (deferred iterator)
    //   IncomingEdges         O(E)             O(1)
    //   OutDegree / InDegree  O(E)             O(1)
    //   GetAllEdges           O(1)             O(1)   (returns the storage directly)
    //   SortByWeight          O(E log E)       O(1)   (in-place)
    //   EdgeCount             O(1)             O(1)
    //   Clone                 O(E)             O(E)
    //   Clear                 O(E)             O(1)
    //
    // ============================================================================================
    public class EdgeListOperations
    {
        private readonly List<Edge> edges;
        private readonly int vertexCount;
        private readonly bool isDirected;
        private readonly bool isWeighted;

        /// <summary>
        /// Builds an empty edge list for a graph with <paramref name="vertexCount"/> vertices
        /// labelled <c>0 .. vertexCount − 1</c>.
        /// </summary>
        /// <param name="vertexCount"> Number of vertices. Must be non-negative. </param>
        /// <param name="isDirected"> If <c>true</c>, an edge (u, v) does NOT imply (v, u). </param>
        /// <param name="isWeighted"> If <c>false</c>, callers should treat weights as 1
        /// regardless of what is stored; the <c>AddEdge(u, v)</c> overload will default to 1. </param>
        public EdgeListOperations(int vertexCount, bool isDirected = false, bool isWeighted = false)
        {
            if (vertexCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(vertexCount), $"Vertex count must be non-negative; got {vertexCount}.");
            }
            this.vertexCount = vertexCount;
            this.isDirected = isDirected;
            this.isWeighted = isWeighted;
            edges = new List<Edge>();
        }

        /// <summary> Number of vertices. Fixed at construction. </summary>
        public int VertexCount => vertexCount;

        /// <summary> Number of edges currently stored. </summary>
        public int EdgeCount => edges.Count;

        /// <summary> <c>true</c> iff the graph treats (u, v) as distinct from (v, u). </summary>
        public bool IsDirected => isDirected;

        /// <summary> <c>true</c> iff edge weights carry meaning; on an unweighted graph, treat every weight as 1. </summary>
        public bool IsWeighted => isWeighted;

        /// <summary>
        /// Adds edge <c>(source, destination)</c> with the given weight. On an undirected
        /// graph the edge is stored ONCE with unordered semantics — subsequent queries treat
        /// <c>(source, destination)</c> and <c>(destination, source)</c> as the same edge.
        ///
        /// <para>
        /// If the edge already exists, the stored weight is OVERWRITTEN in place (matching
        /// Graph project semantics). If the two endpoints are equal this throws — self-loops
        /// are deliberately out of scope for the simple-graph model this project targets.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) — linear scan to detect an existing edge. </para>
        /// </summary>
        public void AddEdge(int source, int destination, int weight = 1)
        {
            EnsureVertexInRange(source);
            EnsureVertexInRange(destination);
            if (source == destination)
            {
                throw new InvalidOperationException($"Self-loops are not supported: source == destination == {source}.");
            }

            for (var i = 0; i < edges.Count; i++)
            {
                if (Matches(edges[i], source, destination))
                {
                    // Equivalent with-expression form (record struct sugar): edges[i] = edges[i] with { Weight = weight };
                    edges[i] = new Edge(edges[i].Source, edges[i].Destination, weight);
                    return;
                }
            }
            edges.Add(new Edge(source, destination, weight));
        }

        /// <summary>
        /// Removes edge <c>(source, destination)</c>. Returns <c>true</c> if an edge was
        /// removed, <c>false</c> if it did not exist. On an undirected graph, matches either
        /// stored orientation.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) — linear scan plus an O(E) <see cref="List{T}.RemoveAt"/> shift. </para>
        /// </summary>
        public bool RemoveEdge(int source, int destination)
        {
            EnsureVertexInRange(source);
            EnsureVertexInRange(destination);
            for (var i = 0; i < edges.Count; i++)
            {
                if (Matches(edges[i], source, destination))
                {
                    edges.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// <c>true</c> iff edge <c>(source, destination)</c> is present. On an undirected
        /// graph, matches either stored orientation.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) — must scan the pile because edges aren't indexed by endpoint. This
        /// is EdgeList's structural weakness. </para>
        /// </summary>
        public bool HasEdge(int source, int destination)
        {
            EnsureVertexInRange(source);
            EnsureVertexInRange(destination);
            foreach (var e in edges)
            {
                if (Matches(e, source, destination))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the weight of edge <c>(source, destination)</c>, or throws if the edge
        /// does not exist.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public int GetWeight(int source, int destination)
        {
            EnsureVertexInRange(source);
            EnsureVertexInRange(destination);
            foreach (var e in edges)
            {
                if (Matches(e, source, destination))
                {
                    return e.Weight;
                }
            }
            throw new InvalidOperationException($"No edge ({source}, {destination}) exists.");
        }

        /// <summary>
        /// Enumerates every edge leaving <paramref name="vertex"/>. On a directed graph this
        /// is edges whose <c>Source == vertex</c>; on an undirected graph this is every edge
        /// TOUCHING <paramref name="vertex"/>, with endpoints normalized so the caller sees
        /// <paramref name="vertex"/> as the source of each returned edge.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) — full scan. </para>
        /// </summary>
        public IEnumerable<Edge> OutgoingEdges(int vertex)
        {
            EnsureVertexInRange(vertex);
            foreach (var e in edges)
            {
                // For directed edges, or undirected edges where vertex is the source
                if (e.Source == vertex)
                {
                    yield return e;
                }
                // For undirected edges where vertex is the destination, yield a normalized edge with vertex as the source
                else if (!isDirected && e.Destination == vertex)
                {
                    yield return new Edge(vertex, e.Source, e.Weight);
                }
            }
        }

        /// <summary>
        /// Enumerates every edge entering <paramref name="vertex"/>. On a directed graph this
        /// is edges whose <c>Destination == vertex</c>. On an undirected graph this is the
        /// same set as <see cref="OutgoingEdges"/> — every touching edge — because
        /// undirected edges have no inherent direction.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public IEnumerable<Edge> IncomingEdges(int vertex)
        {
            EnsureVertexInRange(vertex);
            foreach (var e in edges)
            {
                // For directed edges, or undirected edges where vertex is the destination
                if (e.Destination == vertex)
                {
                    yield return e;
                }
                // For undirected edges where vertex is the source, yield a normalized edge with vertex as the destination
                else if (!isDirected && e.Source == vertex)
                {
                    yield return new Edge(e.Destination, vertex, e.Weight);
                }
            }
        }

        /// <summary>
        /// Number of edges leaving <paramref name="vertex"/>. See <see cref="OutgoingEdges"/>
        /// for how undirected edges are counted.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public int OutDegree(int vertex)
        {
            EnsureVertexInRange(vertex);
            var count = 0;
            foreach (var e in edges)
            {
                if (e.Source == vertex || (!isDirected && e.Destination == vertex))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Number of edges entering <paramref name="vertex"/>. On an undirected graph this
        /// equals <see cref="OutDegree"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public int InDegree(int vertex)
        {
            EnsureVertexInRange(vertex);
            var count = 0;
            foreach (var e in edges)
            {
                if (e.Destination == vertex || (!isDirected && e.Source == vertex))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Returns a read-only view over the current edge storage in insertion (or last-sorted)
        /// order. The result is a live view — mutations through <see cref="AddEdge"/> etc.
        /// are visible through it.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — direct reference. This is EdgeList's structural superpower: the
        /// storage IS the enumerable. </para>
        /// </summary>
        public IReadOnlyList<Edge> GetAllEdges() => edges;

        /// <summary>
        /// Sorts the edge list in place by weight (ascending by default). Ties break on
        /// <c>Source</c>, then <c>Destination</c>, matching <see cref="Edge.CompareTo"/>.
        ///
        /// <para>
        /// This is the operation that makes EdgeList shine. Once sorted, algorithms that
        /// need edges in weight order (Kruskal's MST, several offline problems) can just
        /// walk the pile front-to-back with no additional structure.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E log E) — <see cref="List{T}.Sort()"/>. </para>
        /// </summary>
        public void SortByWeight(bool ascending = true)
        {
            // The CompareTo method of Edge sorts by weight first, then by source, then by destination
            edges.Sort();
            if (!ascending)
            {
                edges.Reverse();
            }
        }

        /// <summary>
        /// Returns an independent deep copy. Mutating the clone leaves the original untouched.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public EdgeListOperations Clone()
        {
            var clone = new EdgeListOperations(vertexCount, isDirected, isWeighted);
            clone.edges.AddRange(edges);
            return clone;
        }

        /// <summary>
        /// Removes every edge. The vertex count and mode flags survive.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(E) </para>
        /// </summary>
        public void Clear() => edges.Clear();

        /// <summary>
        /// Prints the edge list as a flat table in the order edges are currently stored.
        /// This is the "unsorted pile on the desk" view — after <see cref="SortByWeight"/>
        /// the same output shows the sorted order.
        /// </summary>
        public void PrintList()
        {
            Console.WriteLine($"  Vertices: {vertexCount}   Edges: {edges.Count}   Directed: {isDirected}   Weighted: {isWeighted}");
            if (edges.Count == 0)
            {
                Console.WriteLine("  (no edges)");
                return;
            }

            var arrow = isDirected ? "->" : "--";
            var weightHeader = isWeighted ? "  weight" : string.Empty;
            Console.WriteLine($"  #   src {arrow} dst{weightHeader}");
            for (var i = 0; i < edges.Count; i++)
            {
                var e = edges[i];
                var weightPart = isWeighted ? $"     {e.Weight}" : string.Empty;
                Console.WriteLine($"  {i,2}   {e.Source,2}  {arrow} {e.Destination,-2}{weightPart}");
            }
        }

        /// <summary>
        /// Prints the same edges REGROUPED by source vertex — an adjacency-list-shaped view
        /// of an edge-list-shaped storage. Purely diagnostic: shows the reader that the same
        /// information is present, just organized differently, and reinforces WHY converting
        /// to an adjacency list costs O(V + E).
        /// </summary>
        public void PrintGroupedBySource()
        {
            if (vertexCount == 0)
            {
                Console.WriteLine("  (no vertices)");
                return;
            }

            var buckets = new List<Edge>[vertexCount];
            for (var i = 0; i < vertexCount; i++)
            {
                buckets[i] = new List<Edge>();
            }
            foreach (var e in edges)
            {
                buckets[e.Source].Add(e);
                if (!isDirected && e.Source != e.Destination)
                {
                    buckets[e.Destination].Add(new Edge(e.Destination, e.Source, e.Weight));
                }
            }

            for (var v = 0; v < vertexCount; v++)
            {
                var bucket = buckets[v];
                if (bucket.Count == 0)
                {
                    Console.WriteLine($"  {v}: (none)");
                    continue;
                }
                var parts = new List<string>(bucket.Count);
                foreach (var e in bucket)
                {
                    parts.Add(isWeighted ? $"{e.Destination}(w={e.Weight})" : e.Destination.ToString());
                }
                Console.WriteLine($"  {v}: [{string.Join(", ", parts)}]");
            }
        }

        private bool Matches(Edge edge, int source, int destination)
        {
            if (edge.Source == source && edge.Destination == destination)
            {
                return true;
            }
            if (!isDirected && edge.Source == destination && edge.Destination == source)
            {
                return true;
            }
            return false;
        }

        private void EnsureVertexInRange(int vertex)
        {
            if (vertex < 0 || vertex >= vertexCount)
            {
                throw new ArgumentOutOfRangeException(nameof(vertex), $"Vertex {vertex} is out of range [0, {vertexCount - 1}].");
            }
        }
    }
}
