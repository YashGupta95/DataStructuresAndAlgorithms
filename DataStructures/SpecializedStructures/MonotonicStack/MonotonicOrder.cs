namespace DataStructures.SpecializedStructures.MonotonicStack
{
    /// <summary>
    /// The direction the stack's values follow when read from BOTTOM to TOP.
    ///
    /// <para>
    /// The counter-intuitive mapping that trips people up:
    /// an <see cref="Increasing"/> stack answers "next / previous SMALLER" questions, and a
    /// <see cref="Decreasing"/> stack answers "next / previous GREATER" questions. The incoming
    /// element evicts everything that breaks the order — and the evicted elements are exactly
    /// the ones for which the incoming element is the answer.
    /// </para>
    /// </summary>
    public enum MonotonicOrder
    {
        Increasing,
        Decreasing,
    }
}
