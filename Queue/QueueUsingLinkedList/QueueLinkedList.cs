using System;

namespace QueueUsingLinkedList
{
    /// <summary>
    /// This Linked List implementation of Queue uses something known as Double-ended Linked Lists, where we maintain the reference to both first and last nodes
    /// </summary>
    internal class QueueLinkedList
    {
        private Node front;
        private Node rear;

        public QueueLinkedList()
        {
            front = null;
            rear = null;
        }

        internal bool IsEmpty()
        {
            return (front == null);
        }

        internal int Size()
        {
            var size = 0;
            var node = front;

            while (node != null)
            {
                size++;
                node = node.Link;
            }

            return size;
        }

        internal void Insert(int element)
        {
            var tempNode = new Node(element);

            if (front == null) //// If Queue is empty
            {
                front = tempNode;
            }
            else
            {
                rear.Link = tempNode;
            }

            rear = tempNode;
        }

        internal int Delete()
        {
            int element;

            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue Underflow!");
            }

            element = front.Info;
            front = front.Link;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue Underflow!");
            }

            return front.Info;
        }

        internal void Display()
        {
            var node = front;

            if (IsEmpty())
            {
                Console.WriteLine("Queue is empty!");
                return;
            }

            Console.WriteLine("Queue elements are : ");
            while (node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }

            Console.WriteLine();
        }
    }
}
