using System;

namespace GCDUsingEuclidsAlgorithm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter the first number : ");
            var num1 = Convert.ToInt32(Console.ReadLine());
            
            Console.Write("Enter the second number : ");
            var num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"GCD of {num1} and {num2} is: {FindGCD(num1, num2)}");
        }

        private static int FindGCD(int num1, int num2)
        {
            if (num2 == 0)
                return num1;

            return FindGCD(num2, num1 % num2);
        }
    }
}