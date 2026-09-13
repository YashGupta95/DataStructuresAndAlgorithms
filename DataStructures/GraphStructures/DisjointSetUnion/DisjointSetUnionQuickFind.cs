namespace DataStructures.GraphStructures.DisjointSetUnion
{
    // The Quick-Find variant of the Disjoint Set Union (DSU / Union-Find) data structure.
    // Kept here as a deliberately-naive BASELINE so the sibling <see cref="DisjointSetUnionOperations"/>
    // has a foil to be compared against — the pedagogical arc is "here's the obvious first idea,
    // here's why we don't ship it".
    //
    // (Taxonomy note: DSU lives under GraphStructures because its dominant applications —
    // connectivity queries, Kruskal's MST, connected components, cycle detection in undirected
    // graphs — are all graph problems. The internal storage is a partition encoded in an array,
    // but the folder tracks USE, not implementation.)
    //
    // -----------------------------------------------------------------------
    // The Idea — "The Array IS the Answer"
    // -----------------------------------------------------------------------
    // Every element i stores the ID of its set's REPRESENTATIVE directly:
    //
    //     parent[i] == root(i)          — no chain, no walk, no lookup indirection.
    //
    // Find(i) is a single array read. That's as fast as any data structure can possibly answer
    // "which set does i belong to?".
    //
    // The catch is Union(x, y). To merge two sets, EVERY element currently pointing at one
    // root must be relabelled to point at the other. That's a full O(n) scan of the array per
    // union — no cheaper option exists in this representation, because there's no "list of
    // members of this set" to walk. The pain of Quick-Find is not that Find is fast; it is
    // that Union pays for Find's speed at every single call.
    //
    // -----------------------------------------------------------------------
    // Worked Example (n = 8)
    // -----------------------------------------------------------------------
    // Start:                parent = [0, 1, 2, 3, 4, 5, 6, 7]     — 8 singleton sets.
    // Union(0, 1):          parent = [0, 0, 2, 3, 4, 5, 6, 7]     — scan, relabel one entry.
    // Union(2, 3):          parent = [0, 0, 2, 2, 4, 5, 6, 7]
    // Union(4, 5):          parent = [0, 0, 2, 2, 4, 4, 6, 7]
    // Union(6, 7):          parent = [0, 0, 2, 2, 4, 4, 6, 6]
    // Union(0, 2):          parent = [0, 0, 0, 0, 4, 4, 6, 6]     — TWO entries relabelled.
    // Union(4, 6):          parent = [0, 0, 0, 0, 4, 4, 4, 4]     — TWO entries relabelled.
    //
    // Notice the array is always a "flat forest": every element points DIRECTLY at its root.
    // That flatness is the whole story — it's what makes Find O(1) and Union O(n).
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation      Time        Space
    //   ---------      --------    ------
    //   Constructor    O(n)        O(n) for parent + O(n) for setSize
    //   Find           O(1)        O(1)
    //   Union          O(n)        O(1)                — the deal-breaker
    //   Connected      O(1)        O(1)
    //   SetSize        O(1)        O(1)                — read setSize[Find(x)]
    //   SetCount       O(1)        O(1)                — cached counter
    //   GetSet         O(n)        O(size)
    //   Reset          O(n)        O(1)
    //   Clone          O(n)        O(n)
    //
    // -----------------------------------------------------------------------
    // Why We Don't Ship This
    // -----------------------------------------------------------------------
    // A workload of M unions on N elements costs O(M · N) here — quadratic in the worst case.
    // Any algorithm that unions proportionally to input size (Kruskal's MST processes E unions
    // on V elements) becomes unusable. The sibling <see cref="DisjointSetUnionOperations"/>
    // trades Find's O(1) for O(α(n)) — a bound so tight that α(n) ≤ 4 for any n a computer
    // will ever hold — and gets Union down to O(α(n)) too. That trade wins every real
    // workload.
    //
    // The rule is stark: use Quick-Find ONLY if your workload is Find-dominated to the point
    // that Unions are rare (say, one union followed by a million Finds and never a union
    // again). In practice, that's a phantom use-case and you should always reach for the
    // optimized version.
    //
    // See Section 9 of Program.cs for the head-to-head decision table.
    // ============================================================================================
    public class DisjointSetUnionQuickFind
    {
        private readonly int[] parent;
        private readonly int[] setSize;
        private int setCount;

        /// <summary>
        /// Builds a DSU with <paramref name="size"/> elements labelled <c>0 .. size - 1</c>, each in its own singleton set.
        /// </summary>
        /// <param name="size"> The initial element count. Must be non-negative. </param>
        public DisjointSetUnionQuickFind(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), $"Size must be non-negative; got {size}.");
            }

            parent = new int[size];
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
        /// Current number of disjoint sets. Starts at <see cref="Count"/> and drops by one on every successful <see cref="Union"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — cached counter. </para>
        /// </summary>
        public int SetCount => setCount;

        /// <summary>
        /// Returns the representative (root ID) of the set containing <paramref name="x"/>.
        ///
        /// <para>
        /// Quick-Find's headline feature: a single array read. This is as fast as any DSU implementation can possibly answer this query.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public int Find(int x)
        {
            EnsureInRange(x);
            return parent[x];
        }

        /// <summary>
        /// Merges the set containing <paramref name="x"/> with the set containing <paramref name="y"/>. 
        /// Returns <c>true</c> if the sets were actually merged (they were different) and <c>false</c> if the pair was already connected.
        ///
        /// <para>
        /// The <c>bool</c> return is not decoration — Kruskal's MST uses it to detect the "edge would form a cycle" case in O(1), which is why every self-respecting DSU exposes this signature.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — a full array scan to relabel every member of the losing set. 
        /// This is Quick-Find's dealbreaker cost. </para>
        ///
        /// <para>
        /// Note: union-by-size wouldn't rescue Quick-Find — the O(n) scan is baked into the representation, not the merge direction. That trick pays off in Quick-Union (see <c>DisjointSetUnionOperations</c>).
        /// </para>
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

            for (var i = 0; i < parent.Length; i++)
            {
                // No comparison involved in this version of Quick-Find; we simply relabel all members of the other set.
                if (parent[i] == ry)
                {
                    parent[i] = rx;
                }
            }
            setSize[rx] += setSize[ry];
            setSize[ry] = 0; // The losing set is now empty.
            setCount--;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> iff <paramref name="x"/> and <paramref name="y"/> are in the
        /// same set. Sugar over <c>Find(x) == Find(y)</c>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        public bool Connected(int x, int y) => Find(x) == Find(y);

        /// <summary>
        /// Number of elements in the set containing <paramref name="x"/>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) — direct array read on the cached size at the root. </para>
        /// </summary>
        public int SetSize(int x) => setSize[Find(x)];

        /// <summary>
        /// Enumerates every element in the set containing <paramref name="x"/>, in ascending order by label.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — a full scan. No representation of DSU can do better than O(size of the set) for enumeration, 
        /// and without an explicit member list Quick-Find must scan the whole array to filter. </para>
        /// </summary>
        public IEnumerable<int> GetSet(int x)
        {
            var root = Find(x);
            for (var i = 0; i < parent.Length; i++)
            {
                if (parent[i] == root)
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Enumerates every current set as a <see cref="IReadOnlyList{Int32}"/> of members.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public IEnumerable<IReadOnlyList<int>> GetAllSets()
        {
            var buckets = new Dictionary<int, List<int>>();
            for (var i = 0; i < parent.Length; i++)
            {
                var root = parent[i];
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
        /// — matches the guarantee provided by the Graph classes.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public DisjointSetUnionQuickFind Clone()
        {
            var clone = new DisjointSetUnionQuickFind(parent.Length);
            Array.Copy(parent, clone.parent, parent.Length);
            Array.Copy(setSize, clone.setSize, setSize.Length);
            clone.setCount = setCount;
            return clone;
        }

        /// <summary>
        /// Snaps every element back into its own singleton set — cheap way to reuse the DSU across multiple algorithm phases.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < parent.Length; i++)
            {
                parent[i] = i;
                setSize[i] = 1; // Each element is now in its own singleton set.
            }
            setCount = parent.Length;
        }

        /// <summary>
        /// Prints a diagnostic table showing, for each element, its stored root ID. In
        /// Quick-Find this table IS the data structure — there's no derived state to inspect
        /// beyond it.
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

            Console.Write("  is-root:  ");
            for (var i = 0; i < parent.Length; i++)
            {
                Console.Write(Center(parent[i] == i ? "*" : ".", cellWidth));
            }
            Console.WriteLine();
            Console.WriteLine("  legend: `*` root  |  `.` non-root");
        }

        /// <summary>
        /// Prints each set as a depth-1 star with the root on top and members underneath.
        /// Every Quick-Find set is a depth-1 star BY CONSTRUCTION — that's the invariant this
        /// representation upholds and pays for.
        /// </summary>
        public void PrintForest()
        {
            var buckets = new SortedDictionary<int, List<int>>();
            for (var i = 0; i < parent.Length; i++)
            {
                if (!buckets.TryGetValue(parent[i], out var bucket))
                {
                    bucket = new List<int>();
                    buckets[parent[i]] = bucket;
                }
                bucket.Add(i);
            }

            foreach (var (root, members) in buckets)
            {
                members.Sort();
                Console.WriteLine($"  Set rooted at {root} (size {members.Count}):");
                Console.WriteLine($"    {root}");
                var nonRoots = members.FindAll(m => m != root);
                for (var i = 0; i < nonRoots.Count; i++)
                {
                    var connector = i == nonRoots.Count - 1 ? "└──" : "├──";
                    Console.WriteLine($"    {connector} {nonRoots[i]}");
                }
                Console.WriteLine();
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
