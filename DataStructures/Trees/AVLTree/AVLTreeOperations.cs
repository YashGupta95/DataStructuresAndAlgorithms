namespace DataStructures.Trees.AVLTree
{
    internal class AVLTreeOperations
    {
        /// <summary>
        /// Returns the height of the specified AVL tree node.
        ///
        /// <para>
        /// The height of a node is defined as the number of nodes on the longest path from the node to a leaf.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        ///      30
        ///     /
        ///   20
        ///
        /// Height(30) = 2
        /// Height(20) = 1
        /// Height(null) = 0
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="node">
        /// The node whose height is to be returned.
        /// </param>
        /// <returns> The height of the specified node, or 0 if the node is null. </returns>
        private static int Height(AVLNode? node)
        {
            return node?.Height ?? 0;
        }

        /// <summary>
        /// Calculates the balance factor of the specified AVL tree node.
        ///
        /// <para>
        /// The balance factor is defined as: Height(Left Subtree) - Height(Right Subtree)
        /// An AVL tree remains balanced as long as the balance factor of every node is one of the following:
        /// • -1
        /// •  0
        /// • +1
        ///
        /// Any value outside this range indicates that a rotation is required to restore balance.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        ///      30
        ///     /
        ///   20
        ///
        /// Height(Left)  = 1
        /// Height(Right) = 0
        ///
        /// Balance = 1
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="node">
        /// The node whose balance factor is to be calculated.
        /// </param>
        /// <returns>
        /// The balance factor of the node. Returns 0 if the node is null.
        /// </returns>
        private static int GetBalance(AVLNode? node)
        {
            if (node is null)
            {
                return 0;
            }

            return Height(node.Left) - Height(node.Right);
        }

        /// <summary>
        /// Performs a right rotation on the specified subtree.
        ///
        /// <para>
        /// A right rotation is performed when the left subtree becomes heavier than the right subtree, such as in the Left-Left (LL) imbalance case.
        ///
        /// Before Rotation:
        /// <code>
        ///        40
        ///       / 
        ///      30
        ///     /  \ 
        ///    10  35
        /// </code>
        ///
        /// After Rotation:
        /// <code>
        ///        30
        ///       /  \
        ///     10   40
        ///         /  \
        ///       35    .
        /// </code>
        ///
        /// After the rotation:
        /// <list type="bullet">
        /// <item>
        /// <description> The left child becomes the new root of the subtree. </description>
        /// </item>
        /// <item>
        /// <description> The original root becomes the right child. </description>
        /// </item>
        /// <item>
        /// <description> Heights of the affected nodes are recalculated. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root">
        /// The root of the subtree to rotate.
        /// </param>
        /// <returns> The new root of the rotated subtree. </returns>
        private static AVLNode RightRotate(AVLNode root)
        {
            var newRoot = root.Left!;
            var transferredSubtree = newRoot.Right; // The right child of the new root will be transferred to become the left child of the original root.

            // Perform rotation.
            newRoot.Right = root; // The original root becomes the right child of the new root.
            root.Left = transferredSubtree; // The transferred subtree becomes the left child of the original root.

            // Update heights.
            root.Height = Math.Max(Height(root.Left), Height(root.Right)) + 1;
            newRoot.Height = Math.Max(Height(newRoot.Left), Height(newRoot.Right)) + 1;

            return newRoot;
        }

        /// <summary>
        /// Performs a left rotation on the specified subtree.
        ///
        /// <para>
        /// A left rotation is performed when the right subtree becomes heavier than the left subtree, such as in the Right-Right (RR) imbalance case.
        ///
        /// Before Rotation:
        /// <code>
        ///       20
        ///        \
        ///         40
        ///        /  \
        ///      30   50
        /// </code>
        ///
        /// After Rotation:
        /// <code>
        ///         40
        ///        /  \
        ///      20   50
        ///     /  \
        ///    .   30
        /// </code>
        ///
        /// After the rotation:
        /// <list type="bullet">
        /// <item>
        /// <description> The right child becomes the new root of the subtree. </description>
        /// </item>
        /// <item>
        /// <description> The original root becomes the left child. </description>
        /// </item>
        /// <item>
        /// <description> Heights of the affected nodes are recalculated. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root">
        /// The root of the subtree to rotate.
        /// </param>
        /// <returns> The new root of the rotated subtree. </returns>
        private static AVLNode LeftRotate(AVLNode root)
        {
            var newRoot = root.Right!;
            var transferredSubtree = newRoot.Left; // The left child of the new root will be transferred to become the right child of the original root.

            // Perform rotation.
            newRoot.Left = root; // The original root becomes the left child of the new root.
            root.Right = transferredSubtree; // The transferred subtree becomes the right child of the original root.

            // Update heights.
            root.Height = Math.Max(Height(root.Left), Height(root.Right)) + 1;
            newRoot.Height = Math.Max(Height(newRoot.Left), Height(newRoot.Right)) + 1;

            return newRoot;
        }

        /// <summary>
        /// Inserts a new value into the AVL tree while maintaining its self-balancing property.
        ///
        /// <para>
        /// The insertion process consists of four stages:
        /// <list type="number">
        /// <item> <description> Perform a standard Binary Search Tree (BST) insertion. </description> </item>
        /// <item> <description> Update the height of each ancestor node. </description> </item>
        /// <item> <description> Calculate the balance factor to determine whether the subtree has become unbalanced. </description> </item>
        /// <item> <description> Perform the appropriate rotation, if required, to restore the AVL balance property. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// An AVL tree allows only the following balance factors:
        /// <list type="bullet">
        /// <item><description>-1</description></item>
        /// <item><description>0</description></item>
        /// <item><description>+1</description></item>
        /// </list>
        ///
        /// Any node with a balance factor outside this range is rebalanced using one of the four AVL rotation cases.
        /// </para>
        /// <b>Rotation Cases</b>
        /// <para>
        /// <b>1. Left-Left (LL)</b>
        /// <code>
        /// Before:
        ///        30
        ///       /
        ///     20
        ///    /
        ///  10
        ///
        /// After Right Rotation:
        ///      20
        ///     /  \
        ///   10   30
        /// </code>
        /// </para>
        ///
        /// <para>
        /// <b>2. Right-Right (RR)</b>
        /// <code>
        /// Before:
        /// 10
        ///   \
        ///    20
        ///      \
        ///      30
        ///
        /// After Left Rotation:
        ///      20
        ///     /  \
        ///   10   30
        /// </code>
        /// </para>
        ///
        /// <para>
        /// <b>3. Left-Right (LR)</b>
        /// <code>
        /// Before:
        ///      30
        ///     /
        ///   10
        ///      \
        ///      20
        ///
        /// Step 1: Left Rotate (10)
        ///
        ///      30
        ///     /
        ///    20
        ///   /
        /// 10
        ///
        /// Step 2: Right Rotate (30)
        ///
        ///      20
        ///     /  \
        ///   10   30
        /// </code>
        /// </para>
        ///
        /// <para>
        /// <b>4. Right-Left (RL)</b>
        /// <code>
        /// Before:
        /// 10
        ///   \
        ///    30
        ///   /
        /// 20
        ///
        /// Step 1: Right Rotate (30)
        ///
        /// 10
        ///   \
        ///    20
        ///      \
        ///      30
        ///
        /// Step 2: Left Rotate (10)
        ///
        ///      20
        ///     /  \
        ///   10   30
        /// </code>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n), due to the recursive call stack. </para>
        /// </summary>
        /// <param name="root"> The root of the current subtree. </param>
        /// <param name="value"> The value to insert. </param>
        /// <returns> The root of the balanced subtree after insertion. </returns>
        public static AVLNode Insert(AVLNode? root, int value)
        {
            // Step 1: Perform standard BST insertion.
            if (root is null)
            {
                return new AVLNode(value);
            }

            if (value < root.Value)
            {
                root.Left = Insert(root.Left, value);
            }
            else if (value > root.Value)
            {
                root.Right = Insert(root.Right, value);
            }
            else
            {
                return root; // Duplicate values are not allowed in this AVL Tree.
            }

            // Step 2: Update the height of the current node.
            root.Height = Math.Max(Height(root.Left), Height(root.Right)) + 1;

            // Step 3: Calculate the balance factor.
            var balance = GetBalance(root);

            // Step 4: Perform rotations if the tree becomes unbalanced.

            // Left-Left (LL) Case.
            if (balance > 1 && value < root.Left!.Value)
            {
                return RightRotate(root); // Perform a right rotation on the current node to restore balance.
            }

            // Right-Right (RR) Case.
            if (balance < -1 && value > root.Right!.Value)
            {
                return LeftRotate(root); // Perform a left rotation on the current node to restore balance.
            }

            // Left-Right (LR) Case.
            if (balance > 1 && value > root.Left!.Value)
            {
                root.Left = LeftRotate(root.Left); // Perform a left rotation on the left child to convert it into a Left-Left case.
                return RightRotate(root); // Perform a right rotation on the current node to restore balance.
            }

            // Right-Left (RL) Case.
            if (balance < -1 && value < root.Right!.Value)
            {
                root.Right = RightRotate(root.Right); // Perform a right rotation on the right child to convert it into a Right-Right case.
                return LeftRotate(root); // Perform a left rotation on the current node to restore balance.
            }

            return root; // The subtree is already balanced.
        }

        /// <summary>
        /// Searches for the specified value in the AVL tree.
        ///
        /// <para>
        /// The search operation in an AVL Tree is identical to that of a Binary Search Tree (BST). 
        /// Starting from the root, the method compares the target value with the current node's value and recursively traverses either the left or right subtree.
        /// Since an AVL Tree maintains a balanced height after every insertion and deletion, the search operation is guaranteed to execute in logarithmic time.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// AVL Tree:
        ///
        ///          20
        ///         /  \
        ///       10    30
        ///      / \    / \
        ///     5  15 25  40
        ///
        /// Search(25)
        ///
        /// Step 1:
        /// 25 > 20
        /// Move to the right subtree.
        ///
        /// Step 2:
        /// 25 < 30
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
        public static AVLNode? Search(AVLNode? root, int value)
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
        /// Performs an in-order traversal of the AVL tree.
        ///
        /// <para>
        /// In an in-order traversal, the nodes are visited in the following order:
        /// <list type="number">
        /// <item><description>Left Subtree</description></item>
        /// <item><description>Root Node</description></item>
        /// <item><description>Right Subtree</description></item>
        /// </list>
        ///
        /// Since an AVL Tree is a self-balancing Binary Search Tree (BST), an in-order traversal always produces the node values in ascending sorted order.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// AVL Tree:
        ///
        ///         20
        ///        /  \
        ///      10    30
        ///     / \    / \
        ///    5  15 25  40
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
        /// <param name="result"> Stores the traversal result. </param>
        public static List<int> InOrder(AVLNode? root)
        {
            var result = new List<int>();
            InOrder(root, result);

            return result;
        }

        private static void InOrder(AVLNode? root, List<int> result)
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
        /// Performs a pre-order traversal of the AVL tree.
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
        /// AVL Tree:
        ///
        ///         20
        ///        /  \
        ///      10    30
        ///     / \    / \
        ///    5  15 25  40
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
        /// <param name="result"> Stores the traversal result. </param>
        public static List<int> PreOrder(AVLNode? root)
        {
            var result = new List<int>();
            PreOrder(root, result);

            return result;
        }

        private static void PreOrder(AVLNode? root, List<int> result)
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
        /// Performs a post-order traversal of the AVL tree.
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
        /// AVL Tree:
        ///
        ///         20
        ///        /  \
        ///      10    30
        ///     / \    / \
        ///    5  15 25  40
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
        /// <param name="result">
        /// Stores the traversal result.
        /// </param>
        public static List<int> PostOrder(AVLNode? root)
        {
            var result = new List<int>();
            PostOrder(root, result);

            return result;
        }

        private static void PostOrder(AVLNode? root, List<int> result)
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
        /// Performs a level-order traversal (Breadth-First Traversal) of the AVL tree.
        ///
        /// <para>
        /// Unlike the depth-first traversals, level-order traversal visits the nodes level by level, starting from the root and progressing toward the leaf nodes.
        /// A <see cref="Queue{T}"/> is used to process nodes in FIFO order.
        /// Level-order traversal is particularly useful for visualizing the balanced structure of an AVL tree.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// AVL Tree:
        ///
        ///         20
        ///        /  \
        ///      10    30
        ///     / \    / \
        ///    5  15 25  40
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
        /// The root of the AVL tree.
        /// </param>
        /// <returns> A list containing the nodes in level-order. </returns>
        public static List<int> LevelOrder(AVLNode? root)
        {
            var result = new List<int>();

            if (root is null)
            {
                return result;
            }

            var queue = new Queue<AVLNode>();
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
        /// In a Binary Search Tree (BST) and an AVL Tree, the leftmost node always contains the minimum value.
        ///
        /// This helper method is primarily used during the deletion of a node that has two children, where the inorder successor is required.
        ///
        /// <b>Example</b>
        /// <code>
        ///      30
        ///     /
        ///   20
        ///   /
        /// 10
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
        private static AVLNode GetMinValueNode(AVLNode root)
        {
            var current = root;

            while (current.Left is not null)
            {
                current = current.Left;
            }

            return current;
        }

        /// <summary>
        /// Deletes the specified value from the AVL tree while maintaining its self-balancing property.
        ///
        /// <para>
        /// The deletion process consists of four stages:
        /// <list type="number">
        /// <item> <description> Perform a standard Binary Search Tree (BST) deletion. </description> </item>
        /// <item> <description> Update the height of each ancestor node. </description> </item>
        /// <item> <description> Calculate the balance factor. </description> </item>
        /// <item> <description> Perform the appropriate rotation to restore AVL balance. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// During BST deletion, one of the following cases occurs:
        ///
        /// <list type="bullet">
        /// <item> <description> Deleting a leaf node.</description> </item>
        /// <item> <description> Deleting a node with one child.</description> </item>
        /// <item> <description> Deleting a node with two children by replacing it with its inorder successor. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Initial AVL Tree:
        ///
        ///         20
        ///        /  \
        ///      10    30
        ///           /  \
        ///         25    40
        ///
        /// Delete(30)
        /// Inorder successor = 40
        ///
        /// Final Tree:
        ///
        ///         20
        ///        /  \
        ///      10    40
        ///           /
        ///         25
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(log n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(log n), due to recursion. </para>
        /// </summary>
        /// <param name="root">
        /// The root of the current subtree.
        /// </param>
        /// <param name="value">
        /// The value to delete.
        /// </param>
        /// <returns>
        /// The root of the balanced subtree.
        /// </returns>
        public static AVLNode? Delete(AVLNode? root, int value)
        {
            // Step 1: Perform standard BST deletion.
            if (root is null)
            {
                return null;
            }

            if (value < root.Value)
            {
                root.Left = Delete(root.Left, value);
            }
            else if (value > root.Value)
            {
                root.Right = Delete(root.Right, value);
            }
            else
            {
                // Case 1 & 2: Node has one child or no child.
                if (root.Left is null)
                {
                    return root.Right;
                }

                if (root.Right is null)
                {
                    return root.Left;
                }

                // Case 3: Node has two children.
                var successor = GetMinValueNode(root.Right); // Find the inorder successor (smallest in the right subtree).

                root.Value = successor.Value; // Replace the value of the node to be deleted with the successor's value.
                root.Right = Delete(root.Right, successor.Value); // Delete the inorder successor.
            }

            // Step 2: Update the height.
            root.Height = Math.Max(Height(root.Left), Height(root.Right)) + 1;

            // Step 3: Calculate the balance factor.
            var balance = GetBalance(root);

            // Step 4: Rebalance the tree if required.
            // Unlike insertion, the rotation case cannot be determined using the deleted value because the subtree structure may have changed. Instead, the balance factor of the child subtree is examined.

            // Left-Left (LL)
            if (balance > 1 && GetBalance(root.Left) >= 0)
            {
                return RightRotate(root);
            }

            // Left-Right (LR)
            if (balance > 1 && GetBalance(root.Left) < 0)
            {
                root.Left = LeftRotate(root.Left!);
                return RightRotate(root);
            }

            // Right-Right (RR)
            if (balance < -1 && GetBalance(root.Right) <= 0)
            {
                return LeftRotate(root);
            }

            // Right-Left (RL)
            if (balance < -1 && GetBalance(root.Right) > 0)
            {
                root.Right = RightRotate(root.Right!);
                return LeftRotate(root);
            }

            return root;
        }
    }
}
