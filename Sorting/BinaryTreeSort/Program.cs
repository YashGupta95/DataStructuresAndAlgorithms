using System;

namespace BinaryTreeSort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = new int[20];

            Console.Write("Enter the number of elements : ");
            var size = Convert.ToInt32(Console.ReadLine());

            for (var i = 0; i < size; i++)
            {
                Console.Write($"Enter element {(i + 1)} : ");
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }

            BinaryTreeSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void BinaryTreeSort(int[] arr, int size)
        {
            var bst = new BinarySearchTree();

            for (var i = 0; i < size; i++)
                bst.Insert(arr[i]);

            bst.Inorder(arr);
        }
    }
}