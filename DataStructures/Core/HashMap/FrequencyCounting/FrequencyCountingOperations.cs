using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.Core.HashMap
{
    internal class FrequencyCountingOperations
    {
        /// <summary>
        /// Counts the frequency of each character in the specified string.
        ///
        /// <para>
        /// The method traverses the input string one character at a time and builds a frequency map using a <see cref="Dictionary{TKey, TValue}"/>, where:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Character </description>
        /// </item>
        /// <item>
        /// <description> Value = Number of occurrences </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// Frequency counting is one of the most common applications of hash maps and forms the basis of numerous interview problems, 
        /// including finding duplicate characters, identifying unique characters and checking for anagrams.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: programming
        ///
        /// Output:
        /// p → 1
        /// r → 2
        /// o → 1
        /// g → 2
        /// a → 1
        /// m → 2
        /// i → 1
        /// n → 1
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct characters.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The input string whose character frequencies are to be counted.
        /// </param>
        /// <returns>
        /// A dictionary containing each unique character and its frequency.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="input"/> is null. </exception>
        public static Dictionary<char, int> CharacterFrequency(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            Dictionary<char, int> charFrequencies = new(); // Initialize a new dictionary to hold character frequencies

            foreach (var currentChar in input)
            {
                if (charFrequencies.ContainsKey(currentChar))
                {
                    charFrequencies[currentChar]++;
                }
                else
                {
                    charFrequencies[currentChar] = 1;
                }
            }

            return charFrequencies;
        }

        /// <summary>
        /// Counts the frequency of each word in the specified sentence.
        ///
        /// <para>
        /// Words are separated using whitespace characters. The method builds a frequency map where:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Word </description>
        /// </item>
        /// <item>
        /// <description> Value = Number of occurrences </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// This implementation uses <see cref="string.Split(char[], StringSplitOptions)"/> solely to tokenize the sentence. 
        /// The primary focus of this method is demonstrating frequency counting using a hash map rather than manual string parsing.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: this is a test this is only a test
        ///
        /// Output:
        /// this → 2
        /// is → 2
        /// a → 2
        /// test → 2
        /// only → 1
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of words.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct words.
        /// </para>
        /// </summary>
        /// <param name="sentence">
        /// The sentence whose word frequencies are to be counted.
        /// </param>
        /// <returns>
        /// A dictionary containing each unique word and its frequency.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="sentence"/> is null. </exception>
        public static Dictionary<string, int> WordFrequency(string sentence)
        {
            ArgumentNullException.ThrowIfNull(sentence);

            Dictionary<string, int> wordFrequencies = new(); // Initialize a new dictionary to hold word frequencies

            var words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (wordFrequencies.ContainsKey(word))
                {
                    wordFrequencies[word]++;
                }
                else
                {
                    wordFrequencies[word] = 1;
                }
            }

            return wordFrequencies;
        }

        /// <summary>
        /// Counts the frequency of each integer in the specified array.
        ///
        /// <para>
        /// The method traverses the array and records the number of occurrences of every distinct integer using a <see cref="Dictionary{TKey, TValue}"/>.
        /// </para>
        ///
        /// <para>
        /// This technique is widely used in coding interviews for solving problems related to duplicate detection, majority elements, frequency analysis and lookup optimization.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: [4, 2, 7, 2, 4, 1, 4, 7]
        ///
        /// Output:
        /// 4 → 3
        /// 2 → 2
        /// 7 → 2
        /// 1 → 1
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of elements in the array.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct integers.
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array whose element frequencies are to be counted.
        /// </param>
        /// <returns>
        /// A dictionary containing each unique integer and its frequency.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static Dictionary<int, int> IntegerFrequency(int[] numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            Dictionary<int, int> integerFrequencies = new(); // Initialize a new dictionary to hold integer frequencies

            foreach (var num in numbers)
            {
                if (integerFrequencies.ContainsKey(num))
                {
                    integerFrequencies[num]++;
                }
                else
                {
                    integerFrequencies[num] = 1;
                }
            }

            return integerFrequencies;
        }

        /// <summary>
        /// Finds the first non-repeating character in the specified string.
        ///
        /// <para>
        /// The method first builds a frequency map of all characters in the string. It then traverses the string a second time to identify the first character whose frequency is exactly one.
        ///
        /// Using two passes ensures that the original character order is preserved while still achieving linear time complexity.
        /// </para>
        ///
        /// <para>
        /// This is a common interview problem that demonstrates how frequency counting can be combined with sequential traversal to efficiently locate unique elements.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: swiss
        ///
        /// Frequency Map:
        /// s → 3
        /// w → 1
        /// i → 1
        ///
        /// Second Traversal:
        /// s ✗
        /// w ✓
        ///
        /// Output: w
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        ///
        /// The string is traversed twice:
        /// <list type="bullet">
        /// <item>
        /// <description> First pass builds the frequency map. </description>
        /// </item>
        /// <item>
        /// <description> Second pass finds the first unique character. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct characters.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The input string to search.
        /// </param>
        /// <returns>
        /// The first non-repeating character if one exists; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="input"/> is null. </exception>
        public static char? FirstUniqueCharacter(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            Dictionary<char, int> charFrequencies = new(); // Initialize a new dictionary to hold character frequencies

            foreach (var currentChar in input)
            {
                if (charFrequencies.ContainsKey(currentChar))
                {
                    charFrequencies[currentChar]++;
                }
                else
                {
                    charFrequencies[currentChar] = 1;
                }
            }

            foreach (var currentChar in input)
            {
                if (charFrequencies[currentChar] == 1)
                {
                    return currentChar;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the most frequently occurring element in the specified integer array.
        ///
        /// <para>
        /// The method first builds a frequency map for all integers in the array. It then traverses the frequency map to identify the element having the highest occurrence count.
        /// If multiple elements share the highest frequency, the element encountered first while traversing the original array is returned.
        /// </para>
        ///
        /// <para>
        /// This problem demonstrates how a frequency map can be used to efficiently identify statistical properties of a dataset.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: [4, 2, 7, 2, 4, 1, 4, 7]
        ///
        /// Frequency Map:
        /// 4 → 3
        /// 2 → 2
        /// 7 → 2
        /// 1 → 1
        ///
        /// Output: 4
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of elements.
        ///
        /// The algorithm performs:
        /// <list type="bullet">
        /// <item>
        /// <description> One traversal to build the frequency map. </description>
        /// </item>
        /// <item>
        /// <description> One traversal of the frequency map to determine the maximum frequency. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct integers.
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array whose most frequent element is to be found.
        /// </param>
        /// <returns>
        /// The integer having the highest frequency.
        /// Returns <c>null</c> when the array is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static int? MostFrequentElement(int[] numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            if (numbers.Length == 0)
            {
                return null;
            }

            Dictionary<int, int> intFrequencies = new();

            foreach (var num in numbers)
            {
                if (intFrequencies.ContainsKey(num))
                {
                    intFrequencies[num]++;
                }
                else
                {
                    intFrequencies[num] = 1;
                }
            }

            var mostFrequentElement = numbers[0]; // Initialize with the first element of the array
            var highestFrequency = intFrequencies[mostFrequentElement]; // Initialize with the frequency of the first element

            foreach (var frequency in intFrequencies)
            {
                if (frequency.Value > highestFrequency)
                {
                    highestFrequency = frequency.Value;
                    mostFrequentElement = frequency.Key;
                }
            }

            return mostFrequentElement;
        }

        /// <summary>
        /// Groups elements based on their frequency of occurrence.
        ///
        /// <para>
        /// The method first builds a frequency map where each distinct integer is associated with its occurrence count. It then constructs a reverse mapping that groups all integers having the same frequency.
        ///
        /// The resulting dictionary uses:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Frequency </description>
        /// </item>
        /// <item>
        /// <description> Value = List of integers occurring that many times </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// This technique demonstrates how an existing frequency map can be transformed into another data structure for further processing.
        /// Reverse mappings like this are useful in various interview problems, including bucket-based grouping and frequency analysis.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// [5, 5, 2, 2, 8, 8, 1]
        ///
        /// Frequency Map:
        /// 5 → 2
        /// 2 → 2
        /// 8 → 2
        /// 1 → 1
        ///
        /// Grouped Result:
        /// 1 → [1]
        /// 2 → [5, 2, 8]
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of elements.
        ///
        /// The algorithm performs:
        /// <list type="bullet">
        /// <item>
        /// <description> One traversal to build the frequency map. </description>
        /// </item>
        /// <item>
        /// <description> One traversal of the frequency map to build the reverse mapping. </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of distinct elements.
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array whose elements are to be grouped by frequency.
        /// </param>
        /// <returns>
        /// A dictionary where:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Frequency </description>
        /// </item>
        /// <item>
        /// <description> Value = List of elements having that frequency </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static Dictionary<int, List<int>> GroupElementsByFrequency(int[] numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            Dictionary<int, int> elementFrequencies = new();

            foreach (var num in numbers)
            {
                if (elementFrequencies.ContainsKey(num))
                {
                    elementFrequencies[num]++;
                }
                else
                {
                    elementFrequencies[num] = 1;
                }
            }

            Dictionary<int, List<int>> groupedElements = new(); // Initialize a new dictionary to hold grouped elements by frequency

            foreach (var frequency in elementFrequencies)
            {
                if (!groupedElements.ContainsKey(frequency.Value))
                {
                    groupedElements[frequency.Value] = new List<int>();
                }

                groupedElements[frequency.Value].Add(frequency.Key);
            }

            foreach (var elements in groupedElements.Values)
            {
                elements.Sort(); // Sort the list of elements for each frequency to maintain a consistent order
            }

            return groupedElements;
        }
    }
}
