namespace DataStructures.Core.HashMap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              FREQUENCY COUNTING DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateCharacterFrequency();

            DemonstrateWordFrequency();

            DemonstrateIntegerFrequency();

            DemonstrateFirstUniqueCharacter();

            DemonstrateMostFrequentElement();

            DemonstrateGroupElementsByFrequency();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates counting character frequencies in a string.
        /// </summary>
        private static void DemonstrateCharacterFrequency()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Character Frequency");
            Console.WriteLine("==============================================================");

            var input = "programming";

            Console.WriteLine($"Input: {input}");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Count the occurrence of each character");

            var frequencies = FrequencyCountingOperations.CharacterFrequency(input);

            Console.WriteLine("\nCharacter Frequencies:");

            foreach (var frequency in frequencies.OrderBy(f => f.Key))
            {
                Console.WriteLine($"{frequency.Key} -> {frequency.Value}");
            }
        }

        /// <summary>
        /// Demonstrates counting word frequencies in a sentence.
        /// </summary>
        private static void DemonstrateWordFrequency()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Word Frequency");
            Console.WriteLine("==============================================================");

            var sentence = "this is a test this is only a test";

            Console.WriteLine($"Input: {sentence}");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Count the occurrence of each word");

            var frequencies = FrequencyCountingOperations.WordFrequency(sentence);

            Console.WriteLine("\nWord Frequencies:");

            foreach (var frequency in frequencies.OrderBy(f => f.Key))
            {
                Console.WriteLine($"{frequency.Key} -> {frequency.Value}");
            }
        }

        /// <summary>
        /// Demonstrates counting integer frequencies in an array.
        /// </summary>
        private static void DemonstrateIntegerFrequency()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Integer Frequency");
            Console.WriteLine("==============================================================");

            var numbers = new int[] { 4, 2, 7, 2, 4, 1, 4, 7 };

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Count the occurrence of each integer");

            var frequencies = FrequencyCountingOperations.IntegerFrequency(numbers);

            Console.WriteLine("\nInteger Frequencies:");

            foreach (var frequency in frequencies.OrderBy(f => f.Key))
            {
                Console.WriteLine($"{frequency.Key} -> {frequency.Value}");
            }
        }

        /// <summary>
        /// Demonstrates finding the first unique character.
        /// </summary>
        private static void DemonstrateFirstUniqueCharacter()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. First Unique Character");
            Console.WriteLine("==============================================================");

            var input = "swiss";

            Console.WriteLine($"Input: {input}");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find the first character whose frequency is exactly one");

            var firstUniqueCharacter = FrequencyCountingOperations.FirstUniqueCharacter(input);

            Console.WriteLine("\nResult:");

            if (firstUniqueCharacter.HasValue)
            {
                Console.WriteLine(firstUniqueCharacter.Value);
            }
            else
            {
                Console.WriteLine("No unique character found.");
            }
        }

        /// <summary>
        /// Demonstrates finding the most frequent element.
        /// </summary>
        private static void DemonstrateMostFrequentElement()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Most Frequent Element");
            Console.WriteLine("==============================================================");

            var numbers = new int[] { 4, 2, 7, 2, 4, 1, 4, 7 };

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find the element having the highest frequency");

            var mostFrequentElement = FrequencyCountingOperations.MostFrequentElement(numbers);

            Console.WriteLine("\nResult:");

            if (mostFrequentElement.HasValue)
            {
                Console.WriteLine(mostFrequentElement.Value);
            }
            else
            {
                Console.WriteLine("The input array is empty.");
            }
        }

        /// <summary>
        /// Demonstrates grouping elements based on their frequencies.
        /// </summary>
        private static void DemonstrateGroupElementsByFrequency()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Group Elements By Frequency");
            Console.WriteLine("==============================================================");

            var numbers = new int[] { 5, 5, 2, 2, 8, 8, 1 };

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Group all elements having the same frequency");

            var groupedElements = FrequencyCountingOperations.GroupElementsByFrequency(numbers);

            Console.WriteLine("\nGrouped Elements:");

            foreach (var group in groupedElements.OrderBy(group => group.Key))
            {
                Console.WriteLine($"Frequency {group.Key}: [{string.Join(", ", group.Value)}]");
            }
        }
    }
}
