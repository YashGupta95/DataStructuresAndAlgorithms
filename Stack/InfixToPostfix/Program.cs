using System;

namespace InfixToPostfix
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter infix expression : ");
            var infix = Console.ReadLine();

            var postfix = InfixToPostfix(infix);

            Console.WriteLine("Postfix expression is : " + postfix);
            Console.WriteLine("Value of expression : " + EvaluatePostfix(postfix));
        }

        private static string InfixToPostfix(string infix)
        {
            var postfix = "";
            var stackChar = new StackChar(20);

            char next;

            for (var i = 0; i < infix.Length; i++)
            {
                var symbol = infix[i];

                if (symbol == ' ' || symbol == '\t') //// Ignore blanks and tabs
                    continue;

                switch (symbol)
                {
                    case '(':
                        stackChar.Push(symbol);
                        break;
                    case ')':
                        while ((next = stackChar.Pop()) != '(')
                        {
                            postfix += next;
                        }
                        break;
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '^':
                        while (!stackChar.IsEmpty() && GetPrecedence(stackChar.Peek()) >= GetPrecedence(symbol))
                        {
                            postfix += stackChar.Pop();
                        }

                        stackChar.Push(symbol);
                        break;
                    default: //// If the char is an operand
                        postfix += symbol;
                        break;
                }
            }

            while (!stackChar.IsEmpty())
            {
                postfix += stackChar.Pop();
            }

            return postfix;
        }

        private static int GetPrecedence(char symbol)
        {
            switch (symbol)
            {
                case '(':
                    return 0;
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                case '%':
                    return 2;
                case '^':
                    return 3;
                default:
                    return 0;
            }
        }

        private static int EvaluatePostfix(string postfix)
        {
            var stackInt = new StackInt(20);

            for (var i = 0; i < postfix.Length; i++)
            {
                if (char.IsDigit(postfix[i]))
                    stackInt.Push(Convert.ToInt32(char.GetNumericValue(postfix[i])));
                else
                {
                    var x = stackInt.Pop();
                    var y = stackInt.Pop();

                    switch (postfix[i])
                    {
                        case '+':
                            stackInt.Push(y + x); 
                            break;
                        case '-':
                            stackInt.Push(y - x); 
                            break;
                        case '*':
                            stackInt.Push(y * x); 
                            break;
                        case '/':
                            stackInt.Push(y / x); 
                            break;
                        case '%':
                            stackInt.Push(y % x); 
                            break;
                        case '^':
                            stackInt.Push(Power(y, x));
                            break;
                    }
                }
            }

            return stackInt.Pop();
        }

        private static int Power(int num, int pow)
        {
            var x = 1;

            for (var i = 1; i <= pow; i++)
                x *= num;
            
            return x;
        }
    }
}