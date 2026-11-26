namespace DataStructures.SpecializedStructures.MonotonicStack
{
    /// <summary>
    /// One element on the monotonic stack — its position in the input AND its value.
    ///
    /// <para>
    /// Almost every monotonic-stack problem needs the position, not just the value: "how many
    /// days until a warmer temperature" is an index DISTANCE, and "write the answer for
    /// element i" needs to know i. Carrying both avoids the classic workaround of pushing
    /// bare indices and re-reading <c>arr[i]</c> in every comparison.
    /// </para>
    /// </summary>
    public readonly record struct StackEntry(int Index, int Value)
    {
        public override string ToString() => $"{Value} (i={Index})";
    }
}
