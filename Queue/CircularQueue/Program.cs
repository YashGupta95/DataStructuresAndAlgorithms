﻿using System;

namespace CircularQueue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data;

            var circularQueue = new CircularQueue(8);

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Insert an element in the queue");
                Console.WriteLine("2. Delete an element from the queue");
                Console.WriteLine("3. Display element at the front");
                Console.WriteLine("4. Display all elements of the queue");
                Console.WriteLine("5. Display size of the queue");
                Console.WriteLine("6. Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 6)
                    break;

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        circularQueue.Insert(data);
                        break;
                    case 2:
                        data = circularQueue.Delete();
                        Console.WriteLine($"Element deleted is: {data}");
                        break;
                    case 3:
                        Console.WriteLine($"Element at the front is: {circularQueue.Peek()}");
                        break;
                    case 4:
                        circularQueue.Display();
                        break;
                    case 5:
                        Console.WriteLine($"Size of queue is: {circularQueue.Size()}");
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}