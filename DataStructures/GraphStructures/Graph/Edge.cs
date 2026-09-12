namespace DataStructures.GraphStructures.Graph
{
    /// <summary>
    /// One outgoing half of a graph edge — the destination vertex and the weight travelling
    /// along it. Used exclusively by <see cref="GraphAdjacencyList"/>, whose per-vertex bucket
    /// is a <c>List&lt;Edge&gt;</c>. The origin vertex is implicit — it is the index of the
    /// bucket the Edge lives in.
    ///
    /// <para>
    /// The adjacency-matrix representation does not need this type: each matrix cell already
    /// stores the weight directly, and the (row, col) coordinates identify the endpoints.
    /// </para>
    ///
    /// <para>
    /// For UNWEIGHTED graphs the convention throughout this project is <c>Weight = 1</c>. Using
    /// 1 (rather than 0) means an unweighted shortest-path query degenerates neatly into
    /// "count the edges", which matches how algorithms in the future <c>Algorithms/Graph/</c>
    /// projects will consume this type.
    /// </para>
    /// </summary>
    public readonly record struct Edge(int Destination, int Weight);
}
