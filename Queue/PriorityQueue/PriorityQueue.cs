using System;

namespace PriorityQueue
{
    /// <summary>
    /// Priority Queue has been implemented using Sorted Linked Lists (that is sorted based on Priority)
    /// </summary>
    internal class PriorityQueue
    {
        private Node front;

        public PriorityQueue()
        {
            front = null;
        }

        internal bool IsEmpty()
        {
            return (front == null);
        }

        internal void Insert(int element, int elementPriority)
        {
            var temp = new Node(element, elementPriority);

            //// If the Queue is empty or element to be added has priority more than first element
            if (IsEmpty() || elementPriority < front.Priority)
            {
                temp.Link = front;
                front = temp;
            }
            else
            {
                var node = front;

                while (node.Link != null && node.Link.Priority <= elementPriority)
                {
                    node = node.Link;
                }

                temp.Link = node.Link;
                node.Link = temp;
            }
        }

        internal int Delete()
        {
            int element;

            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow!");
            else
            {
                element = front.Info;
                front = front.Link;
            }

            return element;
        }

        internal void Display()
        {
            var node = front;

            if (IsEmpty())
                Console.WriteLine("Queue is empty!");
            else
            {
                Console.WriteLine("Queue elements are: ");
                Console.WriteLine("Element  Priority");

                while (node != null)
                {
                    Console.WriteLine($"{node.Info}             {node.Priority}");
                    node = node.Link;
                }
            }

            Console.WriteLine("");
        }
    }
}
