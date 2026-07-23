using System.Text;
using System.Diagnostics;

namespace DataStructures.Core.String
{
    /// <summary>
    /// Provides implementations of commonly used <see cref="System.Text.StringBuilder"/> operations for efficient string construction and modification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unlike the <see cref="string"/> type, which is immutable, <see cref="StringBuilder"/> allows characters to be modified without creating a new string object after every operation.
    /// </para>
    ///
    /// <para>
    /// This significantly improves performance when performing repeated string manipulations such as concatenation, insertion, removal, replacement or dynamic text generation.
    /// </para>
    ///
    /// <b>When should StringBuilder be used?</b>
    /// <list type="bullet">
    /// <item>
    /// <description>Building large strings inside loops.</description>
    /// </item>
    /// <item>
    /// <description>Generating reports, CSV files or HTML.</description>
    /// </item>
    /// <item>
    /// <description>Constructing SQL queries or log messages.</description>
    /// </item>
    /// <item>
    /// <description>Performing multiple modifications on the same string.</description>
    /// </item>
    /// </list>
    ///
    /// <b>When should StringBuilder NOT be used?</b>
    /// <list type="bullet">
    /// <item>
    /// <description>Small strings with only one or two concatenations.</description>
    /// </item>
    /// <item>
    /// <description>Read-only strings that never change.</description>
    /// </item>
    /// <item>
    /// <description>
    /// String interpolation or simple formatting where readability is more important than performance.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <b>Operation Complexity Summary</b>
    ///
    /// | Operation        | Typical Time Complexity   |
    /// |------------------|---------------------------|
    /// | Append()         | Amortized O(k)            |
    /// | AppendLine()     | Amortized O(k)            |
    /// | Insert()         | O(n)                      |
    /// | Remove()         | O(n)                      |
    /// | Replace()        | O(n)                      |
    /// | Clear()          | O(1)                      |
    /// | EnsureCapacity() | O(n) (only when resizing) |
    ///
    /// where:
    /// n = Current length of the <see cref="StringBuilder"/>.
    /// k = Length of the string being appended.
    ///
    /// <b>Key Interview Takeaways</b>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="string"/> is immutable, whereas <see cref="StringBuilder"/> is mutable.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="StringBuilder"/> minimizes object allocations during repeated string modifications.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Prefer <see cref="StringBuilder"/> over repeated <c>string +=</c> operations inside loops.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="StringBuilder.Capacity"/> and <see cref="StringBuilder.EnsureCapacity(int)"/> help reduce unnecessary buffer reallocations for large strings.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// This class is intended for interview preparation and educational purposes. Each method demonstrates a specific capability of <see cref="StringBuilder"/> along with its usage, implementation, performance characteristics and time complexity.
    /// </remarks>
    internal static class StringBuilderOperations
    {
        /// <summary>
        /// Demonstrates the usage of the various overloads of <see cref="StringBuilder.Append"/> method.
        ///
        /// <para>
        /// The <see cref="StringBuilder.Append"/> method appends data to the end of the current <see cref="StringBuilder"/> instance without creating a new object.
        /// </para>
        ///
        /// <para>
        /// Unlike repeated string concatenation (<c>+=</c>), which creates a new immutable string object for every modification, <see cref="StringBuilder.Append"/> modifies the existing buffer, making it significantly more efficient for repeated operations.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Initial:
        /// Hello
        ///
        /// Append(" World")
        /// Hello World
        ///
        /// Append('!')
        /// Hello World!
        ///
        /// Append(2025)
        /// Hello World!2025
        ///
        /// Append(99.95)
        /// Hello World!202599.95
        ///
        /// Append(true)
        /// Hello World!202599.95True
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Amortized O(k), where k is the length of the appended value.
        /// Occasional buffer resizing may temporarily require O(n) time.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k)
        /// </para>
        /// </summary>
        /// <returns>
        /// The populated <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder AppendText()
        {
            StringBuilder builder = new("Hello");

            builder.Append(" World");
            builder.Append('!');
            builder.Append(' ');
            builder.Append(2025);
            builder.Append(' ');
            builder.Append(99.95);
            builder.Append(' ');
            builder.Append(true);

            return builder;
        }

