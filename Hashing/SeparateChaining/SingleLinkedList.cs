using System;

namespace SeparateChaining
{
    internal class SingleLinkedList
    {
        private Node start;

        public SingleLinkedList()
        {
            start = null;
        }

        internal void DisplayList()
        {
            Node node;

            if (start == null)
            {
                Console.WriteLine("   ");
                return;
            }

            node = start;
            while (node != null)
            {
                Console.Write($"{node.Info.ToString()}  ");
                node = node.Link;
            }

            Console.WriteLine();
        }


        internal StudentRecord Search(int data)
        {
            var node = start;

            while (node != null)
            {
                if (node.Info.GetStudentId() == data)
                    break;

                node = node.Link;
            }

            if (node == null)
            {
                Console.WriteLine($"{data} not found in list");
                return null;
            }
            else
                return node.Info;
        }

        internal void InsertInBeginning(StudentRecord studentRecord)
        {
            var temp = new Node(studentRecord);
            
            temp.Link = start;
            start = temp;
        }

        internal void DeleteNode(int data)
        {
            if (start == null)
            {
                Console.WriteLine($"Value {data} not present in the list.");
                return;
            }

            //// Deletion of first node
            if (start.Info.GetStudentId() == data)
            {
                start = start.Link;
                return;
            }

            //// Deletion in between or at the end
            var node = start;
            while (node.Link != null)
            {
                if (node.Link.Info.GetStudentId() == data)
                    break;

                node = node.Link;
            }

            if (node.Link == null)
                Console.WriteLine($"Value {data} not present in the list.");
            else
                node.Link = node.Link.Link;
        }
    }
}
