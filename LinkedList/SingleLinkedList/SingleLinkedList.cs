using System;

namespace SingleLinkedList
{
    internal class SingleLinkedList
    {
        private Node start;

        public SingleLinkedList()
        {
            start = null;
        }

        internal void DisplayList()
        {
            if(start == null)
            {
                Console.WriteLine("The Linked List is empty.");
                return;
            }

            Console.WriteLine("The elements of the List are: ");
            var node = start;

            while(node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }

            Console.WriteLine();
        }

        internal void CountNodes()
        {
            var numberofNodes = 0;
            var node = start;

            while(node != null)
            {
                numberofNodes++;
                node = node.Link;  //// move to next node
            }

            Console.WriteLine($"Number of nodes in the List: {numberofNodes}");
        }

        internal void Search(int data)
        {
            var position = 1;
            var node = start;

            while(node != null)
            {
                if (node.Info == data)
                    break;

                position++;
                node = node.Link;
            }

            if(node == null)
            {
                Console.WriteLine($"Element {data} was not found in the List");
            }
            else
            {
                Console.WriteLine($"Element {data} is present at position: {position}");
            }
        }

        internal void InsertInBeginning(int data)
        {
            var tempNode = new Node(data);
            
            //// the reference to existing first node of list (which is stored in start) should be shifted to the Link of tempNode, so that tempNode becomes the first node
            tempNode.Link = start;
            start = tempNode; //// start has become null after above statement, so it should now refer to this new node since this one will be the first node in list
        }

        internal void InsertAtEnd(int data)
        {
            var tempNode = new Node(data);

            if(start == null)
            {
                start = tempNode;
                return;
            }

            var node = start;

            while(node.Link != null) //// traverse the whole list and reach the reference of last node
            {
                node = node.Link;
            }

            node.Link = tempNode; //// assign the new node to the link of last node, making the new node as the last node
        }

        internal void CreateList()
        {
            Console.Write("Enter the number of nodes in the list: ");
            var numberOfNodes = Convert.ToInt32(Console.ReadLine());

            if(numberOfNodes == 0)
            {
                return;
            }

            for(var i = 1; i <= numberOfNodes; i++)
            {
                Console.Write("Enter the element to be inserted: ");
                var data = Convert.ToInt32(Console.ReadLine());
                InsertAtEnd(data);
            }

            Console.WriteLine("Nodes inserted successfully");
        }

        internal void InsertAfter(int data, int anchorNode)
        {
            if (start == null)
            {
                Console.WriteLine("The list is empty");
                return;
            }

            var node = start;

            while(node != null) //// this loop will find the reference to the anchor node
            {
                if (node.Info == anchorNode)
                    break;

                node = node.Link;
            }

            if(node == null)
            {
                Console.WriteLine($"The anchor node: {anchorNode} is not present in the list");
            }
            else
            {
                var tempNode = new Node(data);
                tempNode.Link = node.Link; //// assign the link of node (which was after anchor node) to the new node
                node.Link = tempNode; //// assign the new node to the link of anchor node, thus inserting it after the anchor node
                Console.WriteLine($"Element {data} inserted successfully");
            }
        }

