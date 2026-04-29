using System;

namespace DataStructures.Core.Stack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Postfix Expression Evaluation =====");
                Console.WriteLine("1. Evaluate Expression");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("\nEnter postfix expression: ");
                        string input = Console.ReadLine();

                        try
                        {
                            Console.WriteLine("\n[INFO] Evaluating expression...");
                            int result = DataStructures.Core.Stack.PostfixEvaluator.Evaluate(input);

                            Console.WriteLine($"[RESULT] Evaluation Result: {result}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] {ex.Message}");
                        }
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("[INFO] Exiting program...");
                        break;

                    default:
                        Console.WriteLine("[ERROR] Invalid choice.");
                        break;
                }
            }
        }
    }
}