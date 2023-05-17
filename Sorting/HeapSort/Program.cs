using System;

namespace HeapSort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = new int[20];

            Console.Write("Enter the number of elements : ");
            var size = Convert.ToInt32(Console.ReadLine());

            for (var i = 1; i <= size; i++)
            {
                Console.Write($"Enter element {i} : ");
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }

            HeapSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 1; i <= size; i++)
            {
                Console.Write($"{arr[i]} ");
            }
            
            Console.WriteLine();
        }

        internal static void HeapSort(int[] arr, int size)
        {
            BuildHeap_BottomUp(arr, size);

            Console.WriteLine("Heap is  : ");
            for (var i = 1; i <= size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();

            while (size > 1)
            {
                var maxValue = arr[1];
                arr[1] = arr[size];
                arr[size] = maxValue;
                size--;
                RestoreDown(1, arr, size);
            }
        }

        public static void BuildHeap_BottomUp(int[] arr, int size)
        {
            for (var i = size / 2; i >= 1; i--) //// First non-leaf node will be floor of n/2
            {
                RestoreDown(i, arr, size);
            }
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