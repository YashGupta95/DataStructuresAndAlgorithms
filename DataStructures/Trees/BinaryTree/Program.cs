using System;

namespace DataStructures.Trees.BinaryTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                BINARY TREE DEMONSTRATION");
            Console.WriteLine("==============================================================");

            DemonstrateBuildAndTraversals();
            DemonstrateSearch();
            DemonstrateDelete();
            DemonstrateClear();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DemonstrateBuildAndTraversals()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Build and Traverse a Binary Tree");
            Console.WriteLine("==============================================================");

            var tree = new BinaryTree();
            tree.CreateTree();

            Console.WriteLine("Binary Tree Structure:");
            PrintTree(tree.Root);

            Console.WriteLine($"\nIn-Order Traversal: [{string.Join(", ", tree.InOrder())}]");
            Console.WriteLine($"Pre-Order Traversal: [{string.Join(", ", tree.PreOrder())}]");
            Console.WriteLine($"Post-Order Traversal: [{string.Join(", ", tree.PostOrder())}]");
            Console.WriteLine($"Level-Order Traversal: [{string.Join(", ", tree.LevelOrder())}]");
            Console.WriteLine($"Height: {tree.Height()}");
            Console.WriteLine($"Total Nodes: {tree.CountNodes()}");
            Console.WriteLine($"Leaf Nodes: {tree.CountLeaves()}");
        }

        private static void DemonstrateSearch()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Search");
            Console.WriteLine("==============================================================");

            var tree = new BinaryTree();
            tree.CreateTree();

            var searchValue = 40;
            Console.WriteLine($"Searching for {searchValue}...");

            var node = tree.Search(searchValue);
            Console.WriteLine(node != null ? $"Value {searchValue} found." : $"Value {searchValue} not found.");
        }

        private static void DemonstrateDelete()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Delete");
            Console.WriteLine("==============================================================");

            var tree = new BinaryTree();
            tree.CreateTree();

            Console.WriteLine($"Initial Level-Order: [{string.Join(", ", tree.LevelOrder())}]");
            Console.WriteLine("Binary Tree Before Deletion:");
            PrintTree(tree.Root);

            var deleteValue = 20;
            Console.WriteLine($"\nDeleting value: {deleteValue}");
            var removed = tree.Delete(deleteValue);
            Console.WriteLine(removed ? "Deletion completed." : "Value not found.");

            Console.WriteLine($"\nLevel-Order After Deletion: [{string.Join(", ", tree.LevelOrder())}]");
            Console.WriteLine("Binary Tree After Deletion:");
            PrintTree(tree.Root);
        }

        private static void DemonstrateClear()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Clear and Empty Check");
            Console.WriteLine("==============================================================");

            var tree = new BinaryTree();
            tree.CreateTree();
            tree.Clear();

            Console.WriteLine($"Tree empty: {tree.IsEmpty()}");
        }

        private static void PrintTree(BinaryTreeNode node, int level = 0)
        {
            if (node == null)
            {
                return;
            }

            PrintTree(node.Right, level + 1);
            Console.WriteLine();
            Console.Write(new string(' ', level * 4));
            Console.Write(node.Value);
            PrintTree(node.Left, level + 1);
        }
    }
}
