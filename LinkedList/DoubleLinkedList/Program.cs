using System;

namespace DoubleLinkedList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data, node;
            var doubleLinkedList = new DoubleLinkedList();

            doubleLinkedList.CreateList();

            while(true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Display List");
                Console.WriteLine("2. Insert in an empty List");
                Console.WriteLine("3. Insert a node in the beginning of List");
                Console.WriteLine("4. Insert a node at the end of the List");
                Console.WriteLine("5. Insert a node after a specified node");
                Console.WriteLine("6. Insert a node before a specified node");
                Console.WriteLine("7. Delete first node");
                Console.WriteLine("8. Delete last node");
                Console.WriteLine("9. Delete any node");
                Console.WriteLine("10. Reverse the List");
                Console.WriteLine("11. Exit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice: ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 11)
                    break;

                switch(choice)
                {
                    case 1:
                        doubleLinkedList.DisplayList();
                        break;
                    case 2:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.InsertInEmptyList(data);
                        break;
                    case 3:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.InsertInBeginning(data);
                        break;
                    case 4:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.InsertAtEnd(data);
                        break;
                    case 5:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element after which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.InsertAfter(data, node);
                        break;
                    case 6:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element before which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.InsertBefore(data, node);
                        break;
                    case 7:
                        doubleLinkedList.DeleteFirstNode();
                        break;
                    case 8:
                        doubleLinkedList.DeleteLastNode();
                        break;
                    case 9:
                        Console.Write("Enter the element to be deleted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        doubleLinkedList.DeleteNode(data);
                        break;
                    case 10:
                        doubleLinkedList.ReverseList();
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