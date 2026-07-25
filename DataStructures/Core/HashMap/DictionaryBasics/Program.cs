namespace DataStructures.Core.HashMap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("               DICTIONARY BASICS DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateCreateAndPopulateDictionary();

            DemonstrateAccessAndUpdateValues();

            DemonstrateSearchDictionary();

            DemonstrateRemoveEntries();

            DemonstrateIterateDictionary();

            DemonstrateDisplayDictionaryProperties();

            DemonstrateClearDictionary();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates creating and populating a dictionary.
        /// </summary>
        private static void DemonstrateCreateAndPopulateDictionary()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Create and Populate Dictionary");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial Dictionary:");
            Console.WriteLine("(Empty)");

            Console.WriteLine("\nOperations Performed:");
            Console.WriteLine("• Add(101, \"Alice\")");
            Console.WriteLine("• Add(102, \"Bob\")");
            Console.WriteLine("• dictionary[103] = \"Charlie\"");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("\nResult:");
            PrintDictionary(students);
        }

        /// <summary>
        /// Demonstrates accessing and updating dictionary values.
        /// </summary>
        private static void DemonstrateAccessAndUpdateValues()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Access and Update Values");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Original Dictionary:");
            PrintDictionary(students);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Access value associated with Student ID 102");
            Console.WriteLine("• Update Student ID 102 from \"Bob\" to \"Robert\"");

            var updatedStudents = DictionaryBasicsOperations.AccessAndUpdateValues(students);

            Console.WriteLine("\nUpdated Dictionary:");
            PrintDictionary(updatedStudents);
        }

        /// <summary>
        /// Demonstrates searching within a dictionary.
        /// </summary>
        private static void DemonstrateSearchDictionary()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Search Dictionary");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Dictionary:");
            PrintDictionary(students);

            Console.WriteLine("\nOperations Performed:");
            Console.WriteLine("• ContainsKey(102)");
            Console.WriteLine("• ContainsValue(\"David\")");
            Console.WriteLine("• TryGetValue(103)");

            var result = DictionaryBasicsOperations.SearchDictionary(students);

            Console.WriteLine("\nSearch Results:");
            Console.WriteLine($"ContainsKey(102)       : {result.ContainsStudentId}");
            Console.WriteLine($"ContainsValue(\"David\") : {result.ContainsStudentName}");
            Console.WriteLine($"TryGetValue(103)       : {result.StudentFound}");

            if (result.StudentFound)
            {
                Console.WriteLine($"Retrieved Value        : {result.StudentName}");
            }
        }

        /// <summary>
        /// Demonstrates removing dictionary entries.
        /// </summary>
        private static void DemonstrateRemoveEntries()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Remove Entries");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Original Dictionary:");
            PrintDictionary(students);

            Console.WriteLine("\nOperations Performed:");
            Console.WriteLine("• Remove(102)");
            Console.WriteLine("• Remove(999)");

            var result = DictionaryBasicsOperations.RemoveEntries(students);

            Console.WriteLine("\nOperation Results:");
            Console.WriteLine($"Removed Student ID 102 : {result.ExistingEntryRemoved}");
            Console.WriteLine($"Removed Student ID 999 : {result.NonExistingEntryRemoved}");

            Console.WriteLine("\nUpdated Dictionary:");
            PrintDictionary(result.UpdatedDictionary);
        }

        /// <summary>
        /// Demonstrates iterating through a dictionary.
        /// </summary>
        private static void DemonstrateIterateDictionary()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Iterate Dictionary");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Dictionary:");
            PrintDictionary(students);

            Console.WriteLine("\nIterating Through Dictionary:");

            var entries = DictionaryBasicsOperations.IterateDictionary(students);

            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }

        /// <summary>
        /// Demonstrates commonly used dictionary properties.
        /// </summary>
        private static void DemonstrateDisplayDictionaryProperties()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Dictionary Properties");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Dictionary:");
            PrintDictionary(students);

            var result = DictionaryBasicsOperations.DisplayDictionaryProperties(students);

            Console.WriteLine("\nCount:");
            Console.WriteLine(result.Count);

            Console.WriteLine("\nKeys:");
            foreach (var key in result.Keys)
            {
                Console.WriteLine(key);
            }

            Console.WriteLine("\nValues:");
            foreach (var value in result.Values)
            {
                Console.WriteLine(value);
            }
        }

        /// <summary>
        /// Demonstrates clearing a dictionary.
        /// </summary>
        private static void DemonstrateClearDictionary()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Clear Dictionary");
            Console.WriteLine("==============================================================");

            var students = DictionaryBasicsOperations.CreateAndPopulateDictionary();

            Console.WriteLine("Original Dictionary:");
            PrintDictionary(students);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Clear()");

            var clearedDictionary = DictionaryBasicsOperations.ClearDictionary(students);

            Console.WriteLine("\nDictionary After Clear():");

            if (clearedDictionary.Count == 0)
            {
                Console.WriteLine("(Empty)");
            }
            else
            {
                PrintDictionary(clearedDictionary);
            }

            Console.WriteLine($"\nCount: {clearedDictionary.Count}");
        }

        /// <summary>
        /// Prints all key-value pairs present in the specified dictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary to print.
        /// </param>
        private static void PrintDictionary(Dictionary<int, string> dictionary)
        {
            if (dictionary.Count == 0)
            {
                Console.WriteLine("(Empty)");
                return;
            }

            foreach (var student in dictionary)
            {
                Console.WriteLine($"{student.Key} -> {student.Value}");
            }
        }
    }
}