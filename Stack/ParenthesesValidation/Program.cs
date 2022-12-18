using System;

namespace ParenthesesValidation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter an expression with parentheses : ");
            var expression = Console.ReadLine();

            if (IsValid(expression))
                Console.WriteLine("Valid expression");
            else
                Console.WriteLine("Invalid expression");
        }

        private static bool IsValid(string expr)
        {
            var stackArray = new StackArray();

            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '(' || expr[i] == '{' || expr[i] == '[')
                    stackArray.Push(expr[i]);

                if (expr[i] == ')' || expr[i] == '}' || expr[i] == ']')
                {
                    if (stackArray.IsEmpty())
                    {
                        Console.WriteLine("Right parentheses are more than left parentheses");
                        return false;
                    }
                    else
                    {
                        var ch = stackArray.Pop();
                        if (!MatchParentheses(ch, expr[i]))
                        {
                            Console.WriteLine("Mismatched parentheses are : ");
                            Console.WriteLine(ch + " and " + expr[i]);
                            return false;
                        }
                    }
                }
            }

            if (stackArray.IsEmpty())
            {
                Console.WriteLine("Balanced Parentheses");
                return true;
            }
            else
            {
                Console.WriteLine("Left parentheses are more than right parentheses");
                return false;
            }
        }

        private static bool MatchParentheses(char char1, char char2)
        {
            if (char1 == '[' && char2 == ']')
                return true;
            if (char1 == '{' && char2 == '}')
                return true;
            if (char1 == '(' && char2 == ')')
                return true;
            
            return false;
        }
    }
}