namespace DataStructures.AdvancedTrees.SegmentTree
{
    // A Segment Tree is a binary-tree data structure that answers RANGE QUERIES on an array
    // efficiently while still supporting POINT UPDATES. Both operations run in O(log n) — a
    // sweet spot that neither a plain array nor a prefix-sum array can hit at the same time.
    //
    // -----------------------------------------------------------------------
    // Motivation — The Tension Two Naive Approaches Leave Us With
    // -----------------------------------------------------------------------
    // Say we have an array of n numbers and we want to answer many queries of the form
    // "what is the sum of arr[L..R]?" while occasionally changing individual elements.
    //
    //   • Plain array         — Query is O(n) (scan L..R). Update is O(1).
    //   • Prefix-sum array    — Query is O(1) (one subtraction). Update is O(n) (rebuild).
    //
    // Either approach is fast at one operation and slow at the other. A Segment Tree gives us
    // O(log n) for BOTH by storing partial aggregates over nested sub-ranges of the array.
    //
    // -----------------------------------------------------------------------
    // The Core Idea
    // -----------------------------------------------------------------------
    // Recursively split the array in half. Each internal node stores the AGGREGATE (sum, min,
    // max, ...) of the sub-range it covers. Leaves store the original array values.
    //
    // Input array of size 6:  [ 1, 3, 5, 7, 9, 11 ]  (indices 0..5)
    //
    //                            [0..5] = 36
    //                     /                        \
    //             [0..2] = 9                   [3..5] = 27
    //            /          \                  /          \
    //       [0..1] = 4   [2..2] = 5      [3..4] = 16   [5..5] = 11
    //       /       \                    /        \
    //   [0..0]=1  [1..1]=3           [3..3]=7   [4..4]=9
    //
    // To answer Query(1, 4) — the sum of arr[1..4] — the traversal picks up [1..1]=3, [2..2]=5,
    // and [3..4]=16, combining them into 24 without ever touching arr[0] or arr[5].
    //
    // -----------------------------------------------------------------------
    // Array Representation (Why We Allocate 4 * n)
    // -----------------------------------------------------------------------
    // Rather than storing the tree as linked nodes, we lay it out in a plain int[] with:
    //
    //   • Root at index 1.
    //   • For a node at index i:  left child = 2 * i,  right child = 2 * i + 1.
    //
    // The tree is always a complete binary tree over the leaves representing arr[0..n-1]. When
    // n is a power of 2, exactly 2n - 1 slots are used. When n is NOT a power of 2 (like our
    // demo n = 6), some indices are skipped and the last populated slot can be as high as ~4n.
    // Allocating 4 * n is a safe, standard upper bound that avoids off-by-one drama.
    //
    // -----------------------------------------------------------------------
    // How Each Operation Works
    // -----------------------------------------------------------------------
    //   • BUILD  — recurse to the leaves (each leaf takes one input value), then combine
    //              children into parents on the way back up. O(n) total.
    //
    //   • QUERY(L, R) — at every visited node, its segment [segmentStart..segmentEnd] falls
    //                   into one of three relationships with the query range [L..R]:
    //
    //                     1. FULLY OUTSIDE — segment does not overlap [L..R].
    //                                        Return the "identity" value (0 for sum, +∞ for
    //                                        min, -∞ for max). Contributes nothing.
    //
    //                     2. FULLY INSIDE  — segment lies entirely within [L..R].
    //                                        Return the node's stored aggregate as-is. No
    //                                        need to descend further.
    //
    //                     3. PARTIAL       — segment straddles the boundary of [L..R].
    //                                        Recurse into BOTH children and combine.
    //
    //                   Case (2) is what makes the walk cheap — it prunes entire subtrees.
    //                   A careful proof shows at most O(log n) nodes are ever visited.
    //
    //   • UPDATE(i, v) — descend from the root, always taking the child whose segment contains
    //                    index i, until we hit the leaf. Overwrite the leaf, then bubble the
    //                    change back up by re-combining each ancestor's children. O(log n).
    //
    // -----------------------------------------------------------------------
    // Complexity Summary
    // -----------------------------------------------------------------------
    //   Operation   Time        Space (auxiliary)
    //   Build       O(n)        O(4n) for the tree array + O(log n) recursion stack
    //   Query       O(log n)    O(log n) recursion stack
    //   Update      O(log n)    O(log n) recursion stack
    //
    // -----------------------------------------------------------------------
    // The Same Tree, Different Questions — Why This Class Takes an AggregationKind
    // -----------------------------------------------------------------------
    // Notice that NOTHING in the explanation above depends on the aggregate being a SUM. The
    // tree's shape, Build's recursion, Query's three-case logic, and Update's leaf-to-root walk
    // work identically for MIN, MAX, GCD, XOR, or any operator that is:
    //
    //   • associative        —   Combine(a, Combine(b, c)) == Combine(Combine(a, b), c)
    //   • has an identity    —   a neutral value id such that Combine(x, id) == x
    //
    // (In algebra this pair is called a "monoid" — the same tree structure works for any monoid.)
    //
    // This project supports SUM, MIN, and MAX — chosen by the AggregationKind passed to the
    // constructor. The ONLY code that differs across the three variants lives in two tiny
    // helpers below: Combine() and Identity(). Every other method — Build, Query, Update,
    // PrintTree — is completely aggregation-agnostic.
    //
    // -----------------------------------------------------------------------
    // What This Project Does NOT Cover
    // -----------------------------------------------------------------------
    //   • Range UPDATES with lazy propagation (e.g., "add 5 to every element in arr[L..R]").
    //     This is a substantial extension and belongs in its own follow-up project.
    //   • Non-monoidal aggregates (e.g., median, mode) — those need different structures.
    //   • 2D / persistent / dynamic-segment-tree variants — advanced topics.
    // =============================================================================================

