namespace DataStructures.SpecializedStructures.MonotonicQueue
{
    // A Monotonic Queue keeps its values sorted from FRONT to BACK, and elements leave it
    // from BOTH ends:
    //
    //   • BACK  — a new element evicts every element at the back that breaks the order
    //             (the same rule as the Monotonic Stack).
    //   • FRONT — elements whose index has slid out of the window expire.
    //
    // With a DECREASING queue the front is always the MAXIMUM of the current window; with an
    // INCREASING queue it is the MINIMUM. That turns "max/min of every window of size k"
    // from O(n·k) into O(n).
    //
    // -----------------------------------------------------------------------
    // Why Evicting From The Back Is Safe
    // -----------------------------------------------------------------------
    // In a max-queue, when a new value x arrives, any older value <= x can NEVER be a window
    // maximum again: x is at least as large AND stays in the window longer. So it is thrown
    // away immediately. What remains is the short list of values that could still become the
    // maximum later — and it is sorted, so the best one sits at the front.
    //
    // -----------------------------------------------------------------------
    // Worked Example — Max-Queue, Window Size 3, Input [1, 3, -1, -3, 5]
    // -----------------------------------------------------------------------
    //   i  value  evicted from back  expired from front  queue (front → back)  window max
    //   0    1    —                  —                   [1]                   —
    //   1    3    1                  —                   [3]                   —
    //   2   -1    —                  —                   [3, -1]               3
    //   3   -3    —                  —                   [3, -1, -3]           3
    //   4    5    -3, -1, 3          —                   [5]                   5
    //
    // -----------------------------------------------------------------------
    // Duplicates — Why There Is No "strict" Flag
    // -----------------------------------------------------------------------
    // Entries expire by INDEX, not by value, so equal values cause no bugs. This class evicts
    // the OLDER of two equal values: the newer one is just as good and lives longer.
    // (The variant that expires by VALUE — "pop the front if it equals the outgoing value" —
    // must keep equal values instead, or it would pop the wrong copy.)
    //
    // -----------------------------------------------------------------------
    // Cost
    // -----------------------------------------------------------------------
    // Every element is added once and removed at most once (from either end), so n pushes
    // cost at most 2n operations in total — amortized O(1) each.
    //
    // Storage is a LinkedList<QueueEntry> — O(1) add/remove at both ends and easy to read.
    // An array-backed ring buffer avoids the per-node allocation; see
    // Core/Queue/DequeUsingArray for that implementation.
    //
    //   Operation      Time
    //   ------------   --------------------------
    //   PushBack       O(1) amortized, O(k) worst
    //   ExpireBefore   O(1) amortized
    //   PeekFront      O(1)
    //   n pushes       O(n) total
    //
    // ============================================================================================
    public class MonotonicQueueOperations
    {
        private readonly LinkedList<QueueEntry> entries = new();
        private readonly MonotonicOrder order;
        private int totalPushes;
        private int totalPops;

        /// <param name="order"> Direction the values follow from front to back.
        /// <see cref="MonotonicOrder.Decreasing"/> gives a max-queue. </param>
        public MonotonicQueueOperations(MonotonicOrder order)
        {
            if (!Enum.IsDefined(order))
            {
                throw new ArgumentOutOfRangeException(nameof(order), $"Unknown order: {order}.");
            }
            this.order = order;
        }

        /// <summary> Direction of the values from front to back. </summary>
        public MonotonicOrder Order => order;

        /// <summary> Number of entries currently in the queue. </summary>
        public int Count => entries.Count;

        /// <summary> <c>true</c> iff the queue holds no entries. </summary>
        public bool IsEmpty => entries.Count == 0;

        /// <summary> Total pushes since construction or the last <see cref="Clear"/>. Exposed for teaching. </summary>
        public int TotalPushes => totalPushes;

        /// <summary> Total removals from either end since construction or the last <see cref="Clear"/>. </summary>
        public int TotalPops => totalPops;

        /// <summary>
        /// Evicts every entry at the back that breaks the order (including equal values), then
        /// adds <c>(index, value)</c> at the back. Returns the evicted entries, newest first.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) amortized. </para>
        /// </summary>
        public IReadOnlyList<QueueEntry> PushBack(int index, int value)
        {
            var evicted = new List<QueueEntry>();
            // We used last.Value.Value instead of last.Value because last.Value is a node of type QueueEntry, and we need the node's actual value to compare.
            while (entries.Last is { } last && ShouldEvict(last.Value.Value, value))
            {
                evicted.Add(last.Value);
                entries.RemoveLast();
            }
            entries.AddLast(new QueueEntry(index, value));

            totalPops += evicted.Count;
            totalPushes++;
            return evicted;
        }

        /// <summary>
        /// Removes entries from the front whose index is below <paramref name="windowStart"/> —
        /// the elements that have slid out of the window. Returns how many were removed.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) amortized. </para>
        /// </summary>
        public int ExpireBefore(int windowStart)
        {
            var removed = 0;
            while (entries.First is { } first && first.Value.Index < windowStart)
            {
                entries.RemoveFirst();
                removed++;
            }
            totalPops += removed;
            return removed;
        }

        /// <summary>
        /// Returns the front entry — the window's maximum (decreasing queue) or minimum
        /// (increasing queue).
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public QueueEntry PeekFront()
        {
            EnsureNotEmpty();
            return entries.First!.Value;
        }

        /// <summary> Non-throwing <see cref="PeekFront"/>. Returns <c>false</c> on an empty queue. </summary>
        public bool TryPeekFront(out QueueEntry front)
        {
            if (entries.First is { } first)
            {
                front = first.Value;
                return true;
            }
            front = default;
            return false;
        }

        /// <summary>
        /// Removes and returns the front entry.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public QueueEntry PopFront()
        {
            EnsureNotEmpty();
            var front = entries.First!.Value;
            entries.RemoveFirst();
            totalPops++;
            return front;
        }

        /// <summary> Empties the queue and resets the counters. </summary>
        public void Clear()
        {
            entries.Clear();
            totalPushes = 0;
            totalPops = 0;
        }

        /// <summary> Snapshot of the entries ordered from FRONT to BACK. </summary>
        public QueueEntry[] ToArray() => entries.ToArray();

        /// <summary> Prints the queue on one line, front to back. </summary>
        public void Print()
        {
            var role = order == MonotonicOrder.Decreasing ? "front = max" : "front = min";
            var body = entries.Count == 0 ? "(empty)" : string.Join(", ", entries);
            Console.WriteLine($"  front [ {body} ] back   ({role})");
        }

        // Equal values are evicted too: the newer copy is just as good and expires later.
        private bool ShouldEvict(int back, int incoming) => order == MonotonicOrder.Decreasing
            ? back <= incoming
            : back >= incoming;

        private void EnsureNotEmpty()
        {
            if (entries.Count == 0)
            {
                throw new InvalidOperationException("The monotonic queue is empty.");
            }
        }
    }
}
