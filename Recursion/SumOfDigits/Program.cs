using System;

namespace SumOfDigits
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter a number : ");
            var n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Sum of digits is: {SumDigits(n)}");
        }

        private static int SumDigits(int n)
        {
            if (n / 10 == 0)
                return n;

            return SumDigits(n / 10) + n % 10;
        }
    }
}