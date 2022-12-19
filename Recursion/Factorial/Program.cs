using System;

namespace Factorial
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter a number (greater than or equal to zero) : ");
            var n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Factorial of {n} is: {Factorial(n)}");
        }

        private static long Factorial(int n)
        {
            if (n == 0)
                return 1;

            return n * Factorial(n - 1);
        }
    }
}