using System;

namespace LinearSearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = new int[100];

            Console.Write("Enter the number of elements : ");
            var size = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the elements: ");
            for (var i = 0; i < size; i++)
                arr[i] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the search value : ");
            var searchValue = Convert.ToInt32(Console.ReadLine());

            var index = LinearSearch(arr, size, searchValue);

            if (index == -1)
                Console.WriteLine($"Value {searchValue} not present in the array.");
            else
                Console.WriteLine($"Value {searchValue} present at index: {index}");
        }

        internal static int LinearSearch(int[] arr, int size, int searchValue)
        {
            for (var i = 0; i < size; i++)
            {
                if (arr[i] == searchValue)
                    return i;
            }

            return -1;
        }
    }
}