    /// <summary>
    /// The aggregation this Segment Tree instance answers queries about.
    ///
    /// <para>
    /// A single tree stores exactly one kind of aggregate per node — you cannot ask a sum-tree
    /// for a minimum. This enum lets the constructor commit an instance to Sum, Min, or Max,
    /// and the <see cref="SegmentTreeOperations.Combine"/> and <see cref="SegmentTreeOperations.Identity"/>
    /// helpers branch on it. Using an enum (rather than a delegate, generic, or three separate
    /// classes) keeps the choice discoverable in IntelliSense and compile-time checked, without
    /// forcing the reader to learn any new language concepts.
    /// </para>
    /// </summary>
    internal enum AggregationKind
    {
        Sum,
        Min,
        Max,
    }

    internal class SegmentTreeOperations
    {
        // The Segment Tree, stored as a flat array. Root at index 1; children of node i live at
        // 2*i and 2*i+1. Size 4*n is a safe upper bound (see the top-of-file note).
        private readonly int[] tree;

        // Length of the original input array. Segments in the tree are ranges over [0..n-1].
        private readonly int n;

        // Which aggregate this instance answers — set once at construction and never changed.
        private readonly AggregationKind kind;

        /// <summary>
        /// Builds a Segment Tree over the specified input array for the specified aggregation.
        ///
        /// <para>
        /// Allocates an internal tree array of size 4 * n and fills it via a post-order
        /// recursion that visits every input element once, so construction is linear in n.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) for the internal tree array, plus O(log n) recursion stack. </para>
        /// </summary>
        /// <param name="input"> The array to build the tree over. Must be non-empty. </param>
        /// <param name="kind"> The aggregate (Sum, Min, or Max) this instance will answer. </param>
        public SegmentTreeOperations(int[] input, AggregationKind kind)
        {
            if (input is null || input.Length == 0)
            {
                throw new ArgumentException("Input array must be non-empty.", nameof(input));
            }

            this.n = input.Length;
            this.kind = kind;
            this.tree = new int[4 * n];

            Build(node: 1, segmentStart: 0, segmentEnd: n - 1, input);
        }

