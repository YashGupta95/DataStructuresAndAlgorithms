using System;
using System.Collections.Generic;

namespace DataStructures.Trees.BinaryTree
{
    // A Binary Tree is a hierarchical data structure in which every node has AT MOST two children,
    // conventionally called the LEFT child and the RIGHT child. Unlike a Binary Search Tree, a
    // plain Binary Tree does NOT impose any ordering on the values — it is purely a shape.
    //
    // Binary Trees are the foundation on which many other tree data structures are built:
    // Binary Search Trees, AVL Trees, Red-Black Trees, and Heaps are all specialized Binary Trees
    // with extra rules layered on top.
    //
    // -----------------------------------------------------------------------
    // Common Terminology
    // -----------------------------------------------------------------------
    //   • ROOT      — the topmost node; the only one with no parent.
    //   • LEAF      — a node with no children.
    //   • INTERNAL  — a node with at least one child.
    //   • HEIGHT    — the length of the longest path from a node down to a leaf.
    //   • DEPTH     — the length of the path from the root down to a node.
    //   • LEVEL     — depth + 1 (root is at level 1).
    //   • COMPLETE  — every level is fully filled except possibly the last, which is filled from
    //                  the left. This implementation builds complete binary trees via level-order
    //                  insertion.
    //   • FULL      — every internal node has exactly two children.
    //   • PERFECT   — full AND every leaf is at the same depth.
    //
    // -----------------------------------------------------------------------
    // Traversals
    // -----------------------------------------------------------------------
    // Because a Binary Tree has no ordering, walking it in a well-defined order is a common task.
    // There are four standard traversals:
    //
    //   • IN-ORDER    — Left, Root, Right    (depth-first)
    //   • PRE-ORDER   — Root, Left, Right    (depth-first)
    //   • POST-ORDER  — Left, Right, Root    (depth-first)
    //   • LEVEL-ORDER — visit every node at depth d before any node at depth d+1
    //                   (breadth-first; uses a queue).
    //
    // -----------------------------------------------------------------------
    // Time and Space
    // -----------------------------------------------------------------------
    // For a Binary Tree with n nodes:
    //   • Traversals — O(n) time, O(h) space where h is the height.
    //   • Height h   — ranges from ⌈log₂(n+1)⌉ (perfect) up to n (degenerate "vine" of only
    //                  left or only right children).
    //
    // Because a general Binary Tree can degenerate into a line, most data-structure use cases
    // rely on one of its balanced descendants (BST, AVL, RB) rather than the plain form.
    // =============================================================================================
    /// <remarks>
    /// This implementation uses level-order insertion to create a complete binary tree structure for demonstration. Traversal methods return lists to separate algorithm behavior from I/O.
    /// </remarks>
    internal class BinaryTreeOperations
    {
        private BinaryTreeNode root;

        /// <summary>
        /// Gets the root node of the binary tree.
        /// </summary>
        public BinaryTreeNode Root => root;

        public BinaryTreeOperations()
        {
            root = null;
        }

        /// <summary>
        /// Inserts a value into the tree using level-order insertion.
        /// </summary>
        /// <remarks>
        /// Level-order insertion places a new value at the first available position from left to right. This keeps the tree as complete as possible when the tree is built.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinaryTree();
        /// tree.Insert(10);
        /// tree.Insert(20);
        /// tree.Insert(30);
        /// // Tree now contains 10 as root, 20 as left child, 30 as right child.
        /// </code>
        /// </example>
        /// <param name="value">The value to insert.</param>
        /// <returns>None.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public void Insert(int value)
        {
            root = Insert(root, value);
        }

        private static BinaryTreeNode Insert(BinaryTreeNode node, int value)
        {
            if (node == null)
            {
                return new BinaryTreeNode(value);
            }

            var queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Left == null)
                {
                    current.Left = new BinaryTreeNode(value);
                    break;
                }

                queue.Enqueue(current.Left);

