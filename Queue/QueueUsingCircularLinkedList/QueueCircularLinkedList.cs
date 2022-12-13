using System;

namespace QueueUsingCircularLinkedList
{
    internal class QueueCircularLinkedList
    {
        private Node rear;

        public QueueCircularLinkedList()
        {
            rear = null;
        }

        internal bool IsEmpty()
        {
            return (rear == null);
        }

        internal int Size()
        {
            var size = 0;
            var node = rear.Link;

            if (IsEmpty())
            {
                return 0;
            }

            do
            {
                size++;
                node = node.Link;
            } 
            while (node != rear.Link);

            return size;
        }

        internal void Insert(int x)
        {
            var tempNode = new Node(x);

            if (IsEmpty())
            {
                rear = tempNode;
                rear.Link = rear;
            }
            else
            {
                tempNode.Link = rear.Link;
                rear.Link = tempNode;
                rear = tempNode;
            }
        }

        internal int Delete()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow!");

            Node tempNode;

            if (rear.Link == rear)
            {
                tempNode = rear;
                rear = null;
            }
            else
            {
                tempNode = rear.Link;
                rear.Link = tempNode.Link;
            }

            return tempNode.Info;
        }

        internal int Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow!");

            return rear.Link.Info;
        }

        internal void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue is empty!");
                return;
            }

            Console.WriteLine("Queue elements are: ");
            var node = rear.Link;

            do
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            } 
            while (node != rear.Link);

            Console.WriteLine();
        }
    }
}
