namespace DataStructures.SpecializedStructures.MonotonicStack
{
    // A Monotonic Stack is an ordinary stack with one extra rule: the values on it are ALWAYS
    // sorted from bottom to top (increasing or decreasing). Before a new element is pushed,
    // every element on top that would break the order is popped off first.
    //
    // That one rule turns a whole family of O(n²) "look left / look right" problems into O(n):
    //
    //     • Next Greater Element         • Daily Temperatures
    //     • Previous Smaller Element     • Stock Span
    //     • Largest Rectangle in Histogram
    //
    // -----------------------------------------------------------------------
    // The Core Insight — One Push Answers Two Questions
    // -----------------------------------------------------------------------
    // Take a DECREASING stack (bottom → top) and push x:
    //
    //   1. Every element popped to make room for x was smaller than x, and x is the FIRST
    //      thing to its right that beat it — so x is each popped element's NEXT GREATER.
    //   2. The element left on top after the pops is larger than x, and it is the closest
    //      such element to x's left — so it is x's PREVIOUS GREATER.
    //
    // Mirror it for an INCREASING stack and you get NEXT SMALLER / PREVIOUS SMALLER. This
    // class surfaces both halves through <see cref="PushResult"/>.
    //
    //   Question you're asking               Stack order (bottom → top)
    //   ----------------------------------   --------------------------
    //   Next / previous GREATER element      Decreasing
    //   Next / previous SMALLER element      Increasing
    //
    // The mapping feels backwards at first. Remember it as: "the stack keeps the elements
    // still WAITING for an answer, and an increasing stack is waiting for something smaller".
    //
    // -----------------------------------------------------------------------
    // Worked Example — Decreasing Stack on [3, 1, 4, 1, 5]
    // -----------------------------------------------------------------------
    //   push 3   pops: —          stack: [3]
    //   push 1   pops: —          stack: [3, 1]
    //   push 4   pops: 1, 3       stack: [4]        → next greater of 1 and 3 is 4
    //   push 1   pops: —          stack: [4, 1]     → previous greater of 1 is 4
    //   push 5   pops: 1, 4       stack: [5]        → next greater of 1 and 4 is 5
    //
    // -----------------------------------------------------------------------
    // Why It's O(n) Even Though One Push Can Pop Many
    // -----------------------------------------------------------------------
    // A single push can pop the whole stack — O(n) in the worst case. But every element is
    // pushed exactly ONCE and can be popped at most ONCE, so across n pushes the total work
    // is at most 2n operations. Amortized O(1) per push, O(n) for the whole input. This is
    // the argument interviewers want to hear verbatim.
    //
    // -----------------------------------------------------------------------
    // Strict vs Non-Strict — Where Do Duplicates Go?
    // -----------------------------------------------------------------------
    // "Strict" means the stack values are STRICTLY monotonic, so an equal incoming value
    // evicts the top. Non-strict lets equal values sit on top of each other. The choice
    // decides which side of the answer gets the "or equal":
    //
    //   Order        Strict  Pop while      Popped element's NEXT     Remaining top is incoming's PREVIOUS
    //   ----------   ------  -------------  ------------------------  ------------------------------------
    //   Increasing   true    top >= x       smaller-or-equal          strictly smaller
    //   Increasing   false   top >  x       strictly smaller          smaller-or-equal
    //   Decreasing   true    top <= x       greater-or-equal          strictly greater
    //   Decreasing   false   top <  x       strictly greater          greater-or-equal
    //
    // Getting this wrong is the #1 source of duplicate-value bugs in monotonic-stack
    // solutions: "next greater" with a strict decreasing stack would wrongly report that
    // the next greater of 2 in [2, 2, 3] is 2.
    //
    // -----------------------------------------------------------------------
    // Teaching Class vs Production Code
    // -----------------------------------------------------------------------
    // <see cref="Push"/> allocates a fresh list of popped entries on every call — great for
    // making the mechanics visible, wasteful on a hot path. Real solutions inline the
    // "while (top violates) pop" loop directly into the problem, usually over a
    // <c>Stack&lt;int&gt;</c> of indices. The pattern is identical; only the packaging differs.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation      Time                         Space
    //   ---------      --------------------------   ----------------------
    //   Push           O(1) amortized, O(n) worst   O(k) for the popped list
    //   Pop / Peek     O(1)                         O(1)
    //   n pushes       O(n) total                   O(n)
    //
    // ============================================================================================
    public class MonotonicStackOperations
    {
        private readonly List<StackEntry> entries = new();
        private readonly MonotonicOrder order;
        private readonly bool strict;
        private int totalPushes;
        private int totalPops;

