using System;
using DataStructures.Core.Stack;

namespace DataStructures.Core.Stack
{
    /// <summary>
    /// Validates whether a given expression has balanced parentheses.
    /// 
    /// Supports:
    /// (), {}, []
    /// 
    /// Example:
    /// Input:  "{[()]}"
    /// Output: true
    /// 
    /// Input:  "{[(])}"
    /// Output: false
    /// </summary>
    public static class ParenthesesValidator
    {
        /// <summary>
        /// Checks if the given expression has balanced parentheses.
        /// </summary>
        /// <param name="expression">Input string</param>
        /// <returns>True if balanced, otherwise false</returns>
        public static bool IsValid(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var stack = new StackArray<char>();

            foreach (char ch in expression)
            {
                if (IsOpeningBracket(ch))
                {
                    stack.Push(ch);
                }
                else if (IsClosingBracket(ch))
                {
                    if (stack.IsEmpty())
                        return false;

                    char top = stack.Pop();

                    if (!IsMatchingPair(top, ch))
                        return false;
                }
            }

            return stack.IsEmpty();
        }

        private static bool IsOpeningBracket(char ch)
        {
            return ch == '(' || ch == '{' || ch == '[';
        }

        private static bool IsClosingBracket(char ch)
        {
            return ch == ')' || ch == '}' || ch == ']';
        }

        private static bool IsMatchingPair(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '{' && close == '}') ||
                   (open == '[' && close == ']');
        }
    }
}