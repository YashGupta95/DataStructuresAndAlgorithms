using System;

namespace ShellSort
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

            ShellSort(arr, size);

            Console.WriteLine("Sorted array is : ");
            for (var i = 0; i < size; i++)
            {
                Console.Write($"{arr[i]} ");
            }

            Console.WriteLine();
        }

        private static void ShellSort(int[] arr, int n)
        {
            int j, temp;

            Console.Write("Enter maximum increment(odd value) : ");
            var h = Convert.ToInt32(Console.ReadLine());

            while (h >= 1)
            {
                for (var i = h; i < n; i++)
                {
                    temp = arr[i];
                    
                    for (j = i - h; j >= 0 && arr[j] > temp; j = j - h)
                    {
                        arr[j + h] = arr[j];
                    }

                    arr[j + h] = temp;
                }

                h -= 2;
            }
        }
    }
}