using System;

namespace ParenthesesValidation
{
    internal class StackArray
    {
        private char[] stackArray;
        private int top;

        public StackArray()
        {
            stackArray = new char[10];
            top = -1;
        }

        public StackArray(int maxSize)
        {
            stackArray = new char[maxSize];
            top = -1;
        }

        internal bool IsEmpty()
        {
            return (top == -1);
        }

        internal bool IsFull()
        {
            return (top == stackArray.Length - 1);
        }

        internal void Push(char element)
        {
            if (IsFull())
            {
                Console.WriteLine("Stack Overflow\n");
                return;
            }

            top++;
            stackArray[top] = element;
        }

        internal char Pop()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Stack Underflow\n");
                throw new InvalidOperationException();
            }

            var element = stackArray[top];
            top--;

            return element;
        }
    }
}
