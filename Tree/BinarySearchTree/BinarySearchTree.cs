using System;

namespace BinarySearchTree
{
    internal class BinarySearchTree
    {
        private Node root;

        public BinarySearchTree()
        {
            root = null;
        }

        internal bool IsEmpty()
        {
            return (root == null);
        }

        #region Insert a node in BST
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
        internal bool RecursiveSearch(int element)
        {
            return (Search(root, element) != null);
        }

        private static Node Search(Node node, int element)
        {
            if (node == null) //// Key not found
                return null; 

            if (element < node.Info) //// Search in left subtree
                return Search(node.LeftChild, element);

            if (element > node.Info) //// Search in right subtree
                return Search(node.RightChild, element);

            return node;
        }

        internal bool IterativeSearch(int element)
        {
            var node = root;

            while (node != null)
            {
                if (element < node.Info)
                    node = node.LeftChild; //// Move to left child
                else if (element > node.Info)
                    node = node.RightChild;  //// Move to right child
                else
                    return true;
            }

            return false; // Key not found
        }
        #endregion

        #region Deleting a node from BST
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

            if (element < node.Info)                                    //// Node will be found and deleted from left subtree
                node.LeftChild = Delete(node.LeftChild, element);
            else if (element > node.Info)                               //// Node will be found and deleted from left subtree
                node.RightChild = Delete(node.RightChild, element);
            else                                                        //// Key to be deleted is found
            {
                if (node.LeftChild != null && node.RightChild != null)  //// Case C: Node to be deleted has 2 children
                {
                    var successor = node.RightChild;

                    while (successor.LeftChild != null)
                        successor = successor.LeftChild;
                    
                    node.Info = successor.Info;
                    node.RightChild = Delete(node.RightChild, successor.Info);
                }
                else   //// Case B and Case A : Node to be deleted has either 1 or no child
                {
                    if (node.LeftChild != null) //// Node has only left child
                        child = node.LeftChild;
                    else                        //// Node has only right child or no child
                        child = node.RightChild;

                    node = child;
                }
            }

            return node;
        }

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

            //// Case C: Node to be deleted has 2 children - Find the inorder successor and its parent
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

            //// Case B and Case A : Node to be deleted has either 1 or no child
            Node child;

            if (node.LeftChild != null)         //// Node to be deleted has a left child
                child = node.LeftChild;
            else                                //// Node to be deleted has a right child or no child
                child = node.RightChild;

            if (parent == null)                 //// Node to be deleted is the root node
                root = child;
            else if (node == parent.LeftChild)  //// Node is the left child of its parent
                parent.LeftChild = child;
            else                                //// Node is the right child of its parent
                parent.RightChild = child;
        }
        #endregion

        #region Find the node with minimum key
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
