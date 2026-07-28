namespace DataStructures.Trees.RedBlackTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              RED-BLACK TREE DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateRecolorOnlyInsertion();

            DemonstrateLLFixup();

            DemonstrateRRFixup();

            DemonstrateLRFixup();

            DemonstrateRLFixup();

            DemonstrateSearch();

            DemonstrateDeleteRedLeaf();

            DemonstrateDeleteBlackNode();

            DemonstrateTraversals();

            DemonstrateLevelOrderTraversal();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates an insertion that violates only property 4 (no two consecutive reds) with a red uncle,
        /// which is resolved by recoloring alone — no rotation is performed.
        /// </summary>
        private static void DemonstrateRecolorOnlyInsertion()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Insertion Fix-up: Recolor Only (Uncle is Red)");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5 };
            var triggerValue = values[^1];

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values[..^1])
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"\nTree Before Inserting {triggerValue}:");
            PrintTree(root);

            root = RedBlackTreeOperations.Insert(root, triggerValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Insert {triggerValue}. Its parent (10) and uncle (30) are both red.");
            Console.WriteLine("• Red-Black Tree recolors the parent and uncle to black, and the grandparent to red.");
            Console.WriteLine("• No rotation is required.");

            Console.WriteLine($"\nTree After Inserting {triggerValue}:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Left-Left (LL) fix-up case: uncle is black and the new node forms a straight line on the left.
        /// </summary>
        private static void DemonstrateLLFixup()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Insertion Fix-up: Left-Left (LL) Case");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 30, 20, 10 };
            var triggerValue = values[^1];

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values[..^1])
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"\nTree Before Inserting {triggerValue}:");
            PrintTree(root);

            root = RedBlackTreeOperations.Insert(root, triggerValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Insert {triggerValue}. It becomes the left child of a red parent (20), forming a straight line on the left with a null (black) uncle — an LL imbalance.");
            Console.WriteLine("• Red-Black Tree recolors and performs a right rotation on the grandparent (30).");

            Console.WriteLine($"\nTree After Inserting {triggerValue}:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Right-Right (RR) fix-up case: uncle is black and the new node forms a straight line on the right.
        /// </summary>
        private static void DemonstrateRRFixup()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Insertion Fix-up: Right-Right (RR) Case");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 10, 20, 30 };
            var triggerValue = values[^1];

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values[..^1])
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"\nTree Before Inserting {triggerValue}:");
            PrintTree(root);

            root = RedBlackTreeOperations.Insert(root, triggerValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Insert {triggerValue}. It becomes the right child of a red parent (20), forming a straight line on the right with a null (black) uncle — an RR imbalance.");
            Console.WriteLine("• Red-Black Tree recolors and performs a left rotation on the grandparent (10).");

            Console.WriteLine($"\nTree After Inserting {triggerValue}:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Left-Right (LR) fix-up case: uncle is black and the new node forms a zig-zag from the left.
        /// </summary>
        private static void DemonstrateLRFixup()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Insertion Fix-up: Left-Right (LR) Case");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 30, 10, 20 };
            var triggerValue = values[^1];

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values[..^1])
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"\nTree Before Inserting {triggerValue}:");
            PrintTree(root);

            root = RedBlackTreeOperations.Insert(root, triggerValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Insert {triggerValue}. It becomes the right child of a red parent (10), forming a zig-zag from the left with a null (black) uncle — an LR imbalance.");
            Console.WriteLine("• Red-Black Tree performs a left rotation on the parent (10), then recolors and right-rotates the grandparent (30).");

            Console.WriteLine($"\nTree After Inserting {triggerValue}:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates the Right-Left (RL) fix-up case: uncle is black and the new node forms a zig-zag from the right.
        /// </summary>
        private static void DemonstrateRLFixup()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Insertion Fix-up: Right-Left (RL) Case");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 10, 30, 20 };
            var triggerValue = values[^1];

            Console.WriteLine($"Insertion Order: [{string.Join(", ", values)}]");

            foreach (var value in values[..^1])
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"\nTree Before Inserting {triggerValue}:");
            PrintTree(root);

            root = RedBlackTreeOperations.Insert(root, triggerValue);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Insert {triggerValue}. It becomes the left child of a red parent (30), forming a zig-zag from the right with a null (black) uncle — an RL imbalance.");
            Console.WriteLine("• Red-Black Tree performs a right rotation on the parent (30), then recolors and left-rotates the grandparent (10).");

            Console.WriteLine($"\nTree After Inserting {triggerValue}:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order Traversal After Balancing: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates searching for values in a Red-Black Tree.
        /// </summary>
        private static void DemonstrateSearch()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Search");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"Tree Values: [{string.Join(", ", values)}]");

            var searchValue = 25;
            Console.WriteLine($"\nSearching for {searchValue}...");

            var node = RedBlackTreeOperations.Search(root, searchValue);

            Console.WriteLine("\nResult:");
            Console.WriteLine(node is not null ? $"Value {searchValue} found." : $"Value {searchValue} not found.");

            Console.WriteLine("\nRed-Black Tree:");
            PrintTree(root);
        }

        /// <summary>
        /// Demonstrates deletion of a red leaf, which requires no fix-up because removing a red node preserves every Red-Black property.
        /// </summary>
        private static void DemonstrateDeleteRedLeaf()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Delete: Red Leaf (No Fix-up Required)");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5 };

            foreach (var value in values)
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"Initial Level-Order: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");

            Console.WriteLine("\nRed-Black Tree before deletion:");
            PrintTree(root);

            Console.WriteLine("\nDeleting value: 5 (a red leaf)");
            root = RedBlackTreeOperations.Delete(root, 5);

            Console.WriteLine("\nRed-Black Tree after deletion:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order After Deletion: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates deletion of a black node, which triggers <c>DeleteFixup</c> to restore the black-height property.
        /// </summary>
        private static void DemonstrateDeleteBlackNode()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Delete: Black Node (Triggers Fix-up)");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5 };

            foreach (var value in values)
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine($"Initial Level-Order: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");

            Console.WriteLine("\nRed-Black Tree before deletion:");
            PrintTree(root);

            Console.WriteLine("\nDeleting value: 30 (a black node)");
            root = RedBlackTreeOperations.Delete(root, 30);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Removing 30 reduces the black-height on its side of the tree.");
            Console.WriteLine("• Red-Black Tree performs a rotation and recoloring on the sibling subtree to restore balance.");

            Console.WriteLine("\nRed-Black Tree after deletion:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order After Deletion: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates all depth-first traversals.
        /// </summary>
        private static void DemonstrateTraversals()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Depth-First Traversals");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nRed-Black Tree:");
            PrintTree(root);

            Console.WriteLine($"In-Order   : [{string.Join(", ", RedBlackTreeOperations.InOrder(root))}]");
            Console.WriteLine($"Pre-Order  : [{string.Join(", ", RedBlackTreeOperations.PreOrder(root))}]");
            Console.WriteLine($"Post-Order : [{string.Join(", ", RedBlackTreeOperations.PostOrder(root))}]");
        }

        /// <summary>
        /// Demonstrates level-order traversal.
        /// </summary>
        private static void DemonstrateLevelOrderTraversal()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. Level-Order Traversal");
            Console.WriteLine("==============================================================");

            RedBlackNode? root = null;

            var values = new[] { 20, 10, 30, 5, 15, 25, 40 };

            foreach (var value in values)
            {
                root = RedBlackTreeOperations.Insert(root, value);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Traverse the Red-Black Tree level by level.");

            Console.WriteLine("\nRed-Black Tree:");
            PrintTree(root);

            Console.WriteLine($"\nLevel-Order: [{string.Join(", ", RedBlackTreeOperations.LevelOrder(root))}]");
        }

        /// <summary>
        /// Prints the Red-Black tree in a tree-like structure, annotating each node's color.
        /// </summary>
        /// <param name="root"> The root of the Red-Black tree. </param>
        private static void PrintTree(RedBlackNode? root)
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
        /// Each node is annotated with its color: (R) for red and (B) for black.
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="indent">Current indentation.</param>
        private static void PrintTree(RedBlackNode? node, int indent)
        {
            const int IndentSize = 6;

            if (node is null)
            {
                return;
            }

            PrintTree(node.Right, indent + IndentSize);
            var colorTag = node.Color == NodeColor.Red ? "(R)" : "(B)";
            Console.WriteLine($"{new string(' ', indent)}{node.Value}{colorTag}");
            PrintTree(node.Left, indent + IndentSize);
        }
    }
}
