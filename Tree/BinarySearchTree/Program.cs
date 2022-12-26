using System;

namespace BinarySearchTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bst = new BinarySearchTree();
            int data;

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Display Binary Search Tree");
                Console.WriteLine("2. Search a node in BST");
                Console.WriteLine("3. Insert a new node");
                Console.WriteLine("4. Delete a node");
                Console.WriteLine("5. Preorder Traversal");
                Console.WriteLine("6. Inorder Traversal");
                Console.WriteLine("7. Postorder Traversal");
                Console.WriteLine("8. Height of tree");
                Console.WriteLine("9. Find Minimum key");
                Console.WriteLine("10. Find Maximum key");
                Console.WriteLine("11. Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 11)
                    break;

                switch (choice)
                {
                    case 1:
                        bst.Display();
                        break;
                    case 2:
                        Console.Write("Enter the key to be searched : ");
                        data = Convert.ToInt32(Console.ReadLine());

                        if (bst.RecursiveSearch(data))
                            Console.WriteLine("Key found");
                        else
                            Console.WriteLine("Key not found");

                        //if (bst.IterativeSearch(data))
                        //    Console.WriteLine("Key found");
                        //else
                        //    Console.WriteLine("Key not found");
                        break;
                    case 3:
                        Console.Write("Enter the key to be inserted : ");
                        data = Convert.ToInt32(Console.ReadLine());
                        bst.InsertRecursive(data);
                        //bst.InsertIterative(data);
                        break;
                    case 4:
                        Console.Write("Enter the key to be deleted : ");
                        data = Convert.ToInt32(Console.ReadLine());
                        bst.DeleteRecursive(data);
                        //bst.DeleteIterative(data);
                        break;
                    case 5:
                        bst.Preorder();
                        break;
                    case 6:
                        bst.Inorder();
                        break;
                    case 7:
                        bst.Postorder();
                        break;
                    case 8:
                        Console.WriteLine($"Height of tree is: {bst.Height()}");
                        break;
                    case 9:
                        Console.WriteLine($"Minimum key is: {bst.FindMinRecursive()}");
                        //Console.WriteLine($"Minimum key is: {bst.FindMinIterative()}");
                        break;
                    case 10:
                        Console.WriteLine($"Maximum key is: {bst.FindMaxRecursive()}");
                        //Console.WriteLine($"Maximum key is: {bst.FindMaxIterative()}");
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
    }
}