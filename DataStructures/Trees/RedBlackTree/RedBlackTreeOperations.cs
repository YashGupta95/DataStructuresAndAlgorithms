namespace DataStructures.Trees.RedBlackTree
{
    // A Red-Black Tree is a self-balancing Binary Search Tree (BST) in which every node carries
    // one extra bit of information: its color, either Red or Black. By enforcing a small set of
    // color-based rules, the tree keeps its height within O(log n) at all times, which guarantees
    // O(log n) worst-case time for search, insertion, and deletion.
    //
    // -----------------------------------------------------------------------
    // The Five Red-Black Properties (must hold after every operation)
    // -----------------------------------------------------------------------
    //   1. Every node is either RED or BLACK.
    //   2. The root is always BLACK.
    //   3. Every leaf (a NIL / null child) is considered BLACK.
    //   4. A RED node cannot have a RED child. (No two consecutive reds on any path.)
    //   5. Every path from a given node down to any of its descendant NIL leaves contains the
    //      same number of BLACK nodes. This count is called the node's "black-height".
    //
    // Property 5 is the reason the tree stays balanced: it forces every root-to-leaf path to have
    // roughly the same length, so no path can be more than twice as long as any other.
    //
    // -----------------------------------------------------------------------
    // Red vs. Black — Intuition
    // -----------------------------------------------------------------------
    //   • BLACK nodes contribute to the "height budget" (black-height) of every path.
    //   • RED nodes are "free" — they do not count toward black-height. They are used to absorb
    //     small local imbalances between rebalancing operations.
    //   • Newly inserted nodes are always RED. This keeps the black-height unchanged and shifts
    //     the burden of any repair to property 4 (no two consecutive reds), which is cheaper to
    //     fix than a black-height violation.
    //
    // -----------------------------------------------------------------------
    // How Balance Is Restored
    // -----------------------------------------------------------------------
    // After an insertion or deletion, the tree may temporarily violate property 4 (insert) or
    // property 5 (delete). Balance is restored using two low-level operations:
    //
    //   • RECOLORING — flipping the colors of a node and/or its relatives (parent, uncle,
    //                  sibling, grandparent). Cheap, O(1).
    //   • ROTATION   — a local structural change (Left Rotate or Right Rotate) that re-parents
    //                  three neighboring nodes without changing the in-order sequence of the
    //                  tree. Also O(1).
    //
    // Insertion needs at most 2 rotations; deletion needs at most 3. Both operations walk upward
    // from the affected node using parent pointers, applying a small fixed set of cases at each
    // step (see the doc-comments on Insert / InsertFixup / Delete / DeleteFixup).
    //
    // -----------------------------------------------------------------------
    // Comparison with the AVL Tree:
    // -----------------------------------------------------------------------
    //   • AVL trees are more strictly balanced (height diff of ≤ 1 between siblings), which
    //     makes lookups slightly faster.
    //   • Red-Black trees allow a looser balance (any path is at most 2× another), which makes
    //     insertions and deletions faster on average because fewer rotations are needed.
    //   • Both offer O(log n) worst-case time for all operations.
    //
    // Red-Black trees are the data structure of choice for many standard libraries — for example
    // the ordered map/set containers in the C++ STL (`std::map`, `std::set`) and Java's
    // `TreeMap` / `TreeSet`, as well as the Linux kernel's process scheduler.
    // =============================================================================================
    internal class RedBlackTreeOperations
    {
        /// <summary>
        /// Returns the color of the specified Red-Black Tree node.
        ///
        /// <para>
        /// In a Red-Black Tree, every leaf (NIL) node is considered black. Since this implementation represents empty subtrees as <see langword="null"/> rather than using a shared sentinel NIL node, this helper treats a <see langword="null"/> reference as a black node.
        /// This allows the color-based invariants to be checked uniformly without special-casing the leaves.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        ///      20(B)
        ///     /
        ///   10(R)
        ///
        /// Color(20)   = Black
        /// Color(10)   = Red
        /// Color(null) = Black
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="node"> The node whose color is to be returned. </param>
        /// <returns>
        /// The color of the specified node, or <see cref="NodeColor.Black"/> if the node is <see langword="null"/>.
        /// </returns>
        private static NodeColor GetColor(RedBlackNode? node)
        {
            return node?.Color ?? NodeColor.Black;
        }

        /// <summary>
        /// Performs a left rotation on the specified node.
        ///
        /// <para>
        /// A left rotation pivots the subtree around <paramref name="node"/>: its right child is promoted upward to become the new subtree root, and <paramref name="node"/> is demoted to become the left child of that new root. This is a local, O(1) restructuring that preserves the in-order (BST) sequence of values — the Red-Black fix-up routines call it to correct specific color-based configurations.
        ///
        /// Before Rotation:
        /// <code>
        ///        x
        ///       / \
        ///      a   y
        ///         / \
        ///        b   c
        /// </code>
        ///
        /// After Rotation:
        /// <code>
        ///        y
        ///       / \
        ///      x   c
        ///     / \
        ///    a   b
        /// </code>
        ///
        /// After the rotation:
        /// <list type="bullet">
        /// <item> <description> The right child (y) becomes the new root of the subtree. </description> </item>
        /// <item> <description> The original root (x) becomes the left child of y. </description> </item>
        /// <item> <description> The left child of y (b) is transferred to become the right child of x. </description> </item>
        /// <item> <description> Parent pointers of all affected nodes are updated. </description> </item>
        /// </list>
        ///
        /// Because the rotated subtree may sit at the root of the tree, this method returns the (possibly updated) tree root so that callers can propagate the change upward.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="node"> The node around which the rotation is to be performed. </param>
        /// <returns> The root of the tree after the rotation. </returns>
        private static RedBlackNode LeftRotate(RedBlackNode root, RedBlackNode node)
        {
            var newRoot = node.Right!;
            var transferredSubtree = newRoot.Left; // The left subtree of the new root will be transferred to become the right subtree of the original node.

            node.Right = transferredSubtree; // Transferred subtree becomes the right child of the original node.

            if (transferredSubtree is not null)
            {
                transferredSubtree.Parent = node; // Update the parent pointer of the transferred subtree to point back to the original node.
            }

            newRoot.Parent = node.Parent; // Node's parent becomes the parent of the new root.

            if (node.Parent is null)
            {
                root = newRoot; // The rotated node was the tree root; update the tree root reference.
            }
            // Check if the original node was a left child of its parent.
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = newRoot; // Update the parent's left child to point to the new root.
            }
            else
            {
                node.Parent.Right = newRoot; // Update the parent's right child to point to the new root.
            }

            // The original node becomes the left child of the new root.
            newRoot.Left = node;
            node.Parent = newRoot;

            return root;
        }

        /// <summary>
        /// Performs a right rotation on the specified node.
        ///
        /// <para>
        /// A right rotation is the mirror image of a left rotation: it pivots the subtree around <paramref name="node"/>, promoting its left child to become the new subtree root and demoting <paramref name="node"/> to become the right child of that new root. Like left rotation, it is a local O(1) restructuring that preserves the in-order (BST) sequence of values.
        ///
        /// Before Rotation:
        /// <code>
        ///          y
        ///         / \
        ///        x   c
        ///       / \
        ///      a   b
        /// </code>
        ///
        /// After Rotation:
        /// <code>
        ///        x
        ///       / \
        ///      a   y
        ///         / \
        ///        b   c
        /// </code>
        ///
        /// After the rotation:
        /// <list type="bullet">
        /// <item> <description> The left child (x) becomes the new root of the subtree. </description> </item>
        /// <item> <description> The original root (y) becomes the right child of x. </description> </item>
        /// <item> <description> The right child of x (b) is transferred to become the left child of y. </description> </item>
        /// <item> <description> Parent pointers of all affected nodes are updated. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="node"> The node around which the rotation is to be performed. </param>
        /// <returns> The root of the tree after the rotation. </returns>
        private static RedBlackNode RightRotate(RedBlackNode root, RedBlackNode node)
        {
            var newRoot = node.Left!;
            var transferredSubtree = newRoot.Right; // The right subtree of the new root will be transferred to become the left subtree of the original node.

            node.Left = transferredSubtree; // Transferred subtree becomes the left child of the original node.

            if (transferredSubtree is not null)
            {
                transferredSubtree.Parent = node; // Update the parent pointer of the transferred subtree to point back to the original node.
            }

            newRoot.Parent = node.Parent; // Node's parent becomes the parent of the new root.

            if (node.Parent is null)
            {
                root = newRoot; // The rotated node was the tree root; update the tree root reference.
            }
            // Check if the original node was a right child of its parent.
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = newRoot; // Update the parent's right child to point to the new root.
            }
            else
            {
                node.Parent.Left = newRoot; // Update the parent's left child to point to the new root.
            }

            // The original node becomes the right child of the new root.
            newRoot.Right = node;
            node.Parent = newRoot;

            return root;
        }

        /// <summary>
        /// Inserts a new value into the Red-Black Tree while preserving all Red-Black properties.
        ///
        /// <para>
        /// The insertion process consists of two stages:
        /// <list type="number">
        /// <item> <description> Perform a standard Binary Search Tree (BST) insertion. The new node is always colored <see cref="NodeColor.Red"/>. </description> </item>
        /// <item> <description> Restore the Red-Black properties by walking upward from the inserted node, applying recoloring and rotations as needed. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// A Red-Black Tree must satisfy the following properties at all times:
        /// <list type="number">
        /// <item> <description> Every node is either red or black. </description> </item>
        /// <item> <description> The root is black. </description> </item>
        /// <item> <description> Every leaf (NIL) is black. </description> </item>
        /// <item> <description> A red node cannot have a red child (no two consecutive red nodes). </description> </item>
        /// <item> <description> Every path from a given node to any of its descendant NIL leaves contains the same number of black nodes (the black-height). </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// Coloring the new node red preserves the black-height on every path (property 5).
        /// The only property that can be violated by the insertion is property 4, which happens when the parent of the newly inserted node is also red.
        /// Rebalancing is delegated to <see cref="InsertFixup"/>, which walks upward from the inserted node and restores property 4 through recoloring and rotations — see there for the case-by-case diagrams.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1), because the fix-up walks upward using parent pointers instead of recursion. </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="value"> The value to insert. </param>
        /// <returns> The root of the tree after the insertion. </returns>
        public static RedBlackNode Insert(RedBlackNode? root, int value)
        {
            // Step 1: Standard BST insertion with parent pointer wiring.
            var newNode = new RedBlackNode(value);

            RedBlackNode? parent = null;
            var current = root;

            while (current is not null)
            {
                parent = current;

                if (value < current.Value)
                {
                    current = current.Left;
                }
                else if (value > current.Value)
                {
                    current = current.Right;
                }
                else
                {
                    return root!; // Duplicate values are not allowed in this Red-Black Tree.
                }
            }

            newNode.Parent = parent; // Set the parent of the new node to the last non-null node encountered during the search.

            if (parent is null)
            {
                // Tree was empty. The new node becomes the root; color it black to satisfy property 2, and no fix-up is needed.
                newNode.Color = NodeColor.Black;
                return newNode;
            }

            // If the new value is less than the parent's value, insert it as the left child.
            if (value < parent.Value)
            {
                parent.Left = newNode;
            }
            // If the new value is greater than the parent's value, insert it as the right child.
            else
            {
                parent.Right = newNode;
            }

            // Step 2: Restore the Red-Black properties.
            return InsertFixup(root!, newNode);
        }

        /// <summary>
        /// Restores the Red-Black properties after an insertion.
        ///
        /// <para>
        /// This helper is invoked with a freshly inserted red node whose parent may also be red (violating property 4).
        /// It walks upward from the inserted node, applying the three canonical fix-up cases (and their mirrors) until the violation is resolved.
        /// </para>
        ///
        /// <b>Fix-up Cases</b>
        /// <para>
        /// Throughout the diagrams below, <c>z</c> denotes the newly inserted (red) node, <c>P</c> its parent, <c>G</c> its grandparent, and <c>U</c> its uncle (the sibling of <c>P</c>). These labels stay attached to the same physical node in both the Before and After pictures. The cases apply symmetrically depending on whether <c>P</c> is a left or right child of <c>G</c>.
        /// </para>
        ///
        /// <para>
        /// <b>Case 1: Uncle is red — recolor only.</b>
        /// <code>
        /// Before:                       After:
        ///        G(B)                          G(R)     <- loop restarts from G (walk upward and re-check)
        ///       /   \                         /   \
        ///     P(R)   U(R)                   P(B)   U(B)
        ///     /                             /
        ///   z(R)                          z(R)
        /// </code>
        /// The parent and uncle are recolored black, the grandparent is recolored red, and the process continues from the grandparent.
        /// </para>
        ///
        /// <para>
        /// <b>Case 2: Uncle is black, z forms a "zig-zag" with its parent — rotate to reduce to Case 3.</b>
        /// <code>
        /// Before:                       After Left Rotate (P):
        ///        G(B)                          G(B)
        ///       /   \                         /   \
        ///     P(R)   U(B)                   z(R)   U(B)
        ///        \                          /
        ///        z(R)                     P(R)              <- z, P, G now form a straight line — falls into Case 3.
        /// </code>
        /// </para>
        ///
        /// <para>
        /// <b>Case 3: Uncle is black, z forms a straight line with its parent — recolor and rotate the grandparent.</b>
        /// <code>
        /// Before:                       After Right Rotate (G):
        ///        G(B)                          P(B)
        ///       /   \                         /   \
        ///     P(R)   U(B)                   z(R)   G(R)
        ///     /                                       \
        ///   z(R)                                       U(B)
        /// </code>
        /// The parent becomes the new subtree root; the grandparent is demoted to a child. The tree is now balanced and the loop terminates.
        /// </para>
        ///
        /// <para>
        /// Finally, the root is unconditionally recolored black to preserve property 2 (in case Case 1 propagated all the way to the root).
        /// </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="node"> The newly inserted node. </param>
        /// <returns> The root of the tree after the fix-up. </returns>
        private static RedBlackNode InsertFixup(RedBlackNode root, RedBlackNode node)
        {
            var z = node;

            // A violation can only exist while z's parent is red. If the parent is black (or z is the root), no property is violated.
            while (z.Parent is not null && z.Parent.Color == NodeColor.Red)
            {
                // Since z.Parent is red, it cannot be the root (the root is always black). Therefore z has a grandparent.
                var parent = z.Parent;
                var grandparent = parent.Parent!;

                if (parent == grandparent.Left)
                {
                    var uncle = grandparent.Right;

                    if (GetColor(uncle) == NodeColor.Red)
                    {
                        // Case 1: Uncle is red. Recolor and move z up to the grandparent.
                        parent.Color = NodeColor.Black;
                        uncle!.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        z = grandparent;
                    }
                    else
                    {
                        if (z == parent.Right)
                        {
                            // Case 2: Uncle is black; z forms a zig-zag with its parent. Left-rotate the parent to convert to Case 3 (straight line).
                            z = parent;
                            root = LeftRotate(root, z);
                        }

                        // Case 3: Uncle is black; z forms a straight line with its parent. Recolor and right-rotate the grandparent.
                        z.Parent!.Color = NodeColor.Black; // Recolor the parent to black to fix the red-red violation.
                        z.Parent.Parent!.Color = NodeColor.Red; // Change grandparent's color to red before the rotation to maintain the black-height property.
                        root = RightRotate(root, z.Parent.Parent);
                    }
                }
                else
                {
                    // Mirror of the above with left and right swapped.
                    var uncle = grandparent.Left;

                    if (GetColor(uncle) == NodeColor.Red)
                    {
                        // Case 1 (mirror).
                        parent.Color = NodeColor.Black;
                        uncle!.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        z = grandparent;
                    }
                    else
                    {
                        if (z == parent.Left)
                        {
                            // Case 2 (mirror): z forms a zig-zag with its parent. Right-rotate the parent to convert to Case 3 (straight line).
                            z = parent;
                            root = RightRotate(root, z);
                        }

                        // Case 3 (mirror): z forms a straight line with its parent. Recolor and left-rotate the grandparent.
                        z.Parent!.Color = NodeColor.Black;
                        z.Parent.Parent!.Color = NodeColor.Red;
                        root = LeftRotate(root, z.Parent.Parent);
                    }
                }
            }

            // Property 2: The root must be black. Case 1 may have recolored the root to red as it propagated upward.
            root.Color = NodeColor.Black;

            return root;
        }

        /// <summary>
        /// Searches for the specified value in the Red-Black Tree.
        ///
        /// <para>
        /// Because a Red-Black Tree is a Binary Search Tree (BST), the search operation is identical to a standard BST search.
        /// Starting from the root, the method compares the target value with the current node's value and recursively traverses either the left or right subtree.
        /// Since the tree is guaranteed to have a height of O(log n), the search operation runs in logarithmic time.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///       / \     / \
        ///      5  15   25 40
        ///     (R) (R) (R) (R)
        ///
        /// Search(25)
        ///
        /// Step 1:
        /// 25 > 20
        /// Move to the right subtree.
        ///
        /// Step 2:
        /// 25 &lt; 30
        /// Move to the left subtree.
        ///
        /// Step 3:
        /// Current node = 25
        ///
        /// Value found.
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n), due to the recursive call stack. </para>
        /// </summary>
        /// <param name="root"> The root of the current subtree. </param>
        /// <param name="value"> The value to search for. </param>
        /// <returns>
        /// The node containing the specified value if found; otherwise, <see langword="null"/>.
        /// </returns>
        public static RedBlackNode? Search(RedBlackNode? root, int value)
        {
            // The subtree is empty or the value has been found.
            if (root is null || root.Value == value)
            {
                return root;
            }

            // Search in the left subtree.
            if (value < root.Value)
            {
                return Search(root.Left, value);
            }

            // Search in the right subtree.
            return Search(root.Right, value);
        }

        /// <summary>
        /// Performs an in-order traversal of the Red-Black Tree.
        ///
        /// <para>
        /// In an in-order traversal, the nodes are visited in the following order:
        /// <list type="number">
        /// <item><description>Left Subtree</description></item>
        /// <item><description>Root Node</description></item>
        /// <item><description>Right Subtree</description></item>
        /// </list>
        ///
        /// Since a Red-Black Tree is a self-balancing Binary Search Tree (BST), an in-order traversal always produces the node values in ascending sorted order.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///       / \     / \
        ///      5  15   25 40
        ///     (R) (R) (R) (R)
        ///
        /// Output:
        /// 5 → 10 → 15 → 20 → 25 → 30 → 40
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h), where h is the height of the tree.
        /// </para>
        /// </summary>
        /// <param name="root"> The root of the current subtree. </param>
        /// <returns> A list containing the node values in in-order. </returns>
        public static List<int> InOrder(RedBlackNode? root)
        {
            var result = new List<int>();
            InOrder(root, result);

            return result;
        }

        private static void InOrder(RedBlackNode? root, List<int> result)
        {
            if (root is null)
            {
                return;
            }

            InOrder(root.Left, result);
            result.Add(root.Value);
            InOrder(root.Right, result);
        }

        /// <summary>
        /// Performs a pre-order traversal of the Red-Black Tree.
        ///
        /// <para>
        /// In a pre-order traversal, the nodes are visited in the following order:
        /// <list type="number">
        /// <item><description>Root Node</description></item>
        /// <item><description>Left Subtree</description></item>
        /// <item><description>Right Subtree</description></item>
        /// </list>
        ///
        /// Pre-order traversal is commonly used to serialize tree structures and to reconstruct trees.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///       / \     / \
        ///      5  15   25 40
        ///     (R) (R) (R) (R)
        ///
        /// Output:
        /// 20 → 10 → 5 → 15 → 30 → 25 → 40
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        /// </summary>
        /// <param name="root"> The root of the current subtree. </param>
        /// <returns> A list containing the node values in pre-order. </returns>
        public static List<int> PreOrder(RedBlackNode? root)
        {
            var result = new List<int>();
            PreOrder(root, result);

            return result;
        }

        private static void PreOrder(RedBlackNode? root, List<int> result)
        {
            if (root is null)
            {
                return;
            }

            result.Add(root.Value);
            PreOrder(root.Left, result);
            PreOrder(root.Right, result);
        }

        /// <summary>
        /// Performs a post-order traversal of the Red-Black Tree.
        ///
        /// <para>
        /// In a post-order traversal, the nodes are visited in the following order:
        /// <list type="number">
        /// <item><description>Left Subtree</description></item>
        /// <item><description>Right Subtree</description></item>
        /// <item><description>Root Node</description></item>
        /// </list>
        ///
        /// Post-order traversal is commonly used when deleting an entire tree, evaluating expression trees, and performing bottom-up processing.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///       / \     / \
        ///      5  15   25 40
        ///     (R) (R) (R) (R)
        ///
        /// Output:
        /// 5 → 15 → 10 → 25 → 40 → 30 → 20
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        /// </summary>
        /// <param name="root">
        /// The root of the current subtree.
        /// </param>
        /// <returns> A list containing the node values in post-order. </returns>
        public static List<int> PostOrder(RedBlackNode? root)
        {
            var result = new List<int>();
            PostOrder(root, result);

            return result;
        }

        private static void PostOrder(RedBlackNode? root, List<int> result)
        {
            if (root is null)
            {
                return;
            }

            PostOrder(root.Left, result);
            PostOrder(root.Right, result);
            result.Add(root.Value);
        }

        /// <summary>
        /// Performs a level-order traversal (Breadth-First Traversal) of the Red-Black Tree.
        ///
        /// <para>
        /// Unlike the depth-first traversals, level-order traversal visits the nodes level by level, starting from the root and progressing toward the leaf nodes.
        /// A <see cref="Queue{T}"/> is used to process nodes in FIFO order.
        /// Level-order traversal is particularly useful for visualizing the balanced structure of a Red-Black Tree.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///       / \     / \
        ///      5  15   25 40
        ///     (R) (R) (R) (R)
        ///
        /// Output:
        /// 20 → 10 → 30 → 5 → 15 → 25 → 40
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(w), where w is the maximum width of the tree.
        /// </para>
        /// </summary>
        /// <param name="root">
        /// The root of the Red-Black Tree.
        /// </param>
        /// <returns> A list containing the node values in level-order. </returns>
        public static List<int> LevelOrder(RedBlackNode? root)
        {
            var result = new List<int>();

            if (root is null)
            {
                return result;
            }

            var queue = new Queue<RedBlackNode>();
            queue.Enqueue(root); // Start with the root node.

            while (queue.Count > 0)
            {
                var current = queue.Dequeue(); // Dequeue the next node to process.
                result.Add(current.Value);

                if (current.Left is not null)
                {
                    queue.Enqueue(current.Left); // Enqueue the left child.
                }

                if (current.Right is not null)
                {
                    queue.Enqueue(current.Right); // Enqueue the right child.
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the node with the smallest value in the specified subtree.
        ///
        /// <para>
        /// Starting from the root of the subtree, the method repeatedly traverses the left child until the leftmost node is reached.
        /// In a Binary Search Tree (BST) and a Red-Black Tree, the leftmost node always contains the minimum value.
        ///
        /// This helper method is primarily used during the deletion of a node that has two children, where the inorder successor is required.
        ///
        /// <b>Example</b>
        /// <code>
        ///      30(B)
        ///     /
        ///   20(B)
        ///   /
        /// 10(R)
        ///
        /// Minimum Value Node = 10
        /// </code>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The root of the subtree. </param>
        /// <returns> The node containing the minimum value. </returns>
        private static RedBlackNode GetMinValueNode(RedBlackNode root)
        {
            var current = root;

            while (current.Left is not null)
            {
                current = current.Left;
            }

            return current;
        }

        /// <summary>
        /// Replaces the subtree rooted at <paramref name="target"/> with the subtree rooted at <paramref name="replacement"/>.
        ///
        /// <para>
        /// This helper is used during deletion to detach a node from its parent and attach a replacement subtree in its place.
        /// It only updates the link from <paramref name="target"/>'s parent to <paramref name="replacement"/> and sets <paramref name="replacement"/>'s parent pointer.
        /// The child pointers of <paramref name="replacement"/> are not modified.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="target"> The node being replaced. </param>
        /// <param name="replacement"> The replacement node (can be <see langword="null"/>). </param>
        /// <returns> The root of the tree after the replacement. </returns>
        private static RedBlackNode? Transplant(RedBlackNode? root, RedBlackNode target, RedBlackNode? replacement)
        {
            if (target.Parent is null)
            {
                root = replacement; // target was the tree root; replacement becomes the new root.
            }
            else if (target == target.Parent.Left)
            {
                target.Parent.Left = replacement;
            }
            else
            {
                target.Parent.Right = replacement;
            }

            if (replacement is not null)
            {
                replacement.Parent = target.Parent; // Link replacement into the tree where target used to sit.
            }

            return root;
        }

        /// <summary>
        /// Deletes the specified value from the Red-Black Tree while preserving all Red-Black properties.
        ///
        /// <para>
        /// The deletion process consists of two stages:
        /// <list type="number">
        /// <item> <description> Perform a standard Binary Search Tree (BST) deletion using the transplant operation. Keep track of the node that is "physically" removed and its original color. </description> </item>
        /// <item> <description> If the removed node was black, invoke <c>DeleteFixup</c> to restore the black-height property along the affected path. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// During BST deletion, one of the following cases occurs:
        /// <list type="bullet">
        /// <item> <description> The node has no left child. Replace it with its right child. </description> </item>
        /// <item> <description> The node has no right child. Replace it with its left child. </description> </item>
        /// <item> <description> The node has two children. Replace it with its inorder successor (the minimum of the right subtree). The successor takes over the deleted node's slot and inherits the deleted node's color, so the color at that position is preserved — only the successor's <i>original</i> color matters for the fix-up. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// Deleting a red node cannot violate any Red-Black property; the fix-up is only needed when a black node is physically removed, because that reduces the black-height on one path by one. Rebalancing is delegated to <see cref="DeleteFixup"/>, which walks upward from the affected path and restores the black-height property through recoloring and rotations — see there for the case-by-case diagrams.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Initial Red-Black Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   30(B)
        ///                 \
        ///                 40(R)
        ///
        /// Delete(30)
        /// The removed color is black, so a fix-up is required.
        ///
        /// Final Tree:
        ///
        ///          20(B)
        ///         /    \
        ///       10(B)   40(B)
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n), due to the recursive search for the target node. The fix-up itself uses O(1) space via parent pointers. </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="value"> The value to delete. </param>
        /// <returns> The root of the tree after the deletion. </returns>
        public static RedBlackNode? Delete(RedBlackNode? root, int value)
        {
            // Step 1: Locate the node to delete.
            var nodeToDelete = Search(root, value);

            if (nodeToDelete is null)
            {
                return root; // Value not found. Nothing to delete.
            }

            // Track the node that is physically spliced out of the tree (removedNode), its original color, the node that ends up in the removed node's position (replacementNode), and that replacement's parent (replacementParent).
            // The replacement may be null, so replacementParent is tracked separately because null nodes cannot expose a Parent pointer.
            var removedNode = nodeToDelete;
            var removedNodeOriginalColor = removedNode.Color;
            RedBlackNode? replacementNode;
            RedBlackNode? replacementParent;

            if (nodeToDelete.Left is null)
            {
                // Case 1: nodeToDelete has no left child. Replace it with its right subtree.
                replacementNode = nodeToDelete.Right;
                replacementParent = nodeToDelete.Parent;
                root = Transplant(root, nodeToDelete, nodeToDelete.Right);
            }
            else if (nodeToDelete.Right is null)
            {
                // Case 2: nodeToDelete has no right child. Replace it with its left subtree.
                replacementNode = nodeToDelete.Left;
                replacementParent = nodeToDelete.Parent;
                root = Transplant(root, nodeToDelete, nodeToDelete.Left);
            }
            else
            {
                // Case 3: nodeToDelete has two children. Find the inorder successor, splice it out, and move it into nodeToDelete's position.
                removedNode = GetMinValueNode(nodeToDelete.Right); // The inorder successor is the minimum node in the right subtree.
                removedNodeOriginalColor = removedNode.Color;
                replacementNode = removedNode.Right; // The successor cannot have a left child, so its right child (which may be null) will replace it.

                if (removedNode.Parent == nodeToDelete)
                {
                    // The successor is nodeToDelete's immediate right child. After promoting the successor, it becomes the replacement's parent.
                    replacementParent = removedNode;
                }
                else
                {
                    // The successor is deeper in nodeToDelete's right subtree. So, we will first replace the successor (stored in removedNode at this point) with its right child, then move the successor into nodeToDelete's position.
                    replacementParent = removedNode.Parent;
                    root = Transplant(root, removedNode, removedNode.Right); // Replace the successor with its right child (which may be null).
                    removedNode.Right = nodeToDelete.Right; // Move nodeToDelete's right subtree to be the right child of the successor.
                    removedNode.Right!.Parent = removedNode; // Update the parent pointer of the right child to point to the successor.
                }

                root = Transplant(root, nodeToDelete, removedNode); // Now, move the successor into nodeToDelete's position.
                removedNode.Left = nodeToDelete.Left;
                removedNode.Left!.Parent = removedNode;
                removedNode.Color = nodeToDelete.Color; // Preserve nodeToDelete's color at its position; only the removed node's original color affects the fix-up.
            }

            // Step 2: If a black node was physically removed, the black-height property may have been violated. Restore it.
            if (removedNodeOriginalColor == NodeColor.Black)
            {
                root = DeleteFixup(root, replacementNode, replacementParent);
            }

            return root;
        }

        /// <summary>
        /// Restores the Red-Black properties after a deletion that removed a black node.
        ///
        /// <para>
        /// Removing a black node reduces the black-height on the affected path by one. The fix-up conceptually assigns an "extra black" to the replacement node (<paramref name="extraBlackNode"/>) and walks upward, applying one of four cases at each step to redistribute the extra black until the tree is balanced again.
        /// </para>
        ///
        /// <b>Fix-up Cases</b>
        /// <para>
        /// Throughout the diagrams below, <c>x</c> denotes the extra-black node (the replacement that carries the "extra black" left behind by the removed node), <c>P</c> its parent, and <c>S</c> the sibling (<c>P</c>'s other child). <c>SL</c> and <c>SR</c> are the sibling's left and right children (the "nephews"); when <c>x</c> is a left child, <c>SL</c> is the "near" nephew and <c>SR</c> is the "far" nephew. <c>(BB)</c> marks a node carrying the doubly-black overload the fix-up is trying to shed. Labels stay attached to the same physical node across Before and After. The cases apply symmetrically when <c>x</c> is a right child.
        /// </para>
        ///
        /// <para>
        /// <b>A note on "extra black":</b> the extra black is bookkeeping only — it lives in the algorithm's cursor variable, not in any node's <c>Color</c> field. Whenever a case description says "x sheds its extra black" or "the extra black moves to P", that is a shift of the cursor, not a color write on the node. Real color writes only happen where the code says so explicitly. The extra black ultimately disappears in one of two ways: (a) it reaches a red ancestor, which the final cleanup line then recolors black (the identity <i>red + extra-black = black</i>), or (b) it reaches the root, where it can be safely discarded because the whole tree lost a black uniformly.
        /// </para>
        ///
        /// <para>
        /// <b>Case 1: Sibling S is red.</b>
        /// <code>
        /// Before:                          After Left Rotate (P) + recolor S->B, P->R:
        ///        P(B)                              S(B)
        ///       /   \                             /   \
        ///    x(BB)   S(R)                      P(R)   SR(B)
        ///            /  \                     /   \
        ///         SL(B) SR(B)              x(BB)  SL(B)     <- SL is x's new sibling; falls into Case 2, 3, or 4
        /// </code>
        /// Case 1 is a <b>setup case</b>: it doesn't cure the deficit on its own — it rearranges the neighborhood so that one of Cases 2, 3, or 4 applies immediately after. Because <c>S</c> is red, <c>P</c> must be black (property 4 forbids two consecutive reds). Recolor <c>S</c> black and <c>P</c> red, then left-rotate <c>P</c> so that <c>S</c> rises to <c>P</c>'s old position. <c>x</c>'s new sibling is the former near nephew (<c>SL</c>), which is guaranteed black — so the fix-up now falls into Case 2, 3, or 4.
        /// </para>
        ///
        /// <para>
        /// <b>Case 2: Sibling S is black, and both nephews are black.</b>
        /// <code>
        /// Before:                          After recolor S->R:
        ///        P(?)                              P(?+B)    <- P absorbs the extra black; loop continues from P
        ///       /   \                             /   \
        ///    x(BB)   S(B)                      x(B)   S(R)
        ///            /  \                             /  \
        ///         SL(B) SR(B)                      SL(B) SR(B)
        /// </code>
        /// The only code-level operation here is recoloring <c>S</c> red — no rotation happens, and <c>x</c>'s color field is not written. Making <c>S</c> red subtracts one black from the S-side path (matching <c>x</c>'s shorter black-height), but the whole subtree at <c>P</c> is now one black short compared to siblings higher up, so the imbalance has effectively moved up to <c>P</c>. The loop restarts with <c>P</c> as the new cursor: if <c>P</c> was red, the loop exits immediately (the cursor must be black to continue) and the final "residual extra black" step recolors <c>P</c> black — that is where the extra black is actually cancelled. If <c>P</c> was black, the deficit propagates further upward.
        /// </para>
        ///
        /// <para>
        /// <b>Case 3: Sibling S is black, near nephew SL is red, far nephew SR is black.</b>
        /// <code>
        /// Before:                          After Right Rotate (S) + recolor SL->B, S->R:
        ///        P(?)                              P(?)
        ///       /   \                             /   \
        ///    x(BB)   S(B)                      x(BB)  SL(B)     <- SL is x's new sibling; falls into Case 4
        ///            /  \                                 \
        ///         SL(R) SR(B)                             S(R)
        ///                                                    \
        ///                                                    SR(B)
        /// </code>
        /// Like Case 1, Case 3 is a <b>setup case</b> — it converts a "near-red, far-black" nephew configuration into a "far-red" configuration that Case 4 can absorb. Recolor <c>SL</c> black and <c>S</c> red, then right-rotate <c>S</c> so that <c>SL</c> rises into <c>S</c>'s old position. <c>x</c>'s new sibling is now <c>SL</c> (black), and its far nephew is <c>S</c> (red) — Case 4 applies immediately.
        /// </para>
        ///
        /// <para>
        /// <b>Case 4: Sibling S is black and far nephew SR is red.</b>
        /// <code>
        /// Before:                          After Left Rotate (P) + recolor S->c, P->B, SR->B:
        ///        P(c)                              S(c)          <- S inherits P's original color
        ///       /   \                             /   \
        ///    x(BB)   S(B)                      P(B)   SR(B)
        ///            /  \                     /   \
        ///         SL(?) SR(R)              x(B)   SL(?)          <- deficit resolved; loop terminates
        /// </code>
        /// Case 4 is where the deficit is finally cured, so the loop terminates. Four code operations happen: <c>S</c> takes on <c>P</c>'s original color; <c>P</c> and <c>SR</c> are forced black; <c>P</c> is left-rotated so <c>S</c> becomes the new subtree root and <c>P</c> becomes <c>x</c>'s parent. The key point: <c>P</c>, now black, sits between <c>S</c> and <c>x</c> — which adds one black to <c>x</c>'s path, replacing the black that was missing (this is what "absorbing the extra black" means). Forcing <c>SR</c> black at the same time keeps <c>S</c>'s side balanced. <c>x</c>'s color field itself is not written.
        /// </para>
        ///
        /// <para>
        /// After the loop exits, any leftover extra black on <c>x</c> is discarded by unconditionally coloring <c>x</c> black. This handles two termination scenarios: (a) Case 2 propagated the deficit up to a red node — the loop condition fails and this line applies the identity <i>red + extra-black = black</i> to actually cancel the deficit; (b) <c>x</c> reached the tree root — the whole tree lost one black uniformly, so coloring the root black is safe (either a no-op or the effective discard).
        /// </para>
        /// </summary>
        /// <param name="root"> The current root of the tree. </param>
        /// <param name="extraBlackNode"> The node that replaced the deleted node and now carries the "extra black" (may be <see langword="null"/>). </param>
        /// <param name="extraBlackParent"> The parent of <paramref name="extraBlackNode"/> (tracked explicitly because <paramref name="extraBlackNode"/> may be <see langword="null"/>). </param>
        /// <returns> The root of the tree after the fix-up. </returns>
        private static RedBlackNode? DeleteFixup(RedBlackNode? root, RedBlackNode? extraBlackNode, RedBlackNode? extraBlackParent)
        {
            // Continue while the extra-black node still carries an extra black (either it's a doubly-black null leaf, or it's a black node) and it is not the root.
            while (extraBlackNode != root && GetColor(extraBlackNode) == NodeColor.Black)
            {
                // extraBlackParent must exist here: if the current node is not the root, it has a parent (even a null current node's parent is tracked explicitly).
                var parent = extraBlackParent!;

                if (extraBlackNode == parent.Left)
                {
                    var sibling = parent.Right!; // Sibling must exist to maintain black-height on the other side.

                    if (sibling.Color == NodeColor.Red)
                    {
                        // Case 1: Sibling is red. Rotate to make it black, then re-evaluate.
                        sibling.Color = NodeColor.Black;
                        parent.Color = NodeColor.Red;
                        root = LeftRotate(root!, parent);
                        sibling = parent.Right!;
                    }

                    if (GetColor(sibling.Left) == NodeColor.Black && GetColor(sibling.Right) == NodeColor.Black)
                    {
                        // Case 2: Both nephews are black. Push the extra black up to the parent.
                        sibling.Color = NodeColor.Red;
                        extraBlackNode = parent;
                        extraBlackParent = extraBlackNode.Parent;
                    }
                    else
                    {
                        if (GetColor(sibling.Right) == NodeColor.Black)
                        {
                            // Case 3: Near nephew (sibling.Left) is red, far nephew (sibling.Right) is black. Rotate sibling right to convert to Case 4.
                            sibling.Left!.Color = NodeColor.Black;
                            sibling.Color = NodeColor.Red;
                            root = RightRotate(root!, sibling);
                            sibling = parent.Right!;
                        }

                        // Case 4: Far nephew is red. Absorb the extra black by rotating the parent left.
                        sibling.Color = parent.Color;
                        parent.Color = NodeColor.Black;
                        sibling.Right!.Color = NodeColor.Black;
                        root = LeftRotate(root!, parent);
                        extraBlackNode = root; // Terminate the loop.
                        extraBlackParent = null;
                    }
                }
                else
                {
                    // Mirror of the above with left and right swapped.
                    var sibling = parent.Left!;

                    if (sibling.Color == NodeColor.Red)
                    {
                        // Case 1 (mirror).
                        sibling.Color = NodeColor.Black;
                        parent.Color = NodeColor.Red;
                        root = RightRotate(root!, parent);
                        sibling = parent.Left!;
                    }

                    if (GetColor(sibling.Right) == NodeColor.Black && GetColor(sibling.Left) == NodeColor.Black)
                    {
                        // Case 2 (mirror).
                        sibling.Color = NodeColor.Red;
                        extraBlackNode = parent;
                        extraBlackParent = extraBlackNode.Parent;
                    }
                    else
                    {
                        if (GetColor(sibling.Left) == NodeColor.Black)
                        {
                            // Case 3 (mirror).
                            sibling.Right!.Color = NodeColor.Black;
                            sibling.Color = NodeColor.Red;
                            root = LeftRotate(root!, sibling);
                            sibling = parent.Left!;
                        }

                        // Case 4 (mirror).
                        sibling.Color = parent.Color;
                        parent.Color = NodeColor.Black;
                        sibling.Left!.Color = NodeColor.Black;
                        root = RightRotate(root!, parent);
                        extraBlackNode = root;
                        extraBlackParent = null;
                    }
                }
            }

            if (extraBlackNode is not null)
            {
                extraBlackNode.Color = NodeColor.Black; // Discard any residual extra black.
            }

            return root;
        }
    }
}
