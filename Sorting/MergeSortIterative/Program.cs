using System;

namespace MergeSortIterative
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

            MergeSortIterative(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void MergeSortIterative(int[] arr, int n)
        {
            var temp = new int[n];
            var size = 1; //// Variable 'size' denotes the size of the sublist

            while (size <= n - 1)
            {
                SortPass(arr, temp, size, n);
                size = size * 2; //// Size of sublists will keep increasing with each iteration
            }
        }

        private static void SortPass(int[] arr, int[] temp, int size, int n)
        {
            int i, low1, high1, low2, high2;

            low1 = 0;

            while (low1 + size <= n - 1)
            {
                high1 = low1 + size - 1;
                low2 = low1 + size;
                high2 = low2 + size - 1;

                if (high2 >= n) //// If the length of last sub-list is less than size
                    high2 = n - 1;

                Merge(arr, temp, low1, high1, low2, high2);

                low1 = high2 + 1; //// Take next two sublists for merging
            }

            for (i = low1; i <= n - 1; i++)
                temp[i] = arr[i]; //// If any sublist is left alone (in case n is an odd number)

            Copy(arr, temp, n);
        }

        private static void Merge(int[] arr, int[] temp, int low1, int high1, int low2, int high2)
        {
            var i = low1;
            var j = low2;
            var k = low1;

            while ((i <= high1) && (j <= high2))
            {
                if (arr[i] <= arr[j])
                    temp[k++] = arr[i++];
                else
                    temp[k++] = arr[j++];
            }

            while (i <= high1)
                temp[k++] = arr[i++];

            while (j <= high2)
                temp[k++] = arr[j++];
        }

        private static void Copy(int[] arr, int[] temp, int n)
        {
            for (int i = 0; i < n; i++)
                arr[i] = temp[i];
        }
    }
}