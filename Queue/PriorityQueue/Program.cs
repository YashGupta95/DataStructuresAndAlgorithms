using System;

namespace PriorityQueue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int element, elementPriority;
            var priorityQueue = new PriorityQueue();

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1.Insert a new element");
                Console.WriteLine("2.Delete an element");
                Console.WriteLine("3.Display the queue");
                Console.WriteLine("4.Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 4)
                    break;

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the element to be inserted : ");
                        element = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter its priority : ");
                        elementPriority = Convert.ToInt32(Console.ReadLine());
                        priorityQueue.Insert(element, elementPriority);
                        break;
                    case 2:
                        Console.WriteLine($"Deleted element is: {priorityQueue.Delete()} ");
                        break;
                    case 3:
                        priorityQueue.Display();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}