        /// <summary>
        /// Returns the aggregate of the input over the inclusive range [<paramref name="queryLeft"/>, <paramref name="queryRight"/>].
        ///
        /// <para>
        /// The recursion visits at most O(log n) nodes because any segment that lies entirely
        /// inside the query range is returned as-is without descending further. See the
        /// three-case explanation in the top-of-file comment.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Sum tree over [ 1, 3, 5, 7, 9, 11 ]:
        ///     Query(1, 4) → 3 + 5 + 7 + 9 = 24
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n) recursion stack. </para>
        /// </summary>
        /// <param name="queryLeft"> Inclusive left index of the query range. Must be in [0, n-1]. </param>
        /// <param name="queryRight"> Inclusive right index of the query range. Must satisfy queryLeft &lt;= queryRight &lt; n. </param>
        /// <returns> The aggregate of input[queryLeft..queryRight]. </returns>
        public int Query(int queryLeft, int queryRight)
        {
            if (queryLeft < 0 || queryRight >= n || queryLeft > queryRight)
            {
                throw new ArgumentOutOfRangeException($"Invalid query range [{queryLeft}, {queryRight}] for input of length {n}.");
            }

            return Query(node: 1, segmentStart: 0, segmentEnd: n - 1, queryLeft, queryRight);
        }

        /// <summary>
        /// Sets input[<paramref name="targetIndex"/>] to <paramref name="newValue"/> and updates every
        /// aggregate on the path from the corresponding leaf up to the root.
        ///
        /// <para>
        /// Only one root-to-leaf path is touched, so the work is proportional to the tree's
        /// height. All other subtrees are left alone.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n) recursion stack. </para>
        /// </summary>
        /// <param name="targetIndex"> The input index to overwrite. Must be in [0, n-1]. </param>
        /// <param name="newValue"> The new value to store at <paramref name="targetIndex"/>. </param>
        public void Update(int targetIndex, int newValue)
        {
            if (targetIndex < 0 || targetIndex >= n)
            {
                throw new ArgumentOutOfRangeException(nameof(targetIndex), $"Index {targetIndex} is out of range for input of length {n}.");
            }

            Update(node: 1, segmentStart: 0, segmentEnd: n - 1, targetIndex, newValue);
        }

        /// <summary>
        /// Prints the tree indented by depth, showing each node's array index, the input segment
        /// it covers, and its stored aggregate. Useful for visualizing how a query descends.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n) recursion stack. </para>
        /// </summary>
        public void PrintTree()
        {
            Console.WriteLine($"Node[1] ([0..{n - 1}]) = {tree[1]}");
            PrintSubtree(node: 1, segmentStart: 0, segmentEnd: n - 1, prefix: string.Empty);
        }

        /// <summary>
        /// Recursive Build helper.
        ///
        /// <para>
        /// If the current segment is a single element, the tree slot holds that element directly.
        /// Otherwise, the segment is split in half, both children are built first, then their
        /// aggregates are combined into the current slot — a classic post-order fill.
        /// </para>
        /// </summary>
        /// <param name="node"> Index of the current node in the tree array. </param>
        /// <param name="segmentStart"> Inclusive left end of the input range this node covers. </param>
        /// <param name="segmentEnd"> Inclusive right end of the input range this node covers. </param>
        /// <param name="input"> The source input array. </param>
        private void Build(int node, int segmentStart, int segmentEnd, int[] input)
        {
            if (segmentStart == segmentEnd)
            {
                tree[node] = input[segmentStart];
                return;
            }

            int mid = (segmentStart + segmentEnd) / 2;
            int leftChild = 2 * node;
            int rightChild = 2 * node + 1;

            Build(leftChild, segmentStart, mid, input);
            Build(rightChild, mid + 1, segmentEnd, input);

            tree[node] = Combine(tree[leftChild], tree[rightChild]);
        }

        /// <summary>
        /// Recursive Query helper implementing the three-case dispatch.
        /// </summary>
        /// <param name="node"> Index of the current node in the tree array. </param>
        /// <param name="segmentStart"> Inclusive left end of the input range this node covers. </param>
        /// <param name="segmentEnd"> Inclusive right end of the input range this node covers. </param>
        /// <param name="queryLeft"> Inclusive left end of the query range. </param>
        /// <param name="queryRight"> Inclusive right end of the query range. </param>
        /// <returns> The aggregate over the intersection of [segmentStart, segmentEnd] and [queryLeft, queryRight]. </returns>
        private int Query(int node, int segmentStart, int segmentEnd, int queryLeft, int queryRight)
        {
            // Case 1: this segment is entirely outside the query range. Contributes nothing.
            if (segmentEnd < queryLeft || segmentStart > queryRight)
            {
                return Identity();
            }

            // Case 2: this segment is entirely inside the query range. Return its aggregate as-is.
            if (queryLeft <= segmentStart && segmentEnd <= queryRight)
            {
                return tree[node];
            }

            // Case 3: partial overlap. Recurse into both children and combine.
            int mid = (segmentStart + segmentEnd) / 2;
            int leftResult = Query(2 * node, segmentStart, mid, queryLeft, queryRight);
            int rightResult = Query(2 * node + 1, mid + 1, segmentEnd, queryLeft, queryRight);

            return Combine(leftResult, rightResult);
        }

