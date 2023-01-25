using System;

namespace QuickSort
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

            QuickSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void QuickSort(int[] arr, int n)
        {
            Sort(arr, 0, n - 1);
        }

        private static void Sort(int[] arr, int low, int high)
        {
            if (low >= high) //// If there are zero or one element in sublist
                return;

            var p = Partition(arr, low, high);
            Sort(arr, low, p - 1);
            Sort(arr, p + 1, high);
        }

        private static int Partition(int[] arr, int low, int high)
        {
            var pivot = arr[low];

            var i = low + 1; //// Moves from left to right
            var j = high;    //// Moves from right to left

            while (i <= j)
            {
                while (arr[i] < pivot && i < high)
                    i++;

                while (arr[j] > pivot)
                    j--;

                if (i < j) //// Swap a[i] and a[j]
                {
                    var temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    i++;
                    j--;
                }
                else //// Found the right position for pivot element
                    break;
            }

            //// The right position for pivot is j
            arr[low] = arr[j];
            arr[j] = pivot;

            return j;
        }
    }
}