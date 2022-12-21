using System;
using System.Collections.Generic;

namespace BinaryTree
{
    internal class BinaryTree
    {
        private Node root;

        public BinaryTree()
        {
            root = null;
        }

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
            {
                Console.Write("    ");
            }
            Console.Write(node.Info);

            Display(node.LeftChild, level + 1);
        }

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

        internal void LevelOrder()
        {
            if (root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }

            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                Console.Write($"{node.Info} ");

                if (node.LeftChild != null)
                    queue.Enqueue(node.LeftChild);

                if (node.RightChild != null)
                    queue.Enqueue(node.RightChild);
            }

            Console.WriteLine();
        }

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

        internal void CreateTree()
        {
            root = new Node('P');
            root.LeftChild = new Node('Q');
            root.RightChild = new Node('R');
            root.LeftChild.LeftChild = new Node('A');
            root.LeftChild.RightChild = new Node('B');
            root.RightChild.LeftChild = new Node('X');
        }
    }
}
