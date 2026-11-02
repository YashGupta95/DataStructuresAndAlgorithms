namespace DataStructures.GraphStructures.EdgeList
{
    /// <summary>
    /// A single edge in the graph — both endpoints AND the weight, all in one struct.
    ///
    /// <para>
    /// Contrast with the sibling <c>Edge</c> in the <c>Graph</c> project, which stores only
    /// <c>(Destination, Weight)</c>: there, the source vertex is IMPLICIT in the index of the
    /// adjacency-list bucket the edge lives in. EdgeList has no per-vertex buckets, so the
    /// source has to travel with the edge itself. That single extra <c>int</c> per row is the
    /// whole reason EdgeList looks the way it does.
    /// </para>
    ///
    /// <para>
    /// For UNWEIGHTED graphs the convention across all GraphStructures projects is
    /// <c>Weight = 1</c>. This keeps unweighted shortest-path queries collapsing neatly into
    /// "count the edges", and lets weighted algorithms feed on unweighted graphs without a
    /// separate code path.
    /// </para>
    ///
    /// <para>
    /// The struct is <see cref="IComparable{Edge}"/> so that <see cref="List{T}.Sort()"/>
    /// on an <c>List&lt;Edge&gt;</c> uses ascending weight by default. This is the ordering
    /// that Kruskal's MST wants; other algorithms are free to pass a custom comparer.
    /// </para>
    /// </summary>
    public readonly record struct Edge(int Source, int Destination, int Weight) : IComparable<Edge>
    {
        public int CompareTo(Edge other)
        {
            var byWeight = Weight.CompareTo(other.Weight);
            if (byWeight != 0)
            {
                return byWeight;
            }
            var bySource = Source.CompareTo(other.Source);
            if (bySource != 0)
            {
                return bySource;
            }
            return Destination.CompareTo(other.Destination);
        }
    }
}
