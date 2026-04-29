using System;
using System.Text;
using DataStructures.Core.Stack;

namespace DataStructures.Core.Stack
{
    /// <summary>
    /// Provides utility methods to convert infix expressions to postfix.
    /// 
    /// Example:
    /// Infix:  A + B * C
    /// Postfix: ABC*+
    /// 
    /// Uses Stack-based approach.
    /// </summary>
    public static class ExpressionConverter
    {
        /// <summary>
        /// Converts an infix expression to postfix notation.
        /// </summary>
        /// <param name="expression">Input infix expression</param>
        /// <returns>Postfix expression</returns>
        /// <exception cref="ArgumentException">Invalid expression</exception>
        public static string InfixToPostfix(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be null or empty.");

            var stack = new StackArray<char>();
            var result = new StringBuilder();

            foreach (char ch in expression)
            {
                // Ignore whitespace
                if (char.IsWhiteSpace(ch))
                    continue;

                // Operand
                if (char.IsLetterOrDigit(ch))
                {
                    result.Append(ch);
                }
                // Opening bracket
                else if (ch == '(')
                {
                    stack.Push(ch);
                }
                // Closing bracket
                else if (ch == ')')
                {
                    while (!stack.IsEmpty() && stack.Peek() != '(')
                    {
                        result.Append(stack.Pop());
                    }

                    if (stack.IsEmpty())
                        throw new ArgumentException("Mismatched parentheses.");

                    stack.Pop(); // remove '('
                }
                // Operator
                else if (IsOperator(ch))
                {
                    while (!stack.IsEmpty() &&
                           Precedence(stack.Peek()) >= Precedence(ch))
                    {
                        result.Append(stack.Pop());
                    }

                    stack.Push(ch);
                }
                else
                {
                    throw new ArgumentException($"Invalid character: {ch}");
                }
            }

            // Pop remaining operators
            while (!stack.IsEmpty())
            {
                char top = stack.Pop();

                if (top == '(')
                    throw new ArgumentException("Mismatched parentheses.");

                result.Append(top);
            }

            return result.ToString();
        }

        private static bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^' || ch == '%';
        }

        private static int Precedence(char op)
        {
            return op switch
            {
                '+' or '-' => 1,
                '*' or '/' or '%' => 2,
                '^' => 3,
                _ => 0
            };
        }
    }
}