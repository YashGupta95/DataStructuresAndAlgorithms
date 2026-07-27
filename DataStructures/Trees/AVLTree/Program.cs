namespace DataStructures.Trees.AVLTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                 AVL TREE DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateLLRotation();

            DemonstrateRRRotation();

            DemonstrateLRRotation();

            DemonstrateRLRotation();

            DemonstrateSearch();

            DemonstrateDelete();

            DemonstrateTraversals();

            DemonstrateLevelOrderTraversal();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates the Left-Left (LL) rotation.
        /// </summary>
        private static void DemonstrateLLRotation()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Left-Left (LL) Rotation");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 30, 20, 10 };

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Insert nodes causing an LL imbalance.");
            Console.WriteLine("• AVL Tree automatically performs a right rotation.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Right-Right (RR) rotation.
        /// </summary>
        private static void DemonstrateRRRotation()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Right-Right (RR) Rotation");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 10, 20, 30 };

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Insert nodes causing an RR imbalance.");
            Console.WriteLine("• AVL Tree automatically performs a left rotation.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Left-Right (LR) rotation.
        /// </summary>
        private static void DemonstrateLRRotation()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Left-Right (LR) Rotation");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 30, 10, 20 };

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Insert nodes causing an LR imbalance.");
            Console.WriteLine("• AVL Tree performs a left rotation followed by a right rotation.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Right-Left (RL) rotation.
        /// </summary>
        private static void DemonstrateRLRotation()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Right-Left (RL) Rotation");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 10, 30, 20 };

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Insert nodes causing an RL imbalance.");
            Console.WriteLine("• AVL Tree performs a right rotation followed by a left rotation.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates searching for values in an AVL Tree.
        /// </summary>
        private static void DemonstrateSearch()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Search");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"Tree Values: [{string.Join(", ", values)}]");

            var searchValue = 25;
            Console.WriteLine($"\nSearching for {searchValue}...");

            var node = AVLTreeOperations.Search(root, searchValue);

            Console.WriteLine("\nResult:");
            Console.WriteLine(node is not null ? $"Value {searchValue} found." : $"Value {searchValue} not found.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);
        }

        /// <summary>
        /// Demonstrates deletion from an AVL Tree.
        /// </summary>
        private static void DemonstrateDelete()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Delete");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"Initial Level-Order: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");

            Console.WriteLine("\nAVL Tree before deletion:");
            PrintTree(root);

            Console.WriteLine("\nDeleting value: 30");
            root = AVLTreeOperations.Delete(root, 30);

            Console.WriteLine("\nAVL Tree after deletion:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order After Deletion: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates all depth-first traversals.
        /// </summary>
        private static void DemonstrateTraversals()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Depth-First Traversals");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"In-Order   : [{string.Join(", ", AVLTreeOperations.InOrder(root))}]");
            Console.WriteLine($"Pre-Order  : [{string.Join(", ", AVLTreeOperations.PreOrder(root))}]");
            Console.WriteLine($"Post-Order : [{string.Join(", ", AVLTreeOperations.PostOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates level-order traversal.
        /// </summary>
        private static void DemonstrateLevelOrderTraversal()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Level-Order Traversal");
            Console.WriteLine("==============================================================");

            AVLNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = AVLTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Traverse the AVL Tree level by level.");

            Console.WriteLine("\nAVL Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order: [{string.Join(", ", AVLTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Prints the AVL tree in a tree-like structure.
        /// </summary>
        /// <param name="root"> The root of the AVL tree. </param>
        private static void PrintTree(AVLNode? root)
        {
            if (root is null)
            {
                Console.WriteLine("Tree is empty.");
                return;
            }

            PrintTree(root, 0);
        }

        /// <summary>
        /// Recursively prints the tree sideways. The right subtree is printed first so that it appears above the root when viewed in the console.
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="indent">Current indentation.</param>
        private static void PrintTree(AVLNode? node, int indent)
        {
            const int IndentSize = 6;

            if (node is null)
            {
                return;
            }

            PrintTree(node.Right, indent + IndentSize);
            Console.WriteLine($"{new string(' ', indent)}{node.Value}");
            PrintTree(node.Left, indent + IndentSize);
        }
    }
}
