using System.Text;

namespace DataStructures.Core.String
{
    internal class StringParsingOperations
    {
        /// <summary>
        /// Parses a comma-separated string into individual values without using <see cref="string.Split(char[])"/>.
        ///
        /// <para>
        /// The method scans the input string character by character, identifies comma separators and extracts each value manually.
        /// Leading and trailing whitespaces around each value are removed.
        ///
        /// This implementation demonstrates the fundamental idea behind tokenization and simple CSV parsing.
        /// </para>
        ///
        /// <para>
        /// <b>Note:</b>
        /// This is a simplified CSV parser intended for educational purposes. It does not support quoted fields containing commas, escaped quotes or other features defined by RFC 4180.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: Apple, Banana, Orange, Mango
        ///
        /// Output:
        /// Apple
        /// Banana
        /// Orange
        /// Mango
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), for storing the extracted values.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The comma-separated string to parse.
        /// </param>
        /// <returns>
        /// An array containing all parsed values.
        /// Returns an empty array if the input is null or empty.
        /// </returns>
        public static string[] ParseCommaSeparatedValues(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Array.Empty<string>();
            }

            List<string> values = new();

            StringBuilder currentValue = new();

            foreach (var currentChar in input)
            {
                if (currentChar == ',')
                {
                    // Add the current value to the list and reset the StringBuilder for the next value
                    values.Add(currentValue.ToString().Trim());
                    currentValue.Clear();
                }
                else
                {
                    // Append the current character to the current value until a comma is encountered
                    currentValue.Append(currentChar);
                }
            }

            values.Add(currentValue.ToString().Trim());

