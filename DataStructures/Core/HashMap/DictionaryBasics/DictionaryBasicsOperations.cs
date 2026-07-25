namespace DataStructures.Core.HashMap
{
    internal class DictionaryBasicsOperations
    {
        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> and demonstrates different techniques for adding key-value pairs.
        ///
        /// <para>
        /// A dictionary stores data as unique key-value pairs, enabling efficient lookups based on keys.
        ///
        /// This method demonstrates:
        /// <list type="bullet">
        /// <item><description>Creating an empty dictionary.</description></item>
        /// <item><description>Adding entries using the <see cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/> method.</description></item>
        /// <item><description>Adding entries using the indexer.</description></item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// <b>Note:</b>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Calling <see cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/> with an existing key throws an exception.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Using the indexer with a new key inserts a new entry.
        /// </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(1) per insertion.
        /// Worst Case: O(n), when hash collisions require extensive probing or rehashing.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), where n is the number of entries stored.
        /// </para>
        /// </summary>
        /// <returns>
        /// A dictionary populated with sample student records.
        /// </returns>
        public static Dictionary<int, string> CreateAndPopulateDictionary()
        {
            Dictionary<int, string> students = new();

            students.Add(101, "Alice");
            students.Add(102, "Bob");

            // Using the indexer with a new key inserts a new entry.
            students[103] = "Charlie";

            return students;
        }

        /// <summary>
        /// Demonstrates how to access and update values stored in a <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The method retrieves an existing value using its key and then updates that value using the dictionary indexer.
        ///
        /// Unlike <see cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>, assigning a value through the indexer for an existing key replaces the previous value instead of throwing an exception.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(1) for both lookup and update.
        /// Worst Case: O(n), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1), as the existing value is updated in place.
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary whose values are to be accessed and updated.
        /// </param>
        /// <returns>
        /// The updated dictionary.
        /// </returns>
        /// <exception cref="ArgumentNullException"> thrown when <paramref name="students"/> is null. </exception>
        /// <exception cref="KeyNotFoundException"> thrown when the specified key does not exist. </exception>
        public static Dictionary<int, string> AccessAndUpdateValues(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            const int studentId = 102;

            // Access the existing value.
            var existingStudentName = students[studentId];

            // Update the value associated with the same key.
            students[studentId] = "Robert";

            return students;
        }

        /// <summary>
        /// Demonstrates various techniques for searching data within a <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The method showcases the most commonly used search operations:
        /// <list type="bullet">
        /// <item>
        /// <description> Checking whether a specific key exists using <see cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>. </description>
        /// </item>
        /// <item>
        /// <description> Checking whether a specific value exists using <see cref="Dictionary{TKey, TValue}.ContainsValue(TValue)"/>. </description>
        /// </item>
        /// <item>
        /// <description> Safely retrieving a value using <see cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// <b>Why use TryGetValue?</b>
        /// <br/>
        /// Accessing a value using the dictionary indexer throws a <see cref="KeyNotFoundException"/> when the specified key does not exist. 
        /// In contrast, <see cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/> safely attempts the lookup without throwing an exception, making it the preferred approach when the existence of a key is uncertain.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// ContainsKey:
        /// Average Case: O(1)
        /// Worst Case: O(n)
        ///
        /// ContainsValue:
        /// O(n), since every value may need to be examined.
        ///
        /// TryGetValue:
        /// Average Case: O(1)
        /// Worst Case: O(n)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary to search.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <description> Whether Student ID 102 exists. </description>
        /// </item>
        /// <item>
        /// <description> Whether a student named "David" exists. </description>
        /// </item>
        /// <item>
        /// <description> Whether Student ID 103 was successfully found. </description>
        /// </item>
        /// <item>
        /// <description> The retrieved student name if found; otherwise <c>null</c>. </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException"> thrown when <paramref name="students"/> is null. </exception>
        public static (bool ContainsStudentId, bool ContainsStudentName, bool StudentFound, string? StudentName) SearchDictionary(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            var containsStudentId = students.ContainsKey(102);
            var containsStudentName = students.ContainsValue("David");
            var studentFound = students.TryGetValue(103, out string? studentName);

            return (containsStudentId, containsStudentName, studentFound, studentName);
        }

        /// <summary>
        /// Demonstrates removing entries from a <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The method attempts to remove:
        /// <list type="bullet">
        /// <item>
        /// <description> An existing key. </description>
        /// </item>
        /// <item>
        /// <description> A non-existing key. </description>
        /// </item>
        /// </list>
        ///
        /// The return value of <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/> indicates whether the removal operation was successful.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(1)
        /// Worst Case: O(n)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary from which entries should be removed.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <description> Whether removal of Student ID 102 succeeded. </description>
        /// </item>
        /// <item>
        /// <description> Whether removal of Student ID 999 succeeded. </description>
        /// </item>
        /// <item>
        /// <description> The updated dictionary. </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="students"/> is null. </exception>
        public static (bool ExistingEntryRemoved, bool NonExistingEntryRemoved, Dictionary<int, string> UpdatedDictionary) RemoveEntries(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            var existingEntryRemoved = students.Remove(102);
            var nonExistingEntryRemoved = students.Remove(999);

            return (existingEntryRemoved, nonExistingEntryRemoved, students);
        }

        /// <summary>
        /// Demonstrates iterating through all key-value pairs in a <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The method traverses the dictionary using a <c>foreach</c> loop and collects each key-value pair in a human-readable format.
        ///
        /// Dictionary enumeration does not guarantee any specific ordering. Although the current .NET implementation preserves insertion order, application logic should never rely on this behavior unless explicitly documented.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of entries in the dictionary.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), for storing the formatted output.
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary whose entries are to be enumerated.
        /// </param>
        /// <returns>
        /// A list containing the formatted representation of each key-value pair.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="students"/> is null. </exception>
        public static List<string> IterateDictionary(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            List<string> entries = new();

            foreach (var student in students)
            {
                entries.Add($"{student.Key} -> {student.Value}");
            }

            return entries;
        }

        /// <summary>
        /// Retrieves commonly used properties of a <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The method demonstrates how to obtain:
        /// <list type="bullet">
        /// <item>
        /// <description> The total number of entries using <see cref="Dictionary{TKey, TValue}.Count"/>. </description>
        /// </item>
        /// <item>
        /// <description> The collection of all keys. </description>
        /// </item>
        /// <item>
        /// <description> The collection of all values. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Count: O(1)
        /// Enumerating Keys: O(n)
        /// Enumerating Values: O(n)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1), since the method returns references to the existing key and value collections.
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary whose properties are to be retrieved.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <description> Total number of entries. </description>
        /// </item>
        /// <item>
        /// <description> Collection of all keys. </description>
        /// </item>
        /// <item>
        /// <description> Collection of all values. </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="students"/> is null. </exception>
        public static (int Count, Dictionary<int, string>.KeyCollection Keys, Dictionary<int, string>.ValueCollection Values) DisplayDictionaryProperties(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            return (students.Count, students.Keys, students.Values);
        }

        /// <summary>
        /// Removes all entries from the specified <see cref="Dictionary{TKey, TValue}"/>.
        ///
        /// <para>
        /// The <see cref="Dictionary{TKey, TValue}.Clear"/> method removes every key-value pair from the dictionary while preserving the dictionary instance for future use.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of entries removed.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        /// </summary>
        /// <param name="students">
        /// The dictionary to clear.
        /// </param>
        /// <returns>
        /// The cleared dictionary.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="students"/> is null. </exception>
        public static Dictionary<int, string> ClearDictionary(Dictionary<int, string> students)
        {
            ArgumentNullException.ThrowIfNull(students);

            students.Clear();

            return students;
        }
    }
}
