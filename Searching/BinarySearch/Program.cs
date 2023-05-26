using System;

namespace BinarySearch
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

            var index = BinarySearch(arr, size, searchValue);

            if (index == -1)
                Console.WriteLine($"Value {searchValue} not present in the array.");
            else
                Console.WriteLine($"Value {searchValue} present at index: {index}");
        }

        internal static int BinarySearch(int[] arr, int size, int searchValue)
        {
            int first = 0, last = size - 1;

            while (first <= last)
            {
                var mid = (first + last) / 2;

                if (searchValue < arr[mid])
                    last = mid - 1;         //// Search in left half
                else if (searchValue > arr[mid])
                    first = mid + 1;        //// Search in right half
                else
                    return mid;             //// searchValue present at index mid
            }

            return -1;
        }
    }
}