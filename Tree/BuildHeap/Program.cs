using System;

namespace BuildHeap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr1 = new int[] { int.MaxValue, 1, 4, 5, 7, 9, 10 };
            var size1 = arr1.Length - 1;

            HeapifyTopDown(arr1, size1);

            for (var i = 1; i <= size1; i++)
                Console.Write($"{arr1[i]} ");

            var arr2 = new int[] { int.MaxValue, 1, 4, 5, 7, 9, 10 };
            var size2 = arr2.Length - 1;

            HeapifyBottomUp(arr2, size2);

            for (var i = 1; i <= size2; i++)
                Console.Write($"{arr2[i]} ");
            
            _ = Convert.ToInt32(Console.ReadLine()); //// To stop window
        }

        private static void HeapifyTopDown(int[] arr, int size)
        {
            for (var i = 2; i <= size; i++)
            {
                RestoreUp(i, arr);
            }
        }

        private static void HeapifyBottomUp(int[] arr, int size)
        {
            for (var i = size / 2; i >= 1; i--)
            {
                RestoreDown(i, arr, size);
            }
        }

        private static void RestoreUp(int index, int[] arr)
        {
            var value = arr[index];
            int parentIndex = index / 2;

            while (arr[parentIndex] < value)
            {
                arr[index] = arr[parentIndex];
                index = parentIndex;
                parentIndex = index / 2;
            }

            arr[index] = value;
        }

        private static void RestoreDown(int index, int[] arr, int size)
        {
            var value = arr[index];
            var leftChildIndex = 2 * index;
            var rightChildIndex = leftChildIndex + 1;

            while (rightChildIndex <= size)
            {
                if (value >= arr[leftChildIndex] && value >= arr[rightChildIndex])
                {
                    arr[index] = value;
                    return;
                }
                else if (arr[leftChildIndex] > arr[rightChildIndex])
                {
                    arr[index] = arr[leftChildIndex];
                    index = leftChildIndex;
                }
                else
                {
                    arr[index] = arr[rightChildIndex];
                    index = rightChildIndex;
                }

                leftChildIndex = 2 * index;
                rightChildIndex = leftChildIndex + 1;
            }

            //// If the number of nodes is even
            if (leftChildIndex == size && value < arr[leftChildIndex])
            {
                arr[index] = arr[leftChildIndex];
                index = leftChildIndex;
            }

            arr[index] = value;
        }
    }
}