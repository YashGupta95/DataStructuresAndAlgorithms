using System;

namespace CircularQueue
{
    internal class CircularQueue
    {
        private int[] queueArray;
        private int front;
        private int rear;

        public CircularQueue()
        {
            queueArray = new int[10];
            front = -1;
            rear = -1;
        }

        public CircularQueue(int maxSize)
        {
            queueArray = new int[maxSize];
            front = -1;
            rear = -1;
        }

        internal bool IsEmpty()
        {
            return (front == -1);
        }

        internal bool IsFull()
        {
            return ((front == 0 && rear == queueArray.Length - 1) || (front == rear + 1));
        }

        internal int Size()
        {
            if (IsEmpty())
                return 0;

            if (IsFull())
                return queueArray.Length;

            var i = front;
            var size = 0;

            if (front <= rear)
            {
                while (i <= rear)
                {
                    size++;
                    i++;
                }
            }
            else
            {
                while (i <= queueArray.Length - 1)
                {
                    size++;
                    i++;
                }

                i = 0;
                while (i <= rear)
                {
                    size++;
                    i++;
                }
            }

            return size;
        }

        internal void Insert(int element)
        {
            if (IsFull())
            {
                Console.WriteLine("Queue Overflow!");
                return;
            }

            if (front == -1)
                front = 0;

            if (rear == queueArray.Length - 1)
                rear = 0;
            else
                rear++;

            queueArray[rear] = element;
        }

        internal int Delete()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow!");

            var element = queueArray[front];

            if (front == rear) //// Queue has only one element
            {
                front = -1;
                rear = -1;
            }
            else if (front == queueArray.Length - 1)
                front = 0;
            else
                front++;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow!");

            return queueArray[front];
        }

        internal void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue is empty!");
                return;
            }

            Console.WriteLine("Queue elements are: ");
            var i = front;

            if (front <= rear)
            {
                while (i <= rear)
                    Console.Write($"{queueArray[i++]} ");
            }
            else
            {
                while (i <= queueArray.Length - 1)
                    Console.Write($"{queueArray[i++]} ");
                
                i = 0;
                while (i <= rear)
                    Console.Write($"{queueArray[i++]} ");
            }

            Console.WriteLine();
        }
    }
}
