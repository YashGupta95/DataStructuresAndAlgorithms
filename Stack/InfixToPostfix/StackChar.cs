using System;

namespace InfixToPostfix
{
    internal class StackChar
    {
        private char[] stackArray;
        private int top;

        public StackChar()
        {
            stackArray = new char[10];
            top = -1;
        }

        public StackChar(int maxSize)
        {
            stackArray = new char[maxSize];
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

        internal void Push(char element)
        {
            if (IsFull())
            {
                Console.WriteLine("Stack Overflow!");
                return;
            }

            top++;
            stackArray[top] = element;
        }

        internal char Pop()
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

        internal char Peek()
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