        internal void InsertBefore(int data, int anchorNode)
        {
            if(start == null)
            {
                Console.WriteLine("The list is empty");
                return;
            }

            //// if anchor node is the very first node in the list
            if(anchorNode == start.Info)
            {
                var tempNode = new Node(data);
                tempNode.Link = start;
                start = tempNode;
                return;
            }

            //// find reference to predecessor of the anchor node
            var node = start;

            while(node.Link != null)
            {
                if (node.Link.Info == anchorNode)
                    break;

                node = node.Link;
            }

            if(node.Link == null)
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

            if(position == 1)
            {
                var tempNode = new Node(data);
                tempNode.Link = start;
                start = tempNode;
                return;
            }

            var node = start;

            //// find reference to the node before the specified position
            for(i=1; i < position - 1 && node != null; i++)
            {
                node = node.Link;
            }

            if(node == null) //// node will become null if the user provided a position which does not exist in the list
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

        internal void DeleteFirstNode()
        {
            if(start == null)
            {
                return;
            }

            start = start.Link; //// remove the reference to first node, thus deleting the first node from the list
        }

        internal void DeleteLastNode()
        {
            if(start == null)
            {
                return;
            }

            if(start.Link == null) //// if there's only one node in list
            {
                start = null;
                return;
            }

            var node = start;

            while(node.Link.Link != null)
            {
                node = node.Link;
            }

            node.Link = null; //// remove the reference to last node, thus deleting the last node from the list
        }

        internal void DeleteNode(int data)
        {
            if (start == null)
            {
                Console.WriteLine("The list is empty");
                return;
            }

            if(start.Info == data) //// if the node to be deleted is the first node
            {
                start = start.Link;
                return;
            }

            var node = start;

            while (node.Link != null)
            {
                if(node.Link.Info == data) //// find the predecessor of the node specified
                {
                    break;
                }

                node = node.Link;
            }

            if(node.Link == null)
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
            var node = start;

            while(node != null)
            {
                var next = node.Link; //// assign the reference of next node to a temp variable "next"
                node.Link = previous; //// assign previous node to the link of current node
                previous = node; //// make the current node as previous
                node = next; //// make the next node as current node
            }

            start = previous; //// at this point, previous will contain the last node, so we'll assign it to start
        }

        internal void BubbleSortExchangeData()
        {
            Node node, next, end;

            //// outer for loop coressponds to each pass of Bubble Sort
            for (end = null; end != start.Link; end = node) //// stop end when it refers to the second node
            {
                //// inner for loop will perform swapping (wherever required) in that pass
                for (node = start; node.Link != end; node = node.Link) 
                {
                    next = node.Link;

                    if(node.Info > next.Info) ///// compare adjacent nodes
                    {
                        var tempNode = node.Info;
                        node.Info = next.Info;
                        next.Info = tempNode;
                    }
                }
            }
        }

        internal void BubbleSortExchangeLinks()
        {
            Node previous, node, next, end;

            //// outer for loop coressponds to each pass of Bubble Sort
            for (end = null; end != start.Link; end = node)
            {
                //// inner for loop will perform swapping (wherever required) in that pass
                for (previous = node = start; node.Link != end; previous = node, node = node.Link)
                {
                    next = node.Link;
                    
                    if(node.Info > next.Info)
                    {
                        node.Link = next.Link;
                        next.Link = node;

                        if(node != start)
                        {
                            previous.Link = next;
                        }
                        else
                        {
                            start = next;
                        }

                        //// although the links of nodes have been swapped, our reference variables are still holding the nodes 
                        //// as assigned before (the order of variables now is 'previous' -> 'next' -> 'node'), so we need to swap
                        //// 'next' and 'node' so that thee reference variables are in correct order for the next iteration of for loop
                        var tempNode = node;
                        node = next;
                        next = tempNode;
                    }
                }
            }
        }

        internal SingleLinkedList MergeIntoNewList(SingleLinkedList list2)
        {
            var mergedList = new SingleLinkedList();
            mergedList.start = MergeIntoNewList(start, list2.start);
            return mergedList;
        }

        private static Node MergeIntoNewList(Node node1, Node node2)
        {
            Node startMerged;

            if (node1 == null)
                return node2;
            else if (node2 == null)
                return node1;

            if (node1.Info <= node2.Info)
            {
                startMerged = new Node(node1.Info);
                node1 = node1.Link;
            }
            else
            {
                startMerged = new Node(node2.Info);
                node2 = node2.Link;
            }
            
            var nodeMerged = startMerged; //// nodeMerged will always refer to the newly inserted node in the merged list

            while(node1 != null && node2 != null)
            {
                if(node1.Info <= node2.Info)
                {
                    nodeMerged.Link = new Node(node1.Info);
                    node1 = node1.Link;
                }
                else
                {
                    nodeMerged.Link = new Node(node2.Info);
                    node2 = node2.Link;
                }

                nodeMerged = nodeMerged.Link;
            }

            //// if the second list has finished and some nodes are still left in first list
            while(node1 != null)
            {
                nodeMerged.Link = new Node(node1.Info);
                node1 = node1.Link;
                nodeMerged = nodeMerged.Link;
            }

            //// if the first list has finished and some nodes are still left in second list
            while(node2 != null)
            {
                nodeMerged.Link = new Node(node2.Info);
                node2 = node2.Link;
                nodeMerged = nodeMerged.Link;
            }

            return startMerged;
        }

        internal SingleLinkedList MergeByRearrangingLinks(SingleLinkedList list2)
        {
            var mergedList = new SingleLinkedList();
            mergedList.start = MergeByRearrangingLinks(start, list2.start);
            return mergedList;
        }

        private static Node MergeByRearrangingLinks(Node node1, Node node2)
        {
            Node startMerged;

            if (node1 == null)
                return node2;
            else if (node2 == null)
                return node1;

            if(node1.Info <= node2.Info)
            {
                startMerged = node1;
                node1 = node1.Link;
            }
            else
            {
                startMerged = node2;
                node2 = node2.Link;
            }

            var nodeMerged = startMerged;

            while(node1 != null && node2 != null)
            {
                if(node1.Info <= node2.Info)
                {
                    nodeMerged.Link = node1;
                    nodeMerged = nodeMerged.Link;
                    node1 = node1.Link;
                }
                else
                {
                    nodeMerged.Link = node2;
                    nodeMerged = nodeMerged.Link;
                    node2 = node2.Link;
                }
            }

            if (node1 == null)
                nodeMerged.Link = node2;
            else
                nodeMerged.Link = node1;

            return startMerged;
        }

        internal void MergeSort()
        {
            start = RecursiveMergeSort(start);
        }

        private static Node RecursiveMergeSort(Node listStart)
        {
            if (listStart == null || listStart.Link == null) //// if the list is empty or has just one node
                return listStart;

            var start1 = listStart;
            var start2 = DivideList(listStart);

            start1 = RecursiveMergeSort(start1);
            start2 = RecursiveMergeSort(start2);

            var startMerged = MergeByRearrangingLinks(start1, start2);
            return startMerged;
        }

        private static Node DivideList(Node start)
        {
            var node = start.Link.Link; //// start will point to first element and node will point to third element of list

            //// node will become null if the list has even number of nodes, and node.Link will become null if the list has odd number of nodes
            while(node != null && node.Link != null)
            {
                start = start.Link; //// increment by 1 node at a time
                node = node.Link.Link; //// increment by 2 nodes at a time
            }

            //// at this point, start will be approx halfway through the list, so we assign the link to middle node to start2
            var start2 = start.Link;
            start.Link = null; //// make start.Link as null to end the first half of list
            return start2;
        }

        internal void InsertCycle(int data)
        {
            if (start == null)
                return;

            Node node = start, nodeData = null, previous = null;

            while(node != null)
            {
                if (node.Info == data)
                    nodeData = node; //// store the reference of the node where the cycle needs to be inserted

                previous = node;
                node = node.Link;
            }

            //// at this point, previous points to the last node
            if(nodeData != null)
                previous.Link = nodeData; //// make the Link of previous refer to nodeData, thus inserting a cycle at nodeData
        }

        internal bool HasCycle()
        {
            if(FindCycle() == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// The below function is an implementation of Floyd's Cycle Detection Algorithm, also known as Hare & Tortoise Algorithm
        /// </summary>
        /// <returns>The reference of the node where cycle was found if a cycle is present, null if no cycle is present</returns>
        private Node FindCycle()
        {
            if (start == null || start.Link == null)
                return null;

            Node slowRef = start, fastRef = start;

            while(fastRef != null && fastRef.Link != null)
            {
                slowRef = slowRef.Link; //// increment by 1 node at a time
                fastRef = fastRef.Link.Link; //// increment by 2 nodes at a time

                if(slowRef == fastRef)
                    return slowRef;
            }

            return null;
        }

        internal void RemoveCycle()
        {
            var cycleNode = FindCycle(); //// cycleNode contains the node where slowRef and fastRef met

            if (cycleNode == null)
                return;

            Console.WriteLine($"The node at which the cycle was detected is: {cycleNode.Info}");

            //// to remove a cycle, we need to find the total length of the list
            Node ref1 = cycleNode, ref2 = cycleNode;
            var cycleLength = 0;

            //// a do-while loop is used because initially both ref1 & ref2 are equal, and so a while loop would've terminated in the first iteration itself
            do
            {
                cycleLength++;
                ref2 = ref2.Link;
            }
            while(ref1 != ref2);

            Console.WriteLine($"Length of the cycle is: {cycleLength}");

            var remainingListLength = 0;
            ref1 = start; //// pointing ref1 to start and keeping ref2 where it was, i.e. somewhere within the cycle

            while(ref1 != ref2) //// the number of times they will move before meeting again will give us the length of remaining list
            {
                remainingListLength++;
                ref1 = ref1.Link;
                ref2 = ref2.Link;
            }

            Console.WriteLine($"Length of the remaining list is: {remainingListLength}");

            var listLength = cycleLength + remainingListLength;
            Console.WriteLine($"Total length of the list is: {listLength}");

            ref1 = start;

            for(var i = 1; i <= listLength - 1; i++)
            {
                ref1 = ref1.Link;
            }

            ref1.Link = null; //// assigning null to the reference of last node, thus removing the cycle
        }

        internal void ConcatenateLists(SingleLinkedList list)
        {
            if(start == null)
            {
                start = list.start;
                return;
            }

            if (list.start == null)
                return;

            var node = start;

            while (node.Link != null)
                node = node.Link;

            node.Link = list.start;
        }
    }
}
