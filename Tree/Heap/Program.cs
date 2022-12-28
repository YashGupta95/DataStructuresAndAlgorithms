using System;

namespace Heap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var heap = new Heap(20);

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Insert a node in Heap");
                Console.WriteLine("2. Delete root");
                Console.WriteLine("3. Display the Heap");
                Console.WriteLine("4. Exit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 4)
                    break;

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the value to be inserted : ");
                        var value = Convert.ToInt32(Console.ReadLine());
                        heap.Insert(value);
                        break;
                    case 2:
                        Console.WriteLine($"Maximum value is {heap.DeleteRoot()}");
                        break;
                    case 3:
                        heap.Display();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}