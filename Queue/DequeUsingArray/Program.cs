using System;

namespace DequeUsingArray
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data;

            var dequeArray = new DequeArray(8);

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Insert an element at the front end");
                Console.WriteLine("2. Insert an element at the rear end");
                Console.WriteLine("3. Delete an element from front end");
                Console.WriteLine("4. Delete an element from rear end");
                Console.WriteLine("5. Display all elements of deque");
                Console.WriteLine("6. Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 6)
                    break;

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the element to be inserted : ");
                        data = Convert.ToInt32(Console.ReadLine());
                        dequeArray.InsertFront(data);
                        break;
                    case 2:
                        Console.Write("Enter the element to be inserted : ");
                        data = Convert.ToInt32(Console.ReadLine());
                        dequeArray.InsertRear(data);
                        break;
                    case 3:
                        Console.WriteLine($"Element deleted from front end is: {dequeArray.DeleteFront()}");
                        break;
                    case 4:
                        Console.WriteLine($"Element deleted from rear end is: {dequeArray.DeleteRear()}");
                        break;
                    case 5:
                        dequeArray.Display();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}