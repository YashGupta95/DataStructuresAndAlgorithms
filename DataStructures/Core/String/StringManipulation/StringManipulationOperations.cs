using System.Text;

namespace DataStructures.Core.String
{
    internal class StringManipulationOperations
    {
        /// <summary>
        /// Reverses the order of words in the given sentence while preserving the characters within each word.
        /// Multiple consecutive spaces are treated as a single separator and are not preserved in the output.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "I Love DSA"
        ///
        /// Output:
        /// "DSA Love I"
        /// ------------------------------------------------------------
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "  Hello    World  "
        ///
        /// Output:
        /// "World Hello"
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Traverse the string from right to left.
        /// 2. Skip any trailing or intermediate whitespace.
        /// 3. Identify the boundaries of each word.
        /// 4. Append each word to the result in reverse order.
        /// 5. Insert a single space between consecutive words.
        ///
        /// Time Complexity:
        /// O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), due to the StringBuilder used for constructing the output.
        /// </summary>
        /// <param name="input">
        /// Input sentence whose word order needs to be reversed.
        /// </param>
        /// <returns>
        /// A new string with the order of words reversed. Returns the original string if it is null, empty, or contains only whitespace.
        /// </returns>
        public static string ReverseWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            StringBuilder result = new();
            var index = input.Length - 1;

            while (index >= 0)
            {
                // Skip trailing or multiple spaces.
                while (index >= 0 && char.IsWhiteSpace(input[index]))
                {
                    index--;
                }

                if (index < 0)
                    break;
                
                // Mark the end of the current word.
                var wordEnd = index;

                // Find the beginning of the current word.
                while (index >= 0 && !char.IsWhiteSpace(input[index]))
                {
                    index--;
                }

                var wordStart = index + 1;

                // Append a space before every word except the first one.
                if (result.Length > 0)
                {
                    result.Append(' ');
                }

                // Append the current word.
                for (var i = wordStart; i <= wordEnd; i++)
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// This implementation performs a case-sensitive comparison and considers all characters (including spaces and punctuation) as part of the string.
        ///
        /// ------------------------------------------------------------
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "racecar"
        ///
        /// Output:
        /// true
        /// ------------------------------------------------------------
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "Madam"
        ///
        /// Output:
        /// false
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Initialize two pointers:
        ///    - Left pointer at the beginning of the string.
        ///    - Right pointer at the end of the string.
        /// 2. Compare the characters at both pointers.
        /// 3. If they differ, the string is not a palindrome.
        /// 4. Move the pointers towards the center.
        /// 5. Continue until the pointers meet or cross.
        ///
        /// Time Complexity:
        /// O(n), where n is the length of the input string.
        ///
        /// Space Complexity:
        /// O(1), as no additional data structures are used.
        /// </summary>
        /// <param name="input">
        /// String to be checked for palindrome.
        /// </param>
        /// <returns>
        /// <c>true</c> if the input string is a palindrome; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPalindrome(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var left = 0;
            var right = input.Length - 1;

            while (left < right)
            {
                if (input[left] != input[right])
                {
                    return false;
                }

                left++;
                right--;
            }

            return true;
        }

        /// <summary>
        /// Toggles the case of each alphabetic character in the given string.
        /// Non-alphabetic characters (digits, symbols, punctuation, whitespace, etc.) remain unchanged.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "CSharp123!"
        ///
        /// Output:
        /// "csHARP123!"
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Traverse the string one character at a time.
        /// 2. If the current character is an uppercase letter ('A'-'Z'), convert it to lowercase using ASCII arithmetic.
        /// 3. If the current character is a lowercase letter ('a'-'z'), convert it to uppercase using ASCII arithmetic.
        /// 4. Append all other characters unchanged and return the newly constructed string.
        ///
        /// Note:
        /// This implementation intentionally uses ASCII arithmetic for demo purposes. In production applications requiring Unicode support, prefer <see cref="char.ToUpperInvariant(char)"/> and <see cref="char.ToLowerInvariant(char)"/>.
        ///
        /// Time Complexity: O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), due to the StringBuilder used for constructing the result.
        /// </summary>
        /// <param name="input">
        /// Input string whose character case needs to be toggled.
        /// </param>
        /// <returns>
        /// A new string with the case of each alphabetic character toggled. Returns the original string if it is null or empty.
        /// </returns>
        public static string ToggleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder result = new();

