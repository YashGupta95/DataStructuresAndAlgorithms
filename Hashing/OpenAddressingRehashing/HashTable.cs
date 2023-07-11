using System;

namespace OpenAddressingRehashing
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
            if (_records >= _tableSize / 2)
            {
                Rehash(NextPrime(2 * _tableSize));
                Console.WriteLine("New Table Size is : " + _tableSize);
            }

            InsertRecord(studentRecord);
        }

        private void InsertRecord(StudentRecord newRecord)
        {

            var key = newRecord.GetStudentId();
            var hash = Hash(key);

            var location = hash;

            for (var i = 1; i < _tableSize; i++)
            {
                if (_studentRecords[location] == null || _studentRecords[location].GetStudentId() == -1)
                {
                    _studentRecords[location] = newRecord;
                    _records++;
                    return;
                }
                if (_studentRecords[location].GetStudentId() == key)
                    throw new InvalidOperationException("Duplicate key");

                location = (hash + i) % _tableSize;
            }
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

        internal void Delete(int key)
        {
            var hash = Hash(key);
            var location = hash;

            for (var i = 1; i < _tableSize; i++)
            {
                if (_studentRecords[location] == null)
                    return;
                if (_studentRecords[location].GetStudentId() == key)
                {
                    _studentRecords[location].SetStudentId(-1);
                    _records--;

                    if (_records > 0 && _records <= _tableSize / 8)
                    {
                        Rehash(NextPrime(_tableSize / 2));
                        Console.WriteLine("New Table Size is : " + _tableSize);
                    }
                }

                location = (hash + i) % _tableSize;
            }
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

        private void Rehash(int newSize)
        {
            var tempHashTable = new HashTable(newSize);

            for (var i = 0; i < _tableSize; i++)
                if (_studentRecords[i] != null && _studentRecords[i].GetStudentId() != -1)
                    tempHashTable.InsertRecord(_studentRecords[i]);

            _studentRecords = tempHashTable._studentRecords;
            _tableSize = newSize;
        }

        private int NextPrime(int num)
        {
            while (!IsPrime(num))
                num++;

            return num;
        }

        private static bool IsPrime(int num)
        {
            for (var i = 2; i < num; i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }
    }
}
