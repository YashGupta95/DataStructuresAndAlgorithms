namespace DataStructures.GraphStructures.Graph
{
    // The ADJACENCY LIST representation stores, for each vertex, the list of its outgoing edges. 
    // It is the workhorse representation for real-world graphs — social networks, road networks, 
    // package dependencies — because those graphs are almost always SPARSE (E is far less than V²), 
    // and the list version's memory cost is O(V + E) instead of O(V²).
    //
    // The public surface is IDENTICAL to <see cref="GraphAdjacencyMatrix"/> so learners can compare implementations method-by-method. What changes is where the cost lives:
    //
    //   • O(1) edge-existence queries become O(deg(u)) here.
    //   • O(V²) all-edge enumeration becomes O(V + E) here.
    //   • O(V²) space becomes O(V + E) here.
    //   • InDegree in a DIRECTED graph flips from O(V) (column scan on the matrix) to O(V + E) here (scan every bucket) — a real gotcha to flag in interviews.
    //
    // -----------------------------------------------------------------------
    // The Storage Layout
    // -----------------------------------------------------------------------
    //   adjacency[0] = [ Edge(1, w), Edge(2, w), ... ]     // outgoing edges of vertex 0
    //   adjacency[1] = [ Edge(0, w), Edge(2, w), ... ]     // outgoing edges of vertex 1
    //   ...
    //
    // For the demo graph {0–1, 0–2, 1–2, 1–3, 3–4} (undirected, unweighted):
    //
    //   0: [ 1, 2 ]
    //   1: [ 0, 2, 3 ]
    //   2: [ 0, 1 ]
    //   3: [ 1, 4 ]
    //   4: [ 3 ]
    //
    // Each undirected edge appears twice — once in each endpoint's bucket. That's the list-side echo of the matrix's symmetry across the diagonal.
    //
    // -----------------------------------------------------------------------
    // Vertex Lifetime — vertexPresent[]
    // -----------------------------------------------------------------------
    // Same rule as the matrix: vertex labels are stable. Removing a vertex clears its bucket
    // AND filters every OTHER bucket to drop edges pointing at it — an O(V + E) sweep in the worst case
    // AddVertex() appends to the outer list in amortized O(1). The label is never reused.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation                       Time             Space
    //   Constructor(n)                  O(n)             O(n) — n empty buckets
    //   AddVertex                       O(1) amortized   grows the outer list by one
    //   RemoveVertex                    O(V + E)         drop bucket + scrub incoming edges
    //   AddEdge / RemoveEdge            O(deg(u))        bucket search
    //   HasEdge / GetWeight             O(deg(u))        bucket search
    //   Neighbors(u) / OutDegree(u)     O(deg(u))        one bucket walk
    //   InDegree(u) — undirected        O(deg(u))        (symmetry — same as OutDegree)
    //   InDegree(u) — directed          O(V + E)         must scan every bucket
    //   EdgeCount                       O(V)             sum of bucket sizes
    //   GetAllEdges                     O(V + E)
    //   Clone                           O(V + E)         deep copy each bucket
    //
    // -----------------------------------------------------------------------
    // When to Reach for This Class
    // -----------------------------------------------------------------------
    // Prefer the adjacency list whenever
    //   • the graph is SPARSE (E ≪ V²), which is nearly every real-world graph,
    //   • the vertex set grows dynamically (AddVertex is amortized O(1) here vs O(V²) on the matrix),
    //   • the algorithm iterates NEIGHBORS rather than probing arbitrary pairs (BFS, DFS, Dijkstra, Prim, every classic graph algorithm).
    //
    // Reach for the sibling <see cref="GraphAdjacencyMatrix"/> only when the graph is DENSE or the algorithm is intrinsically matrix-shaped (Floyd–Warshall, PageRank).
    //
    // See section 9 of Program.cs for the head-to-head decision table.
    // ============================================================================================
    public class GraphAdjacencyList
    {
        private readonly List<List<Edge>> adjacency;
        private readonly List<bool> vertexPresent;
        private readonly bool isDirected;
        private readonly bool isWeighted;

        /// <summary>
        /// Builds a graph with <paramref name="initialVertexCount"/> vertices, all initially
        /// present and labelled <c>0 .. initialVertexCount - 1</c>. No edges are added.
        /// </summary>
        /// <param name="initialVertexCount"> The initial vertex count. Must be non-negative. </param>
        /// <param name="isDirected"> <c>true</c> for a directed graph, <c>false</c> for undirected. </param>
        /// <param name="isWeighted"> <c>true</c> to accept caller-supplied weights, <c>false</c> to force weight = 1. </param>
        public GraphAdjacencyList(int initialVertexCount, bool isDirected = false, bool isWeighted = false)
        {
            if (initialVertexCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount), $"Vertex count must be non-negative; got {initialVertexCount}.");
            }

            this.isDirected = isDirected;
            this.isWeighted = isWeighted;
            adjacency = new List<List<Edge>>(initialVertexCount);
            vertexPresent = new List<bool>(initialVertexCount);

            for (var i = 0; i < initialVertexCount; i++)
            {
                adjacency.Add(new List<Edge>());
                vertexPresent.Add(true);
            }
        }

        /// <summary> <c>true</c> if this is a directed graph. </summary>
        public bool IsDirected => isDirected;

        /// <summary> <c>true</c> if this graph carries caller-supplied edge weights. </summary>
        public bool IsWeighted => isWeighted;

        /// <summary>
        /// Total number of vertex slots ever allocated — includes removed vertices, whose
        /// labels are never reused.
        /// </summary>
        public int Capacity => adjacency.Count;

        /// <summary>
        /// Number of PRESENT vertices.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) </para>
        /// </summary>
        public int VertexCount
        {
            get
            {
                var count = 0;
                for (var i = 0; i < vertexPresent.Count; i++)
                {
                    if (vertexPresent[i])
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Number of edges. For an UNDIRECTED graph each edge is counted once (the sum of bucket sizes
        /// is divided by two); for a DIRECTED graph each direction is a separate edge.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) </para>
        /// </summary>
        public int EdgeCount
        {
            get
            {
                var total = 0;
                for (var u = 0; u < adjacency.Count; u++)
                {
                    if (vertexPresent[u])
                    {
                        total += adjacency[u].Count;
                    }
                }
                return isDirected ? total : total / 2;
            }
        }

        /// <summary>
        /// Adds a new vertex to the graph and returns its label.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) amortized — one append to the outer list. Contrast with the adjacency matrix, where the same operation is O(V²). </para>
        /// </summary>
        /// <returns> The label of the newly added vertex. </returns>
        public int AddVertex()
        {
            var newLabel = adjacency.Count;
            adjacency.Add(new List<Edge>());
            vertexPresent.Add(true);
            return newLabel;
        }

        /// <summary>
        /// Removes a vertex and all its incident edges. The label is not reused — a subsequent <see cref="AddVertex"/> will allocate a fresh label.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V + E) worst case — the bucket clear is O(deg(vertex)), but scrubbing incoming edges requires filtering every other bucket. </para>
        /// </summary>
        /// <param name="vertex"> The vertex to remove. </param>
        public void RemoveVertex(int vertex)
        {
            EnsurePresent(vertex);

            adjacency[vertex].Clear();
            for (var u = 0; u < adjacency.Count; u++)
            {
                if (u == vertex || !vertexPresent[u])
                {
                    continue;
                }
                adjacency[u].RemoveAll(edge => edge.Destination == vertex);
            }
            vertexPresent[vertex] = false;
        }

        /// <summary>
        /// Adds (or replaces) the edge from <paramref name="source"/> to <paramref name="destination"/>.
        ///
        /// <para>
        /// Simple-graph semantics: at most one edge exists between any given pair of vertices.
        /// Calling AddEdge on an already-existing edge overwrites its weight in place. For parallel edges use a Multigraph (future project).
        /// </para>
        ///
        /// <para>
        /// For an UNDIRECTED graph an edge is also appended to <c>destination</c>'s bucket — this is why every undirected edge lives in two places in this representation.
        /// </para>
        ///
        /// <para>
        /// If the graph is UNWEIGHTED, <paramref name="weight"/> is ignored and the stored weight is forced to 1.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(source)) — a bucket scan to detect an existing edge (and, for undirected, O(deg(destination)) on the mirrored side). </para>
        /// </summary>
        public void AddEdge(int source, int destination, int weight = 1)
        {
            EnsurePresent(source);
            EnsurePresent(destination);
            var effectiveWeight = isWeighted ? weight : 1;

            ReplaceOrAppend(source, destination, effectiveWeight);
            if (!isDirected)
            {
                ReplaceOrAppend(destination, source, effectiveWeight);
            }
        }

        /// <summary>
        /// Removes the edge from <paramref name="source"/> to <paramref name="destination"/>. No-op if the edge is absent.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(source)) — plus the mirrored side for undirected graphs. </para>
        /// </summary>
        public void RemoveEdge(int source, int destination)
        {
            EnsurePresent(source);
            EnsurePresent(destination);

            adjacency[source].RemoveAll(edge => edge.Destination == destination);
            if (!isDirected)
            {
                adjacency[destination].RemoveAll(edge => edge.Destination == source);
            }
        }

        /// <summary>
        /// Returns <c>true</c> iff an edge exists from <paramref name="source"/> to <paramref name="destination"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(source)) — a bucket scan. This is where the adjacency list pays for its space savings: the matrix answers the same query in O(1). </para>
        /// </summary>
        public bool HasEdge(int source, int destination)
        {
            EnsurePresent(source);
            EnsurePresent(destination);
            foreach (var edge in adjacency[source])
            {
                if (edge.Destination == destination)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the weight of the edge from <paramref name="source"/> to <paramref name="destination"/>. Throws if no such edge exists.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(source)) </para>
        /// </summary>
        public int GetWeight(int source, int destination)
        {
            EnsurePresent(source);
            EnsurePresent(destination);
            foreach (var edge in adjacency[source])
            {
                if (edge.Destination == destination)
                {
                    return edge.Weight;
                }
            }
            throw new InvalidOperationException($"No edge from {source} to {destination}.");
        }

        /// <summary>
        /// Enumerates every vertex <c>v</c> such that an edge exists from
        /// <paramref name="vertex"/> to <c>v</c>. Order matches insertion order into the bucket.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(vertex)) — a bucket walk. Compare with the matrix's O(V) row scan regardless of degree. </para>
        /// </summary>
        public IEnumerable<int> Neighbors(int vertex)
        {
            EnsurePresent(vertex);
            foreach (var edge in adjacency[vertex])
            {
                yield return edge.Destination;
            }
        }

        /// <summary>
        /// Number of edges leaving <paramref name="vertex"/>. For undirected graphs this equals the vertex's degree.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — the bucket's <c>Count</c> is precomputed. </para>
        /// </summary>
        public int OutDegree(int vertex)
        {
            EnsurePresent(vertex);
            return adjacency[vertex].Count;
        }

        /// <summary>
        /// Number of edges arriving at <paramref name="vertex"/>.
        ///
        /// <para>
        /// For UNDIRECTED graphs this returns <see cref="OutDegree"/> — the two are equal by symmetry.
        /// </para>
        ///
        /// <para>
        /// For DIRECTED graphs, however, the adjacency list stores only OUTGOING edges — there is no cheap "who points at me" pointer — so we must walk every bucket. 
        /// This is the single biggest asymmetry between the two representations. 
        /// Interview trap: when a senior candidate says "InDegree is O(1)", they're thinking of the matrix; for directed adjacency lists it's O(V + E).
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(deg(vertex)) — undirected. O(V + E) — directed. </para>
        /// </summary>
        public int InDegree(int vertex)
        {
            EnsurePresent(vertex);
            if (!isDirected)
            {
                return OutDegree(vertex);
            }

            var count = 0;
            for (var u = 0; u < adjacency.Count; u++)
            {
                if (!vertexPresent[u])
                {
                    continue;
                }
                foreach (var edge in adjacency[u])
                {
                    if (edge.Destination == vertex)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Enumerates every edge in the graph as a <c>(source, destination, weight)</c> triple.
        ///
        /// <para>
        /// UNDIRECTED graphs emit each edge ONCE (the pair with <c>source &lt;= destination</c>), so the enumeration matches <see cref="EdgeCount"/>. 
        /// DIRECTED graphs emit both (u, v) and (v, u) when both exist.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V + E). Contrast with the matrix's O(V²) enumeration — on a sparse graph this can be dramatically faster. </para>
        /// </summary>
        public IEnumerable<(int Source, int Destination, int Weight)> GetAllEdges()
        {
            for (var u = 0; u < adjacency.Count; u++)
            {
                if (!vertexPresent[u])
                {
                    continue;
                }
                foreach (var edge in adjacency[u])
                {
                    // For undirected graphs, only emit the edge if the source is less than or equal to the destination to avoid duplicates.
                    if (isDirected || u <= edge.Destination) 
                    {
                        yield return (u, edge.Destination, edge.Weight);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an independent deep copy of the graph. Each bucket is freshly allocated, so
        /// mutating the clone does not touch the original.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V + E) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(V + E) </para>
        /// </summary>
        public GraphAdjacencyList Clone()
        {
            var clone = new GraphAdjacencyList(0, isDirected, isWeighted);
            for (var i = 0; i < adjacency.Count; i++)
            {
                clone.adjacency.Add(new List<Edge>(adjacency[i]));
                clone.vertexPresent.Add(vertexPresent[i]);
            }
            return clone;
        }

        /// <summary>
        /// Removes every edge and marks every vertex as absent. Capacity is preserved (the
        /// outer list is not shrunk).
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V + E) </para>
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < adjacency.Count; i++)
            {
                adjacency[i].Clear();
                vertexPresent[i] = false;
            }
        }

        /// <summary>
        /// Prints each vertex's bucket, one per line. Weighted graphs display each neighbor as
        /// <c>dest(w=weight)</c>; unweighted graphs display just the neighbor label. Removed
        /// vertices are shown as <c>&lt;n&gt;x: (removed)</c>.
        /// </summary>
        public void PrintList()
        {
            if (adjacency.Count == 0)
            {
                Console.WriteLine("  (empty graph)");
                return;
            }

            var labelWidth = (adjacency.Count - 1).ToString().Length;
            for (var u = 0; u < adjacency.Count; u++)
            {
                if (!vertexPresent[u])
                {
                    Console.WriteLine($"  {u.ToString().PadLeft(labelWidth)}x: (removed)");
                    continue;
                }

                var bucket = adjacency[u];
                if (bucket.Count == 0)
                {
                    Console.WriteLine($"  {u.ToString().PadLeft(labelWidth)}: []");
                    continue;
                }

                var entries = new List<string>(bucket.Count);
                foreach (var edge in bucket)
                {
                    entries.Add(isWeighted
                        ? $"{edge.Destination}(w={edge.Weight})"
                        : edge.Destination.ToString());
                }
                Console.WriteLine($"  {u.ToString().PadLeft(labelWidth)}: [{string.Join(", ", entries)}]");
            }
        }

        /// <summary>
        /// Removes any existing edge to <paramref name="destination"/> from
        /// <paramref name="source"/>'s bucket, then appends the new one. This is what makes
        /// AddEdge idempotent (calling it twice with different weights leaves the second
        /// weight winning, without creating a parallel edge).
        /// </summary>
        private void ReplaceOrAppend(int source, int destination, int weight)
        {
            var bucket = adjacency[source];
            for (var i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Destination == destination)
                {
                    bucket[i] = new Edge(destination, weight);
                    return;
                }
            }
            bucket.Add(new Edge(destination, weight)); // Append the new edge if it wasn't found in the existing bucket.
        }

        /// <summary>
        /// Throws if <paramref name="vertex"/> is out of range or has been removed. Central
        /// choke-point for the "vertex-labels-are-stable" invariant.
        /// </summary>
        private void EnsurePresent(int vertex)
        {
            if (vertex < 0 || vertex >= adjacency.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(vertex), $"Vertex {vertex} is out of range [0, {adjacency.Count - 1}].");
            }
            if (!vertexPresent[vertex])
            {
                throw new InvalidOperationException($"Vertex {vertex} has been removed and cannot be operated on.");
            }
        }
    }
}
