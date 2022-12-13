using System;

namespace QueueUsingArray
{
    internal class QueueArray
    {
        private int[] queueArray;
        private int front;
        private int rear;

        public QueueArray()
        {
            queueArray = new int[10];
            front = -1;
            rear = -1;
        }

        public QueueArray(int maxSize)
        {
            queueArray = new int[maxSize];
            front = -1;
            rear = -1;
        }

        internal bool IsEmpty()
        {
            return (front == -1 || front == rear + 1);
        }

        internal bool IsFull()
        {
            return (rear == queueArray.Length - 1);
        }

        internal int Size()
        {
            if (IsEmpty())
                return 0;
            else
                return rear - front + 1;
        }

        internal void Insert(int element)
        {
            if (IsFull())
            {
                Console.WriteLine("Queue Overflow!");
                return;
            }

            if (front == -1)
            {
                front = 0;
            }

            rear++;
            queueArray[rear] = element;
        }

        internal int Delete()
        {
            int element;

            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue Underflow!");
            }

            element = queueArray[front];
            front++;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue Underflow!");
            }

            return queueArray[front];
        }

        internal void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue is empty.");
                return;
            }

            Console.WriteLine("Queue elements are :");
            for (var i = front; i <= rear; i++)
            {
                Console.Write($"{queueArray[i]} ");
            }

            Console.WriteLine();
        }
    }
}
