
namespace DataStructures.Core.String
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("           STRING SEARCHING ALGORITHMS");
            Console.WriteLine("======================================================");

            //----------------------------------------------------------
            // Demo 1
            //----------------------------------------------------------

            var text = "AABAACAADAABAABA";
            var pattern = "AABA";

            Console.WriteLine("\nTEXT");
            Console.WriteLine($"Text    : {text}");
            Console.WriteLine($"Pattern : {pattern}");

            Console.WriteLine("\n======================================================");
            Console.WriteLine("1. NAIVE PATTERN SEARCH");
            Console.WriteLine("======================================================");

            PrintMatchIndices(StringSearchingOperations.NaivePatternSearch(text, pattern));

            Console.WriteLine("\n======================================================");
            Console.WriteLine("2. COMPUTE LPS ARRAY");
            Console.WriteLine("======================================================");

            var lps = StringSearchingOperations.ComputeLpsArray(pattern);

            Console.WriteLine($"Pattern : {pattern}");
            Console.WriteLine($"LPS     : {string.Join(" ", lps)}");

            Console.WriteLine("\n======================================================");
            Console.WriteLine("3. KNUTH-MORRIS-PRATT (KMP)");
            Console.WriteLine("======================================================");

            PrintMatchIndices(StringSearchingOperations.KnuthMorrisPrattSearch(text, pattern));

            Console.WriteLine("\n======================================================");
            Console.WriteLine("4. RABIN-KARP");
            Console.WriteLine("======================================================");

            PrintMatchIndices(StringSearchingOperations.RabinKarpSearch(text, pattern));

            Console.WriteLine("\n======================================================");
            Console.WriteLine("5. COMPUTE Z-ARRAY");
            Console.WriteLine("======================================================");

            var combinedString = pattern + "$" + text;

            var zArray = StringSearchingOperations.ComputeZArray(combinedString);

            Console.WriteLine($"Combined String : {combinedString}");
            Console.WriteLine($"Z-Array         : {string.Join(" ", zArray)}");

            Console.WriteLine("\n======================================================");
            Console.WriteLine("6. Z-ALGORITHM");
            Console.WriteLine("======================================================");

            PrintMatchIndices(StringSearchingOperations.ZAlgorithmSearch(text, pattern));

            //----------------------------------------------------------
            // Demo 2
            //----------------------------------------------------------

            Console.WriteLine("\n\n======================================================");
            Console.WriteLine("OVERLAPPING MATCHES");
            Console.WriteLine("======================================================");

            text = "AAAAA";
            pattern = "AA";

            Console.WriteLine($"Text    : {text}");
            Console.WriteLine($"Pattern : {pattern}");

            Console.WriteLine();

            Console.Write("Naive        : ");
            PrintInline(StringSearchingOperations.NaivePatternSearch(text, pattern));

            Console.Write("KMP          : ");
            PrintInline(StringSearchingOperations.KnuthMorrisPrattSearch(text, pattern));

            Console.Write("Rabin-Karp   : ");
            PrintInline(StringSearchingOperations.RabinKarpSearch(text, pattern));

            Console.Write("Z-Algorithm  : ");
            PrintInline(StringSearchingOperations.ZAlgorithmSearch(text, pattern));

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Prints all match indices returned by a searching algorithm.
        /// </summary>
        private static void PrintMatchIndices(int[] matchIndices)
        {
            if (matchIndices.Length == 0)
            {
                Console.WriteLine("No matches found.");
                return;
            }

            Console.WriteLine("Match Indices:");

            foreach (var index in matchIndices)
            {
                Console.WriteLine(index);
            }
        }

        /// <summary>
        /// Prints match indices on a single line.
        /// </summary>
        private static void PrintInline(int[] matchIndices)
        {
            if (matchIndices.Length == 0)
            {
                Console.WriteLine("No matches");
                return;
            }

            Console.WriteLine(string.Join(", ", matchIndices));
        }
    }
}