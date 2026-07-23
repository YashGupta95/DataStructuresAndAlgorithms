namespace DataStructures.Core.String
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                STRING PARSING DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateParseCommaSeparatedValues();

            DemonstrateParseKeyValuePairs();

            DemonstrateParseWords();

            DemonstrateExtractIntegers();

            DemonstrateExtractDecimals();

            DemonstrateParseDateComponents();

            DemonstrateParseQueryString();

            DemonstrateParseFilePath();

            DemonstrateParseEmailAddress();

            DemonstrateParseUrlComponents();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates manual parsing of comma-separated values.
        /// </summary>
        private static void DemonstrateParseCommaSeparatedValues()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Parse Comma-Separated Values");
            Console.WriteLine("==============================================================");

            var input = "Apple, Banana, Orange, Mango";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nParsed Values:");

            var values = StringParsingOperations.ParseCommaSeparatedValues(input);

            foreach (var value in values)
            {
                Console.WriteLine($"• {value}");
            }
        }

        /// <summary>
        /// Demonstrates parsing of key-value pairs.
        /// </summary>
        private static void DemonstrateParseKeyValuePairs()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Parse Key-Value Pairs");
            Console.WriteLine("==============================================================");

            var input = "Name=John;Age=30;City=London";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nParsed Key-Value Pairs:");

            var result = StringParsingOperations.ParseKeyValuePairs(input);

            foreach (var pair in result)
            {
                Console.WriteLine($"{pair.Key} -> {pair.Value}");
            }
        }

        /// <summary>
        /// Demonstrates manual word parsing.
        /// </summary>
        private static void DemonstrateParseWords()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Parse Words");
            Console.WriteLine("==============================================================");

            var input = "Learning Data Structures is fun";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nParsed Words:");

            var words = StringParsingOperations.ParseWords(input);

            foreach (var word in words)
            {
                Console.WriteLine($"• {word}");
            }
        }

        /// <summary>
        /// Demonstrates extraction of integer values.
        /// </summary>
        private static void DemonstrateExtractIntegers()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Extract Integers");
            Console.WriteLine("==============================================================");

            var input = "Order123Item45Price999";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nExtracted Integers:");

            var numbers = StringParsingOperations.ExtractIntegers(input);

            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }
        }

        /// <summary>
        /// Demonstrates extraction of decimal values.
        /// </summary>
        private static void DemonstrateExtractDecimals()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Extract Decimals");
            Console.WriteLine("==============================================================");

            var input = "Weight:72.5 Height:180.25";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nExtracted Decimals:");

            var decimals = StringParsingOperations.ExtractDecimals(input);

            foreach (var value in decimals)
            {
                Console.WriteLine(value);
            }
        }

        /// <summary>
        /// Demonstrates parsing of date components.
        /// </summary>
        private static void DemonstrateParseDateComponents()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Parse Date Components");
            Console.WriteLine("==============================================================");

            var input = "21-07-2026";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            var date = StringParsingOperations.ParseDateComponents(input);

            Console.WriteLine("\nParsed Components:");
            Console.WriteLine($"Day   : {date.Day}");
            Console.WriteLine($"Month : {date.Month}");
            Console.WriteLine($"Year  : {date.Year}");
        }

        /// <summary>
        /// Demonstrates parsing of query-string parameters.
        /// </summary>
        private static void DemonstrateParseQueryString()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Parse Query String");
            Console.WriteLine("==============================================================");

            var input = "id=10&name=John&city=London";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            Console.WriteLine("\nParsed Parameters:");

            var result = StringParsingOperations.ParseQueryString(input);

            foreach (var pair in result)
            {
                Console.WriteLine($"{pair.Key} -> {pair.Value}");
            }
        }

        /// <summary>
        /// Demonstrates parsing of a file path.
        /// </summary>
        private static void DemonstrateParseFilePath()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Parse File Path");
            Console.WriteLine("==============================================================");

            var input = @"C:\Projects\DataStructures\Program.cs";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            var result = StringParsingOperations.ParseFilePath(input);

            Console.WriteLine("\nParsed Components:");
            Console.WriteLine($"Directory : {result.Directory}");
            Console.WriteLine($"File Name : {result.FileName}");
            Console.WriteLine($"Extension : {result.Extension}");
        }

        /// <summary>
        /// Demonstrates parsing of an email address.
        /// </summary>
        private static void DemonstrateParseEmailAddress()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Parse Email Address");
            Console.WriteLine("==============================================================");

            var input = "john.smith@gmail.com";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            var result = StringParsingOperations.ParseEmailAddress(input);

            Console.WriteLine("\nParsed Components:");
            Console.WriteLine($"Username         : {result.Username}");
            Console.WriteLine($"Domain           : {result.Domain}");
            Console.WriteLine($"Domain Name      : {result.DomainName}");
            Console.WriteLine($"Top-Level Domain : {result.TopLevelDomain}");
        }

        /// <summary>
        /// Demonstrates parsing of URL components.
        /// </summary>
        private static void DemonstrateParseUrlComponents()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. Parse URL Components");
            Console.WriteLine("==============================================================");

            var input = "https://www.example.com/products?id=5";

            Console.WriteLine("Input:");
            Console.WriteLine(input);

            var result = StringParsingOperations.ParseUrlComponents(input);

            Console.WriteLine("\nParsed Components:");
            Console.WriteLine($"Protocol : {result.Protocol}");
            Console.WriteLine($"Host     : {result.Host}");
            Console.WriteLine($"Path     : {result.Path}");
            Console.WriteLine($"Query    : {result.Query}");
        }
    }
}