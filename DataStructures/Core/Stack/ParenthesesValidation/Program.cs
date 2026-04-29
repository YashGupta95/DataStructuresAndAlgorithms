using System;
using DataStructures.Core.Stack;

namespace DataStructures.Core.Stack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Parentheses Validation =====");
                Console.WriteLine("1. Validate Expression");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("\nEnter expression: ");
                        string input = Console.ReadLine();

                        try
                        {
                            Console.WriteLine("\n[INFO] Validating expression...");
                            bool isValid = ParenthesesValidator.IsValid(input);

                            Console.WriteLine(isValid
                                ? "[RESULT] Expression is BALANCED."
                                : "[RESULT] Expression is NOT balanced.");
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