using System;

namespace BinaryTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binaryTree = new BinaryTree();

            binaryTree.CreateTree();

            binaryTree.Display();
            Console.WriteLine();

            Console.WriteLine("Preorder : ");
            binaryTree.Preorder();
            Console.WriteLine("");

            Console.WriteLine("Inorder : ");
            binaryTree.Inorder();
            Console.WriteLine();

            Console.WriteLine("Postorder : ");
            binaryTree.Postorder();
            Console.WriteLine();

            Console.WriteLine("Level order : ");
            binaryTree.LevelOrder();
            Console.WriteLine();

            Console.WriteLine($"Height of tree is: {binaryTree.Height()}");
        }
    }
}