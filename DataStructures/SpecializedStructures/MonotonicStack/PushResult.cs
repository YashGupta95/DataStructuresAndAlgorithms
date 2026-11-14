namespace DataStructures.SpecializedStructures.MonotonicStack
{
    /// <summary>
    /// Everything a single <see cref="MonotonicStackOperations.Push"/> reveals — both halves of
    /// the "one push answers two questions" insight.
    ///
    /// <para>
    /// <b>Popped</b> — the entries evicted by the incoming element, in the order they were
    /// popped (old top first). The incoming element is the NEXT greater/smaller of each of them.
    /// </para>
    ///
    /// <para>
    /// <b>Previous</b> — the entry left on top after the evictions, i.e. the one directly below
    /// the newly pushed element. It is the incoming element's PREVIOUS greater/smaller, or
    /// <c>null</c> if the stack emptied out (no such element exists).
    /// </para>
    /// </summary>
    public readonly record struct PushResult(IReadOnlyList<StackEntry> Popped, StackEntry? Previous);
}
