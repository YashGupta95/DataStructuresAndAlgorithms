using System;

namespace CircularLinkedList
{
    internal class CircularLinkedList
    {
        private Node last;

        public CircularLinkedList()
        {
            last = null;
        }

        internal void DisplayList()
        {
            if(last == null)
            {
                Console.WriteLine("The list is empty");
                return;
            }

            var node = last.Link;

            Console.WriteLine("The elements of the List are: ");
            // a do-while loop is used because while's terminating condition is the same as node's initial value
            do
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }
            while (node != last.Link);

            Console.WriteLine();
        }

        internal void InsertInBeginning(int data)
        {
            var tempNode = new Node(data);
            tempNode.Link = last.Link; //// the reference of the node which was at first position should be stored in Link of new node
            last.Link = tempNode; //// last.Link also needs to be updated to point to new node, thus making it the first node
        }

        internal void InsertInEmptyList(int data)
        {
            var tempNode = new Node(data);
            last = tempNode;
            last.Link = last; //// this has to be done because there should be no null link in Circular Linked List
        }

        internal void InsertAtEnd(int data)
        {
            var tempNode = new Node(data);
            tempNode.Link = last.Link;
            last.Link = tempNode;
            last = tempNode; //// this statement will make tempNode as the last node
        }

        internal void CreateList()
        {
            int data;

            Console.Write("Enter the number of nodes in the list: ");
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
            var node = last.Link;

            // a do-while loop is used because while's terminating condition is the same as node's initial value
            do
            {
                if (node.Info == anchorNode)
                    break;

                node = node.Link;
            }
            while (node != last.Link);

            if(node == last.Link && node.Info != anchorNode)
                Console.WriteLine($"Node {anchorNode} is not present in the list");
            else
            {
                var tempNode = new Node(data);
                tempNode.Link = node.Link;
                node.Link = tempNode;

                if (node == last) //// if the node needs to be inserted after the last node
                    last = tempNode;
            }
        }

        internal void DeleteFirstNode()
        {
            if (last == null)
                return;

            if(last.Link == last)
            {
                last = null;
                return;
            }

            last.Link = last.Link.Link;
        }

        internal void DeleteLastNode()
        {
            if (last == null)
                return;

            if(last.Link == last)
            {
                last = null;
                return;
            }

            var node = last.Link;

            //// find reference to second last node of the list
            while (node.Link != last)
                node = node.Link;

            node.Link = last.Link; //// assign the reference of first node (last.Link) to the second last node, thus deleting the last node
            last = node; //// assign second last node to "last"
        }

        internal void DeleteNode(int data)
        {
            if (last == null)
                return;

            if (last.Link == last && last.Info == data) //// deletion of the only node
            {
                last = null;
                return;
            }

            if(last.Link.Info == data) //// deletion of the first node
            {
                last.Link = last.Link.Link;
                return;
            }

            var node = last.Link;

            while(node.Link != last.Link)
            {
                if (node.Link.Info == data) //// find the predecessor of the node specified
                    break;

                node = node.Link;
            }

            if(node.Link == last.Link)
                Console.WriteLine($"Element {data} is not present in the list");
            else
            {
                node.Link = node.Link.Link;

                if (last.Info == data) //// if the last node has to be deleted, updated the reference of last
                    last = node;
            }
        }

        internal void ConcatenateLists(CircularLinkedList list)
        {
            if(last == null)
            {
                last = list.last;
                return;
            }

            if (list.last == null)
                return;

            var node = last.Link; //// store the reference to first node of original list in "node"
            last.Link = list.last.Link; //// "last" of original list should now point to the first node of new list instead of its own first node
            list.last.Link = node; //// assign the reference to first node of original list to "last" of new list
            last = list.last; //// both the "last" references should point to the first node of new concatenated list
        }
    }
}
