using System;

namespace SeparateChaining
{
    internal class HashTable
    {
        private SingleLinkedList[] _studentRecords;
        private int _tableSize;
        private int _records;

        public HashTable() : this(11)
        { }

        public HashTable(int tableSize)
        {
            _studentRecords = new SingleLinkedList[tableSize];
            _tableSize = tableSize;
            _records = 0;
        }

        internal void Insert(StudentRecord studentRecord)
        {
            var key = studentRecord.GetStudentId();
            var hash = Hash(key);

            if (_studentRecords[hash] == null)
                _studentRecords[hash] = new SingleLinkedList();

            _studentRecords[hash].InsertInBeginning(studentRecord);
            _records++;
        }

        internal StudentRecord Search(int key)
        {
            var hash = Hash(key);

            if (_studentRecords[hash] != null)
                return _studentRecords[hash].Search(key);

            return null;
        }

        internal void Delete(int key)
        {
            var hash = Hash(key);

            if (_studentRecords[hash] != null)
            {
                _studentRecords[hash].DeleteNode(key);
                _records--;
            }
            else
                Console.WriteLine($"Value {key} not present in the list.");
        }

        internal void DisplayTable()
        {
            for (var i = 0; i < _tableSize; i++)
            {
                Console.Write($"[  {i}  ]  -->  ");

                if (_studentRecords[i] != null)
                    _studentRecords[i].DisplayList();
                else
                    Console.WriteLine("   ");
            }
        }

        private int Hash(int key)
        {
            return (key % _tableSize);
        }
    }
}
