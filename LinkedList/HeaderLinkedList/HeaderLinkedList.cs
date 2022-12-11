using System;

namespace HeaderLinkedList
{
    internal class HeaderLinkedList
    {
        private Node head;

        public HeaderLinkedList()
        {
            head = new Node(0);
        }

        internal void DisplayList()
        {
            if(head.Link == null)
            {
                Console.WriteLine("The list is empty.");
                return;
            }

            Console.WriteLine("The elements of the List are: ");
            var node = head.Link;

            while(node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }

            Console.WriteLine();
        }

        internal void InsertAtEnd(int data)
        {
            var tempNode = new Node(data);

            //// no need to perform the edge case condition check for empty list
            var node = head;

            while(node.Link != null) //// traverse the whole list and reach the reference of last node
            {
                node = node.Link;
            }

            node.Link = tempNode; //// assign the new node to the link of last node, making the new node as the last node
            tempNode.Link = null;
        }

        internal void CreateList()
        {
            Console.Write("Enter the number of nodes in the list: ");
            var numberOfNodes = Convert.ToInt32(Console.ReadLine());

            if (numberOfNodes == 0)
            {
                return;
            }

            for (var i = 1; i <= numberOfNodes; i++)
            {
                Console.Write("Enter the element to be inserted: ");
                var data = Convert.ToInt32(Console.ReadLine());
                InsertAtEnd(data);
            }

            Console.WriteLine("Nodes inserted successfully");
        }

        internal void InsertBefore(int data, int anchorNode)
        {
            //// no need to perform edge cases condition check for empty list and the anchor node being the first node of the list

            //// find reference to predecessor of the anchor node
            var node = head;

            while (node.Link != null)
            {
                if (node.Link.Info == anchorNode)
                    break;

                node = node.Link;
            }

            if (node.Link == null)
            {
                Console.WriteLine($"The anchor node: {anchorNode} is not present in the list");
            }
            else
            {
                //// here, the node object contains the predecessor of anchor node
                var tempNode = new Node(data);
                tempNode.Link = node.Link; //// assign the link stored in predecessor node (which is actually the reference of anchor node) to the new node
                node.Link = tempNode; //// assign the new node to the link of predecessor node, thus inserting it before the anchor node
            }
        }

        internal void InsertAtPosition(int data, int position)
        {
            int i;

            //// no need to perform the edge case condition check for inserting data at first position
            var node = head;

            for (i = 1; i <= position - 1 && node != null; i++)
            {
                node = node.Link;
            }

            if (node == null) //// node will become null if the user provided a position which does not exist in the list
            {
                Console.WriteLine($"You can insert elements only upto {i}th position");
            }
            else
            {
                var tempNode = new Node(data);
                tempNode.Link = node.Link; //// assign the link of node (before the specified position) to the new node
                node.Link = tempNode; //// assign the new node to the link of prior position's node, thus inserting it at the specified position
            }
        }

        internal void DeleteNode(int data)
        {
            //// no need to perform edge cases condition check for empty list and the node to be deleted being the first node of the list
            var node = head;

            while (node.Link != null)
            {
                if (node.Link.Info == data) //// find the predecessor of the node specified
                {
                    break;
                }

                node = node.Link;
            }

            if (node.Link == null)
            {
                Console.WriteLine($"Element {data} is not present in the list");
            }
            else
            {
                node.Link = node.Link.Link; //// assign the reference stored in specified node to the predecessor node, thus removing the specified node
            }
        }

        internal void ReverseList()
        {
            Node previous = null;
            var node = head.Link;

            while (node != null)
            {
                var next = node.Link; //// assign the reference of next node to a temp variable "next"
                node.Link = previous; //// assign previous node to the link of current node
                previous = node; //// make the current node as previous
                node = next; //// make the next node as current node
            }

            head.Link = previous; //// at this point, previous will contain the last node, so we'll assign it to head.Link
        }
    }
}
