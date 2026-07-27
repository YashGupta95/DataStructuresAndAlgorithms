using System;

namespace DataStructures.Trees.BinarySearchTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                BINARY SEARCH TREE DEMONSTRATION");
            Console.WriteLine("==============================================================");

            var bst = new BinarySearchTreeOperations();
            var sampleValues = new[] { 50, 30, 70, 20, 40, 60, 80 };

            Console.WriteLine("Building a sample tree with values: " + string.Join(", ", sampleValues));
            foreach (var value in sampleValues)
            {
                bst.InsertRecursive(value);
            }

            DemonstrateOperations(bst);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DemonstrateOperations(BinarySearchTreeOperations bst)
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Display Tree");
            Console.WriteLine("==============================================================");
            bst.Display();

            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Search Operations");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Recursive search for 40: {bst.RecursiveSearch(40)}");
            Console.WriteLine($"Iterative search for 90: {bst.IterativeSearch(90)}");

            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Traversals");
            Console.WriteLine("==============================================================");
            Console.WriteLine("Preorder: ");
            bst.Preorder();
            Console.WriteLine("Inorder: ");
            bst.Inorder();
            Console.WriteLine("Postorder: ");
            bst.Postorder();

            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Tree Properties");
            Console.WriteLine("==============================================================");
            Console.WriteLine($"Height: {bst.Height()}");
            Console.WriteLine($"Minimum key: {bst.FindMinRecursive()}");
            Console.WriteLine($"Maximum key: {bst.FindMaxRecursive()}");

            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Delete Operation");
            Console.WriteLine("==============================================================");
            bst.DeleteRecursive(30);
            Console.WriteLine("Deleted 30 recursively.");
            Console.WriteLine("Binary Search Tree after deletion: ");
            bst.Display();
        }
    }
}
