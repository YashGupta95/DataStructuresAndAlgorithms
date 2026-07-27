using System;

namespace DataStructures.Trees.BinarySearchTree
{
    internal class BinarySearchTree
    {
        private Node root;

        public BinarySearchTree()
        {
            root = null;
        }

        /// <summary>
        /// Checks whether the binary search tree contains any nodes.
        /// </summary>
        /// <remarks>
        /// This is a constant-time check using the root reference.
        /// </remarks>
        /// <returns>True if the tree is empty; otherwise, false.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(1) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal bool IsEmpty()
        {
            return (root == null);
        }

        #region Insert a node in BST
        /// <summary>
        /// Inserts a value into the binary search tree using recursion.
        /// </summary>
        /// <remarks>
        /// The insertion follows the BST property: values less than the current node go to the left, while values greater than the current node go to the right.
        /// </remarks>
        /// <example>
        /// <code>
        /// var tree = new BinarySearchTree();
        /// tree.InsertRecursive(50);
        /// tree.InsertRecursive(30);
        /// </code>
        /// </example>
        /// <param name="element">The value to insert.</param>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void InsertRecursive(int element)
        {
            root = Insert(root, element);
        }

        private static Node Insert(Node node, int element)
        {
            if (node == null)
                node = new Node(element);
            else if (element < node.Info)
                node.LeftChild = Insert(node.LeftChild, element);
            else if (element > node.Info)
                node.RightChild = Insert(node.RightChild, element);
            else
                Console.WriteLine($"{element} is already present in tree.");

            return node;
        }

        /// <summary>
        /// Inserts a value into the binary search tree using iteration.
        /// </summary>
        /// <param name="element">The value to insert.</param>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal void InsertIterative(int element)
        {
            var node = root;
            Node parent = null;

            while (node != null)
            {
                parent = node;
                
                if (element < node.Info)
                    node = node.LeftChild;
                else if (element > node.Info)
                    node = node.RightChild;
                else
                {
                    Console.WriteLine($"{element} already present in the tree.");
                    return;
                }
            }

            var temp = new Node(element);

            if (parent == null)
                root = temp;
            else if (element < parent.Info)
                parent.LeftChild = temp;
            else
                parent.RightChild = temp;
        }
        #endregion

        #region Searching a node in BST
        /// <summary>
        /// Searches for a value in the tree using recursion.
        /// </summary>
        /// <param name="element">The value to search for.</param>
        /// <returns>True if the value is found; otherwise, false.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal bool RecursiveSearch(int element)
        {
            return (Search(root, element) != null);
        }

        private static Node Search(Node node, int element)
        {
            // Key not found
            if (node == null)
                return null; 

            // Search in left subtree
            if (element < node.Info)
                return Search(node.LeftChild, element);

            // Search in right subtree
            if (element > node.Info)
                return Search(node.RightChild, element);

            return node;
        }

        /// <summary>
        /// Searches for a value in the tree using iteration.
        /// </summary>
        /// <param name="element">The value to search for.</param>
        /// <returns>True if the value is found; otherwise, false.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal bool IterativeSearch(int element)
        {
            var node = root;

            while (node != null)
            {
                if (element < node.Info)
                    node = node.LeftChild; // Move to left child
                else if (element > node.Info)
                    node = node.RightChild;  // Move to right child
                else
                    return true;
            }

            return false; // Key not found
        }
        #endregion

        #region Deleting a node from BST
        /// <summary>
        /// Deletes a value from the tree using recursion.
        /// </summary>
        /// <remarks>
        /// The method handles three cases: leaf node, node with one child, and node with two children.
        /// When a node has two children, the inorder successor is used to maintain BST properties.
        /// </remarks>
        /// <param name="element">The value to delete.</param>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void DeleteRecursive(int element)
        {
            root = Delete(root, element);
        }

        private static Node Delete(Node node, int element)
        {
            Node child;

            if (node == null)
            {
                Console.WriteLine($"{element} not found.");
                return node;
            }

            // Node will be found and deleted from left subtree
            if (element < node.Info)
                node.LeftChild = Delete(node.LeftChild, element);
            // Node will be found and deleted from left subtree
            else if (element > node.Info)
                node.RightChild = Delete(node.RightChild, element);
            // Key to be deleted is found
            else
            {
                // Case C: Node to be deleted has 2 children
                if (node.LeftChild != null && node.RightChild != null)
                {
                    var successor = node.RightChild;

                    while (successor.LeftChild != null)
                    {
                        // Find the inorder successor (leftmost node in the right subtree)
                        successor = successor.LeftChild;
                    }
                    
                    node.Info = successor.Info;
                    // Delete the inorder successor from the right subtree. Since successor is the left child,
                    // its (possible) right child is automatically reattached to its parent via the recursive return assignment.
                    // Because successor will be deleted in left child's iteration (node.LeftChild = Delete(node.LeftChild, element);)
                    // So the successor's child will be assigned to node.LeftChild in recursive call wind-up.
                    node.RightChild = Delete(node.RightChild, successor.Info);
                }
                // Case B and Case A : Node to be deleted has either 1 or no child
                else
                {
                    // Node has only left child
                    if (node.LeftChild != null)
                        child = node.LeftChild;
                    // Node has only right child or no child
                    else
                        child = node.RightChild;

                    node = child;
                }
            }

            return node;
        }