                if (current.Right == null)
                {
                    current.Right = new BinaryTreeNode(value);
                    break;
                }

                queue.Enqueue(current.Right);
            }

            return node;
        }

        /// <summary>
        /// Searches the tree for the specified value.
        /// </summary>
        /// <remarks>
        /// The search operation performs a depth-first traversal of the tree. This implementation examines the left subtree first and returns the first matching node.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinaryTree();
        /// tree.CreateTree();
        /// var node = tree.Search(40);
        /// </code>
        /// </example>
        /// <param name="value">The value to search for.</param>
        /// <returns>The first node containing the value if found; otherwise, null.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public BinaryTreeNode Search(int value)
        {
            return Search(root, value);
        }

        private static BinaryTreeNode Search(BinaryTreeNode node, int value)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Value == value)
            {
                return node;
            }

            var leftResult = Search(node.Left, value);
            return leftResult ?? Search(node.Right, value);
        }

        /// <summary>
        /// Deletes the specified value from the tree.
        /// </summary>
        /// <remarks>
        /// Deletion replaces the node to remove with the deepest rightmost node in the tree, then removes that deepest node. This preserves the complete tree structure.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinaryTree();
        /// tree.CreateTree();
        /// var removed = tree.Delete(20);
        /// </code>
        /// </example>
        /// <param name="value">The value to delete.</param>
        /// <returns>True if the value was removed; otherwise, false.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public bool Delete(int value)
        {
            if (root == null)
            {
                return false;
            }

            var nodeToDelete = Search(root, value);
            if (nodeToDelete == null)
            {
                return false;
            }

            if (root.Left == null && root.Right == null && root.Value == value)
            {
                root = null;
                return true;
            }

            var (deepestNode, parentOfDeepest) = GetDeepestNode();
            nodeToDelete.Value = deepestNode.Value;

            if (parentOfDeepest.Left == deepestNode)
            {
                parentOfDeepest.Left = null;
            }
            else
            {
                parentOfDeepest.Right = null;
            }

            return true;
        }

        private (BinaryTreeNode deepestNode, BinaryTreeNode parentOfDeepest) GetDeepestNode()
        {
            var queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            BinaryTreeNode current = null;
            BinaryTreeNode parent = null;

            while (queue.Count > 0)
            {
                current = queue.Dequeue();

                if (current.Left != null)
                {
                    parent = current;
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    parent = current;
                    queue.Enqueue(current.Right);
                }
            }

            return (current, parent);
        }

        /// <summary>
        /// Returns the values from an in-order traversal.
        /// </summary>
        /// <remarks>
        /// In-order traversal visits the left subtree first, then the current node, then the right subtree. For a binary search tree, this produces sorted order.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinaryTree();
        /// tree.CreateTree();
        /// var values = tree.InOrder();
        /// </code>
        /// </example>
        /// <returns>A list of values visited in in-order.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public List<int> InOrder()
        {
            var result = new List<int>();
            InOrder(root, result);
            return result;
        }

        private static void InOrder(BinaryTreeNode node, List<int> result)
        {
            if (node == null)
            {
                return;
            }

            InOrder(node.Left, result);
            result.Add(node.Value);
            InOrder(node.Right, result);
        }

        /// <summary>
        /// Returns the values from a pre-order traversal.
        /// </summary>
        /// <remarks>
        /// Pre-order traversal visits the current node first, then the left subtree, then the right subtree. It is useful for serializing tree structure.
        /// </remarks>
        /// <example>
        /// <code>
        /// var values = tree.PreOrder();
        /// </code>
        /// </example>
        /// <returns>A list of values visited in pre-order.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public List<int> PreOrder()
        {
            var result = new List<int>();
            PreOrder(root, result);
            return result;
        }

        private static void PreOrder(BinaryTreeNode node, List<int> result)
        {
            if (node == null)
            {
                return;
            }

            result.Add(node.Value);
            PreOrder(node.Left, result);
            PreOrder(node.Right, result);
        }

        /// <summary>
        /// Returns the values from a post-order traversal.
        /// </summary>
        /// <remarks>
        /// Post-order traversal visits the left subtree first, then the right subtree, and then the current node. It is useful for deleting or freeing tree nodes.
        /// </remarks>
        /// <example>
        /// <code>
        /// var values = tree.PostOrder();
        /// </code>
        /// </example>
        /// <returns>A list of values visited in post-order.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public List<int> PostOrder()
        {
            var result = new List<int>();
            PostOrder(root, result);
            return result;
        }

        private static void PostOrder(BinaryTreeNode node, List<int> result)
        {
            if (node == null)
            {
                return;
            }

            PostOrder(node.Left, result);
            PostOrder(node.Right, result);
            result.Add(node.Value);
        }

        /// <summary>
        /// Returns the values from a level-order traversal.
        /// </summary>
        /// <remarks>
        /// Level-order traversal visits nodes breadth-first, one level at a time. It is useful for inspecting the tree structure and measuring completeness.
        /// </remarks>
        /// <example>
        /// <code>
        /// var values = tree.LevelOrder();
        /// </code>
        /// </example>
        /// <returns>A list of values visited in level-order.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public List<int> LevelOrder()
        {
            var result = new List<int>();

            if (root == null)
            {
                return result;
            }

            var queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current.Value);

                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the height of the tree.
        /// </summary>
        /// <remarks>
        /// The height is defined as the number of nodes along the longest path from the root node down to the farthest leaf node.
        /// </remarks>
        /// <returns>The height of the tree, or 0 if the tree is empty.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public int Height()
        {
            return Height(root);
        }

        private static int Height(BinaryTreeNode node)
        {
            if (node == null)
            {
                return 0;
            }

            var leftHeight = Height(node.Left);
            var rightHeight = Height(node.Right);
            return Math.Max(leftHeight, rightHeight) + 1;
        }

        /// <summary>
        /// Returns the total number of nodes in the tree.
        /// </summary>
        /// <remarks>
        /// This method visits every node and counts each one recursively.
        /// </remarks>
        /// <returns>The count of nodes.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public int CountNodes()
        {
            return CountNodes(root);
        }

        private static int CountNodes(BinaryTreeNode node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }

        /// <summary>
        /// Returns the total number of leaf nodes in the tree.
        /// </summary>
        /// <remarks>
        /// A leaf node is a node with no left or right children. This counts all such nodes recursively.
        /// </remarks>
        /// <returns>The count of leaf nodes.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(n) </para>
        public int CountLeaves()
        {
            return CountLeaves(root);
        }

        private static int CountLeaves(BinaryTreeNode node)
        {
            if (node == null)
            {
                return 0;
            }

            if (node.Left == null && node.Right == null)
            {
                return 1;
            }

            return CountLeaves(node.Left) + CountLeaves(node.Right);
        }

        /// <summary>
        /// Returns true if the tree is empty.
        /// </summary>
        /// <remarks>
        /// This is a constant-time check on the root reference.
        /// </remarks>
        /// <returns>True when the tree has no nodes; otherwise, false.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        public bool IsEmpty()
        {
            return root == null;
        }

        /// <summary>
        /// Clears the binary tree.
        /// </summary>
        /// <remarks>
        /// Clearing the tree removes the root reference, allowing the garbage collector to reclaim all nodes.
        /// </remarks>
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        public void Clear()
        {
            root = null;
        }

        /// <summary>
        /// Creates a sample binary tree for demonstration purposes.
        /// </summary>
        /// <remarks>
        /// This method inserts a fixed set of integer values in level order so that the tree is easy to inspect and demonstrate traversal algorithms.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinaryTree();
        /// tree.CreateTree();
        /// </code>
        /// </example>
        public void CreateTree()
        {
            root = null;

            var values = new[] { 10, 20, 30, 40, 50, 60 };
            foreach (var value in values)
            {
                Insert(value);
            }
        }
    }
}
