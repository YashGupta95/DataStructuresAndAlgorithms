using System;

namespace SingleLinkedList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int data, node, position;
            var singleLinkedList = new SingleLinkedList();

            singleLinkedList.CreateList();

            while(true)
            {
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("1. Display List");
                Console.WriteLine("2. Count the number of nodes");
                Console.WriteLine("3. Search for an element");
                Console.WriteLine("4. Insert in an empty list/Insert in beginning of the List");
                Console.WriteLine("5. Insert a node at the end of the List");
                Console.WriteLine("6. Insert a node after a specified node");
                Console.WriteLine("7. Insert a node before a specified node");
                Console.WriteLine("8. Insert a node at a given position");
                Console.WriteLine("9. Delete first node");
                Console.WriteLine("10. Delete last node");
                Console.WriteLine("11. Delete any node");
                Console.WriteLine("12. Reverse the List");
                Console.WriteLine("13. Bubble Sort by exchanging data");
                Console.WriteLine("14. Bubble Sort by exchanging links");
                Console.WriteLine("15. Merging two sorted Linked Lists");
                Console.WriteLine("16. Merge Sort");
                Console.WriteLine("17. Insert Cycle");
                Console.WriteLine("18. Detect Cycle");
                Console.WriteLine("19. Remove Cycle");
                Console.WriteLine("20. Concatenate 2 linked lists");
                Console.WriteLine("21. Exit");
                Console.WriteLine("------------------------------------------------------------------------");

                Console.Write("Enter your choice: ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 21)
                    break;

                #region Single Linked List switch cases
                switch (choice)
                {
                    case 1:
                        singleLinkedList.DisplayList();
                        break;
                    case 2:
                        singleLinkedList.CountNodes();
                        break;
                    case 3:
                        Console.Write("Enter the element to be searched: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.Search(data);
                        break;
                    case 4:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertInBeginning(data);
                        break;
                    case 5:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertAtEnd(data);
                        break;
                    case 6:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element after which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertAfter(data, node);
                        break;
                    case 7:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the element before which the new element should be inserted: ");
                        node = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertBefore(data, node);
                        break;
                    case 8:
                        Console.Write("Enter the element to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the position at which the new element should be inserted: ");
                        position = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertAtPosition(data, position);
                        break;
                    case 9:
                        singleLinkedList.DeleteFirstNode();
                        break;
                    case 10:
                        singleLinkedList.DeleteLastNode();
                        break;
                    case 11:
                        Console.Write("Enter the element to be deleted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.DeleteNode(data);
                        break;
                    case 12:
                        singleLinkedList.ReverseList();
                        break;
                    case 13:
                        singleLinkedList.BubbleSortExchangeData();
                        break;
                    case 14:
                        singleLinkedList.BubbleSortExchangeLinks();
                        break;
                    case 15:
                        var singleLinkedList2 = new SingleLinkedList();
                        singleLinkedList2.CreateList();

                        singleLinkedList.BubbleSortExchangeData();
                        singleLinkedList2.BubbleSortExchangeData();

                        Console.WriteLine("First List: ");
                        singleLinkedList.DisplayList();

                        Console.WriteLine("Second List: ");
                        singleLinkedList2.DisplayList();

                        SingleLinkedList mergedList;

                        mergedList = singleLinkedList.MergeIntoNewList(singleLinkedList2);
                        Console.WriteLine("Merged List after merging into a new list: ");
                        mergedList.DisplayList();

                        Console.WriteLine("First List: ");
                        singleLinkedList.DisplayList();

                        Console.WriteLine("Second List: ");
                        singleLinkedList2.DisplayList();

                        mergedList = singleLinkedList.MergeByRearrangingLinks(singleLinkedList2);
                        Console.WriteLine("Merged List after merging by rearranging links: ");
                        mergedList.DisplayList();

                        Console.WriteLine("First List: ");
                        singleLinkedList.DisplayList();

                        Console.WriteLine("Second List: ");
                        singleLinkedList2.DisplayList();
                        break;
                    case 16:
                        singleLinkedList.MergeSort();
                        break;
                    case 17:
                        Console.Write("Enter the element at which the cycle has to be inserted: ");
                        data = Convert.ToInt32(Console.ReadLine());
                        singleLinkedList.InsertCycle(data);
                        break;
                    case 18:
                        if (singleLinkedList.HasCycle())
                        {
                            Console.WriteLine("The List has a cycle");
                        }
                        else
                        {
                            Console.WriteLine("The List does not have a cycle");
                        }
                        break;
                    case 19:
                        singleLinkedList.RemoveCycle();
                        break;
                    case 20:
                        var secondList = new SingleLinkedList();
                        secondList.CreateList();

                        singleLinkedList.ConcatenateLists(secondList);
                        singleLinkedList.DisplayList();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                #endregion

                Console.WriteLine();
            }

            Console.WriteLine("Exiting..");
        }
    }
}
