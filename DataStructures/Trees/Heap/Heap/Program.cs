using System;

namespace DataStructures.Trees.Heap.Heap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                   HEAP DEMONSTRATION");
            Console.WriteLine("==============================================================");

            var heap = new HeapOperations(20);
            var values = new[] { 10, 20, 15, 30, 25, 5 };

            Console.WriteLine("Building a sample max-heap with values: " + string.Join(", ", values));
            foreach (var value in values)
            {
                heap.Insert(value);
            }

            Console.WriteLine("\nHeap after insertions:");
            heap.Display();

            Console.WriteLine("\nDeleting root...");
            Console.WriteLine($"Maximum value removed: {heap.DeleteRoot()}");

            Console.WriteLine("\nHeap after delete root:");
            heap.Display();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
