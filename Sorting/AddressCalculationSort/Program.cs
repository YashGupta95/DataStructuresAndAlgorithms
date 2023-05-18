using System;

namespace AddressCalculationSort
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

            AddressCalculationSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }
            
            Console.WriteLine();
        }

        internal static void AddressCalculationSort(int[] arr, int size)
        {
            var sortedLinkedList = new SortedLinkedList[6];
            
            for (var i = 0; i < 6; i++)
                sortedLinkedList[i] = new SortedLinkedList();

            var large = 0;
            for (var i = 0; i < size; i++)
            {
                if (arr[i] > large)
                    large = arr[i];
            }

            for (var i = 0; i < size; i++)
            {
                var hashRes = Hash(arr[i], large);
                sortedLinkedList[hashRes].InsertInOrder(arr[i]);
            }

            //// Elements of linked lists are copied to array
            var k = 0;
            for (var j = 0; j <= 5; j++)
            {
                var node = sortedLinkedList[j].GetStart();
                while (node != null)
                {
                    arr[k++] = node.Info;
                    node = node.Link;
                }
            }
        }

        private static int Hash(int element, int large)
        {
            var temp = (float)element / large;
            return (int)(temp * 5);
        }
    }
}