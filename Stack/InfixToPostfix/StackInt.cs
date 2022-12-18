using System;

namespace InfixToPostfix
{
    internal class StackInt
    {
        private int[] stackArray;
        private int top;

        public StackInt()
        {
            stackArray = new int[10];
            top = -1;
        }

        public StackInt(int maxSize)
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
            if (IsEmpty())
            {
                Console.WriteLine("Stack Underflow!");
                throw new InvalidOperationException();
            }

            var element = stackArray[top];
            top--;

            return element;
        }

        internal int Peek()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Stack Underflow!");
                throw new InvalidOperationException();
            }

            return stackArray[top];
        }

        internal void Display()
        {
            Console.WriteLine("top= " + top);

            if (IsEmpty())
            {
                Console.WriteLine("Stack is empty!");
                return;
            }

            Console.WriteLine("Stack elements are : ");
            for (var i = top; i >= 0; i--)
            {
                Console.WriteLine(stackArray[i] + " ");
            }

            Console.WriteLine();
        }
    }
}