        /// <summary>
        /// Deletes a value from the tree using iteration.
        /// </summary>
        /// <param name="element">The value to delete.</param>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal void DeleteIterative(int element)
        {
            var node = root;
            Node parent = null;

            while (node != null)
            {
                if (element == node.Info)
                    break;

                parent = node;
                if (element < node.Info)
                    node = node.LeftChild;
                else
                    node = node.RightChild;
            }

            if (node == null)
            {
                Console.WriteLine($"{element} not found in BST.");
                return;
            }

            // Case C: Node to be deleted has 2 children - Find the inorder successor and its parent
            if (node.LeftChild != null && node.RightChild != null)
            {
                var successorParent = node;
                var successor = node.RightChild;

                while (successor.LeftChild != null)
                {
                    successorParent = successor;
                    successor = successor.LeftChild;
                }

                node.Info = successor.Info;
                node = successor;
                parent = successorParent;
            }

            // Case B and Case A : Node to be deleted has either 1 or no child
            Node child;

            // Node to be deleted has a left child
            if (node.LeftChild != null)
                child = node.LeftChild;
            // Node to be deleted has a right child or no child
            else
                child = node.RightChild;

            // Node to be deleted is the root node
            if (parent == null)
                root = child;
            // For Case C : 'node' will be successor and 'parent' will be successor's parent.
            // Since node is the left child of its parent, successor's child (if exists) will be re-linked to successor's parent as left child. 
            else if (node == parent.LeftChild)  // Node is the left child of its parent
                parent.LeftChild = child;
            else                                // Node is the right child of its parent
                parent.RightChild = child;
        }
        #endregion

        #region Find the node with minimum key
        /// <summary>
        /// Finds the minimum value in the tree using recursion.
        /// </summary>
        /// <returns>The minimum value stored in the tree.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal int FindMinRecursive()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Tree is empty");

            return FindMin(root).Info;
        }

        private static Node FindMin(Node node)
        {
            if (node.LeftChild == null)
                return node;

            return FindMin(node.LeftChild);
        }

        /// <summary>
        /// Finds the minimum value in the tree using iteration.
        /// </summary>
        /// <returns>The minimum value stored in the tree.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal int FindMinIterative()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Tree is empty");

            var node = root;

            while (node.LeftChild != null)
                node = node.LeftChild;

            return node.Info;
        }
        #endregion

        #region Find the node with maximum key
        /// <summary>
        /// Finds the maximum value in the tree using recursion.
        /// </summary>
        /// <returns>The maximum value stored in the tree.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal int FindMaxRecursive()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Tree is empty");

            return FindMax(root).Info;
        }

        private static Node FindMax(Node node)
        {
            if (node.RightChild == null)
                return node;

            return FindMax(node.RightChild);
        }

        /// <summary>
        /// Finds the maximum value in the tree using iteration.
        /// </summary>
        /// <returns>The maximum value stored in the tree.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(h) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        internal int FindMaxIterative()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Tree is empty");

            var node = root;

            while (node.RightChild != null)
                node = node.RightChild;

            return node.Info;
        }
        #endregion

        #region Display a BST
        /// <summary>
        /// Displays the tree structure in a visual format.
        /// </summary>
        /// <remarks>
        /// The output is useful for understanding the shape of the tree.
        /// </remarks>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void Display()
        {
            Display(root, 0);
            Console.WriteLine();
        }

        private static void Display(Node node, int level)
        {
            if (node == null)
                return;

            Display(node.RightChild, level + 1);
            Console.WriteLine();

            for (var i = 0; i < level; i++)
                Console.Write("    ");
            Console.Write(node.Info);

            Display(node.LeftChild, level + 1);
        }
        #endregion

        #region Preorder Traversal
        /// <summary>
        /// Performs a pre-order traversal of the tree.
        /// </summary>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void Preorder()
        {
            Preorder(root);
            Console.WriteLine();
        }

        private static void Preorder(Node node)
        {
            if (node == null)
                return;

            Console.Write($"{node.Info} ");
            Preorder(node.LeftChild);
            Preorder(node.RightChild);
        }
        #endregion

        #region Inorder Traversal
        /// <summary>
        /// Performs an in-order traversal of the tree.
        /// </summary>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void Inorder()
        {
            Inorder(root);
            Console.WriteLine();
        }

        private static void Inorder(Node node)
        {
            if (node == null)
                return;

            Inorder(node.LeftChild);
            Console.Write($"{node.Info} ");
            Inorder(node.RightChild);
        }
        #endregion

        #region Postorder Traversal
        /// <summary>
        /// Performs a post-order traversal of the tree.
        /// </summary>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal void Postorder()
        {
            Postorder(root);
            Console.WriteLine();
        }

        private static void Postorder(Node node)
        {
            if (node == null)
                return;

            Postorder(node.LeftChild);
            Postorder(node.RightChild);
            Console.Write($"{node.Info} ");
        }
        #endregion

        #region Finding the height of BST
        /// <summary>
        /// Returns the height of the tree.
        /// </summary>
        /// <remarks>
        /// The height is the number of nodes on the longest path from the root to a leaf.
        /// </remarks>
        /// <returns>The height of the tree.</returns>
        /// <b>Time Complexity</b>
        /// <para> O(n) </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(h) </para>
        internal int Height()
        {
            return Height(root);
        }

        private static int Height(Node node)
        {
            if (node == null)
                return 0;

            var heightLeft = Height(node.LeftChild);
            var heightRight = Height(node.RightChild);

            if (heightLeft > heightRight)
                return 1 + heightLeft;
            else
                return 1 + heightRight;
        }
        #endregion
    }
}
