namespace DataStructures.SpecializedStructures.MonotonicQueue
{
    /// <summary>
    /// The direction the queue's values follow when read from FRONT to BACK.
    ///
    /// <para>
    /// <see cref="Decreasing"/> keeps the largest value at the front, so the front is the
    /// current window's MAXIMUM. <see cref="Increasing"/> keeps the smallest value at the
    /// front, so the front is the window's MINIMUM.
    /// </para>
    /// </summary>
    public enum MonotonicOrder
    {
        Increasing,
        Decreasing,
    }
}