        /// <summary>
        /// Demonstrates the usage of <see cref="StringBuilder.AppendLine"/> method.
        ///
        /// <para>
        /// <see cref="StringBuilder.AppendLine"/> appends the specified text followed by the environment's default line terminator.
        /// </para>
        ///
        /// <para>
        /// It is commonly used while generating reports, log files, CSV files and formatted console output.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Student Report
        ///
        /// John
        /// Jane
        /// Alex
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Amortized O(k), where k is the length of the appended text.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(k)
        /// </para>
        /// </summary>
        /// <returns>
        /// A <see cref="StringBuilder"/> containing multiple lines of text.
        /// </returns>
        public static StringBuilder AppendLineText()
        {
            StringBuilder builder = new();

            builder.AppendLine("Student Report");
            builder.AppendLine("----------------------");
            builder.AppendLine("John");
            builder.AppendLine("Jane");
            builder.AppendLine("Alex");

            return builder;
        }

        /// <summary>
        /// Demonstrates the usage of <see cref="StringBuilder.Insert(int, string)"/> method.
        ///
        /// <para>
        /// The <see cref="StringBuilder.Insert"/> method inserts text at the specified index while shifting the existing characters to the right.
        /// </para>
        ///
        /// <para>
        /// This example demonstrates insertion at the beginning, middle and end of the string.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Original:
        /// HelloWorld
        ///
        /// Insert Space:
        /// Hello World
        ///
        /// Insert Prefix:
        /// Say: Hello World
        ///
        /// Insert Suffix:
        /// Say: Hello World!
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), since characters after the insertion point must be shifted.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1) (excluding any internal buffer expansion)
        /// </para>
        /// </summary>
        /// <returns>
        /// The modified <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder InsertText()
        {
            StringBuilder builder = new("HelloWorld");

            // Insert in the middle.
            builder.Insert(5, " ");

            // Insert at the beginning.
            builder.Insert(0, "Say: ");

            // Insert at the end.
            builder.Insert(builder.Length, "!");

            return builder;
        }

        /// <summary>
        /// Demonstrates the usage of <see cref="StringBuilder.Remove(int, int)"/> method.
        ///
        /// <para>
        /// The <see cref="StringBuilder.Remove(int, int)"/> method removes a specified number of characters starting from the given index.
        /// Unlike <see cref="string"/>, the modification is performed on the existing <see cref="StringBuilder"/> instance.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Original:
        /// Hello Beautiful World!
        ///
        /// Remove "Beautiful "
        ///
        /// Result:
        /// Hello World!
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of characters after the removed section, since they must be shifted to fill the gap.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1) (excluding any internal buffer resizing)
        /// </para>
        /// </summary>
        /// <returns>
        /// The modified <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder RemoveText()
        {
            StringBuilder builder = new("Hello Beautiful World!");

            builder.Remove(6, 10);

            return builder;
        }

        /// <summary>
        /// Demonstrates the usage of <see cref="StringBuilder.Replace(string, string)"/> method.
        ///
        /// <para>
        /// The <see cref="StringBuilder.Replace(string, string)"/> method replaces every occurrence of the specified text with another string.
        /// </para>
        ///
        /// <para>
        /// The search is case-sensitive.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Original:
        /// I like Java. Java is powerful.
        ///
        /// Replace:
        /// Java → C#
        ///
        /// Result:
        /// I like C#. C# is powerful.
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the length of the current string.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1) (excluding any internal buffer resizing)
        /// </para>
        /// </summary>
        /// <returns>
        /// The modified <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder ReplaceText()
        {
            StringBuilder builder = new("I like Java. Java is powerful.");

            builder.Replace("Java", "C#");

            return builder;
        }

        /// <summary>
        /// Demonstrates the usage of <see cref="StringBuilder.Clear"/> method.
        ///
        /// <para>
        /// <see cref="StringBuilder.Clear"/> removes all characters from the current instance while retaining the allocated buffer for future
        /// use.
        /// </para>
        ///
        /// <para>
        /// Reusing an existing <see cref="StringBuilder"/> is generally more efficient than creating a new instance when repeated string construction is required.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Original:
        /// Hello World!
        ///
        /// Clear()
        ///
        /// Result:
        /// (Empty StringBuilder)
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(1)
        /// </para>
        /// </summary>
        /// <returns>
        /// The cleared <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder ClearBuilder()
        {
            StringBuilder builder = new("Hello World!");

            builder.Clear();

            return builder;
        }

        /// <summary>
        /// Demonstrates method chaining using <see cref="StringBuilder"/> class.
        ///
        /// <para>
        /// Most modifying methods of <see cref="StringBuilder"/> return the same instance, allowing multiple operations to be chained together into a single, fluent expression.
        /// </para>
        ///
        /// <para>
        /// Method chaining improves code readability and eliminates the need to repeatedly reference the same <see cref="StringBuilder"/> object.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// new StringBuilder()
        ///     .Append("Hello")
        ///     .Append(" World")
        ///     .Replace("World", "C#")
        ///     .Insert(0, "Say: ");
        ///
        /// Result:
        /// Say: Hello C#
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Depends on the individual operations performed. The overall complexity is the sum of the complexities of each chained operation.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), where n is the length of the final string.
        /// </para>
        /// </summary>
        /// <returns>
        /// The modified <see cref="StringBuilder"/> instance.
        /// </returns>
        public static StringBuilder ChainingOperations()
        {
            return new StringBuilder()
                .Append("Hello")
                .Append(" World")
                .Replace("World", "C#")
                .Insert(0, "Say: ")
                .Append('!');
        }

        /// <summary>
        /// Compares the performance of repeated string concatenation using the <see cref="string"/> type against appending text using <see cref="StringBuilder"/>.
        ///
        /// <para>
        /// Since <see cref="string"/> is immutable, every concatenation creates a new string instance, resulting in additional memory allocations and copying of existing characters.
        ///
        /// In contrast, <see cref="StringBuilder"/> modifies its internal character buffer, making it significantly more efficient for repeated string construction.
        /// </para>
        ///
        /// <para>
        /// This method performs the same number of append operations using both approaches and measures the elapsed execution time using <see cref="System.Diagnostics.Stopwatch"/>.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Number of Iterations: 100000
        ///
        /// String: 145 ms
        /// StringBuilder: 4 ms
        /// </code>
        ///
        /// <para>
        /// Actual timings vary depending on hardware, .NET runtime version, JIT optimizations and whether the application is executed in Debug or Release mode.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// String concatenation: O(n²)
        ///
        /// StringBuilder:
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="iterations">
        /// Number of append operations to perform.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Elapsed time (in milliseconds) for repeated string concatenation.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Elapsed time (in milliseconds) for StringBuilder append operations.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        public static (long StringTime, long StringBuilderTime) PerformanceComparison(int iterations)
        {
            Stopwatch stopwatch = new();

            // -------------------------------------------------------------
            // String Concatenation
            // -------------------------------------------------------------

            var text = string.Empty;
            stopwatch.Start();

            for (var i = 0; i < iterations; i++)
            {
                text += "A";
            }

            stopwatch.Stop();
            var stringTime = stopwatch.ElapsedMilliseconds;

            // -------------------------------------------------------------
            // StringBuilder
            // -------------------------------------------------------------

            StringBuilder builder = new();
            stopwatch.Restart();

            for (var i = 0; i < iterations; i++)
            {
                builder.Append('A');
            }

            stopwatch.Stop();
            var stringBuilderTime = stopwatch.ElapsedMilliseconds;

            return (stringTime, stringBuilderTime);
        }
    }
}