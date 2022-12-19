using System;

namespace Exponentiation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter the number : ");
            var num = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the power it should be raised to : ");
            var pow = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"{num} ^ {pow} = {Power(num, pow)}");
        }

        private static double Power(double num, int pow)
        {
            if (pow == 0)
                return 1;

            return num * Power(num, pow - 1);
        }
    }
}