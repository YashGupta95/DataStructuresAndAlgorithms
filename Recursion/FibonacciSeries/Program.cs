using System;

namespace FibonacciSeries
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter number of terms to be printed in Fibonacci Series : ");
            var terms = Convert.ToInt32(Console.ReadLine());

            for (var i = 0; i < terms; i++)
                Console.Write($"{GenerateFibonacciSeries(i)} ");
        }

        private static int GenerateFibonacciSeries(int n)
        {
            if (n < 2)
                return n;

            return GenerateFibonacciSeries(n - 1) + GenerateFibonacciSeries(n - 2);
        }
    }
}