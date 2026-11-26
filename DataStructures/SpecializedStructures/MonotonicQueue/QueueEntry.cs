namespace DataStructures.SpecializedStructures.MonotonicQueue
{
    /// <summary>
    /// One element in the monotonic queue — its position in the input and its value.
    /// The index is what makes expiry possible: an entry leaves the front once its index
    /// falls outside the current window.
    /// </summary>
    public readonly record struct QueueEntry(int Index, int Value)
    {
        public override string ToString() => $"{Value} (i={Index})";
    }
}
