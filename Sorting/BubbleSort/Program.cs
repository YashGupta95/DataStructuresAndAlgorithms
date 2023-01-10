using System;

namespace BubbleSort
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

            Sort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void Sort(int[] arr, int n)
        {
            int swaps;

            for (var i = n - 2; i >= 0; i--)
            {
                swaps = 0;

                for (var j = 0; j <= i; j++)
                {
                    if (arr[j] > arr[j + 1]) //// outer loop runs until second last element to avoid IndexOutOfBound exception at this condition
                    {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swaps++;
                    }
                }

                if (swaps == 0) //// no swaps in a particular pass denotes that the array has been sorted
                    break;
            }
        }
    }
}