        /// <summary>
        /// Creates an empty monotonic stack.
        /// </summary>
        /// <param name="order"> Direction the values follow from bottom to top. </param>
        /// <param name="strict"> If <c>true</c>, values are strictly monotonic and an equal
        /// incoming value evicts the top. See the class header for how this moves the
        /// "or equal" between the NEXT and PREVIOUS answers. </param>
        public MonotonicStackOperations(MonotonicOrder order, bool strict = true)
        {
            if (!Enum.IsDefined(order))
            {
                throw new ArgumentOutOfRangeException(nameof(order), $"Unknown order: {order}.");
            }
            this.order = order;
            this.strict = strict;
        }

        /// <summary> Direction of the values from bottom to top. </summary>
        public MonotonicOrder Order => order;

        /// <summary> <c>true</c> iff equal values evict each other. </summary>
        public bool IsStrict => strict;

        /// <summary> Number of entries currently on the stack. </summary>
        public int Count => entries.Count;

        /// <summary> <c>true</c> iff the stack holds no entries. </summary>
        public bool IsEmpty => entries.Count == 0;

        /// <summary>
        /// Total pushes since construction or the last <see cref="Clear"/>. Exposed for
        /// teaching — lets the demo prove the "at most 2n operations" amortized bound.
        /// </summary>
        public int TotalPushes => totalPushes;

        /// <summary>
        /// Total pops (explicit or eviction) since construction or the last <see cref="Clear"/>.
        /// Can never exceed <see cref="TotalPushes"/>.
        /// </summary>
        public int TotalPops => totalPops;

        /// <summary>
        /// Evicts every entry that would break the monotonic order, then pushes
        /// <c>(index, value)</c>.
        ///
        /// <para>
        /// The returned <see cref="PushResult"/> carries the evicted entries (the incoming
        /// value is each one's NEXT greater/smaller) and the entry left directly below the
        /// new one (the incoming value's PREVIOUS greater/smaller).
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) amortized; O(n) worst case for a single push that empties the stack. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(k) for the list of k popped entries. </para>
        /// </summary>
        public PushResult Push(int index, int value)
        {
            var popped = new List<StackEntry>();
            // entries[^1].Value means the value of last element (current top entry) on the stack.
            while (entries.Count > 0 && ShouldPop(entries[^1].Value, value))
            {
                popped.Add(entries[^1]);
                entries.RemoveAt(entries.Count - 1);
            }

            StackEntry? previous = entries.Count > 0 ? entries[^1] : null;
            entries.Add(new StackEntry(index, value));

            totalPops += popped.Count;
            totalPushes++;
            return new PushResult(popped, previous);
        }

        /// <summary>
        /// Removes and returns the top entry. Removing from the top can never break the
        /// monotonic order, so this is always safe.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public StackEntry Pop()
        {
            EnsureNotEmpty();
            var top = entries[^1];
            entries.RemoveAt(entries.Count - 1);
            totalPops++;
            return top;
        }

        /// <summary>
        /// Returns the top entry without removing it.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public StackEntry Peek()
        {
            EnsureNotEmpty();
            return entries[^1];
        }

        /// <summary>
        /// Non-throwing <see cref="Peek"/>. Returns <c>false</c> on an empty stack.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public bool TryPeek(out StackEntry top)
        {
            if (entries.Count == 0)
            {
                top = default;
                return false;
            }
            top = entries[^1];
            return true;
        }

        /// <summary>
        /// Empties the stack and resets the push/pop counters.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — <see cref="List{T}.Clear"/> on a list of value types. </para>
        /// </summary>
        public void Clear()
        {
            entries.Clear();
            totalPushes = 0;
            totalPops = 0;
        }

        /// <summary>
        /// Snapshot of the entries ordered from BOTTOM to TOP.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public StackEntry[] ToArray() => entries.ToArray();

        /// <summary>
        /// Prints the stack on one line, bottom to top, as <c>value (i=index)</c> entries.
        /// </summary>
        public void Print()
        {
            var direction = order == MonotonicOrder.Increasing ? "increasing" : "decreasing";
            var mode = strict ? "strict" : "non-strict";
            var body = entries.Count == 0 ? "(empty)" : string.Join(", ", entries);
            Console.WriteLine($"  bottom [ {body} ] top   ({direction}, {mode})");
        }

        private bool ShouldPop(int top, int incoming) => order == MonotonicOrder.Increasing
            ? (strict ? top >= incoming : top > incoming)
            : (strict ? top <= incoming : top < incoming);

        private void EnsureNotEmpty()
        {
            if (entries.Count == 0)
            {
                throw new InvalidOperationException("The monotonic stack is empty.");
            }
        }
    }
}
