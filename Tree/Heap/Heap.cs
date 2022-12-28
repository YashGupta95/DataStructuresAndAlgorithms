using System;

namespace Heap
{
    /// <summary>
    /// The below code sample has been implemented for a Max Heap. The respective conditions can be reversed for a Min Heap implementation
    /// </summary>
    internal class Heap
    {
        private readonly int[] arr;
        private int size;

        public Heap()
        {
            arr = new int[10];
            size = 0;
            arr[0] = int.MaxValue; //// Sentinel value
        }

        public Heap(int maxSize)
        {
            arr = new int[maxSize];
            size = 0;
            arr[0] = int.MaxValue;
        }

        internal void Insert(int value)
        {
            size++;
            arr[size] = value;
            RestoreUp(size);
        }

        private void RestoreUp(int index)
        {
            var value = arr[index];
            var parentIndex = index / 2;

            while (arr[parentIndex] < value) //// If there's no sentinel value modify it to: while(parentIndex >= 1 && arr[parentIndex] < value)
            {
                arr[index] = arr[parentIndex];
                index = parentIndex;
                parentIndex = index / 2;
            }

            arr[index] = value;
        }

        internal int DeleteRoot()
        {
            if (size == 0)
                throw new InvalidOperationException("Heap is Empty");

            var maxValue = arr[1];
            arr[1] = arr[size];
            size--;
            RestoreDown(1);

            return maxValue;
        }

        private void RestoreDown(int index)
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

            //// If the number of nodes is even, then there will always be a node that does not have a right children
            if (leftChildIndex == size && value < arr[leftChildIndex])
            {
                arr[index] = arr[leftChildIndex];
                index = leftChildIndex;
            }

            arr[index] = value;
        }

        internal void Display()
        {
            if (size == 0)
            {
                Console.WriteLine("Heap is empty");
                return;
            }

            Console.WriteLine($"Heap size: {size}");
            for (var i = 1; i <= size; i++)
                Console.Write($"{arr[i]} ");

            Console.WriteLine();
        }
    }
}
