using System;

namespace StackUsingLinkedList
{
    internal class StackLinkedList
    {
        private Node top;

        public StackLinkedList()
        {
            top = null;
        }

        internal int Size()
        {
            var size = 0;
            var node = top;

            while (node != null)
            {
                node = node.Link;
                size++;
            }

            return size;
        }

        internal bool IsEmpty()
        {
            return (top == null);
        }

        internal void Push(int element)
        {
            var tempNode = new Node(element);

            tempNode.Link = top;
            top = tempNode;
        }

        internal int Pop()
        {
            int element;
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack Underflow!");
            }

            element = top.Info;
            top = top.Link;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack Underflow!");
            }

            return top.Info;
        }

        internal void Display()
        {
            var node = top;

            if (IsEmpty())
            {
                Console.WriteLine("Stack is empty.");
                return;
            }

            Console.WriteLine("Stack elements are : ");
            while (node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }

            Console.WriteLine();
        }
    }
}
