using System;

namespace DoubleLinkedList
{
    internal class DoubleLinkedList
    {
        private Node start;

        public DoubleLinkedList()
        {
            start = null;
        }

        internal void DisplayList()
        {
            if (start == null)
            {
                Console.WriteLine("The List is empty");
                return;
            }

            Console.WriteLine("The elements of the List are: ");
            var node = start;

            while (node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Next;
            }

            Console.WriteLine();
        }

        internal void InsertInEmptyList(int data)
        {
            var tempNode = new Node(data);
            start = tempNode;
        }

        internal void InsertInBeginning(int data)
        {
            var tempNode = new Node(data);

            tempNode.Next = start; //// the reference to first node (which is stored in start) should be assigned to Next link of new node
            start.Previous = tempNode; //// the previous reference of existing first node should now store the reference to new node
            start = tempNode; //// now start should contain the reference to new node, thus inserting it in the beginning
        }

        internal void InsertAtEnd(int data)
        {
            var tempNode = new Node(data);
            var node = start;

            while (node.Next != null)
            {
                node = node.Next;
            }

            node.Next = tempNode; //// assign the new node to the Next link of last node, making the new node as the last node
            tempNode.Previous = node; //// assign the reference of existing last node to the Previous link of new node
        }

        internal void CreateList()
        {
            int data;

            Console.Write("Enter the number of nodes: ");
            var numberOfNodes = Convert.ToInt32(Console.ReadLine());

            if (numberOfNodes == 0)
                return;

            Console.Write("Enter the element to be inserted: ");
            data = Convert.ToInt32(Console.ReadLine());
            InsertInEmptyList(data);

            for (var i = 2; i <= numberOfNodes; i++)
            {
                Console.Write("Enter the element to be inserted: ");
                data = Convert.ToInt32(Console.ReadLine());
                InsertAtEnd(data);
            }
        }

        internal void InsertAfter(int data, int anchorNode)
        {
            var node = start;

            while (node != null)
            {
                if (node.Info == anchorNode)
                    break;

                node = node.Next;
            }

            if (node == null)
            {
                Console.WriteLine($"The anchor node: {anchorNode} is not present in the list");
            }
            else
            {
                var tempNode = new Node(data);
                tempNode.Previous = node;
                tempNode.Next = node.Next;

                if (node.Next != null)
                    node.Next.Previous = tempNode; //// should not be done if node refers to the last node

                node.Next = tempNode; //// if node refers to the last node, just assign new node to the Next link of node
            }
        }

        internal void InsertBefore(int data, int anchorNode)
        {
            if (start == null)
            {
                Console.WriteLine("The list is empty");
                return;
            }

            //// if anchor node is the very first node in the list
            if (anchorNode == start.Info)
            {
                var tempNode = new Node(data);
                tempNode.Next = start;
                start.Previous = tempNode;
                start = tempNode;
                return;
            }

            //// no need to find the predecessor as we have the link to previous node as well
            var node = start;

            while(node != null)
            {
                if (node.Info == anchorNode)
                    break;

                node = node.Next;
            }

            if (node == null)
            {
                Console.WriteLine($"The anchor node: {anchorNode} is not present in the list");
            }
            else
            {
                var tempNode = new Node(data);

                tempNode.Previous = node.Previous; //// the Previous reference stored in anchor node should now be assigned to the Previous of new node
                tempNode.Next = node; //// the anchor node should be assigned to the Next reference of new node
                node.Previous.Next = tempNode; //// the new node should be assigned to the Next reference of the node before anchor node
                node.Previous = tempNode; //// the new node should also be assigned to the Previous reference of anchor node
            }

        }

        internal void DeleteFirstNode()
        {
            if (start == null) //// list is empty
                return;

            if(start.Next == null) //// list has only one node
            {
                start = null;
                return;
            }

            start = start.Next;
            start.Previous = null;
        }

        internal void DeleteLastNode()
        {
            if (start == null) //// list is empty
                return;

            if (start.Next == null) //// list has only one node
            {
                start = null;
                return;
            }

            var node = start;

            while (node.Next != null)
                node = node.Next;

            node.Previous.Next = null;
        }

        internal void DeleteNode(int data)
        {
            if (start == null) //// list is empty
                return;

            if(start.Next == null) //// list has only one node, i.e. deleting the only node
            {
                if (start.Info == data)
                    start = null;
                else
                    Console.WriteLine($"Node {data} was not found in the list");
                
                return;
            }

            if(start.Info == data) //// if the list has more than one node and data is the first node, i.e. deleting the first node
            {
                start = start.Next;
                start.Previous = null;
                return;
            }

            var node = start.Next;

            while(node.Next != null)
            {
                if (node.Info == data)
                    break;

                node = node.Next;
            }

            if(node.Next != null) //// node to be deleted is in between
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }
            else //// node to be deleted is the last node 
                node.Previous.Next = null;
        }

        internal void ReverseList()
        {
            if (start == null)
                return;

            var node = start;
            var next = node.Next;

            node.Next = null;
            node.Previous = next;

            while(next != null)
            {
                next.Previous = next.Next;
                next.Next = node;
                node = next;
                next = next.Previous;
            }

            start = node;
        }
    }
}
