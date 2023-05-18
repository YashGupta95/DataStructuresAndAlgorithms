using System;

namespace RadixSort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Node node;
            Console.WriteLine("Enter number of elements in the list : ");
            var n = Convert.ToInt32(Console.ReadLine());

            Node start = null;
            for (var i = 1; i <= n; i++)  //// Inserting elements in Linked List
            {
                Console.Write($"Enter element {i} : ");
                var data = Convert.ToInt32(Console.ReadLine());

                var temp = new Node(data);

                if (start == null)
                    start = temp;
                else
                {
                    node = start;

                    while (node.Link != null)
                        node = node.Link;

                    node.Link = temp;
                }
            }

            start = RadixSort(start);

            Console.WriteLine("Sorted list is : ");
            node = start;
            while (node != null)
            {
                Console.Write($"{node.Info} ");
                node = node.Link;
            }

            Console.WriteLine();
        }

        internal static Node RadixSort(Node start)
        {
            var front = new Node[10];
            var rear = new Node[10];

            var leastSigPos = 1;
            var mostSigPos = DigitsInLargestElement(start);

            for (var k = leastSigPos; k <= mostSigPos; k++)
            {
                //// Making all the queues empty at the beginning of each pass
                for (var i = 0; i <= 9; i++)
                {
                    rear[i] = null;
                    front[i] = null;
                }

                for (var node = start; node != null; node = node.Link)
                {
                    //// Find kth digit from right in the number
                    var digit = FindDigit(node.Info, k);

                    //// Insert the node in Queue(digit)
                    if (front[digit] == null)
                        front[digit] = node;
                    else
                        rear[digit].Link = node;

                    rear[digit] = node;
                }

                //// Join all queues to form new linked list
                var j = 0;
                while (front[j] == null)  //// Finding first non-empty queue & appending it to start
                    j++;

                start = front[j];

                while (j <= 8)
                {
                    if (rear[j + 1] != null) //// If (j+1)th  queue is not empty
                        rear[j].Link = front[j + 1]; //// Join end of jth queue to start of (j+1)th queue
                    else
                        rear[j + 1] = rear[j]; //// continue with rear[j]
                    
                    j++;
                }

                rear[9].Link = null;
            }

            return start;
        }

        //// Returns number of digits in the largest element of the list
        private static int DigitsInLargestElement(Node start)
        {
            var largestNum = 0;
            var node = start;

            while (node != null)
            {
                if (node.Info > largestNum)
                    largestNum = node.Info;
                
                node = node.Link;
            }

            //// Find number of digits in largest element
            var digits = 0;
            while (largestNum != 0)
            {
                digits++;
                largestNum /= 10;
            }

            return digits;
        }

        //// Returns kth digit of the number from right
        private static int FindDigit(int num, int k)
        {
            var digit = 0;
            for (var i = 1; i <= k; i++)
            {
                digit = num % 10;
                num /= 10;
            }

            return digit;
        }
    }
}