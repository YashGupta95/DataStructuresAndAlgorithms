namespace DataStructures.AdvancedTrees.FenwickTree
{
    // A Fenwick Tree (a.k.a. Binary Indexed Tree, "BIT") answers PREFIX-SUM queries on an array
    // and POINT UPDATES to it — both in O(log n) — using ~half the memory of a Segment Tree and
    // dramatically less code. It is the tool of choice when
    //   (a) the aggregation is INVERTIBLE (sum, xor, count), and
    //   (b) you only need range queries derivable as PrefixSum(r) - PrefixSum(l - 1).
    //
    // -----------------------------------------------------------------------
    // Segment Tree vs Fenwick Tree — When to Pick Which
    // -----------------------------------------------------------------------
    //   Segment Tree                                  Fenwick Tree (this class)
    //   ------------                                  -------------------------
    //   ~4n storage                                   n + 1 storage
    //   Works for ANY monoid                          Only INVERTIBLE aggregates
    //     (sum, min, max, gcd, matrix product, ...)     (sum, xor, count — NOT min / max)
    //   ~150 LOC for the core                         ~30 LOC for the core
    //   Recursive walk with 3-case dispatch           Two 5-line loops
    //                                                   (i += i & -i,  i -= i & -i)
    //   Naturally extends to Lazy Propagation         Range Update needs a companion class
    //     for range updates                             (see FenwickTreeRangeUpdate.cs)
    //
    // Rule of thumb: reach for the Segment Tree when the aggregate is min / max / gcd or when
    // you need Lazy Propagation. Reach for the Fenwick Tree when you're doing prefix sums,
    // order statistics, or inversion counts — its constant factor is unbeaten.
    //
    // -----------------------------------------------------------------------
    // The 2×2 Mental Model for Prefix-Sum-Style Problems
    // -----------------------------------------------------------------------
    // Any "update the array + ask for aggregates" problem sits in one of four cells:
    //
    //                    │ Point Query               │ Range Query
    //   ---------------- ┼-------------------------- ┼--------------------------------
    //   Point Update     │ trivial: read arr[i]      │ THIS CLASS  (PURQ Fenwick Tree)
    //                    |                           |
    //   Range Update     │ FenwickTreeRangeUpdate    │ two-BIT trick (RURQ; see TODO)
    //                    │ (RUPQ, sibling file)      │
    //
    // Each cell needs its own trick, and — crucially — bit[] means something DIFFERENT in
    // each row:
    //   • PURQ (this class)          bit[i] stores an AGGREGATE  (a sum over a range of arr[]).
    //   • RUPQ (FenwickTreeRangeUpdate)  bit[i] stores a DIFFERENCE  (d[i] = arr[i] - arr[i-1]).
    // Same machinery — one 1-indexed array, the same lowbit trick, the same two loops — but
    // a different INTERPRETATION of what's inside bit[]. That's why one class can only be
    // fast in one row of the table; if you want fast in BOTH rows, you keep two BITs (the
    // RURQ trick).
    //
    // Use this table as your first diagnostic when a problem lands on your desk: identify
    // which cell it lives in, then pick the matching class (or the RURQ pair).
    //
    // -----------------------------------------------------------------------
    // The Two Magic Loops
    // -----------------------------------------------------------------------
    // Everything the BIT does reduces to two loops that walk a 1-indexed array by repeatedly
    // adding or subtracting the LOWEST SET BIT (1) of the current index — that is,
    //
    //     lowbit(i) = i & -i     (the value of the least-significant 1-bit of i)
    //
    // For example, lowbit(12) = lowbit(0b1100) = 0b0100 = 4.
    //
    //   • Update loop  (PointAdd):    i += lowbit(i)   — climbs UP through ancestors.
    //   • Query  loop  (PrefixSum):   i -= lowbit(i)   — walks LEFT through peers.
    //
    // Both loops run for O(log n) iterations because each step either strips off one bit
    // position (query walk) or lifts to a strictly higher one (update walk).
    //
    // -----------------------------------------------------------------------
    // What Each bit[i] Actually Stores
    // -----------------------------------------------------------------------
    // Slot bit[i] stores the SUM of arr[i - lowbit(i) + 1 .. i] in 1-indexed space. The picture
    // below (n = 8) is the single most illuminating diagram in this file:
    //
    //   index i   binary   lowbit(i)   covers arr indices (1-indexed)
    //   -------   ------   ---------   -------------------------------
    //      1       0001         1       [1..1]        — width 1
    //      2       0010         2       [1..2]        — width 2
    //      3       0011         1       [3..3]        — width 1
    //      4       0100         4       [1..4]        — width 4
    //      5       0101         1       [5..5]        — width 1
    //      6       0110         2       [5..6]        — width 2
    //      7       0111         1       [7..7]        — width 1
    //      8       1000         8       [1..8]        — width 8
    //
    // Odd-index slots always cover width 1, powers of 2 cover the WHOLE prefix ending at
    // themselves, and everything in between covers a range whose width equals lowbit(i).
    //
    // -----------------------------------------------------------------------
    // Why We Store 1-Indexed Internally
    // -----------------------------------------------------------------------
    // lowbit(0) = 0, so the update loop `i += lowbit(i)` would spin forever on index 0. To
    // sidestep this, the tree array is 1-indexed. The public API stays 0-indexed to match C#
    // convention (and the SegmentTree class next door) — the constructor and every public
    // method shifts by one at the boundary and works internally in 1-indexed space.
    //
    // -----------------------------------------------------------------------
    // Invertibility (Why This Class Is Sum-Only)
    // -----------------------------------------------------------------------
    // RangeSum(l, r) is computed as PrefixSum(r) - PrefixSum(l - 1). That subtraction is what
    // limits the BIT to invertible aggregates:
    //
    //   • SUM   ✓  — subtract to remove a prefix's contribution.
    //   • XOR   ✓  — xor is its own inverse; the same class works with += replaced by ^=.
    //   • COUNT ✓  — a special case of SUM (each PointAdd is ±1).
    //   • MIN   ✗  — no inverse: given min(a, b) and b, you cannot recover min(a).
    //   • MAX   ✗  — same reason.
    //
    // For min / max range queries, reach for the Segment Tree.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation       Time        Space
    //   ---------       --------    ------
    //   Build           O(n)        O(n) for the bit array
    //   PrefixSum       O(log n)    O(1)
    //   RangeSum        O(log n)    O(1)     (two prefix-sum walks)
    //   PointAdd        O(log n)    O(1)
    //   PointSet        O(log n)    O(1)     (implemented as PointAdd of a computed delta)
    //   GetValue        O(log n)    O(1)     (implemented as RangeSum(i, i))
    //
    // -----------------------------------------------------------------------
    // TODO — Extensions considered future work:
    //   • XOR variant (single-line change: replace += with ^= in the two loops).
    //   • Range Update + Range Query using two BITs (the "RURQ" trick).
    //   • 2D Fenwick Tree.
    //   • Order statistics via FindKth (binary lifting on the tree).
    //
    public class FenwickTreeOperations
    {
        // 1-indexed storage. bit[0] is unused. Length = n + 1.
        private readonly long[] bit;
        private readonly int n;

        /// <summary>
        /// Builds a Fenwick Tree from a 0-indexed input array in O(n).
        ///
        /// <para>
        /// The naive build — n calls to <see cref="PointAdd"/> — runs in O(n log n). This
        /// constructor uses a well-known linear trick instead: copy each input value into its
        /// slot, then push its aggregated value up to its immediate parent slot
        /// (bit[i + lowbit(i)]). By the time the sweep finishes, every slot holds the correct
        /// partial sum.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — one pass over the array. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) for the bit array. </para>
        /// </summary>
        /// <param name="input"> The 0-indexed source array. </param>
        public FenwickTreeOperations(int[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            n = input.Length;
            bit = new long[n + 1];

            for (var i = 1; i <= n; i++)
            {
                bit[i] += input[i - 1];
                var parent = i + LowBit(i);
                if (parent <= n)
                {
                    bit[parent] += bit[i]; // Add the current value to its parent
                }
            }
        }

        /// <summary>
        /// The size of the underlying array — the number of elements this BIT indexes.
        /// </summary>
        public int Length => n;

        /// <summary>
        /// Returns the sum of arr[0..<paramref name="endIndex"/>] inclusive.
        ///
        /// <para>
        /// Convention: <c>PrefixSum(-1) == 0</c>, so the derivation
        /// <c>RangeSum(l, r) = PrefixSum(r) - PrefixSum(l - 1)</c> is well-defined even when
        /// <c>l == 0</c>.
        /// </para>
        ///
        /// <para>
        /// Walk: start at 1-indexed position <c>endIndex + 1</c>; repeatedly subtract lowbit,
        /// collecting the value at each visited slot. Each step jumps to an earlier slot whose
        /// covered range abuts the previous one, so no element is double-counted.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public long PrefixSum(int endIndex)
        {
            if (endIndex < -1 || endIndex >= n)
            {
                throw new ArgumentOutOfRangeException(nameof(endIndex),
                    $"endIndex must be in [-1, {n - 1}]; got {endIndex}.");
            }

            long sum = 0;
            for (var i = endIndex + 1; i > 0; i -= LowBit(i))
            {
                sum += bit[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of arr[<paramref name="left"/>..<paramref name="right"/>] inclusive.
        ///
        /// <para>
        /// Computed as <c>PrefixSum(right) - PrefixSum(left - 1)</c>. This subtraction is what
        /// limits the Fenwick Tree to INVERTIBLE aggregations — see the class comment.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public long RangeSum(int left, int right)
        {
            if (left < 0 || right >= n || left > right)
            {
                throw new ArgumentOutOfRangeException(
                    $"Invalid range [{left}..{right}] for size {n}.");
            }
            // (left - 1) because PrefixSum is defined as the sum from index 0 to the given endIndex.
            return PrefixSum(right) - PrefixSum(left - 1);
        }

        /// <summary>
        /// Adds <paramref name="delta"/> to arr[<paramref name="index"/>].
        ///
        /// <para>
        /// Walk: start at 1-indexed position <c>index + 1</c>; repeatedly add lowbit, updating
        /// every slot whose covered range includes this element. Symmetric mirror image of the
        /// prefix-sum walk.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public void PointAdd(int index, long delta)
        {
            if (index < 0 || index >= n)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            for (int i = index + 1; i <= n; i += LowBit(i))
            {
                bit[i] += delta;
            }
        }

        /// <summary>
        /// Overwrites arr[<paramref name="index"/>] with <paramref name="newValue"/>.
        ///
        /// <para>
        /// The BIT stores no explicit "current value" per index, so we derive it via
        /// <see cref="GetValue"/> and apply the delta. Notice how expressive the two primitives
        /// are — the whole operation is one line of real work.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public void PointSet(int index, long newValue)
        {
            long delta = newValue - GetValue(index);
            PointAdd(index, delta);
        }

        /// <summary>
        /// Returns arr[<paramref name="index"/>].
        ///
        /// <para>
        /// The array is IMPLICIT — nothing in the BIT stores individual elements directly.
        /// We recover arr[index] as <c>RangeSum(index, index)</c>.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public long GetValue(int index) => RangeSum(index, index);

        /// <summary>
        /// Prints a diagnostic table showing, for each 1-indexed slot i, its binary
        /// representation, its lowbit, the 0-indexed arr range it covers, and its stored value.
        /// This is the single most illuminating view of what a Fenwick Tree actually is —
        /// once <c>lowbit(i)</c> and "covered range" click, the two magic loops become obvious.
        /// </summary>
        public void PrintTable()
        {
            int binWidth = Math.Max(4, (int)Math.Ceiling(Math.Log2(n + 1)));
            int idxWidth = n.ToString().Length;

            Console.WriteLine(
                $"  {"i".PadLeft(idxWidth)}   {"binary".PadRight(binWidth)}   lowbit   covers arr[0-indexed]   bit[i]");
            Console.WriteLine(
                $"  {new string('-', idxWidth)}   {new string('-', binWidth)}   ------   ---------------------   ------");

            for (int i = 1; i <= n; i++)
            {
                int lb = LowBit(i);
                int startZero = i - lb;      // 0-indexed inclusive start (= (i - lb + 1) - 1)
                int endZero = i - 1;         // 0-indexed inclusive end   (= i - 1)
                string range = $"[{startZero}..{endZero}]";
                string binary = Convert.ToString(i, 2).PadLeft(binWidth, '0');
                Console.WriteLine(
                    $"  {i.ToString().PadLeft(idxWidth)}   {binary}   {lb,-6}   {range,-21}   {bit[i]}");
            }
        }

        /// <summary>
        /// Prints the BIT as a COVERAGE-BAR CHART aligned to the arr[] axis: every bit[i] slot
        /// is drawn as a horizontal bar spanning exactly the arr[] indices it aggregates. The
        /// header row shows each arr[] index in both DECIMAL and BINARY, and each bit-row label
        /// shows the slot index in both decimal and binary too — the binary makes the lowbit
        /// pattern jump off the page (e.g. bit[100] covers arr[000..011], bit[110] covers
        /// arr[100..101]).
        ///
        /// <para>
        /// The chart is faithful to what the BIT actually IS (a plain array of aggregates), and
        /// tracing an operation becomes visual:
        ///   <br/>• PointAdd(k, ...) — every bar that covers column k must update.
        ///   <br/>• PrefixSum(r)     — pick the smallest set of non-overlapping bars whose right
        ///     edges tile [0..r]; sum those bit[i] values.
        /// </para>
        /// </summary>
        public void PrintTreeVisual()
        {
            var bitIdxWidth = n.ToString().Length;
            var binWidth = Math.Max(3, (int)Math.Ceiling(Math.Log2(n + 1)));

            var maxArrValWidth = 1;
            for (var j = 0; j < n; j++)
            {
                maxArrValWidth = Math.Max(maxArrValWidth, GetValue(j).ToString().Length);
            }
            var maxBitValWidth = 1;
            for (var i = 1; i <= n; i++)
            {
                maxBitValWidth = Math.Max(maxBitValWidth, bit[i].ToString().Length);
            }

            // Odd column width lets 1-char and 3-char content (single digit, 3-bit binary) both
            // center perfectly under the same column midpoint.
            var arrColWidth = Math.Max(7, Math.Max(binWidth, maxArrValWidth) + 2);
            if (arrColWidth % 2 == 0)
            {
                arrColWidth++;
            }

            // Left label width — matched by both header labels ("arr index:", ...) and bit rows.
            var sampleLabel =
                $"  {new string(' ', bitIdxWidth)}   {new string(' ', binWidth)}   {new string(' ', maxBitValWidth)}   ";
            var leftWidth = sampleLabel.Length;

            Console.Write("arr index:".PadLeft(leftWidth));
            for (var j = 0; j < n; j++)
            {
                Console.Write(Center(j.ToString(), arrColWidth));
            }
            Console.WriteLine();

            Console.Write("(binary):".PadLeft(leftWidth));
            for (var j = 0; j < n; j++)
            {
                Console.Write(Center(Convert.ToString(j, 2).PadLeft(binWidth, '0'), arrColWidth));
            }
            Console.WriteLine();

            Console.Write("arr value:".PadLeft(leftWidth));
            for (var j = 0; j < n; j++)
            {
                Console.Write(Center(GetValue(j).ToString(), arrColWidth));
            }
            Console.WriteLine();

            Console.WriteLine();

            Console.WriteLine(
                $"  {"i".PadLeft(bitIdxWidth)}   {"bin".PadLeft(binWidth)}   {"bit[i]".PadLeft(maxBitValWidth)}");
            Console.WriteLine(
                $"  {new string('-', bitIdxWidth)}   {new string('-', binWidth)}   {new string('-', Math.Max(maxBitValWidth, 6))}");

            for (var i = 1; i <= n; i++)
            {
                var lb = LowBit(i);
                var s = i - lb;
                var e = i - 1;
                var barLen = (e - s + 1) * arrColWidth;
                var bar = barLen <= 1 ? "│" : "├" + new string('─', barLen - 2) + "┤";
                var spacer = new string(' ', s * arrColWidth);
                var label =
                    "  " + i.ToString().PadLeft(bitIdxWidth) + "   " + Convert.ToString(i, 2).PadLeft(binWidth, '0') + "   " + bit[i].ToString().PadLeft(maxBitValWidth) + "   ";
                Console.WriteLine(label + spacer + bar);
            }
        }

        /// <summary>
        /// Centers <paramref name="text"/> in a field of <paramref name="width"/> characters
        /// using left-heavy padding when odd space remains. Returns the text unchanged if it
        /// is already wider than the field.
        /// </summary>
        private static string Center(string text, int width)
        {
            if (text.Length >= width) return text;
            var totalPad = width - text.Length;
            var leftPad = totalPad / 2;
            var rightPad = totalPad - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }

        /// <summary>
        /// Returns the value of the least-significant 1-bit of <paramref name="i"/> — the
        /// engine that drives both magic loops. Undefined at i = 0 (returns 0), which is
        /// why the internal array is 1-indexed.
        /// </summary>
        private static int LowBit(int i) => i & -i;
    }
}
