using System;

namespace DataStructures.Core.Array
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter array capacity: ");
            var capacity = Convert.ToInt32(Console.ReadLine());

            var array = new ArrayManager(capacity);
            var exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Array Basics =====");
                Console.WriteLine("1. Insert");
                Console.WriteLine("2. Delete");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Get Element");
                Console.WriteLine("5. Search");
                Console.WriteLine("6. Reverse");
                Console.WriteLine("7. Display");
                Console.WriteLine("8. Size");
                Console.WriteLine("9. Capacity");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine() ?? string.Empty;
                var idx = 0;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter value: ");
                            var val = Convert.ToInt32(Console.ReadLine());

                            array.Insert(val);
                            Console.WriteLine("[INFO] Inserted successfully.");
                            break;

                        case "2":
                            Console.Write("Enter index: ");
                            idx = Convert.ToInt32(Console.ReadLine());

                            var deleted = array.Delete(idx);
                            Console.WriteLine($"[INFO] Deleted element: {deleted}");
                            break;

                        case "3":
                            Console.Write("Enter index: ");
                            idx = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter new value: ");
                            val = Convert.ToInt32(Console.ReadLine());

                            array.Update(idx, val);
                            Console.WriteLine("[INFO] Updated successfully.");
                            break;

                        case "4":
                            Console.Write("Enter index: ");
                            idx = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine($"[INFO] Element: {array.Get(idx)}");
                            break;

                        case "5":
                            Console.Write("Enter value to search: ");
                            val = Convert.ToInt32(Console.ReadLine());

                            var pos = array.Search(val);
                            Console.WriteLine(pos == -1
                                ? "[INFO] Element not found."
                                : $"[INFO] Found at index: {pos}");
                            break;

                        case "6":
                            array.Reverse();
                            Console.WriteLine("[INFO] Array reversed.");
                            break;

                        case "7":
                            var elements = array.GetElements();

                            if (elements.Length == 0)
                            {
                                Console.WriteLine("[INFO] Array is empty.");
                            }
                            else
                            {
                                Console.Write("[INFO] Elements: ");
                                foreach (var e in elements)
                                {
                                    Console.Write($"{e} ");
                                }
                                Console.WriteLine();
                            }
                            break;

                        case "8":
                            Console.WriteLine($"[INFO] Size: {array.Size}");
                            break;

                        case "9":
                            Console.WriteLine($"[INFO] Capacity: {array.Capacity}");
                            break;

                        case "0":
                            exit = true;
                            Console.WriteLine("[INFO] Exiting...");
                            break;

                        default:
                            Console.WriteLine("[ERROR] Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Message}");
                }
            }
        }
    }
}