        /// <summary>
        /// Recursive Update helper. Descends to the target leaf, overwrites it, then re-combines
        /// each ancestor's children on the way back up so every aggregate on the path is
        /// consistent with the new value.
        /// </summary>
        /// <param name="node"> Index of the current node in the tree array. </param>
        /// <param name="segmentStart"> Inclusive left end of the input range this node covers. </param>
        /// <param name="segmentEnd"> Inclusive right end of the input range this node covers. </param>
        /// <param name="targetIndex"> The input index to overwrite. </param>
        /// <param name="newValue"> The new value to store at <paramref name="targetIndex"/>. </param>
        private void Update(int node, int segmentStart, int segmentEnd, int targetIndex, int newValue)
        {
            if (segmentStart == segmentEnd)
            {
                tree[node] = newValue;
                return;
            }

            int mid = (segmentStart + segmentEnd) / 2;
            int leftChild = 2 * node;
            int rightChild = 2 * node + 1;

            if (targetIndex <= mid)
            {
                Update(leftChild, segmentStart, mid, targetIndex, newValue);
            }
            else
            {
                Update(rightChild, mid + 1, segmentEnd, targetIndex, newValue);
            }

            tree[node] = Combine(tree[leftChild], tree[rightChild]);
        }

        /// <summary>
        /// Recursive PrintTree helper. Prints the CHILDREN of <paramref name="node"/> with
        /// tree connectors, using <paramref name="prefix"/> as the accumulated ancestor pattern.
        /// The left child gets the "middle" branch (├──) and the right child gets the "last"
        /// branch (└──) — every internal segment-tree node has exactly two children.
        /// </summary>
        private void PrintSubtree(int node, int segmentStart, int segmentEnd, string prefix)
        {
            if (segmentStart == segmentEnd)
            {
                return;
            }

            int mid = (segmentStart + segmentEnd) / 2;
            int leftChild = 2 * node;
            int rightChild = 2 * node + 1;

            Console.WriteLine($"{prefix}├── Node[{leftChild}] ([{segmentStart}..{mid}]) = {tree[leftChild]}");
            PrintSubtree(leftChild, segmentStart, mid, prefix + "│   ");

            Console.WriteLine($"{prefix}└── Node[{rightChild}] ([{mid + 1}..{segmentEnd}]) = {tree[rightChild]}");
            PrintSubtree(rightChild, mid + 1, segmentEnd, prefix + "    ");
        }

