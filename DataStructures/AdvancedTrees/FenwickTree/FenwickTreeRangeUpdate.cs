namespace DataStructures.AdvancedTrees.FenwickTree
{
    // A Fenwick Tree specialised for RANGE UPDATE + POINT QUERY (the "RUPQ" variant).
    //
    // -----------------------------------------------------------------------
    // What Changes Compared to the Core FenwickTreeOperations Class
    // -----------------------------------------------------------------------
    // The MACHINERY is identical — same 1-indexed array, same lowbit, same two loops. What
    // changes is what bit[] represents:
    //
    //   • Core class (FenwickTreeOperations)    bit[i] stores a SUM of a range of arr[].
    //                                           → fast PrefixSum, fast PointAdd.
    //
    //   • This class (FenwickTreeRangeUpdate)   bit[] represents a DIFFERENCE SEQUENCE d[],
    //                                           where d[i] = arr[i] - arr[i - 1].
    //                                           → fast RangeAdd, fast PointQuery.
    //
    // Under this interpretation:
    //
    //     arr[i] = d[0] + d[1] + ... + d[i] = PrefixSum(i) on the diff array.
    //
    // -----------------------------------------------------------------------
    // Why RangeAdd Collapses to Two Point-Adds
    // -----------------------------------------------------------------------
    // Adding <c>delta</c> to every element in arr[l..r] means:
    //
    //     • arr[l]     grows by delta   → d[l]     grows by delta      (was arr[l] - arr[l-1])
    //     • arr[l+1]   grows by delta   → d[l+1]   unchanged           (both sides grew equally)
    //     • ...        unchanged
    //     • arr[r]     grows by delta   → d[r]     unchanged
    //     • arr[r+1]   unchanged        → d[r+1]   drops by delta      (arr[r] grew, arr[r+1] didn't)
    //
    // So the ENTIRE range update touches exactly TWO positions of d[]: +delta at l, -delta at
    // r+1 (skipped if r+1 == n). Both fit into a single PointAdd on the diff BIT.
    //
    // -----------------------------------------------------------------------
    // Worked Example — RangeAdd(1, 4, +10) on [1, 3, 5, 7, 9, 11]
    // -----------------------------------------------------------------------
    //   Before:       arr = [ 1,  3,  5,  7,  9, 11]
    //   Diff before:  d   = [ 1,  2,  2,  2,  2,  2]     (d[i] = arr[i] - arr[i-1])
    //
    //   Two point-adds on d[]:
    //     +10 at index 1   — arr[1] jumps 3 → 13, so d[1] jumps 2 → 12.
    //     -10 at index 5   — arr[4] rose from 9 to 19 but arr[5] stays 11, so d[5] drops 2 → -8.
    //
    //   Diff after:   d   = [ 1, 12,  2,  2,  2, -8]
    //   After:        arr = [ 1, 13, 15, 17, 19, 11]     (arr[i] = d[0] + d[1] + ... + d[i])
    //
    // Notice d[2..4] were NEVER touched — they only encode DIFFERENCES between neighbours,
    // and every neighbour inside [1..4] rose by the same +10, so those differences didn't
    // change. That's the invariant that makes exactly two point-adds always sufficient.
    // (This is the same example the Program.cs demo runs in section 11 — cross-check the
    // reconstructed array there.)
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation       Time        Space
    //   ---------       --------    ------
    //   Build           O(n)        O(n) for the diff bit array
    //   RangeAdd        O(log n)    O(1)     (two point-adds on the diff BIT)
    //   PointQuery      O(log n)    O(1)     (one prefix-sum walk on the diff BIT)
    //
    // NOTE: RangeSum is NOT provided here. See the class comment in FenwickTreeOperations for
    // the full "which BIT does which" story; the short version is that PrefixSum on the diff
    // BIT gives you a single arr[] element, not a sum of them, so range-sum queries would fall
    // back to O(n). If you need both range-update AND range-sum in O(log n), you'll want the
    // two-BIT trick (deferred as future work).
    //
    public class FenwickTreeRangeUpdate
    {
        // 1-indexed BIT over the DIFFERENCE sequence d[i] = arr[i] - arr[i - 1].
        private readonly long[] bit;
        private readonly int n;

        /// <summary>
        /// Builds a Range-Update Fenwick Tree from a 0-indexed input array in O(n).
        ///
        /// <para>
        /// We first materialise the difference sequence <c>d[i] = arr[i] - arr[i - 1]</c>
        /// (with d[0] = arr[0]), then run the same in-place linear build used by the core
        /// class: seed each slot with d[i-1] and push its aggregated value up to its parent
        /// bit[i + lowbit(i)].
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) for the bit array. </para>
        /// </summary>
        public FenwickTreeRangeUpdate(int[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            n = input.Length;
            bit = new long[n + 1];

            for (var i = 1; i <= n; i++)
            {
                // Original array starts from 0 index, so diff[i] = arr[i] - arr[i - 1] with arr[-1] assumed as 0
                long diff = i == 1 ? input[0] : (long)input[i - 1] - input[i - 2];
                bit[i] += diff;
                var parent = i + LowBit(i);
                if (parent <= n)
                {
                    bit[parent] += bit[i];
                }
            }
        }

        /// <summary>
        /// The size of the underlying array.
        /// </summary>
        public int Length => n;

        /// <summary>
        /// Adds <paramref name="delta"/> to every element of
        /// arr[<paramref name="left"/>..<paramref name="right"/>] inclusive.
        ///
        /// <para>
        /// Collapses to at most two point-adds on the DIFFERENCE BIT — see the class comment.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public void RangeAdd(int left, int right, long delta)
        {
            if (left < 0 || right >= n || left > right)
            {
                throw new ArgumentOutOfRangeException($"Invalid range [{left}..{right}] for size {n}.");
            }

            PointAddOnDiff(left, delta);
            if (right + 1 < n)
            {
                PointAddOnDiff(right + 1, -delta);
            }
        }

        /// <summary>
        /// Returns the current value of arr[<paramref name="index"/>].
        ///
        /// <para>
        /// Because bit[] represents the difference sequence, arr[i] is exactly
        /// <c>d[0] + d[1] + ... + d[i]</c> — a PrefixSum walk on the diff BIT.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n). </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1). </para>
        /// </summary>
        public long PointQuery(int index)
        {
            if (index < 0 || index >= n)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            long sum = 0;
            for (var i = index + 1; i > 0; i -= LowBit(i))
            {
                sum += bit[i];
            }
            return sum;
        }

        /// <summary>
        /// Reconstructs and returns the full current arr[] by calling <see cref="PointQuery"/>
        /// on every index. Handy for demos and debugging; O(n log n) so avoid in hot paths.
        /// </summary>
        public long[] Materialize()
        {
            var result = new long[n];
            for (var i = 0; i < n; i++)
            {
                result[i] = PointQuery(i);
            }
            return result;
        }

        /// <summary>
        /// Prints the current state of the arr[] this class represents. Purely a diagnostic —
        /// remember that these values are RECONSTRUCTED, not stored.
        /// </summary>
        public void PrintArray()
        {
            var values = Materialize();
            Console.WriteLine($"  arr = [{string.Join(", ", values)}]");
        }

        /// <summary>
        /// Standard BIT point-add on the internal (diff) array, at 0-indexed position
        /// <paramref name="index"/>.
        /// </summary>
        private void PointAddOnDiff(int index, long delta)
        {
            for (var i = index + 1; i <= n; i += LowBit(i))
            {
                bit[i] += delta;
            }
        }

        private static int LowBit(int i) => i & -i;
    }
}
