using System;

namespace SeparateChaining
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int studentId;
            string studentName;

            Console.Write("Enter initial size of table : ");
            var size = Convert.ToInt32(Console.ReadLine());

            var hashTable = new HashTable(size);

            while (true)
            {
                Console.WriteLine("1. Insert a record");
                Console.WriteLine("2. Search a record");
                Console.WriteLine("3. Delete a record");
                Console.WriteLine("4. Display table");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice : ");
                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 5)
                    break;

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter student id : ");
                        studentId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter student name : ");
                        studentName = Console.ReadLine();

                        var studentRecord = new StudentRecord(studentId, studentName);

                        hashTable.Insert(studentRecord);
                        break;

                    case 2:
                        Console.Write("Enter a key to be searched : ");
                        studentId = Convert.ToInt32(Console.ReadLine());
                        studentRecord = hashTable.Search(studentId);

                        if (studentRecord == null)
                            Console.WriteLine("Key not found");
                        else
                            Console.WriteLine(studentRecord.ToString());

                        break;

                    case 3:
                        Console.Write("Enter a key to be deleted : ");
                        studentId = Convert.ToInt32(Console.ReadLine());
                        hashTable.Delete(studentId);
                        break;

                    case 4:
                        hashTable.DisplayTable();
                        break;

                }
            }
        }
    }
}