            return values.ToArray();
        }

        /// <summary>
        /// Parses a string containing semicolon-separated key-value pairs.
        ///
        /// <para>
        /// Each key-value pair must follow the format:
        ///
        /// Key=Value
        ///
        /// Individual pairs are separated by semicolons.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// Name=John;Age=30;City=London
        ///
        /// Output:
        /// Name → John
        /// Age → 30
        /// City → London
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The input string containing key-value pairs.
        /// </param>
        /// <returns>
        /// A dictionary containing all parsed key-value pairs.
        /// Invalid pairs are ignored.
        /// </returns>
        public static Dictionary<string, string> ParseKeyValuePairs(string input)
        {
            Dictionary<string, string> result = new();

            if (string.IsNullOrWhiteSpace(input))
            {
                return result;
            }

            foreach (var pair in ParseCommaSeparatedValues(input.Replace(';', ',')))
            {
                var separatorIndex = pair.IndexOf('=');

                if (separatorIndex <= 0) // If the separator is not found or is at the start of the string, skip this pair
                {
                    continue;
                }

                var key = pair[..separatorIndex].Trim(); // Get the substring from the start to the separator index and trim whitespace
                var value = pair[(separatorIndex + 1)..].Trim(); // Get the substring from the character after the separator index to the end and trim whitespace

                result[key] = value;
            }

            return result;
        }

        /// <summary>
        /// Parses a sentence into individual words without using <see cref="string.Split(char[])"/>.
        ///
        /// <para>
        /// Consecutive whitespace characters are treated as a single separator. Leading and trailing whitespaces are ignored.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: Learning Data Structures is fun
        ///
        /// Output:
        /// Learning
        /// Data
        /// Structures
        /// is
        /// fun
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="sentence">
        /// The sentence to parse.
        /// </param>
        /// <returns>
        /// An array containing all parsed words.
        /// Returns an empty array if the input is null or empty.
        /// </returns>
        public static string[] ParseWords(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
            {
                return Array.Empty<string>();
            }

            List<string> words = new();
            StringBuilder currentWord = new();

            foreach (var currentChar in sentence)
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    if (currentWord.Length > 0)
                    {
                        words.Add(currentWord.ToString());
                        currentWord.Clear();
                    }
                }
                else
                {
                    currentWord.Append(currentChar);
                }
            }

            if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
            }

            return words.ToArray();
        }

        /// <summary>
        /// Extracts all integer values present in the specified input string.
        ///
        /// <para>
        /// The method scans the input one character at a time and identifies contiguous sequences of numeric digits. Each sequence is converted into an integer and added to the result collection.
        ///
        /// This implementation demonstrates manual numeric parsing without relying on regular expressions.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: Order123Item45Price999
        ///
        /// Output:
        /// 123
        /// 45
        /// 999
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the input string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of integers extracted.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The string from which integer values are extracted.
        /// </param>
        /// <returns>
        /// A list containing all extracted integers.
        /// Returns an empty list if the input is null or empty.
        /// </returns>
        public static List<int> ExtractIntegers(string input)
        {
            List<int> integers = new();

            if (string.IsNullOrWhiteSpace(input))
            {
                return integers;
            }

            var currentNum = 0;
            var isBuildingNumber = false;

            foreach (var currentChar in input)
            {
                if (char.IsDigit(currentChar))
                {
                    // Convert the character to its integer value and build the number
                    currentNum = (currentNum * 10) + (currentChar - '0');
                    isBuildingNumber = true;
                }
                // If we were building a number and encounter a non-digit character, we finalize the current number
                else if (isBuildingNumber) 
                {
                    integers.Add(currentNum);
                    currentNum = 0;
                    isBuildingNumber = false;
                }
            }
            // If the string ends with a number, we need to add it to the list
            if (isBuildingNumber)
            {
                integers.Add(currentNum);
            }

            return integers;
        }

        /// <summary>
        /// Extracts all decimal numbers present in the specified input string.
        ///
        /// <para>
        /// The method manually scans the input and identifies contiguous sequences consisting of digits and at most one decimal point.
        ///
        /// This implementation is intended for educational purposes and demonstrates manual decimal parsing without using regular expressions or internal parsing methods.
        /// </para>
        ///
        /// <para>
        /// <b>Note:</b>
        /// Scientific notation and signed numbers are intentionally excluded to keep the implementation focused on the fundamentals.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// Weight:72.5 Height:180.2
        ///
        /// Output:
        /// 72.5
        /// 180.2
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k), where k is the number of extracted decimal values.
        /// </para>
        /// </summary>
        /// <param name="input">
        /// The string from which decimal values are extracted.
        /// </param>
        /// <returns>
        /// A list containing all extracted decimal values.
        /// Returns an empty list if the input is null or empty.
        /// </returns>
        public static List<double> ExtractDecimals(string input)
        {
            List<double> decimals = new();

            if (string.IsNullOrWhiteSpace(input))
            {
                return decimals;
            }

            StringBuilder currentNum = new();
            var hasDecimalPoint = false;

            foreach (var currentChar in input)
            {
                if (char.IsDigit(currentChar))
                {
                    currentNum.Append(currentChar);
                }
                else if (currentChar == '.' && !hasDecimalPoint)
                {
                    currentNum.Append(currentChar);
                    hasDecimalPoint = true;
                }
                else
                {
                    if (currentNum.Length > 0)
                    {
                        decimals.Add(ConvertToDouble(currentNum.ToString()));
                    }

                    currentNum.Clear();
                    hasDecimalPoint = false;
                }
            }

            if (currentNum.Length > 0)
            {
                decimals.Add(ConvertToDouble(currentNum.ToString()));
            }

            return decimals;
        }

        private static double ConvertToDouble(string number)
        {
            double result = 0;
            var decimalPointEncountered = false;
            double divisor = 10;

            foreach (var currentChar in number)
            {
                if (currentChar == '.')
                {
                    decimalPointEncountered = true;
                    continue;
                }

                var digit = currentChar - '0';

                if (!decimalPointEncountered)
                {
                    result = (result * 10) + digit;
                }
                else
                {
                    result += digit / divisor; // Add the digit divided by the current divisor to the result
                    divisor *= 10; // Increase the divisor by a factor of 10 for the next decimal place
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a date string in the format <c>dd-MM-yyyy</c> and extracts its individual components.
        ///
        /// <para>
        /// The method validates the overall structure of the date string and returns the day, month and year as separate values.
        /// This implementation focuses on string parsing rather than calendar validation.
        /// </para>
        ///
        /// <para>
        /// <b>Expected Format:</b>
        /// dd-MM-yyyy
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: 21-07-2026
        ///
        /// Output:
        /// Day   : 21
        /// Month : 07
        /// Year  : 2026
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(1), since the expected input length is fixed.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        /// </summary>
        /// <param name="date">
        /// The date string to parse.
        /// </param>
        /// <returns>
        /// A tuple containing the extracted day, month and year.
        ///
        /// Throws an <see cref="ArgumentException"/> if the input does not follow the expected format.
        /// </returns>
        public static (string Day, string Month, string Year) ParseDateComponents(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                throw new ArgumentException("Date cannot be null or empty.");
            }

            if (date.Length != 10 || date[2] != '-' || date[5] != '-')
            {
                throw new ArgumentException("Expected format: dd-MM-yyyy");
            }

            var day = date[..2];
            var month = date.Substring(3, 2);
            var year = date.Substring(6, 4);

            return (day, month, year);
        }

        /// <summary>
        /// Parses a URL query string into individual key-value pairs.
        ///
        /// <para>
        /// The method expects the query string to follow the format:
        /// key1=value1&key2=value2&key3=value3
        ///
        /// Each parameter is separated by '&amp;', while keys and values are separated by '='.
        /// </para>
        ///
        /// <para>
        /// This implementation performs manual parsing without relying on <see cref="string.Split(char[])"/>.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// id=10&name=John&city=London
        ///
        /// Output:
        /// id   → 10
        /// name → John
        /// city → London
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        public static Dictionary<string, string> ParseQueryString(string queryString)
        {
            Dictionary<string, string> result = new();

            if (string.IsNullOrWhiteSpace(queryString))
            {
                return result;
            }

            StringBuilder token = new();

            foreach (var currentChar in queryString)
            {
                if (currentChar == '&')
                {
                    AddQueryParameter(token.ToString(), result);
                    token.Clear();
                }
                else
                {
                    token.Append(currentChar);
                }
            }

            if (token.Length > 0)
            {
                AddQueryParameter(token.ToString(), result);
            }

            return result;
        }

        private static void AddQueryParameter(string parameter, Dictionary<string, string> dictionary)
        {
            var separatorIndex = parameter.IndexOf('=');

            if (separatorIndex <= 0)
            {
                return;
            }

            var key = parameter[..separatorIndex].Trim();
            var value = parameter[(separatorIndex + 1)..].Trim();

            dictionary[key] = value;
        }

        /// <summary>
        /// Parses a file path and extracts the directory, file name and extension.
        ///
        /// <para>
        /// This implementation performs manual parsing without using <see cref="System.IO.Path"/> helper methods.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// C:\Projects\DataStructures\Program.cs
        ///
        /// Output:
        /// Directory : C:\Projects\DataStructures
        /// File Name : Program
        /// Extension : .cs
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// O(n)
        ///
        /// <b>Space Complexity</b>
        /// O(n)
        /// </summary>
        public static (string Directory, string FileName, string Extension) ParseFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be empty.");
            }

            var lastSlash = filePath.LastIndexOf('\\');

            if (lastSlash < 0)
            {
                throw new ArgumentException("Invalid file path.");
            }

            var directory = filePath[..lastSlash];
            var fileWithExtension = filePath[(lastSlash + 1)..];
            var lastDot = fileWithExtension.LastIndexOf('.');

            if (lastDot < 0)
            {
                return (directory, fileWithExtension, string.Empty);
            }

            var fileName = fileWithExtension[..lastDot];
            var extension = fileWithExtension[lastDot..];

            return (directory, fileName, extension);
        }

        /// <summary>
        /// Parses an email address into its major components.
        ///
        /// <para>
        /// The method extracts: Username, Domain, Domain Name, Top-Level Domain (TLD).
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// john.smith@gmail.com
        ///
        /// Output:
        /// Username        : john.smith
        /// Domain          : gmail.com
        /// Domain Name     : gmail
        /// Top Level Domain: com
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// O(n)
        ///
        /// <b>Space Complexity</b>
        /// O(1)
        /// </summary>
        public static (string Username, string Domain, string DomainName, string TopLevelDomain) ParseEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            var atIndex = email.IndexOf('@');

            if (atIndex <= 0)
            {
                throw new ArgumentException("Invalid email address.");
            }

            var username = email[..atIndex];
            var domain = email[(atIndex + 1)..];
            var dotIndex = domain.LastIndexOf('.');

            if (dotIndex < 0)
            {
                throw new ArgumentException("Invalid email address.");
            }

            var domainName = domain[..dotIndex];
            var topLevelDomain = domain[(dotIndex + 1)..];

            return (username, domain, domainName, topLevelDomain);
        }

        /// <summary>
        /// Parses a URL and extracts its primary components.
        ///
        /// <para>
        /// The method extracts: Protocol, Host, Path, Query String.
        ///
        /// This implementation performs simplified URL parsing and is intended for educational purposes.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// https://www.example.com/products?id=5
        ///
        /// Output:
        /// Protocol : https
        /// Host     : www.example.com
        /// Path     : products
        /// Query    : id=5
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// O(n)
        ///
        /// <b>Space Complexity</b>
        /// O(1)
        /// </summary>
        public static (string Protocol, string Host, string Path, string Query) ParseUrlComponents(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be empty.");
            }

            var protocolSeparator = url.IndexOf("://");

            if (protocolSeparator < 0)
            {
                throw new ArgumentException("Invalid URL.");
            }

            var protocol = url[..protocolSeparator];
            var remaining = url[(protocolSeparator + 3)..];
            var firstSlash = remaining.IndexOf('/');

            if (firstSlash < 0)
            {
                return (protocol, remaining, string.Empty, string.Empty);
            }

            var host = remaining[..firstSlash];
            var pathAndQuery = remaining[(firstSlash + 1)..];
            var querySeparator = pathAndQuery.IndexOf('?');

            if (querySeparator < 0)
            {
                return (protocol, host, pathAndQuery, string.Empty);
            }

            var path = pathAndQuery[..querySeparator];
            var query = pathAndQuery[(querySeparator + 1)..];

            return (protocol, host, path, query);
        }
    }
}
