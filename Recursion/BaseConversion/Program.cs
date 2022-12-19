using System;

namespace BaseConversion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter a positive decimal number : ");
            var n = Convert.ToInt32(Console.ReadLine());

            Console.Write("Binary form : "); ToBinary(n);
            Console.WriteLine();
            //Console.Write("Binary form : "); ConvertBase(n, 2);
            //Console.WriteLine();
            Console.Write("Octal form : "); ConvertBase(n, 8);
            Console.WriteLine();
            Console.Write("Hexadecimal form : "); ConvertBase(n, 16);
            Console.WriteLine();
        }

        private static void ToBinary(int n)
        {
            if (n == 0)
                return;

            ToBinary(n / 2);
            Console.Write(n % 2);
        }

        private static void ConvertBase(int num, int b)
        {
            if (num == 0)
                return;
            ConvertBase(num / b, b);

            var remainder = num % b;

            //// For Hexadecimal form, a remainder greater than or equal to 10 will have to be converted into characters A-F
            if (remainder < 10)
                Console.Write(remainder);
            else
                Console.Write((char)(remainder - 10 + 'A'));
        }
    }
}