using System;
using System.Collections.Generic;

namespace DataStructures.Core.String
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== STRING MANIPULATION ==========\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: ReverseWords()");
            Console.WriteLine("Description: Reverses the order of words while preserving the characters within each word.\n");

            string sentence = "I Love DSA";

            Console.WriteLine($"Input : {sentence}");
            Console.WriteLine($"Output: {StringManipulationOperations.ReverseWords(sentence)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: IsPalindrome()");
            Console.WriteLine("Description: Determines whether a string is a palindrome.\n");

            string palindromeInput = "madam";

            Console.WriteLine($"Input : {palindromeInput}");
            Console.WriteLine($"Output: {StringManipulationOperations.IsPalindrome(palindromeInput)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: ToggleCase()");
            Console.WriteLine("Description: Toggles the case of every alphabetic character.\n");

            string toggleInput = "Hello World 123";

            Console.WriteLine($"Input : {toggleInput}");
            Console.WriteLine($"Output: {StringManipulationOperations.ToggleCase(toggleInput)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: RemoveDuplicateCharacters()");
            Console.WriteLine("Description: Removes duplicate characters while preserving the first occurrence.\n");

            string duplicateInput = "programming";

            Console.WriteLine($"Input : {duplicateInput}");
            Console.WriteLine($"Output: {StringManipulationOperations.RemoveDuplicateCharacters(duplicateInput)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: RunLengthEncoding()");
            Console.WriteLine("Description: Compresses consecutive repeated characters using Run-Length Encoding.\n");

            string encodingInput = "aaabbcccc";

            Console.WriteLine($"Input : {encodingInput}");
            Console.WriteLine($"Output: {StringManipulationOperations.RunLengthEncoding(encodingInput)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: CharacterFrequencyUsingArray()");
            Console.WriteLine("Description: Calculates character frequencies using a fixed-size frequency array (English alphabets only).\n");

            string frequencyArrayInput = "Programming";

            Console.WriteLine($"Input : {frequencyArrayInput}");
            Console.WriteLine("Output:");

            int[] frequencies = StringManipulationOperations.CharacterFrequencyUsingArray(frequencyArrayInput);

            for (int index = 0; index < frequencies.Length; index++)
            {
                if (frequencies[index] > 0)
                {
                    Console.WriteLine($"{(char)('a' + index)} : {frequencies[index]}");
                }
            }

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: CharacterFrequencyUsingDictionary()");
            Console.WriteLine("Description: Calculates character frequencies using a Dictionary.\n");

            string frequencyDictionaryInput = "Hello World!";

            Console.WriteLine($"Input : {frequencyDictionaryInput}");
            Console.WriteLine("Output:");

            Dictionary<char, int> characterFrequencies =
                StringManipulationOperations.CharacterFrequencyUsingDictionary(frequencyDictionaryInput);

            foreach (KeyValuePair<char, int> item in characterFrequencies)
            {
                Console.WriteLine($"'{item.Key}' : {item.Value}");
            }

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: FirstNonRepeatingCharacter()");
            Console.WriteLine("Description: Finds the first character that appears only once.\n");

            string nonRepeatingInput = "programming";

            Console.WriteLine($"Input : {nonRepeatingInput}");

            char? firstNonRepeatingCharacter =
                StringManipulationOperations.FirstNonRepeatingCharacter(nonRepeatingInput);

            Console.Write("Output: ");

            if (firstNonRepeatingCharacter.HasValue)
            {
                Console.WriteLine(firstNonRepeatingCharacter.Value);
            }
            else
            {
                Console.WriteLine("No non-repeating character found.");
            }

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: AreAnagrams()");
            Console.WriteLine("Description: Determines whether two strings are anagrams.\n");

            string firstString = "Listen";
            string secondString = "Silent";

            Console.WriteLine($"First String : {firstString}");
            Console.WriteLine($"Second String: {secondString}");
            Console.WriteLine($"Output       : {StringManipulationOperations.AreAnagrams(firstString, secondString)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: LongestCommonPrefix()");
            Console.WriteLine("Description: Finds the longest common prefix among multiple strings.\n");

            string[] words =
            {
                "flower",
                "flow",
                "flight"
            };

            Console.WriteLine("Input:");

            foreach (string word in words)
            {
                Console.WriteLine(word);
            }

            Console.WriteLine($"\nOutput: {StringManipulationOperations.LongestCommonPrefix(words)}");

            Console.WriteLine("\n------------------------------------------------------------\n");

            // -----------------------------------------------------------------
            Console.WriteLine("Method: IsRotation()");
            Console.WriteLine("Description: Determines whether one string is a rotation of another.\n");

            string originalString = "ABCD";
            string rotatedString = "CDAB";

            Console.WriteLine($"Original String : {originalString}");
            Console.WriteLine($"Rotated String  : {rotatedString}");
            Console.WriteLine($"Output          : {StringManipulationOperations.IsRotation(originalString, rotatedString)}");

            Console.WriteLine("\n========== END OF DEMO ==========");
        }
    }
}