using System;

namespace TowerOfHanoi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter the number of disks : ");
            var n = Convert.ToInt32(Console.ReadLine());

            TowerOfHanoi(n, 'A', 'B', 'C');
        }

        private static void TowerOfHanoi(int n, char source, char temp, char dest)
        {
            if (n == 1)
            {
                Console.WriteLine($"Move Disk {n} from {source} --> {dest}");
                return;
            }

            TowerOfHanoi(n - 1, source, dest, temp);
            Console.WriteLine($"Move Disk {n} from {source} --> {dest}");
            TowerOfHanoi(n - 1, temp, source, dest);
        }
    }
}