        /// <summary>
        /// Prints the tree as a classic top-down binary-tree diagram — the "picture" view,
        /// with parent-child branches drawn using Unicode box characters. Each node is labelled
        /// <c>index=value</c> so both the array position and the stored aggregate are visible.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) — every populated tree slot is visited once. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) for the rendered character grid. </para>
        /// </summary>
        public void PrintTreeVisual()
        {
            var (lines, _, _) = BuildVisualBlock(node: 1, segmentStart: 0, segmentEnd: n - 1);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Recursive PrintTreeVisual helper. Returns the rendered subtree rooted at
        /// <paramref name="node"/> as a rectangular block of equal-width lines, plus the block's
        /// width and the column where the ROOT of the block should be connected to its parent.
        ///
        /// <para>
        /// Bottom-up construction: the left and right subtree blocks are rendered first, then
        /// stacked side-by-side beneath a parent label and a connector row whose ┌ / ┐ corners
        /// sit exactly above each child's root column and whose ┴ sits below the parent label.
        /// If the parent label is wider than the space between the two child roots, the block
        /// is padded on the offending side so the label still fits.
        /// </para>
        /// </summary>
        private (List<string> lines, int width, int rootCol) BuildVisualBlock(
            int node, int segmentStart, int segmentEnd)
        {
            string label = $"{node}={tree[node]}";

            // Leaf: the block is just the label. Its root is the label's middle column.
            if (segmentStart == segmentEnd)
            {
                return (new List<string> { label }, label.Length, label.Length / 2);
            }

            int mid = (segmentStart + segmentEnd) / 2;
            var (leftLines, leftWidth, leftRoot) = BuildVisualBlock(2 * node, segmentStart, mid);
            var (rightLines, rightWidth, rightRoot) = BuildVisualBlock(2 * node + 1, mid + 1, segmentEnd);

            const int gap = 2;
            int leftChildCol = leftRoot;
            int rightChildCol = leftWidth + gap + rightRoot;
            int parentMid = (leftChildCol + rightChildCol) / 2;
            int width = leftWidth + gap + rightWidth;

            // Pad the block if the parent label would overflow either edge.
            int labelStart = parentMid - label.Length / 2;
            int labelEnd = labelStart + label.Length;
            int leftPad = Math.Max(0, -labelStart);
            int rightPad = Math.Max(0, labelEnd - width);

            if (leftPad > 0)
            {
                string pad = new string(' ', leftPad);
                for (int i = 0; i < leftLines.Count; i++)
                {
                    leftLines[i] = pad + leftLines[i];
                }
                leftWidth += leftPad;
                leftChildCol += leftPad;
                rightChildCol += leftPad;
                parentMid += leftPad;
                labelStart += leftPad;
                width += leftPad;
            }
            if (rightPad > 0)
            {
                string pad = new string(' ', rightPad);
                for (int i = 0; i < rightLines.Count; i++)
                {
                    rightLines[i] = rightLines[i] + pad;
                }
                rightWidth += rightPad;
                width += rightPad;
            }

            // Row 0: parent label, centered on parentMid.
            string labelLine = new string(' ', labelStart)
                + label
                + new string(' ', width - labelStart - label.Length);

            // Row 1: connector — ┌ over left child, ┐ over right child, ┴ below parent label.
            char[] connector = new string(' ', width).ToCharArray();
            for (int c = leftChildCol + 1; c < rightChildCol; c++)
            {
                connector[c] = '─';
            }
            connector[leftChildCol] = '┌';
            connector[rightChildCol] = '┐';
            if (parentMid != leftChildCol && parentMid != rightChildCol)
            {
                connector[parentMid] = '┴';
            }
            string connectorLine = new string(connector);

            // Rows 2+: children blocks laid out side by side, separated by the same gap.
            int maxHeight = Math.Max(leftLines.Count, rightLines.Count);
            while (leftLines.Count < maxHeight) leftLines.Add(new string(' ', leftWidth));
            while (rightLines.Count < maxHeight) rightLines.Add(new string(' ', rightWidth));

            var combined = new List<string> { labelLine, connectorLine };
            string gapStr = new string(' ', gap);
            for (int i = 0; i < maxHeight; i++)
            {
                combined.Add(leftLines[i] + gapStr + rightLines[i]);
            }

            return (combined, width, parentMid);
        }

        /// <summary>
        /// Combines two child aggregates into their parent aggregate, according to <see cref="kind"/>.
        /// Along with <see cref="Identity"/>, this is the ONLY place the aggregation choice matters.
        /// </summary>
        private int Combine(int left, int right) => kind switch
        {
            AggregationKind.Sum => left + right,
            AggregationKind.Min => Math.Min(left, right),
            AggregationKind.Max => Math.Max(left, right),
            _ => throw new InvalidOperationException($"Unsupported aggregation kind: {kind}"),
        };

        /// <summary>
        /// Returns the identity value for <see cref="kind"/> — the value that leaves any other
        /// operand unchanged when combined with it (0 for sum, +∞ for min, -∞ for max). Used
        /// when a segment is fully outside a query range.
        /// </summary>
        private int Identity() => kind switch
        {
            AggregationKind.Sum => 0,
            AggregationKind.Min => int.MaxValue,
            AggregationKind.Max => int.MinValue,
            _ => throw new InvalidOperationException($"Unsupported aggregation kind: {kind}"),
        };
    }
}
