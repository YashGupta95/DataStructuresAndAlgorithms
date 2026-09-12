namespace DataStructures.GraphStructures.Graph
{
    // A Graph is a pair (V, E) where V is a set of VERTICES and E is a set of EDGES connecting
    // pairs of vertices. That two-line definition covers every graph flavor a senior engineer
    // will see in practice — road networks, dependency graphs, social networks, state machines,
    // build systems, neural network layers, the DOM. What changes between flavors is only the
    // structure of the pair:
    //
    //   • DIRECTED vs UNDIRECTED — is an edge (u, v) the same as (v, u)? For a road with
    //     one-way streets the answer is NO; for a friendship graph the answer is YES.
    //   • WEIGHTED vs UNWEIGHTED — do edges carry a cost (distance, latency, capacity), or is
    //     the graph purely relational?
    //
    // This project ships two independent representations that support all four combinations.
    // This file is the ADJACENCY MATRIX; the sibling file GraphAdjacencyList.cs is the ADJACENCY LIST. 
    // Both expose the same public surface so learners can diff them side-by-side and internalize the trade-off.
    //
    // -----------------------------------------------------------------------
    // The Adjacency Matrix Idea
    // -----------------------------------------------------------------------
    // Lay out a V × V grid where the cell at row u, column v answers the question:
    // "is there an edge from u to v, and if so, what's its weight?"
    //
    //   Undirected, unweighted graph over V = 5:
    //
    //                       col 0  col 1  col 2  col 3  col 4
    //                     ┌───────────────────────────────────┐
    //             row 0   │   .      1      1      .      .   │
    //             row 1   │   1      .      1      1      .   │
    //             row 2   │   1      1      .      .      .   │
    //             row 3   │   .      1      .      .      1   │
    //             row 4   │   .      .      .      1      .   │
    //                     └───────────────────────────────────┘
    //
    //     Edges present: {0–1, 0–2, 1–2, 1–3, 3–4}. Notice the matrix is SYMMETRIC across the
    //     diagonal — a defining property of undirected graphs stored this way. Half the cells
    //     duplicate the other half, which is the first hint that this representation isn't
    //     free.
    //
    // -----------------------------------------------------------------------
    // What Lives in a Cell
    // -----------------------------------------------------------------------
    // A cell must distinguish three states:
    //
    //   1. "No edge here."
    //   2. "Edge with weight w" for some caller-supplied w (which may be 0 or negative).
    //   3. "This vertex slot has been removed" — handled by a separate `vertexPresent[]`
    //      bit-map, not by the matrix itself.
    //
    // A common beginner bug is to use 0 for "no edge", which quietly aliases with a legitimate
    // zero-weight edge. We sidestep that by picking a SENTINEL value — <see cref="NoEdge"/> —
    // set to <c>int.MinValue</c>. Chosen because:
    //   • It is unambiguously outside any weight range a real problem would use.
    //   • Comparisons against it are a single instruction.
    //   • It survives being copied by Clone() with no special handling.
    //
    // -----------------------------------------------------------------------
    // Vertex Lifetime — vertexPresent[]
    // -----------------------------------------------------------------------
    // Vertices are labelled from 0 to (Capacity - 1). Initially every label is PRESENT. RemoveVertex(v)
    // marks v as absent (via `vertexPresent[v] = false`) and clears row v and column v of the
    // matrix so no residual edges survive. The slot is NEVER reused — AddVertex() grows the
    // matrix instead. Reusing vertex labels would silently resurrect deleted edges in any
    // caller code that cached a label, and is the source of a whole category of graph bugs.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation                       Time             Space
    //   Constructor(n)                  O(n²)            O(n²) for the matrix + O(n) for the presence map
    //   AddVertex                       O(n²)            grows the matrix (allocate + copy)
    //   RemoveVertex                    O(n)             clears row + column
    //   AddEdge / RemoveEdge            O(1)             one cell write
    //   HasEdge / GetWeight             O(1)             one cell read
    //   Neighbors(u) / OutDegree(u)     O(n)             one row scan
    //   InDegree(u)                     O(n)             one column scan
    //   EdgeCount                       O(n²)            full scan (cached would defeat the pedagogy)
    //   GetAllEdges                     O(n²)            full scan
    //   Clone                           O(n²)            deep copy of the matrix + presence map
    //
    // -----------------------------------------------------------------------
    // When to Reach for This Class
    // -----------------------------------------------------------------------
    // Use the adjacency matrix when
    //   • E is close to V² (a DENSE graph — think a fully-connected graph, or a distance matrix
    //     between every pair of cities in a state), OR
    //   • the workload is DOMINATED by O(1) edge-existence queries ("does u know v?"), OR
    //   • the algorithm is intrinsically matrix-shaped — Floyd–Warshall all-pairs shortest
    //     paths, matrix-exponentiation reachability, PageRank iterations.
    //
    // Prefer the sibling <see cref="GraphAdjacencyList"/> when V is large but E is small
    // (a SPARSE graph, like a social network where each user knows a few hundred others out
    // of a billion). The V² memory cost is the killer; a graph of a million sparse vertices
    // needs a terabyte of matrix but only megabytes of adjacency lists.
    //
    // See section 9 of Program.cs for the head-to-head decision table.
    // ============================================================================================
    public class GraphAdjacencyMatrix
    {
        /// <summary>
        /// Sentinel value stored in <see cref="matrix"/> to mean "no edge here". Chosen well
        /// outside any realistic weight range so a legitimate zero- or negative-weight edge
        /// cannot be confused with absence.
        /// </summary>
        public const int NoEdge = int.MinValue;

        private int[,] matrix;
        private bool[] vertexPresent;
        private int capacity;
        private readonly bool isDirected;
        private readonly bool isWeighted;

        /// <summary>
        /// Builds a graph with <paramref name="initialVertexCount"/> vertices, all initially
        /// present and labelled <c>0 .. initialVertexCount - 1</c>. No edges are added.
        /// </summary>
        /// <param name="initialVertexCount"> The initial vertex count. Must be non-negative. </param>
        /// <param name="isDirected"> <c>true</c> for a directed graph, <c>false</c> for undirected. </param>
        /// <param name="isWeighted"> <c>true</c> to accept caller-supplied weights, <c>false</c> to force weight = 1. </param>
        public GraphAdjacencyMatrix(int initialVertexCount, bool isDirected = false, bool isWeighted = false)
        {
            if (initialVertexCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount), $"Vertex count must be non-negative; got {initialVertexCount}.");
            }

            capacity = initialVertexCount;
            this.isDirected = isDirected;
            this.isWeighted = isWeighted;
            matrix = new int[capacity, capacity];
            vertexPresent = new bool[capacity];

            for (var i = 0; i < capacity; i++)
            {
                vertexPresent[i] = true;
                for (var j = 0; j < capacity; j++)
                {
                    matrix[i, j] = NoEdge;
                }
            }
        }

        /// <summary> <c>true</c> if this is a directed graph. </summary>
        public bool IsDirected => isDirected;

        /// <summary> <c>true</c> if this graph carries caller-supplied edge weights. </summary>
        public bool IsWeighted => isWeighted;

        /// <summary>
        /// The size of the underlying V × V matrix. Grows with <see cref="AddVertex"/>; removed
        /// vertices still count toward capacity because their slots are never reused.
        /// </summary>
        public int Capacity => capacity;

        /// <summary>
        /// Number of PRESENT vertices. Diverges from <see cref="Capacity"/> once
        /// <see cref="RemoveVertex"/> has been called.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) </para>
        /// </summary>
        public int VertexCount
        {
            get
            {
                var count = 0;
                for (var i = 0; i < capacity; i++)
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
        /// Number of edges. For an UNDIRECTED graph each edge is counted once; for a DIRECTED
        /// graph each direction is a separate edge.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V²) — the matrix has no cheaper way to answer this without a counter that
        /// would obscure the representation for teaching purposes. </para>
        /// </summary>
        public int EdgeCount
        {
            get
            {
                var count = 0;
                for (var u = 0; u < capacity; u++) // capacity is number of vertices
                {
                    if (!vertexPresent[u])
                    {
                        continue;
                    }
                    var start = isDirected ? 0 : u; // if directed, start from 0; if undirected, start from u to avoid double counting
                    for (var v = start; v < capacity; v++)
                    {
                        if (vertexPresent[v] && matrix[u, v] != NoEdge)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Adds a new vertex to the graph and returns its label.
        ///
        /// <para>
        /// Grows the underlying V × V matrix to (V+1) × (V+1) and copies existing edges over an O(V²) operation. 
        /// This is one of the reasons the adjacency matrix is a poor fit for graphs whose vertex set grows dynamically; the adjacency list handles the same
        /// operation in amortized O(1).
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V²) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(V²) </para>
        /// </summary>
        /// <returns> The label of the newly added vertex. </returns>
        public int AddVertex()
        {
            var newCapacity = capacity + 1;
            var newMatrix = new int[newCapacity, newCapacity];
            var newPresent = new bool[newCapacity];

            for (var i = 0; i < newCapacity; i++)
            {
                for (var j = 0; j < newCapacity; j++)
                {
                    newMatrix[i, j] = NoEdge; // initialize all entries to NoEdge
                }
            }
            for (var i = 0; i < capacity; i++)
            {
                newPresent[i] = vertexPresent[i]; // copy the presence status of existing vertices
                for (var j = 0; j < capacity; j++)
                {
                    newMatrix[i, j] = matrix[i, j]; // copy existing edges to the new matrix
                }
            }

            var newLabel = capacity;
            newPresent[newLabel] = true;

            matrix = newMatrix;
            vertexPresent = newPresent;
            capacity = newCapacity;
            return newLabel;
        }

        /// <summary>
        /// Removes a vertex and all its incident edges. The label is not reused — a subsequent
        /// <see cref="AddVertex"/> will allocate a fresh label. This keeps cached vertex labels
        /// safe from silent resurrection of deleted edges.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) — one row clear + one column clear. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="vertex"> The vertex to remove. </param>
        public void RemoveVertex(int vertex)
        {
            EnsurePresent(vertex);

            for (var j = 0; j < capacity; j++)
            {
                matrix[vertex, j] = NoEdge;
                matrix[j, vertex] = NoEdge;
            }
            vertexPresent[vertex] = false;
        }

        /// <summary>
        /// Adds (or replaces) the edge from <paramref name="source"/> to <paramref name="destination"/>.
        ///
        /// <para>
        /// Simple-graph semantics: at most one edge exists between any given pair of vertices.
        /// Calling AddEdge on an already-existing edge overwrites its weight. 
        /// For parallel edges use a Multigraph (future project).
        /// </para>
        ///
        /// <para>
        /// For an UNDIRECTED graph the mirrored cell (destination, source) is also set — this is why
        /// undirected adjacency matrices are always symmetric across the diagonal.
        /// </para>
        ///
        /// <para>
        /// If the graph is UNWEIGHTED, <paramref name="weight"/> is ignored and the stored weight is forced to 1.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public void AddEdge(int source, int destination, int weight = 1)
        {
            EnsurePresent(source);
            EnsurePresent(destination);
            if (weight == NoEdge)
            {
                throw new ArgumentException($"Weight cannot equal the NoEdge sentinel ({NoEdge}).", nameof(weight));
            }

            var effectiveWeight = isWeighted ? weight : 1;
            matrix[source, destination] = effectiveWeight;
            if (!isDirected)
            {
                matrix[destination, source] = effectiveWeight;
            }
        }

        /// <summary>
        /// Removes the edge from <paramref name="source"/> to <paramref name="destination"/>.
        /// No-op if the edge is absent. For undirected graphs the mirrored cell is also cleared.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public void RemoveEdge(int source, int destination)
        {
            EnsurePresent(source);
            EnsurePresent(destination);

            matrix[source, destination] = NoEdge;
            if (!isDirected)
            {
                matrix[destination, source] = NoEdge;
            }
        }

        /// <summary>
        /// Returns <c>true</c> iff an edge exists from <paramref name="source"/> to
        /// <paramref name="destination"/>. This O(1) check is the adjacency matrix's headline
        /// strength.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public bool HasEdge(int source, int destination)
        {
            EnsurePresent(source);
            EnsurePresent(destination);
            return matrix[source, destination] != NoEdge;
        }

        /// <summary>
        /// Returns the weight of the edge from <paramref name="source"/> to
        /// <paramref name="destination"/>. Throws if no such edge exists.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public int GetWeight(int source, int destination)
        {
            if (!HasEdge(source, destination))
            {
                throw new InvalidOperationException($"No edge present from {source} to {destination}.");
            }
            return matrix[source, destination];
        }

        /// <summary>
        /// Enumerates every vertex <c>v</c> such that an edge exists from <paramref name="vertex"/> to <c>v</c>. Order is ascending by destination label.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) — one full row scan regardless of how few neighbors <paramref name="vertex"/>
        /// actually has. This constant O(V) cost per query is the adjacency matrix's cost story on sparse graphs. </para>
        /// </summary>
        public IEnumerable<int> Neighbors(int vertex)
        {
            EnsurePresent(vertex);
            for (var v = 0; v < capacity; v++)
            {
                if (vertexPresent[v] && matrix[vertex, v] != NoEdge)
                {
                    yield return v;
                }
            }
        }

        /// <summary>
        /// Number of edges leaving <paramref name="vertex"/>. For undirected graphs this equals
        /// the vertex's degree.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) </para>
        /// </summary>
        public int OutDegree(int vertex)
        {
            EnsurePresent(vertex);
            var count = 0;
            for (var v = 0; v < capacity; v++)
            {
                if (vertexPresent[v] && matrix[vertex, v] != NoEdge)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Number of edges arriving at <paramref name="vertex"/>. For undirected graphs this
        /// equals <see cref="OutDegree"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V) — one column scan. Note the pleasing symmetry: OutDegree scans a row,
        /// InDegree scans a column. In the sibling adjacency-list class InDegree is instead
        /// O(V + E), because scanning "who points at me" requires visiting every bucket. </para>
        /// </summary>
        public int InDegree(int vertex)
        {
            EnsurePresent(vertex);
            var count = 0;
            for (var u = 0; u < capacity; u++)
            {
                if (vertexPresent[u] && matrix[u, vertex] != NoEdge)
                {
                    count++;
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
        /// <para>
        /// This is the canonical feed for algorithms that consume edges in bulk — Kruskal's MST, Bellman-Ford, weight histograms.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V²) — the matrix has no cheaper enumeration; on a sparse graph the sibling
        /// adjacency-list version answers this in O(V + E). </para>
        /// </summary>
        public IEnumerable<(int Source, int Destination, int Weight)> GetAllEdges()
        {
            for (var u = 0; u < capacity; u++)
            {
                if (!vertexPresent[u])
                {
                    continue;
                }
                var start = isDirected ? 0 : u;
                for (var v = start; v < capacity; v++)
                {
                    if (vertexPresent[v] && matrix[u, v] != NoEdge)
                    {
                        yield return (u, v, matrix[u, v]);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an independent deep copy of the graph. Mutating the clone does not affect the original — the underlying <c>int[,]</c> and <c>bool[]</c> are freshly allocated.
        ///
        /// <para>
        /// A classic use-case is any algorithm that needs to try edits speculatively (residual graphs in max-flow, backtracking search, randomized experiments) without touching the input.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V²) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(V²) </para>
        /// </summary>
        public GraphAdjacencyMatrix Clone()
        {
            var clone = new GraphAdjacencyMatrix(capacity, isDirected, isWeighted);
            for (var i = 0; i < capacity; i++)
            {
                clone.vertexPresent[i] = vertexPresent[i];
                for (var j = 0; j < capacity; j++)
                {
                    clone.matrix[i, j] = matrix[i, j];
                }
            }
            return clone;
        }

        /// <summary>
        /// Removes every edge and marks every vertex as absent. The capacity is preserved so
        /// the storage cost stays the same — this is a logical reset, not a shrink.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V²) </para>
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < capacity; i++)
            {
                vertexPresent[i] = false;
                for (var j = 0; j < capacity; j++)
                {
                    matrix[i, j] = NoEdge;
                }
            }
        }

        /// <summary>
        /// Prints the adjacency matrix with header rows/columns and a <c>.</c> in place of the
        /// <see cref="NoEdge"/> sentinel. Absent vertex slots are marked with <c>x</c> in the
        /// header so the presence map is visible at a glance.
        /// </summary>
        public void PrintMatrix()
        {
            if (capacity == 0)
            {
                Console.WriteLine("  (empty graph)");
                return;
            }

            var maxWeightWidth = 1;
            for (var i = 0; i < capacity; i++)
            {
                for (var j = 0; j < capacity; j++)
                {
                    if (matrix[i, j] != NoEdge)
                    {
                        maxWeightWidth = Math.Max(maxWeightWidth, matrix[i, j].ToString().Length);
                    }
                }
            }
            var labelWidth = (capacity - 1).ToString().Length;
            var cellWidth = Math.Max(maxWeightWidth, labelWidth) + 2;

            Console.Write(new string(' ', labelWidth + 3));
            for (var j = 0; j < capacity; j++)
            {
                var header = vertexPresent[j] ? j.ToString() : $"{j}x";
                Console.Write(Center(header, cellWidth));
            }
            Console.WriteLine();

            Console.Write(new string(' ', labelWidth + 2) + "┌");
            Console.Write(new string('─', cellWidth * capacity));
            Console.WriteLine("┐");

            for (var i = 0; i < capacity; i++)
            {
                var rowLabel = vertexPresent[i] ? i.ToString() : $"{i}x";
                Console.Write(rowLabel.PadLeft(labelWidth + 1) + " │");
                for (var j = 0; j < capacity; j++)
                {
                    string cell;
                    if (!vertexPresent[i] || !vertexPresent[j])
                    {
                        cell = "-";
                    }
                    else if (matrix[i, j] == NoEdge)
                    {
                        cell = ".";
                    }
                    else
                    {
                        cell = matrix[i, j].ToString();
                    }
                    Console.Write(Center(cell, cellWidth));
                }
                Console.WriteLine("│");
            }

            Console.Write(new string(' ', labelWidth + 2) + "└");
            Console.Write(new string('─', cellWidth * capacity));
            Console.WriteLine("┘");

            Console.WriteLine(
                $"  legend: `.` no edge  |  `-` absent-vertex row/col  |  header `<n>x` marks a removed vertex");
        }

        /// <summary>
        /// Throws if <paramref name="vertex"/> is out of range or has been removed. Every
        /// public method that touches a vertex goes through this — a single choke-point makes
        /// the "vertex-labels-are-stable" invariant impossible to violate accidentally.
        /// </summary>
        private void EnsurePresent(int vertex)
        {
            if (vertex < 0 || vertex >= capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(vertex),
                    $"Vertex {vertex} is out of range [0, {capacity - 1}].");
            }
            if (!vertexPresent[vertex])
            {
                throw new InvalidOperationException(
                    $"Vertex {vertex} has been removed and cannot be operated on.");
            }
        }

        private static string Center(string text, int width)
        {
            if (text.Length >= width)
            {
                return text;
            }
            var totalPad = width - text.Length;
            var leftPad = totalPad / 2;
            var rightPad = totalPad - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }
    }
}
