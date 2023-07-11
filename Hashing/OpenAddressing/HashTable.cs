using System;

namespace OpenAddressing
{
    internal class HashTable
    {
        private StudentRecord[] _studentRecords;
        private int _tableSize;
        private int _records;

        public HashTable() : this(11)
        { }

        public HashTable(int tableSize)
        {
            _tableSize = tableSize;
            _studentRecords = new StudentRecord[_tableSize];
            _records = 0;
        }

        internal void Insert(StudentRecord studentRecord)
        {
            var key = studentRecord.GetStudentId();
            var hash = Hash(key);

            var location = hash;

            for (var i = 1; i < _tableSize; i++)
            {
                if (_studentRecords[location] == null || _studentRecords[location].GetStudentId() == -1)
                {
                    _studentRecords[location] = studentRecord;
                    _records++;
                    return;
                }

                if (_studentRecords[location].GetStudentId() == key)
                    throw new InvalidOperationException("Duplicate key");

                location = (hash + i) % _tableSize;                         //// Linear Probing
                // location = (hash + (i*i)) % _tableSize;                  //// Quadratic Probing
                // location = (hash + SecondaryHash(key)) % _tableSize;     //// Double Hashing
            }

            Console.WriteLine("Table is full. Record can't be inserted ");
        }

        internal StudentRecord Search(int key)
        {
            var hash = Hash(key);
            var location = hash;

            for (var i = 1; i < _tableSize; i++)
            {
                if (_studentRecords[location] == null)
                    return null;
                if (_studentRecords[location].GetStudentId() == key)
                    return _studentRecords[location];

                location = (hash + i) % _tableSize;
            }

            return null;
        }

        internal StudentRecord Delete(int key)
        {
            var hash = Hash(key);
            var location = hash;

            for (var i = 1; i < _tableSize; i++)
            {
                if (_studentRecords[location] == null)
                    return null;
                if (_studentRecords[location].GetStudentId() == key)
                {
                    var temp = _studentRecords[location];
                    _studentRecords[location].SetStudentId(-1);
                    _records--;
                    return temp;
                }

                location = (hash + i) % _tableSize;
            }

            return null;
        }

        internal void DisplayTable()
        {
            for (var i = 0; i < _tableSize; i++)
            {
                Console.Write($"[  {i}  ]  -->  ");

                if (_studentRecords[i] != null && _studentRecords[i].GetStudentId() != -1)
                    Console.WriteLine(_studentRecords[i].ToString());
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
