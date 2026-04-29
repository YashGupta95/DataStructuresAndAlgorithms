using System;

namespace DataStructures.Core.Stack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stack = new StackLinkedList<int>();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Stack (LinkedList Implementation) =====");
                Console.WriteLine("1. Push");
                Console.WriteLine("2. Pop");
                Console.WriteLine("3. Peek");
                Console.WriteLine("4. Display Stack");
                Console.WriteLine("5. Count");
                Console.WriteLine("6. Is Empty");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Enter value to push: ");
                        if (int.TryParse(Console.ReadLine(), out int value))
                        {
                            stack.Push(value);
                            Console.WriteLine($"[INFO] {value} pushed onto stack.");
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] Invalid input.");
                        }
                        break;

                    case "2":
                        try
                        {
                            int popped = stack.Pop();
                            Console.WriteLine($"[INFO] Popped element: {popped}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] {ex.Message}");
                        }
                        break;

                    case "3":
                        try
                        {
                            int top = stack.Peek();
                            Console.WriteLine($"[INFO] Top element: {top}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] {ex.Message}");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\n[INFO] Stack elements (Top → Bottom):");

                        if (stack.IsEmpty())
                        {
                            Console.WriteLine("[INFO] Stack is empty.");
                        }
                        else
                        {
                            foreach (var item in stack.GetElements())
                            {
                                Console.WriteLine($" -> {item}");
                            }
                        }
                        break;

                    case "5":
                        Console.WriteLine($"[INFO] Stack count: {stack.Count}");
                        break;

                    case "6":
                        Console.WriteLine(stack.IsEmpty()
                            ? "[INFO] Stack is EMPTY."
                            : "[INFO] Stack is NOT empty.");
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("[INFO] Exiting program...");
                        break;

                    default:
                        Console.WriteLine("[ERROR] Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}