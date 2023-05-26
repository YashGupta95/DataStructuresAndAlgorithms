using System;

namespace BinarySearchRecursive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = new int[100];

            Console.Write("Enter the number of elements : ");
            var size = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the elements in sorted order: ");
            for (var i = 0; i < size; i++)
                arr[i] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the search value : ");
            var searchValue = Convert.ToInt32(Console.ReadLine());

            var index = BinarySearchRecursive(arr, size, searchValue);

            if (index == -1)
                Console.WriteLine($"Value {searchValue} not present in the array.");
            else
                Console.WriteLine($"Value {searchValue} present at index: {index}");
        }

        internal static int BinarySearchRecursive(int[] arr, int size, int searchValue)
        {
            return Search(arr, 0, size - 1, searchValue);
        }

        private static int Search(int[] arr, int first, int last, int searchValue)
        {
            if (first > last)
                return -1;

            var mid = (first + last) / 2;

            if (searchValue > arr[mid])           //// Search in right half
                return Search(arr, mid + 1, last, searchValue);
            else if (searchValue < arr[mid])      //// Search in left half
                return Search(arr, first, mid - 1, searchValue);
            else
                return mid;
        }
    }
}