            foreach (var character in input)
            {
                if (character >= 'A' && character <= 'Z')
                {
                    result.Append((char)(character + ('a' - 'A')));
                }
                else if (character >= 'a' && character <= 'z')
                {
                    result.Append((char)(character - ('a' - 'A')));
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Removes duplicate characters from the given string while preserving the order of their first occurrence.
        /// Character comparison is case-sensitive. For example, 'A' and 'a' are treated as different characters.
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "Hello World"
        ///
        /// Output:
        /// "Helo Wrd"
        ///
        /// Explanation:
        /// Duplicate characters, including repeated spaces (if any), are removed while preserving the order of first occurrence.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Create a HashSet to keep track of characters already seen.
        /// 2. Traverse the input string from left to right.
        /// 3. If the current character has not been seen before:
        ///    - Add it to the HashSet.
        ///    - Append it to the result.
        /// 4. Ignore characters that have already been processed.
        /// 5. Return the constructed string.
        ///
        /// Time Complexity: O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), due to the HashSet used to track unique characters.
        /// </summary>
        /// <param name="input">
        /// Input string from which duplicate characters need to be removed.
        /// </param>
        /// <returns>
        /// A new string containing only the first occurrence of each character. Returns the original string if it is null or empty.
        /// </returns>
        public static string RemoveDuplicateCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            HashSet<char> uniqueCharacters = new();
            StringBuilder result = new();

            foreach (char character in input)
            {
                if (uniqueCharacters.Add(character))
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Performs Run-Length Encoding (RLE) on the given string.
        ///
        /// Run-Length Encoding is a simple lossless data compression technique that replaces consecutive occurrences of the same character with the character followed by the number of its consecutive occurrences.
        /// This implementation assumes the input itself does not require escaping numeric characters. If digits are present in the input, additional decoding rules would be required to avoid ambiguity.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "aaabbcccc"
        ///
        /// Output:
        /// "a3b2c4"
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "abcd"
        ///
        /// Output:
        /// "a1b1c1d1"
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "aaaAA"
        ///
        /// Output:
        /// "a3A2"
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Traverse the string from left to right.
        /// 2. Count consecutive occurrences of the current character.
        /// 3. When a different character is encountered:
        ///    - Append the character.
        ///    - Append its occurrence count.
        /// 4. Continue until the end of the string.
        /// 5. Append the final character and its count.
        ///
        /// Time Complexity:  O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), due to the StringBuilder used for constructing the encoded string.
        /// </summary>
        /// <param name="input">
        /// Input string to be encoded using Run-Length Encoding.
        /// </param>
        /// <returns>
        /// The Run-Length Encoded representation of the input string. Returns the original string if it is null or empty.
        /// </returns>
        public static string RunLengthEncoding(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder result = new();

            var currentCharacter = input[0];
            var currentCount = 1;

            for (var index = 1; index < input.Length; index++)
            {
                if (input[index] == currentCharacter)
                {
                    currentCount++;
                }
                else
                {
                    result.Append(currentCharacter);
                    result.Append(currentCount);

                    currentCharacter = input[index];
                    currentCount = 1;
                }
            }

            // Append the final character group.
            result.Append(currentCharacter);
            result.Append(currentCount);

            return result.ToString();
        }

        /// <summary>
        /// Calculates the frequency of each English alphabet character in the given string using a fixed-size frequency array.
        ///
        /// Notes:
        /// • Suitable when the character set is fixed and known.
        /// • Supports English alphabet characters only.
        /// • Character comparison is case-insensitive.
        /// • Non-alphabetic characters are ignored.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "Hello World!123"
        ///
        /// Output:
        /// d : 1
        /// e : 1
        /// h : 1
        /// l : 3
        /// o : 2
        /// r : 1
        /// w : 1
        ///
        /// Explanation:
        /// Digits, whitespace and punctuation are ignored.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Create a frequency array of size 26.
        /// 2. Traverse each character in the input string.
        /// 3. Convert uppercase letters to lowercase.
        /// 4. Ignore non-alphabetic characters.
        /// 5. Increment the corresponding frequency.
        ///
        ///
        /// Time Complexity: O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(1), since the frequency array size is fixed (26).
        /// </summary>
        /// <param name="input">
        /// Input string whose character frequencies need to be calculated.
        /// </param>
        /// <returns>
        /// An integer array of size 26 where:
        /// index 0 represents 'a',
        /// index 1 represents 'b',
        /// ...
        /// index 25 represents 'z'.
        ///
        /// Returns an empty array if the input is null or empty.
        /// </returns>
        public static int[] CharacterFrequencyUsingArray(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Array.Empty<int>();
            }

            var characterFrequencies = new int[26];

            foreach (var character in input)
            {
                var currentCharacter = character;

                if (currentCharacter >= 'A' && currentCharacter <= 'Z')
                {
                    currentCharacter = (char)(currentCharacter + ('a' - 'A'));
                }

                if (currentCharacter >= 'a' && currentCharacter <= 'z')
                {
                    characterFrequencies[currentCharacter - 'a']++;
                }
            }

            return characterFrequencies;
        }

        /// <summary>
        /// Calculates the frequency of every distinct character in the given string using a Dictionary.
        ///
        /// Unlike the array-based implementation, this method :
        /// • Supports all characters.
        /// • Character comparison is case-sensitive.
        /// • Suitable when the character set is unknown or very large.
        /// • Recommended for real-world applications involving Unicode.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "Hello World!"
        ///
        /// Output:
        /// H : 1
        /// e : 1
        /// l : 3
        /// o : 2
        /// ' ' : 1
        /// W : 1
        /// r : 1
        /// d : 1
        /// ! : 1
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Create an empty Dictionary<char, int>.
        /// 2. Traverse each character in the input string.
        /// 3. If the character already exists: Increment its frequency.
        /// 4. Otherwise: Add it with an initial frequency of 1.
        /// 5. Return the populated dictionary.
        ///
        /// Time Complexity: O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), where n is the number of distinct characters.
        /// </summary>
        /// <param name="input">
        /// Input string whose character frequencies need to be calculated.
        /// </param>
        /// <returns>
        /// A Dictionary where:
        /// Key   = Character
        /// Value = Number of occurrences.
        ///
        /// Returns an empty dictionary if the input is null or empty.
        /// </returns>
        public static Dictionary<char, int> CharacterFrequencyUsingDictionary(string input)
        {
            Dictionary<char, int> characterFrequencies = new();

            if (string.IsNullOrEmpty(input))
            {
                return characterFrequencies;
            }

            foreach (var character in input)
            {
                if (characterFrequencies.ContainsKey(character))
                {
                    characterFrequencies[character]++;
                }
                else
                {
                    characterFrequencies.Add(character, 1);
                }
            }

            return characterFrequencies;
        }

        /// <summary>
        /// Finds the first non-repeating character in the given string.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "programming"
        ///
        /// Output:
        /// 'p'
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Traverse the input string and build a character frequency map using a Dictionary<char, int>.
        /// 2. Traverse the string again in its original order.
        /// 3. Return the first character whose frequency is exactly one.
        /// 4. If no such character exists, return null.
        ///
        /// Notes:
        /// • Character comparison is case-sensitive.
        /// • Whitespace, digits, punctuation and Unicode characters are treated like any other character.
        /// • Two traversals are performed to preserve the original order of characters.
        ///
        /// Time Complexity: O(n), where n is the length of the input string.
        ///
        /// Space Complexity: O(n), where n is the number of distinct characters.
        /// </summary>
        /// <param name="input">
        /// Input string in which the first non-repeating character needs to be found.
        /// </param>
        /// <returns>
        /// The first non-repeating character if one exists; otherwise, <c>null</c>.
        /// </returns>
        public static char? FirstNonRepeatingCharacter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            // First pass: Use existing 'CharacterFrequencyUsingDictionary' method to build the frequency map.
            var characterFrequencies = CharacterFrequencyUsingDictionary(input);

            // Second pass: Find the first character with a frequency of one.
            foreach (char character in input)
            {
                if (characterFrequencies[character] == 1)
                {
                    return character;
                }
            }

            return null;
        }

        /// <summary>
        /// Two strings are said to be anagrams if they contain exactly the same characters with the same frequencies, irrespective of the order of the characters.
        ///
        /// This implementation assumes that:
        /// • Both input strings contain only English alphabet characters.
        /// • Character comparison is case-insensitive.
        /// • Whitespace is ignored.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// "Listen"
        /// "Silent"
        ///
        /// Output:
        /// true
        ///
        /// Explanation:
        /// Both strings contain the same characters with identical
        /// frequencies.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Remove whitespace from both strings.
        /// 2. Convert both strings to lowercase.
        /// 3. If the lengths differ, the strings cannot be anagrams.
        /// 4. Maintain a frequency array of size 26.
        /// 5. Increment the frequency for each character in the first string.
        /// 6. Decrement the frequency for each character in the second string.
        /// 7. If every frequency becomes zero, the strings are anagrams.
        ///
        /// Time Complexity: O(n), where n is the length of the input strings.
        ///
        /// Space Complexity: O(1), since the frequency array size is fixed (26).
        /// </summary>
        /// <param name="firstString">
        /// First input string.
        /// </param>
        /// <param name="secondString">
        /// Second input string.
        /// </param>
        /// <returns>
        /// <c>true</c> if both strings are anagrams; otherwise, <c>false</c>.
        /// </returns>
        public static bool AreAnagrams(string firstString, string secondString)
        {
            if (string.IsNullOrWhiteSpace(firstString) || string.IsNullOrWhiteSpace(secondString))
            {
                return false;
            }

            // Convert them both to lowercase
            firstString = firstString.Replace(" ", "").ToLowerInvariant();
            secondString = secondString.Replace(" ", "").ToLowerInvariant();

            if (firstString.Length != secondString.Length)
            {
                return false;
            }

            var characterFrequency = new int[26];

            foreach (var character in firstString)
            {
                characterFrequency[character - 'a']++;
            }

            foreach (var character in secondString)
            {
                characterFrequency[character - 'a']--;
            }

            foreach (var frequency in characterFrequency)
            {
                if (frequency != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the longest common prefix (a sequence of characters that appears at the beginning of every string) shared by all strings in the given array.
        /// 
        /// 
        /// • Comparison is case-sensitive.
        /// • Returns an empty string if no common prefix exists.
        /// • Uses the Horizontal Scanning technique.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// ["flower", "flow", "flight"]
        ///
        /// Output:
        /// "fl"
        /// ------------------------------------------------------------
        /// Algorithm:
        /// 1. Assume the first string is the current longest common prefix.
        /// 2. Compare it with each remaining string.
        /// 3. While the current string does not start with the prefix,
        ///    remove the last character from the prefix.
        /// 4. Repeat until all strings have been processed.
        /// 5. Return the final prefix.
        ///
        /// Time Complexity: O(n × m),
        /// where:
        /// n = Number of strings
        /// m = Length of the shortest string.
        ///
        /// Space Complexity: O(1), excluding the returned string.
        /// </summary>
        /// <param name="strings">
        /// Array of input strings.
        /// </param>
        /// <returns>
        /// The longest common prefix shared by all strings. Returns an empty string if no common prefix exists or if the input array is null or empty.
        /// </returns>
        public static string LongestCommonPrefix(string[] strings)
        {
            if (strings == null || strings.Length == 0)
            {
                return string.Empty;
            }

            string longestCommonPrefix = strings[0];

            for (int index = 1; index < strings.Length; index++)
            {
                
                while (!strings[index].StartsWith(longestCommonPrefix))
                {
                    // Range operator simple example:
                    // string str = "HelloWorld";
                    // string subStr3 = str[2..5]; // "llo"
                    // Use range operators to remove last character from prefix instead of using Substring for better performance.
                    longestCommonPrefix = longestCommonPrefix[..^1]; // Remove the last character from the prefix
                    //Why? Because the current string does not start with the prefix, so we shorten the prefix and check again.

                    if (longestCommonPrefix.Length == 0)
                    {
                        return string.Empty;
                    }
                }
            }

            return longestCommonPrefix;
        }

        /// <summary>
        /// Determines whether one string is a rotation of another string, i.e. - if it can be obtained by moving one or more leading characters of the original string to its end without changing the relative order of the remaining characters.
        ///
        /// • Character comparison is case-sensitive.
        /// • Both strings must have identical lengths.
        /// • This implementation performs a manual substring search instead of using Contains() for preparation point-of-view.
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// Original String : "ABCD"
        /// Rotated String  : "CDAB"
        ///
        /// Output:
        /// true
        ///
        /// Explanation:
        /// "CDAB" can be obtained by rotating "ABCD" two positions to the left.
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input:
        /// Original String : "waterbottle"
        /// Rotated String  : "erbottlewat"
        ///
        /// Output:
        /// true
        /// ------------------------------------------------------------
        /// Algorithm:
        /// 1. If either string is null or empty, return false.
        /// 2. If the lengths differ, return false.
        /// 3. Concatenate the original string with itself.
        /// 4. Perform a manual substring search to determine whether the rotated string exists within the concatenated string.
        /// 5. If found, the second string is a valid rotation.
        ///
        /// Time Complexity: O(n²)
        ///
        /// Space Complexity: O(n) due to the concatenated string.
        /// </summary>
        /// <param name="originalString">
        /// Original input string.
        /// </param>
        /// <param name="rotatedString">
        /// String to be verified as a rotation of the original string.
        /// </param>
        /// <returns>
        /// <c>true</c> if the second string is a valid rotation of the first; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRotation(string originalString, string rotatedString)
        {
            if (string.IsNullOrEmpty(originalString) || string.IsNullOrEmpty(rotatedString))
            {
                return false;
            }

            if (originalString.Length != rotatedString.Length)
            {
                return false;
            }

            var concatenatedString = originalString + originalString;

            // startIndex only needs to go upto (concatenatedString.Length - rotatedString.Length)
            // because we are checking for a substring of length rotatedString.Length, so there's no need to check beyond that point
            // as there might not be enough characters left in concatenatedString to match the length of rotatedString.
            for (var startIndex = 0; startIndex <= concatenatedString.Length - rotatedString.Length; startIndex++)
            {
                var currentIndex = 0;

                // Check if the substring starting at startIndex matches the rotatedString
                while (currentIndex < rotatedString.Length && concatenatedString[startIndex + currentIndex] == rotatedString[currentIndex])
                {
                    currentIndex++;
                }

                // If we have matched all characters of rotatedString, then it is a valid rotation
                if (currentIndex == rotatedString.Length)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
