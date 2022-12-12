using System;

namespace StackUsingLinkedList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data;
            var stackLinkedList = new StackLinkedList();

            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1.Push an element on the stack");
                Console.WriteLine("2.Pop an element from the stack");
                Console.WriteLine("3.Display the top element");
                Console.WriteLine("4.Display all stack elements");
                Console.WriteLine("5.Display size of the stack");
                Console.WriteLine("6.Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 6)
                    break;
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the element to be pushed : ");
                        data = Convert.ToInt32(Console.ReadLine());
                        stackLinkedList.Push(data);
                        break;
                    case 2:
                        data = stackLinkedList.Pop();
                        Console.WriteLine($"Popped element is : {data}");
                        break;
                    case 3:
                        Console.WriteLine($"Element at the top is : {stackLinkedList.Peek()}");
                        break;
                    case 4:
                        stackLinkedList.Display();
                        break;
                    case 5:
                        Console.WriteLine($"Size of stack: {stackLinkedList.Size()}");
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                Console.WriteLine("");
            }
        }
    }
}