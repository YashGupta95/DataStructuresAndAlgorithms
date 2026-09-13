namespace DataStructures.GraphStructures.DisjointSetUnion
{
    // The Disjoint Set Union (DSU) — also called Union-Find — is a data structure that
    // maintains a partition of {0, 1, ..., n - 1} into disjoint sets and supports two
    // operations at near-constant cost:
    //
    //     Find(x)    — which set does x belong to?
    //     Union(x, y) — merge the sets containing x and y.
    //
    // (Taxonomy note: DSU lives under GraphStructures because its dominant applications —
    // connectivity queries, Kruskal's MST, connected components, cycle detection in undirected
    // graphs — are all graph problems. The internal storage is a partition encoded in an
    // array, but the folder tracks USE, not implementation.)
    //
    // This class is the OPTIMIZED implementation: union-by-rank plus full path compression,
    // which together lift both operations to O(α(n)) amortized. The sibling
    // <see cref="DisjointSetUnionQuickFind"/> is the naive baseline kept alongside for
    // contrast — its Find is O(1) but its Union is O(n), which loses on every real workload.
    //
    // -----------------------------------------------------------------------
    // Same Array Shape, Different Semantics
    // -----------------------------------------------------------------------
    // Both classes store a single <c>int[] parent</c> of length n. What CHANGES between them
    // is what a cell means — a direct echo of the "different bit[] interpretation" story in
    // the FenwickTree project.
    //
    //   • Quick-Find (baseline):    parent[i] == root(i)         — direct root pointer.
    //   • This class (optimized):   parent[i] == i's PARENT      — a step upward in a tree.
    //                               Roots identify themselves via parent[i] == i.
    //
    // Under the optimized semantics, each set is an inverted TREE — every node points at its
    // parent, and the root is a self-loop. Find(x) walks up the tree until it hits a
    // self-loop; Union links one root under the other.
    //
    // -----------------------------------------------------------------------
    // Trick #1 — Union By Rank
    // -----------------------------------------------------------------------
    // Naively linking two trees can produce arbitrarily long chains — n calls to Union in a
    // bad order create a depth-n linked list, which crashes Find to O(n). Union-by-rank
    // prevents that:
    //
    //   • rank[i] is a stored UPPER BOUND on the height of the tree rooted at i. (Only
    //     meaningful for roots — non-root ranks are historical fossils that never affect the
    //     algorithm.)
    //   • On Union(x, y), take rx = Find(x), ry = Find(y). Attach the LOWER-RANK root under
    //     the HIGHER-RANK root. If the ranks are equal, pick either — but bump the new root's
    //     rank by one.
    //
    // This alone caps tree height at O(log n) — because a rank r tree contains at least 2^r
    // elements, and only 2^r ≤ n roots can ever reach rank r, so rank never exceeds log₂(n).
    //
    // Note on the "rank vs size" heuristic — union-by-SIZE (attach the smaller tree under
    // the larger) is an equally valid alternative with identical asymptotic guarantees; both
    // cap tree height at O(log n). We picked rank because its "rank never decreases"
    // invariant is a cleaner sentence to prove; union-by-size trades that for the perk of
    // reusing setSize[] as the union heuristic AND the size query.
    //
    // -----------------------------------------------------------------------
    // Trick #2 — Full Path Compression
    // -----------------------------------------------------------------------
    // Every Find(x) that had to walk from x up to a root r visits a bunch of intermediates
    // along the way. Full path compression rewires ALL of them to point directly at r on the
    // way back, so future Finds through any of those elements are O(1).
    //
    //     Before:  3 → 2 → 1 → 0   (Find(3) walks four hops)
    //     After :  3 → 0
    //              2 → 0            (0, 1, 2, 3 all point straight at 0 now)
    //              1 → 0
    //
    // Implementation is two passes: first walk up to identify the root, then a second walk
    // to rewire each parent pointer. Path halving (rewire every other node in one pass) has
    // the same asymptotic bound with tighter code, but full compression is what a reader can
    // trace on paper — chosen here for teaching value.
    //
    // -----------------------------------------------------------------------
    // Rank Is An UPPER BOUND, Not The True Height
    // -----------------------------------------------------------------------
    // After path compression, real tree heights shrink freely, but ranks NEVER decrease.
    // That's a feature: the analysis needs a monotonically-non-decreasing quantity to charge
    // amortized cost against. Interviewers love to poke at this — a common trap question is
    // "why don't you fix up the rank after compression?" The answer is "because you can't
    // without breaking the amortization proof, and it doesn't matter — the bound already
    // holds".
    //
    // -----------------------------------------------------------------------
    // The O(α(n)) Bound In Plain English
    // -----------------------------------------------------------------------
    // Together, union-by-rank + path compression give Θ(α(n)) amortized per operation, where
    // α is the inverse Ackermann function. In plain terms: for any n a computer will ever
    // hold (say, n ≤ 2^65535), α(n) ≤ 4. Treating each operation as O(1) is a lie by less
    // than a factor of 5 forever.
    //
    // -----------------------------------------------------------------------
    // What DSU Cannot Do — And What To Reach For Instead
    // -----------------------------------------------------------------------
    // DSU is a ONE-WAY structure: sets can only be merged, never split. If your problem
    // needs dynamic connectivity WITH deletion (removing an edge and asking whether two
    // vertices are still connected), DSU is the wrong tool — reach for a Link-Cut Tree or
    // an Euler-Tour Tree. Persistent DSU, rollback DSU, and weighted DSU (values on paths)
    // are advanced variants worth knowing by name but are deliberately out of scope for
    // this teaching project.
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation      Time                   Space
    //   ---------      --------------------   ------
    //   Constructor    O(n)                   O(n)  (parent + rank + setSize arrays)
    //   Find           O(α(n)) amortized      O(1)
    //   Union          O(α(n)) amortized      O(1)
    //   Connected      O(α(n)) amortized      O(1)
    //   SetSize        O(α(n)) amortized      O(1)
    //   SetCount       O(1)                   O(1)
    //   GetSet         O(n)                   O(size)
    //   Reset          O(n)                   O(1)
    //   Clone          O(n)                   O(n)
    //
    // ============================================================================================
    public class DisjointSetUnionOperations
    {
        private readonly int[] parent;
        private readonly int[] rank;
        private readonly int[] setSize;
        private int setCount;

        /// <summary>
        /// Builds a DSU with <paramref name="size"/> elements labelled <c>0 .. size - 1</c>, each in its own singleton set. 
        /// Every element is its own root, has rank 0, and has setSize 1.
        /// </summary>
        /// <param name="size"> The initial element count. Must be non-negative. </param>
        public DisjointSetUnionOperations(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), $"Size must be non-negative; got {size}.");
            }

            parent = new int[size];
            rank = new int[size];
            setSize = new int[size];
            for (var i = 0; i < size; i++)
            {
                parent[i] = i;
                setSize[i] = 1;
            }
            setCount = size;
        }

        /// <summary> Total number of elements in the DSU. Never changes after construction. </summary>
        public int Count => parent.Length;

        /// <summary>
        /// Current number of disjoint sets. Starts at <see cref="Count"/> and drops by one
        /// on every successful <see cref="Union"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — cached counter. </para>
        /// </summary>
        public int SetCount => setCount;

        /// <summary>
        /// Returns the representative (root) of the set containing <paramref name="x"/>.
        /// Applies FULL PATH COMPRESSION as a side-effect: every node visited on the way up
        /// is rewired to point directly at the root, so subsequent Find calls through any of
        /// those nodes are near-constant.
        ///
        /// <para>
        /// Two-pass implementation: first walk finds the root, second walk rewires. 
        /// Path halving (rewire every other node in one pass) is a common equivalent with tighter
        /// code; full compression is used here for teaching clarity.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(α(n)) amortized. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) — iterative, no recursion stack. </para>
        /// </summary>
        public int Find(int x)
        {
            EnsureInRange(x);

            var root = x;
            while (parent[root] != root)
            {
                root = parent[root]; // Climb up to find the root of the set containing x
            }

            var cursor = x;
            while (parent[cursor] != root) // Rewire the path from x to the root for full path compression
            {
                var next = parent[cursor];
                parent[cursor] = root; // Point the current node directly to the root
                cursor = next; // Move to the next node up the path
            }
            return root;
        }

        /// <summary>
        /// Merges the set containing <paramref name="x"/> with the set containing <paramref name="y"/>. 
        /// Returns <c>true</c> if the sets were actually merged and <c>false</c> if the pair was already connected.
        ///
        /// <para>
        /// Applies UNION BY RANK: the lower-rank root is attached under the higher-rank root so the tree height stays O(log n). 
        /// When ranks tie, the second root is attached under the first and the first's rank increments — the ONLY code path that ever increases a rank.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(α(n)) amortized — dominated by the two <see cref="Find"/> calls. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public bool Union(int x, int y)
        {
            var rx = Find(x);
            var ry = Find(y);
            if (rx == ry)
            {
                return false;
            }

            // Attach lower-rank root under higher-rank root so tree height stays O(log n).
            if (rank[rx] < rank[ry])
            {
                (rx, ry) = (ry, rx); // Swap so that rx always has the higher or equal rank
            }
            parent[ry] = rx;
            setSize[rx] += setSize[ry];
            if (rank[rx] == rank[ry])
            {
                rank[rx]++;
            }
            setCount--;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> iff <paramref name="x"/> and <paramref name="y"/> are in the same set. Sugar over <c>Find(x) == Find(y)</c>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(α(n)) amortized. </para>
        /// </summary>
        public bool Connected(int x, int y) => Find(x) == Find(y);

        /// <summary>
        /// Number of elements in the set containing <paramref name="x"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(α(n)) amortized — one Find call, then an O(1) array read. </para>
        /// </summary>
        public int SetSize(int x) => setSize[Find(x)];

        /// <summary>
        /// Returns the rank of the ROOT of <paramref name="x"/>'s set. Exposed purely for
        /// teaching purposes so the demos can show union-by-rank at work; production code
        /// almost never needs this.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(α(n)) amortized. </para>
        /// </summary>
        public int GetRank(int x) => rank[Find(x)];

        /// <summary>
        /// Enumerates every element in the set containing <paramref name="x"/>, in ascending
        /// order by label.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — must scan the whole array and Find each element. DSU does not
        /// maintain an explicit per-set member list. </para>
        /// </summary>
        public IEnumerable<int> GetSet(int x)
        {
            var root = Find(x);
            for (var i = 0; i < parent.Length; i++)
            {
                if (Find(i) == root)
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Enumerates every current set as a <see cref="IReadOnlyList{Int32}"/> of members.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) amortized </para>
        /// </summary>
        public IEnumerable<IReadOnlyList<int>> GetAllSets()
        {
            var buckets = new Dictionary<int, List<int>>();
            for (var i = 0; i < parent.Length; i++)
            {
                var root = Find(i);
                if (!buckets.TryGetValue(root, out var bucket))
                {
                    bucket = new List<int>();
                    buckets[root] = bucket;
                }
                bucket.Add(i);
            }
            foreach (var bucket in buckets.Values)
            {
                yield return bucket;
            }
        }

        /// <summary>
        /// Returns an independent deep copy. Mutating the clone leaves the original untouched
        /// — including the ranks, parent pointers, and cached sizes.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public DisjointSetUnionOperations Clone()
        {
            var clone = new DisjointSetUnionOperations(parent.Length);
            Array.Copy(parent, clone.parent, parent.Length);
            Array.Copy(rank, clone.rank, rank.Length); // Additional copy for the rank array
            Array.Copy(setSize, clone.setSize, setSize.Length);
            clone.setCount = setCount;
            return clone;
        }

        /// <summary>
        /// Snaps every element back into its own singleton set — cheap way to reuse the DSU
        /// across multiple algorithm phases without allocating a fresh instance.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < parent.Length; i++)
            {
                parent[i] = i;
                rank[i] = 0;
                setSize[i] = 1;
            }
            setCount = parent.Length;
        }

        /// <summary>
        /// Prints a diagnostic table showing, for each element, its stored parent pointer,
        /// its root (WITHOUT running Find so the pre-compression state is visible), and the
        /// rank of that root. This is the single most illuminating view for tracing a
        /// union-by-rank sequence.
        /// </summary>
        public void PrintParents()
        {
            if (parent.Length == 0)
            {
                Console.WriteLine("  (empty DSU)");
                return;
            }

            var idxWidth = (parent.Length - 1).ToString().Length;
            var cellWidth = Math.Max(idxWidth, 2) + 2;

            Console.Write("  element:  ");
            for (var i = 0; i < parent.Length; i++)
            {
                Console.Write(Center(i.ToString(), cellWidth));
            }
            Console.WriteLine();

            Console.Write("  parent :  ");
            for (var i = 0; i < parent.Length; i++)
            {
                Console.Write(Center(parent[i].ToString(), cellWidth));
            }
            Console.WriteLine();

            Console.Write("  root(*):  ");
            for (var i = 0; i < parent.Length; i++)
            {
                var r = RootWithoutCompression(i);
                Console.Write(Center(r.ToString(), cellWidth));
            }
            Console.WriteLine();

            Console.Write("  rank   :  ");
            for (var i = 0; i < parent.Length; i++)
            {
                Console.Write(Center(rank[i].ToString(), cellWidth));
            }
            Console.WriteLine();
            Console.WriteLine("  legend: `root(*)` is derived by walking parents WITHOUT path compression");
            Console.WriteLine("          so the tree shape at this moment is visible. `rank` is only");
            Console.WriteLine("          meaningful for roots — non-root ranks are historical fossils.");
        }

        /// <summary>
        /// Prints each set as an indented tree with the root on top. Depths reflect the
        /// CURRENT parent-pointer graph, so a recent path-compressing Find will flatten what
        /// shows here.
        /// </summary>
        public void PrintForest()
        {
            var children = new List<int>[parent.Length];
            for (var i = 0; i < parent.Length; i++)
            {
                children[i] = new List<int>();
            }
            var roots = new List<int>();
            for (var i = 0; i < parent.Length; i++)
            {
                if (parent[i] == i)
                {
                    roots.Add(i);
                }
                else
                {
                    children[parent[i]].Add(i);
                }
            }
            roots.Sort();

            foreach (var root in roots)
            {
                var size = setSize[root];
                Console.WriteLine($"  Set rooted at {root} (size {size}, rank {rank[root]}):");
                Console.WriteLine($"    {root}");
                PrintSubtree(children, root, "    ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Walks parents from <paramref name="x"/> up to the root WITHOUT invoking path
        /// compression. Used only by <see cref="PrintParents"/> so the diagnostic view shows
        /// the current tree shape rather than triggering a rewrite as a side-effect of
        /// inspecting it.
        /// </summary>
        private int RootWithoutCompression(int x)
        {
            var cursor = x;
            while (parent[cursor] != cursor)
            {
                cursor = parent[cursor];
            }
            return cursor;
        }

        private void PrintSubtree(List<int>[] children, int node, string indent)
        {
            var kids = children[node];
            kids.Sort();
            for (var i = 0; i < kids.Count; i++)
            {
                var isLast = i == kids.Count - 1;
                var connector = isLast ? "└──" : "├──";
                Console.WriteLine($"{indent}{connector} {kids[i]}");
                var childIndent = indent + (isLast ? "    " : "│   ");
                PrintSubtree(children, kids[i], childIndent);
            }
        }

        private void EnsureInRange(int x)
        {
            if (x < 0 || x >= parent.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(x), $"Element {x} is out of range [0, {parent.Length - 1}].");
            }
        }

        private static string Center(string text, int width)
        {
            if (text.Length >= width)
            {
                return text;
            }
            var totalPad = width - text.Length;
            var leftPad = totalPad / 2;
            var rightPad = totalPad - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }
    }
}
