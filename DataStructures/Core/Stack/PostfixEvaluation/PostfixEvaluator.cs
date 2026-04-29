using System;
using DataStructures.Core.Stack;

namespace DataStructures.Core.Stack
{
    /// <summary>
    /// Evaluates a postfix (Reverse Polish Notation) expression.
    /// 
    /// Example:
    /// Input:  "23*54*+9-"
    /// Output: 17
    /// 
    /// Uses stack-based evaluation.
    /// </summary>
    public static class PostfixEvaluator
    {
        /// <summary>
        /// Evaluates a postfix expression and returns the result.
        /// </summary>
        /// <param name="expression">Postfix expression</param>
        /// <returns>Integer result</returns>
        /// <exception cref="ArgumentException">Invalid expression</exception>
        public static int Evaluate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be null or empty.");

            var stack = new StackArray<int>();

            foreach (char ch in expression)
            {
                if (char.IsWhiteSpace(ch))
                    continue;

                // Operand
                if (char.IsDigit(ch))
                {
                    stack.Push(ch - '0'); // Convert char to int
                }
                // Operator
                else if (IsOperator(ch))
                {
                    if (stack.Count < 2)
                        throw new ArgumentException("Invalid postfix expression.");

                    int operand2 = stack.Pop();
                    int operand1 = stack.Pop();

                    int result = ApplyOperator(operand1, operand2, ch);
                    stack.Push(result);
                }
                else
                {
                    throw new ArgumentException($"Invalid character: {ch}");
                }
            }

            if (stack.Count != 1)
                throw new ArgumentException("Invalid postfix expression.");

            return stack.Pop();
        }

        private static bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%';
        }

        private static int ApplyOperator(int a, int b, char op)
        {
            return op switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => b == 0
                        ? throw new DivideByZeroException("Division by zero is not allowed.")
                        : a / b,
                '%' => b == 0
                        ? throw new DivideByZeroException("Modulo by zero is not allowed.")
                        : a % b,
                _ => throw new ArgumentException($"Unsupported operator: {op}")
            };
        }
    }
}