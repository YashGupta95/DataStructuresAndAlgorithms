using System;

namespace DequeUsingArray
{
    internal class DequeArray
    {
        private int[] dequeArray;
        private int front;
        private int rear;

        public DequeArray()
        {
            dequeArray = new int[10];
            front = -1;
            rear = -1;
        }

        public DequeArray(int maxSize)
        {
            dequeArray = new int[maxSize];
            front = -1;
            rear = -1;
        }

        internal bool IsEmpty()
        {
            return (front == -1);
        }

        internal bool IsFull()
        {
            return ((front == 0 && rear == dequeArray.Length - 1) || (front == rear + 1));
        }

        internal void InsertFront(int element)
        {
            if (IsFull())
            {
                Console.WriteLine("Deque Overflow!");
                return;
            }

            if (front == -1)
            {
                front = 0;
                rear = 0;
            }
            else if (front == 0)
                front = dequeArray.Length - 1;
            else
                front--;

            dequeArray[front] = element;
        }

        internal void InsertRear(int element)
        {
            if (IsFull())
            {
                Console.WriteLine("Deque Overflow!");
                return;
            }

            if (front == -1)
                front = 0;

            if (rear == dequeArray.Length - 1)
                rear = 0;
            else
                rear++;

            dequeArray[rear] = element;
        }

        internal int DeleteFront()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Deque Underflow!");

            var element = dequeArray[front];

            if (front == rear) //// Deque has only one element
            {
                front = -1;
                rear = -1;
            }
            else if (front == dequeArray.Length - 1)
                front = 0;
            else
                front++;

            return element;
        }

        internal int DeleteRear()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Deque Underflow!");

            var element = dequeArray[rear];

            if (front == rear) //// Deque has only one element
            {
                front = -1;
                rear = -1;
            }
            else if (rear == 0)
                rear = dequeArray.Length - 1;
            else
                rear--;

            return element;
        }

        internal void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Deque is empty!");
                return;
            }

            Console.WriteLine("Deque elements are: ");
            var i = front;

            if (front <= rear)
            {
                while (i <= rear)
                    Console.Write($"{dequeArray[i++]} ");
            }
            else
            {
                while (i <= dequeArray.Length - 1)
                    Console.Write($"{dequeArray[i++]} ");

                i = 0;
                while (i <= rear)
                    Console.Write($"{dequeArray[i++]} ");
            }

            Console.WriteLine();
        }
    }
}
