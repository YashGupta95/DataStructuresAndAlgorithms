using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DataStructures.Core.String
{
    internal class StringSearchingOperations
    {
        /// Total number of possible 8-bit ASCII characters used while computing the rolling hash.
        private const int HashBase = 256;

        /// Prime number used as the modulus while computing hash values. Using a prime modulus helps reduce hash collisions.
        private const int PrimeNumber = 101;

        /// Delimiter used to separate the pattern and text while constructing the combined string for Z-Algorithm.
        /// The delimiter must be a character that does not occur in either the pattern or the text to avoid false matches.
        private const char ZAlgorithmDelimiter = '$';

        /// <summary>
        /// Searches for all occurrences of a pattern within a given text using the Naive Pattern Searching algorithm.
        ///
        /// The algorithm aligns the pattern at every possible position in the text and performs a character-by-character comparison. 
        /// Whenever all characters match, the starting index of the match is recorded.
        ///
        /// Unlike optimized algorithms such as Knuth-Morris-Pratt (KMP) or Rabin-Karp, the Naive approach does not reuse information
        /// from previous comparisons and may compare the same characters multiple times.
        /// 
        /// 
        /// Notes:
        /// • Character comparison is case-sensitive.
        /// • Overlapping occurrences are included.
        /// • This algorithm serves as the foundation for more advanced string searching algorithms.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AABAACAADAABAABA"
        ///
        /// Pattern:
        /// "AABA"
        ///
        /// Output:
        /// [0, 9, 12]
        ///
        /// Explanation:
        /// The pattern "AABA" appears at indices 0, 9 and 12.
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AAAAA"
        ///
        /// Pattern:
        /// "AA"
        ///
        /// Output:
        /// [0, 1, 2, 3]
        ///
        /// Explanation:
        /// Overlapping matches are also considered.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Slide the pattern across the text one position at a time.
        /// 2. At each position, compare the pattern with the corresponding substring in the text character by character.
        /// 3. If every character matches, record the starting index.
        /// 4. Continue until every possible alignment has been checked.
        ///
        /// Time Complexity:
        /// Worst Case: O((n - m + 1) × m)
        /// where:
        /// n = Length of the text
        /// m = Length of the pattern.
        ///
        /// Space Complexity: O(k),
        /// where k is the number of matches found.
        /// </summary>
        /// <param name="text">
        /// The text in which the pattern will be searched.
        /// </param>
        /// <param name="pattern">
        /// The pattern to search for.
        /// </param>
        /// <returns>
        /// An array containing the starting indices of all occurrences of the pattern within the text.
        ///
        /// Returns an empty array if:
        /// • the text is null or empty,
        /// • the pattern is null or empty,
        /// • the pattern is longer than the text,
        /// • or no matches are found.
        /// </returns>
        public static int[] NaivePatternSearch(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern) || pattern.Length > text.Length)
            {
                return Array.Empty<int>();
            }

            List<int> matchIndices = new();

            for (var textIndex = 0; textIndex <= text.Length - pattern.Length; textIndex++)
            {
                var patternIndex = 0;

                // While the characters match, continue checking the next character in the pattern and keep incrementing the pattern index.
                // If a mismatch occurs, the loop will exit.
                while (patternIndex < pattern.Length && text[textIndex + patternIndex] == pattern[patternIndex])
                {
                    patternIndex++;
                }

                // If the entire pattern was matched, record the starting index of the match.
                if (patternIndex == pattern.Length)
                {
                    matchIndices.Add(textIndex);
                }
            }

            return matchIndices.ToArray();
        }

        /// <summary>
        /// Computes the Longest Prefix Suffix (LPS) array for the given pattern.
        ///
        /// The LPS (Longest Proper Prefix which is also a Suffix) array is the preprocessing step of 
        /// the Knuth-Morris-Pratt (KMP) Pattern Searching algorithm.
        ///
        /// For every index in the pattern, the LPS array stores the length of the longest proper prefix 
        /// that is also a suffix for the substring ending at that index.
        ///
        /// Definitions:
        /// ------------------------------------------------------------
        /// Prefix: A prefix is any sequence of characters that starts at the beginning of a string.
        ///
        /// Example:
        /// String = "ABCD"
        ///
        /// Prefixes:
        /// ""
        /// "A"
        /// "AB"
        /// "ABC"
        /// "ABCD"
        /// ------------------------------------------------------------
        /// Proper Prefix: A proper prefix is any prefix except the complete string itself.
        ///
        /// Example:
        /// String = "ABCD"
        ///
        /// Proper Prefixes:
        /// ""
        /// "A"
        /// "AB"
        /// "ABC"
        /// ------------------------------------------------------------
        /// Suffix: A suffix is any sequence of characters that ends at the end of a string.
        ///
        /// Example:
        /// String = "ABCD"
        ///
        /// Suffixes:
        /// ""
        /// "D"
        /// "CD"
        /// "BCD"
        /// "ABCD"
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Pattern:
        /// "ABABCABAB"
        ///
        /// Index:
        ///  0 1 2 3 4 5 6 7 8
        ///
        /// Characters:
        ///  A B A B C A B A B
        ///
        /// LPS:
        ///  0 0 1 2 0 1 2 3 4
        ///
        /// Explanation:
        /// At index 8,
        /// substring = "ABABCABAB"
        ///
        /// Longest Proper Prefix = "ABAB"
        /// Longest Suffix        = "ABAB"
        ///
        /// Therefore, LPS[8] = 4.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Initialize the first LPS value to 0.
        /// 2. Maintain the length of the current longest prefix-suffix.
        /// 3. Compare the current character with the next character in the current prefix.
        /// 4. If they match:
        ///      - Increase the current prefix length.
        ///      - Store it in the LPS array.
        /// 5. If they do not match:
        ///      - Fall back using the previously computed LPS value.
        ///      - Continue until either a match is found or the prefix
        ///        length becomes zero.
        /// 6. Repeat until the end of the pattern.
        ///
        /// Notes:
        /// • LPS preprocessing allows the KMP algorithm to avoid redundant character comparisons during pattern searching.
        /// • The first element of the LPS array is always 0 because a single character cannot have a proper prefix.
        /// • This method is intended to be used by the KnuthMorrisPrattSearch() algorithm but can also be studied independently for interview preparation.
        ///
        /// Time Complexity: O(m),
        /// where m is the length of the pattern.
        ///
        /// Space Complexity: O(m),
        /// for storing the LPS array.
        /// </summary>
        /// <param name="pattern">
        /// Pattern for which the Longest Prefix Suffix (LPS) array needs to
        /// be computed.
        /// </param>
        /// <returns>
        /// An integer array representing the LPS values for every index of the pattern.
        ///
        /// Returns an empty array if the pattern is null or empty.
        /// </returns>
        public static int[] ComputeLpsArray(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return Array.Empty<int>();
            }

            var lps = new int[pattern.Length]; // LPS array will currently have all values initialized to 0.

            // Length of the current longest proper prefix which is also a suffix.
            var currentPrefixLength = 0;

            // LPS of the first character is always zero.
            lps[0] = 0;

            // Start from the second character.
            var currentPatternIndex = 1;

            while (currentPatternIndex < pattern.Length)
            {
                // If characters match, extend the current prefix-suffix.
                if (pattern[currentPatternIndex] == pattern[currentPrefixLength])
                {
                    currentPrefixLength++; // Increment the length of the current longest prefix-suffix.
                    lps[currentPatternIndex] = currentPrefixLength; // Store the length in the LPS array.
                    currentPatternIndex++; // Move to the next character in the pattern.
                }
                else
                {
                    // Some characters matched, but then a mismatch is encountered.
                    if (currentPrefixLength != 0)
                    {
                        // Instead of starting over, fall back to the previous longest prefix-suffix in LPS array.
                        // This allows us to skip unnecessary comparisons and continue searching for a valid prefix-suffix match without starting from scratch.
                        currentPrefixLength = lps[currentPrefixLength - 1];
                    }
                    else
                    {
                        // No valid prefix exists.
                        lps[currentPatternIndex] = 0;
                        currentPatternIndex++;
                    }
                }
            }

            return lps;
        }

        /// <summary>
        /// Searches for all occurrences of a pattern within a given text using the Knuth-Morris-Pratt (KMP) Pattern Searching algorithm.
        ///
        /// Unlike the Naive Pattern Searching algorithm, KMP avoids redundant character comparisons 
        /// by utilizing the Longest Prefix Suffix (LPS) array computed during preprocessing.
        ///
        /// Whenever a mismatch occurs, the LPS array determines how far the pattern can be shifted 
        /// without rechecking characters that are already known to match.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AABAACAADAABAABA"
        ///
        /// Pattern:
        /// "AABA"
        ///
        /// LPS Array for Pattern will be: [0, 1, 0, 1]
        /// 
        /// Output:
        /// [0, 9, 12]
        ///
        /// Explanation:
        /// All occurrences of the pattern, including overlapping matches, are returned.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Compute the LPS array for the pattern.
        /// 2. Compare characters of the text and pattern.
        /// 3. If the characters match: advance both indices.
        /// 4. If the entire pattern matches:
        ///      - Record the starting index.
        ///      - Continue searching using the LPS array.
        /// 5. If a mismatch occurs:
        ///      - Use the LPS array to determine the next pattern index.
        ///      - Do not move the text index unless necessary.
        /// 6. Continue until the entire text has been processed.
        ///
        /// Notes:
        /// • Character comparison is case-sensitive.
        /// • Supports overlapping pattern matches.
        /// • Reuses previously matched information through the LPS array, avoiding unnecessary comparisons.
        /// • This implementation internally uses the ComputeLpsArray() method.
        ///
        /// Time Complexity: O(n + m),
        /// where:
        /// n = Length of the text
        /// m = Length of the pattern.
        ///
        /// Space Complexity: O(m),
        /// due to the LPS array.
        /// </summary>
        /// <param name="text">
        /// The text in which the pattern will be searched.
        /// </param>
        /// <param name="pattern">
        /// The pattern to search for.
        /// </param>
        /// <returns>
        /// An array containing the starting indices of all occurrences of the pattern within the text.
        ///
        /// Returns an empty array if:
        /// • the text is null or empty,
        /// • the pattern is null or empty,
        /// • the pattern is longer than the text,
        /// • or no matches are found.
        /// </returns>
        public static int[] KnuthMorrisPrattSearch(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern) || pattern.Length > text.Length)
            {
                return Array.Empty<int>();
            }

            var lps = ComputeLpsArray(pattern);

            List<int> matchIndices = new();

            var textIndex = 0;
            var patternIndex = 0;

            while (textIndex < text.Length)
            {
                // Characters match.
                if (text[textIndex] == pattern[patternIndex])
                {
                    textIndex++;
                    patternIndex++;
                }

                // Entire pattern matched.
                if (patternIndex == pattern.Length)
                {
                    matchIndices.Add(textIndex - patternIndex);

                    // 1. The LPS array tells us how many characters we can skip or reuse (because that many characters are the same from starting and the end) in the pattern.
                    // 2. If lps[patternIndex - 1] is 0, it means there is no proper prefix which is also a suffix, so we can start matching the pattern from the beginning (patternIndex = 0).
                    // 3. If lps[patternIndex -1] is, lets say: 1, it means that the first 1 character of the pattern (e.g. 'A') is also a suffix (the previous character 'A' you just matched), so we can start matching the pattern from index 1.
                    // 4. So, even if entire pattern is matched, do not reset the patternIndex to 0. Instead, shift one index back in LPS array to get the next potential starting point for the another (possibly overlapping) occurence of pattern in the text.
                    patternIndex = lps[patternIndex - 1];
                }
                // Mismatch after one or more successful matches.
                else if (textIndex < text.Length && text[textIndex] != pattern[patternIndex])
                {
                    if (patternIndex != 0)
                    {
                        // Jump back using the previous longest prefix-suffix value from the LPS array (skipping unnecessary comparisons)
                        patternIndex = lps[patternIndex - 1];
                    }
                    else
                    {
                        // No partial match exists. Move to the next character in the text.
                        textIndex++;
                    }
                }
            }

            return matchIndices.ToArray();
        }

        /// <summary>
        /// Searches for all occurrences of a pattern within a given text using Rabin-Karp Pattern Searching algorithm.
        ///
        /// Rabin-Karp uses a rolling hash technique to efficiently compare the pattern with every possible substring of the text. 
        /// Instead of comparing every character at every position, it first compares hash values. 
        /// A character-by-character comparison is performed only when the hash values match.
        ///
        /// Since different strings can occasionally produce the same hash value (known as a hash collision), 
        /// a direct comparison is always performed after a hash match to verify the result.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AABAACAADAABAABA"
        ///
        /// Pattern:
        /// "AABA"
        ///
        /// Output:
        /// [0, 9, 12]
        ///
        /// Explanation:
        /// The pattern occurs three times within the text.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Compute the hash of the pattern.
        /// 2. Compute the hash of the first window of the text.
        /// 3. Compare both hash values.
        /// 4. If the hashes match: verify by comparing characters one by one.
        /// 5. Slide the window by one character.
        /// 6. Update the text hash using the Rolling Hash technique.
        /// 7. Continue until the entire text has been processed.
        ///
        /// Notes:
        /// • Character comparison is case-sensitive.
        /// • Supports overlapping matches.
        /// • Hash collisions are verified using direct comparison.
        /// • Uses Rolling Hash to avoid recomputing every window hash.
        ///
        /// Time Complexity:
        /// Best / Average: O(n + m)
        ///
        /// Worst Case: O(n × m)
        /// (when many hash collisions occur)
        ///
        /// Space Complexity: O(1)
        /// </summary>
        /// <param name="text">
        /// Text in which the pattern will be searched.
        /// </param>
        /// <param name="pattern">
        /// Pattern to search for.
        /// </param>
        /// <returns>
        /// An array containing the starting indices of every occurrence of the pattern.
        ///
        /// Returns an empty array if:
        /// • the text is null or empty,
        /// • the pattern is null or empty,
        /// • the pattern is longer than the text,
        /// • or no matches are found.
        /// </returns>
        public static int[] RabinKarpSearch(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern) || pattern.Length > text.Length)
            {
                return Array.Empty<int>();
            }

            List<int> matchIndices = new();

            var patternLength = pattern.Length;
            var textLength = text.Length;

            var patternHash = 0;
            var textHash = 0;

            // Highest power of HashBase used while removing the leftmost character from the rolling hash.
            var highestPower = 1;

            // For a pattern of length m, the highest power of HashBase used in the hash calculation is HashBase^(m-1).
            // This will be used to remove the leftmost character from the rolling hash when sliding the window.
            // Why? because in Polynomial Rolling Hash, the leftmost character contributes with the highest power of (m-1) in a string of length m.
            for (var index = 0; index < patternLength - 1; index++)
            {
                highestPower = (highestPower * HashBase) % PrimeNumber;
            }

            // Compute initial hash values using Polynomial Rolling Hash implementation.
            // Polynomial Rolling Hash: For a string of length m, the hash value is computed as: 
            // hash(s) = (s[0] * base^(m-1) + s[1] * base^(m-2) + ... + s[m-1] * base^0) % prime
            // Iterative representation: hash(s) = (base * hash(s[0..m-2]) + s[m-1]) % prime
            for (var index = 0; index < patternLength; index++)
            {
                // First iteration will compute the hash for the first character of the pattern and the first character of the text.
                // Next iteration will multiply the current hash by base and add the next character, then take modulo prime to keep the hash value manageable.
                // In last iteration, the hash will represent the entire string's hash value.
                patternHash = (HashBase * patternHash + pattern[index]) % PrimeNumber;
                textHash = (HashBase * textHash + text[index]) % PrimeNumber;
            }

            // Slide the pattern over the text.
            for (var windowStart = 0; windowStart <= textLength - patternLength; windowStart++)
            {
                // If hashes match, verify using character comparison.
                if (patternHash == textHash)
                {
                    var patternIndex = 0;
                    while (patternIndex < patternLength && text[windowStart + patternIndex] == pattern[patternIndex])
                    {
                        patternIndex++;
                    }

                    if (patternIndex == patternLength)
                    {
                        matchIndices.Add(windowStart);
                    }
                }

                // Compute rolling hash for the next window.
                if (windowStart < textLength - patternLength)
                {
                    // Update the hash value for the next window using the rolling hash formula:
                    // new_hash = (base * (old_hash - leftmost_char * highest_power) + new_rightmost_char) % prime
                    // Why multiply it with highestPower? Because the leftmost character contributes to the hash value with the highest power of base, so we need to remove its contribution before adding the new character.
                    textHash = 
                        (HashBase *
                        (textHash - text[windowStart] * highestPower)
                        + text[windowStart + patternLength])
                        % PrimeNumber;

                    if (textHash < 0)
                    {
                        textHash += PrimeNumber;
                    }
                }
            }

            return matchIndices.ToArray();
        }

        /// <summary>
        /// Computes the Z-array for the given string.
        ///
        /// The Z-array stores, for every index, the length of the longest substring starting at that index which also matches the prefix of the entire string.
        ///
        /// The first element of the Z-array is conventionally set to 0, since it represents the entire string.
        ///
        /// The Z-array is the preprocessing step of the Z-Algorithm used for efficient pattern searching.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Input String:
        /// "aabcaabxaaaz"
        ///
        /// Index:
        ///  0 1 2 3 4 5 6 7 8 9 10 11
        ///
        /// Characters:
        ///  a a b c a a b x a a  a  z
        ///
        /// Z-array:
        ///  0 1 0 0 3 1 0 0 2 2  1  0
        ///
        /// Explanation:
        /// At index 4,
        /// substring = "aabxaaaz"
        ///
        /// The longest prefix that matches the beginning of the string is "aab", whose length is 3.
        ///
        /// Therefore, Z[4] = 3.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Maintain a window [left, right] known as the Z-box.
        /// 2. The Z-box represents the rightmost substring that matches the prefix of the string.
        /// 3. For each character:
        ///      • If it lies outside the current Z-box, compare characters directly.
        ///      • If it lies inside the current Z-box, reuse previously computed Z-values whenever possible.
        /// 4. Expand the current match as long as characters continue to match.
        /// 5. If the new match extends beyond the current Z-box, update the left and right boundaries.
        ///
        /// Notes:
        /// • The first element of the Z-array is always zero.
        /// • Previously computed information is reused to avoid redundant character comparisons.
        /// • This preprocessing enables the Z-Algorithm to perform linear-time pattern searching.
        ///
        /// Time Complexity: O(n),
        /// where n is the length of the input string.
        ///
        /// Space Complexity: O(n),
        /// for storing the Z-array.
        /// </summary>
        /// <param name="input">
        /// Input string for which the Z-array needs to be computed.
        /// </param>
        /// <returns>
        /// An integer array representing the Z-values for every index.
        ///
        /// Returns an empty array if the input string is null or empty.
        /// </returns>
        public static int[] ComputeZArray(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Array.Empty<int>();
            }

            var zArray = new int[input.Length];

            // Current Z-box boundaries.
            var left = 0;
            var right = 0;

            // Z[0] is conventionally defined as zero.
            zArray[0] = 0;

            for (var currentIndex = 1; currentIndex < input.Length; currentIndex++)
            {
                // If the current index lies within the existing Z-box, reuse the previously computed Z-value wherever possible.
                if (currentIndex <= right)
                {
                    // Because the text inside the Z-box [l, r] is an exact match for the prefix of the string starting at index 0, every currentIndex inside the Z-box has an identical "twin" or "mirror" at the start of the string.
                    // Formula: mirrorIndex = currentIndex - left.
                    var mirrorIndex = currentIndex - left;

                    // If the Z-value at the mirror index is less than the remaining length of the Z-box, we can directly assign it to the current index - as the matching sequence is safely trapped inside the current Z-box.
                    // Otherwise, we can only guarantee a match up to the right boundary of the Z-box (right - currentIndex + 1).
                    zArray[currentIndex] = Math.Min(right - currentIndex + 1, zArray[mirrorIndex]);
                }

                // If the current index lies outside the existing Z-box, we need to perform direct character comparisons to find the length of the match (it could form a new Z-box with even longer length).
                // (currentIndex + zArray[currentIndex]) is mathematically equivalent to (right + 1), which is the next character after the current Z-box.
                while (currentIndex + zArray[currentIndex] < input.Length && 
                       input[zArray[currentIndex]] == input[currentIndex + zArray[currentIndex]])
                {
                    zArray[currentIndex]++;
                }

                // If the expanded match extends beyond the length of the current Z-box, update the boundaries (as we have found a new bigger Z-box).
                if (currentIndex + zArray[currentIndex] - 1 > right)
                {
                    left = currentIndex;
                    right = currentIndex + zArray[currentIndex] - 1;
                }
            }

            return zArray;
        }

        /// <summary>
        /// Searches for all occurrences of a pattern within a given text using the Z-Algorithm.
        ///
        /// The Z-Algorithm performs linear-time pattern searching by first constructing a combined string in the following format:
        /// Pattern + Delimiter + Text
        ///
        /// It then computes the Z-array of the combined string. Whenever a Z-value equals the length of the pattern, it indicates that the pattern has been found within the text.
        ///
        /// Unlike the Naive Pattern Searching algorithm, the Z-Algorithm preprocesses the combined string to avoid redundant character comparisons, achieving linear time complexity.
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AABAACAADAABAABA"
        ///
        /// Pattern:
        /// "AABA"
        ///
        /// Combined String:
        /// "AABA$AABAACAADAABAABA"
        ///
        /// Output:
        /// [0, 9, 12]
        ///
        /// Explanation:
        /// Whenever a Z-value equals the pattern length (4), a complete pattern match has been found.
        /// ------------------------------------------------------------
        ///
        /// Example:
        /// ------------------------------------------------------------
        /// Text:
        /// "AAAAA"
        ///
        /// Pattern:
        /// "AA"
        ///
        /// Output:
        /// [0, 1, 2, 3]
        ///
        /// Explanation:
        /// Overlapping occurrences are also detected.
        /// ------------------------------------------------------------
        ///
        /// Algorithm:
        /// 1. Validate the input.
        /// 2. Construct the combined string: Pattern + Delimiter + Text.
        /// 3. Compute the Z-array of the combined string.
        /// 4. Traverse the Z-array.
        /// 5. Whenever a Z-value equals the pattern length: record the corresponding index in the original text.
        /// 6. Return all matching indices.
        ///
        /// Notes:
        /// • Character comparison is case-sensitive.
        /// • Supports overlapping matches.
        /// • Internally uses the ComputeZArray() preprocessing method.
        /// • The delimiter must not occur within either the pattern or the text.
        ///
        /// Time Complexity: O(n + m),
        /// where:
        /// n = Length of the text.
        /// m = Length of the pattern.
        ///
        /// Space Complexity: O(n + m),
        /// for the combined string and Z-array.
        /// </summary>
        /// <param name="text">
        /// The text in which the pattern will be searched.
        /// </param>
        /// <param name="pattern">
        /// The pattern to search for.
        /// </param>
        /// <returns>
        /// An array containing the starting indices of every occurrence of the pattern.
        ///
        /// Returns an empty array if:
        /// • the text is null or empty,
        /// • the pattern is null or empty,
        /// • the pattern is longer than the text,
        /// • or no matches are found.
        /// </returns>
        public static int[] ZAlgorithmSearch(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern) || pattern.Length > text.Length)
            {
                return Array.Empty<int>();
            }

            List<int> matchIndices = new();

            var combinedString = pattern + ZAlgorithmDelimiter + text;
            
            var zArray = ComputeZArray(combinedString);

            // Traverse only the text portion of the combined string.
            var textStartIndex = pattern.Length + 1;

            for (var combinedStringIndex = textStartIndex; combinedStringIndex < combinedString.Length; combinedStringIndex++)
            {
                if (zArray[combinedStringIndex] == pattern.Length)
                {
                    // Convert the index in the combined string back to the corresponding index in the original text.
                    var matchIndex = combinedStringIndex - textStartIndex;

                    matchIndices.Add(matchIndex);
                }
            }

            return matchIndices.ToArray();
        }

    }
}
