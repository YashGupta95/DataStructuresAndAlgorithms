using System;

namespace SelectionSort
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

            SelectionSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void SelectionSort(int[] arr, int n)
        {
            int minIndex;

            for (var i = 0; i < n - 1; i++)
            {
                minIndex = i;

                for (var j = i + 1; j < n; j++) // for each pass, find the index of smallest element
                {
                    if (arr[j] < arr[minIndex])
                        minIndex = j;
                }

                if (i != minIndex) // if the smallest element is not at it's right place, do the swapping
                {
                    var temp = arr[i];
                    arr[i] = arr[minIndex];
                    arr[minIndex] = temp;
                }
            }
        }
    }
}