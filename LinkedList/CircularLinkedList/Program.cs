using System;

namespace CircularLinkedList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data, node;
            var circularLinkedList = new CircularLinkedList();

            circularLinkedList.CreateList();

            while(true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Display List");
                Console.WriteLine("2. Insert in empty list");
                Console.WriteLine("3. Insert in the beginning");
                Console.WriteLine("4. Insert at the end");
                Console.WriteLine("5. Insert after a node");
                Console.WriteLine("6. Delete first node");
                Console.WriteLine("7. Delete last node");
                Console.WriteLine("8. Delete any node");
                Console.WriteLine("9. Concatenate 2 lists");
                Console.WriteLine("10. Quit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice: ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 10)
                    break;

                switch(choice)
                {
                    case 1:
                        circularLinkedList.DisplayList();
                        break;
                    case 2:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        circularLinkedList.InsertInEmptyList(data);
                        break;
                    case 3:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        circularLinkedList.InsertInBeginning(data);
                        break;
                    case 4:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        circularLinkedList.InsertAtEnd(data);
                        break;
                    case 5:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element after which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        circularLinkedList.InsertAfter(data, node);
                        break;
                    case 6:
                        circularLinkedList.DeleteFirstNode();
                        break;
                    case 7:
                        circularLinkedList.DeleteLastNode();
                        break;
                    case 8:
                        Console.Write("Enter the element to be deleted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        circularLinkedList.DeleteNode(data);
                        break;
                    case 9:
                        var secondList = new CircularLinkedList();
                        secondList.CreateList();

                        circularLinkedList.ConcatenateLists(secondList);
                        circularLinkedList.DisplayList();
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