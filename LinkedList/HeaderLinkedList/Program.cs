using System;

namespace HeaderLinkedList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data, node, position;
            var headerLinkedList = new HeaderLinkedList();

            headerLinkedList.CreateList();

            while(true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Display List");
                Console.WriteLine("2. Insert a node at the end of the list");
                Console.WriteLine("3. Insert a new node before a node");
                Console.WriteLine("4. Insert a node at a given position");
                Console.WriteLine("5. Delete a node");
                Console.WriteLine("6. Reverse the list");
                Console.WriteLine("7. Exit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice: ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 7)
                    break;

                switch(choice)
                {
                    case 1:
                        headerLinkedList.DisplayList();
                        break;
                    case 2:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        headerLinkedList.InsertAtEnd(data);
                        break;
                    case 3:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element before which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        headerLinkedList.InsertBefore(data, node);
                        break;
                    case 4:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the position at which the new element should be inserted: ");
                        position = Convert.ToInt32(Console.ReadLine());
                        headerLinkedList.InsertAtPosition(data, position);
                        break;
                    case 5:
                        Console.Write("Enter the element to be deleted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        headerLinkedList.DeleteNode(data);
                        break;
                    case 6:
                        headerLinkedList.ReverseList();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Exiting..");
        }
    }
}