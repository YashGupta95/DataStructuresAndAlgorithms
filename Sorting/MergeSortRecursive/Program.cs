using System;

namespace MergeSortRecursive
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

            MergeSortRecursive(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void MergeSortRecursive(int[] arr, int size)
        {
            var temp = new int[size];
            MergeSort(arr, temp, 0, size - 1);
        }

        private static void MergeSort(int[] arr, int[] temp, int low, int high)
        {
            //// The variables low and high denote the first & last index of the sub-list to be sorted
            if (low == high) //// If the sub-list has only one element
                return;

            var mid = (low + high) / 2;

            MergeSort(arr, temp, low, mid);
            MergeSort(arr, temp, mid + 1, high);

            //// Merge arr[low]...arr[mid] and arr[mid+1]...arr[high] to temp[low]...temp[high]
            Merge(arr, temp, low, mid, mid + 1, high);

            //// Copy temp[low]...temp[high] to arr[low]...arr[high]
            Copy(arr, temp, low, high);
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

        private static void Copy(int[] arr, int[] temp, int low, int high)
        {
            for (var i = low; i <= high; i++)
                arr[i] = temp[i];
        }
    }
}