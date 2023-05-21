using System;

namespace LinearSearchWithSentinel
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

            var index = LinearSearchWithSentinel(arr, size, searchValue);

            if (index == -1)
                Console.WriteLine($"Value {searchValue} not present in the array.");
            else
                Console.WriteLine($"Value {searchValue} present at index: {index}");
        }

        internal static int LinearSearchWithSentinel(int[] arr, int size, int searchValue)
        {
            arr[size] = searchValue;
            var i = 0;
            
            while (searchValue != arr[i])
                i++;

            if (i < size)
                return i;
            else
                return -1;

        }
    }
}