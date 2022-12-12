using System;

namespace StackUsingArray
{
    internal class StackArray
    {
        private int[] stackArray;
        private int top;

        public StackArray()
        {
            stackArray = new int[10];
            top = -1;
        }

        public StackArray(int maxSize)
        {
            stackArray = new int[maxSize];
            top = -1;
        }

        internal int Size()
        {
            return top + 1;
        }

        internal bool IsEmpty()
        {
            return (top == -1);
        }

        internal bool IsFull()
        {
            return (top == stackArray.Length - 1);
        }

        internal void Push(int element)
        {
            if (IsFull())
            {
                Console.WriteLine("Stack Overflow!");
                return;
            }

            top++;
            stackArray[top] = element;
        }

        internal int Pop()
        {
            int element;
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack Underflow!");
            }

            element = stackArray[top];
            top--;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack Underflow!");
            }

            return stackArray[top];
        }

        internal void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Stack is empty.");
                return;
            }

            Console.WriteLine("Stack elements are : ");
            for (var i = top; i >= 0; i--)
            {
                Console.Write($"{stackArray[i]} ");
            }

            Console.WriteLine();
        }
    }
}
