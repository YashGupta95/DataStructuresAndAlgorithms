using System;

namespace InsertionSort
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

            InsertionSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void InsertionSort(int[] arr, int n)
        {
            int j, temp;

            //// In each iteration, elements from 0 to (i-1) will be considered a part of sorted array
            //// and the elements from i to (n-1) will be considered a part of unsorted array
            for (var i = 1; i < n; i++)
            {
                temp = arr[i]; //// arr[i] will be first element of unsorted part

                for (j = i - 1; j >= 0 && arr[j] > temp; j--)
                {
                    arr[j + 1] = arr[j];
                }

                arr[j + 1] = temp;
            }
        }
    }
}