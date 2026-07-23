using System.Text;

namespace DataStructures.Core.String
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("              STRINGBUILDER OPERATIONS DEMO");
            Console.WriteLine("==============================================================");

            DemonstrateAppend();

            DemonstrateAppendLine();

            DemonstrateInsert();

            DemonstrateRemove();

            DemonstrateReplace();

            DemonstrateClear();

            DemonstrateChaining();

            DemonstrateCapacity();

            DemonstratePerformanceComparison();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DemonstrateAppend()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Append()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("Hello\n");

            Console.WriteLine("Operations Performed:");
            Console.WriteLine("• Append(\" World\")");
            Console.WriteLine("• Append('!')");
            Console.WriteLine("• Append(' ')");
            Console.WriteLine("• Append(2025)");
            Console.WriteLine("• Append(' ')");
            Console.WriteLine("• Append(99.95)");
            Console.WriteLine("• Append(' ')");
            Console.WriteLine("• Append(true)\n");

            DisplayResult(StringBuilderOperations.AppendText());
        }

        private static void DemonstrateAppendLine()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. AppendLine()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("(Empty)\n");

            Console.WriteLine("Operations Performed:");
            Console.WriteLine("• AppendLine(\"Student Report\")");
            Console.WriteLine("• AppendLine(\"----------------------\")");
            Console.WriteLine("• AppendLine(\"John\")");
            Console.WriteLine("• AppendLine(\"Jane\")");
            Console.WriteLine("• AppendLine(\"Alex\")\n");

            DisplayResult(StringBuilderOperations.AppendLineText());
        }

        private static void DemonstrateInsert()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Insert()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("HelloWorld\n");

            Console.WriteLine("Operations Performed:");
            Console.WriteLine("• Insert(5, \" \")");
            Console.WriteLine("• Insert(0, \"Say: \")");
            Console.WriteLine("• Insert(builder.Length, \"!\")\n");

            DisplayResult(StringBuilderOperations.InsertText());
        }

        private static void DemonstrateRemove()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Remove()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("Hello Beautiful World!\n");

            Console.WriteLine("Operation Performed:");
            Console.WriteLine("• Remove(6, 10)");
            Console.WriteLine("Removes the text \"Beautiful \".\n");

            DisplayResult(StringBuilderOperations.RemoveText());
        }

        private static void DemonstrateReplace()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Replace()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("I like Java. Java is powerful.\n");

            Console.WriteLine("Operation Performed:");
            Console.WriteLine("• Replace(\"Java\", \"C#\")");
            Console.WriteLine("Replaces all occurrences of \"Java\" with \"C#\".\n");

            DisplayResult(StringBuilderOperations.ReplaceText());
        }

        private static void DemonstrateClear()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Clear()");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("Hello World!\n");

            Console.WriteLine("Operation Performed:");
            Console.WriteLine("• Clear()");
            Console.WriteLine("Removes all characters while retaining the allocated buffer.\n");

            DisplayResult(StringBuilderOperations.ClearBuilder());
        }

        private static void DemonstrateChaining()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Method Chaining");
            Console.WriteLine("==============================================================");

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("(Empty)\n");

            Console.WriteLine("Operations Performed:");
            Console.WriteLine("• Append(\"Hello\")");
            Console.WriteLine("• Append(\" World\")");
            Console.WriteLine("• Replace(\"World\", \"C#\")");
            Console.WriteLine("• Insert(0, \"Say: \")");
            Console.WriteLine("• Append('!')\n");

            DisplayResult(StringBuilderOperations.ChainingOperations());
        }

        private static void DemonstrateCapacity()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Capacity & EnsureCapacity()");
            Console.WriteLine("==============================================================");

            StringBuilder builder = new();

            Console.WriteLine("Initial StringBuilder:");
            Console.WriteLine("(Empty)\n");

            Console.WriteLine("Initial Statistics");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Length   : {builder.Length}");
            Console.WriteLine($"Capacity : {builder.Capacity}");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• EnsureCapacity(50)");

            builder.EnsureCapacity(50);

            Console.WriteLine("\nStatistics After EnsureCapacity()");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Length   : {builder.Length}");
            Console.WriteLine($"Capacity : {builder.Capacity}");

            Console.WriteLine("\nOperations Performed:");
            Console.WriteLine("• Append(\"Learning \")");
            Console.WriteLine("• Append(\"StringBuilder \")");
            Console.WriteLine("• Append(\"Capacity!\")");

            builder.Append("Learning ");
            builder.Append("StringBuilder ");
            builder.Append("Capacity!");

            Console.WriteLine("\nResult");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine(builder.ToString());

            Console.WriteLine("\nFinal Statistics");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Length   : {builder.Length}");
            Console.WriteLine($"Capacity : {builder.Capacity}");
        }

        /// <summary>
        /// Demonstrates the performance difference between repeated string
        /// concatenation and StringBuilder append operations.
        /// </summary>
        private static void DemonstratePerformanceComparison()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. Performance Comparison");
            Console.WriteLine("==============================================================");

            const int iterations = 100000;

            Console.WriteLine("Scenario");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Appending a single character {iterations:N0} times.\n");

            Console.WriteLine("Approaches Compared:");
            Console.WriteLine("• string += \"A\"");
            Console.WriteLine("• StringBuilder.Append('A')");

            var result = StringBuilderOperations.PerformanceComparison(iterations);

            Console.WriteLine("\nResults");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"String           : {result.StringTime} ms");
            Console.WriteLine($"StringBuilder    : {result.StringBuilderTime} ms");

            Console.WriteLine("\nObservation");
            Console.WriteLine("--------------------------------------------------------------");

            if (result.StringBuilderTime < result.StringTime)
            {
                Console.WriteLine("StringBuilder completed the operation faster than repeated string concatenation.");
            }
            else if (result.StringBuilderTime > result.StringTime)
            {
                Console.WriteLine("String concatenation completed faster in this run. This can occasionally happen due to runtime optimizations or measurement noise.");
            }
            else
            {
                Console.WriteLine("Both approaches completed in approximately the same time during this run.");
            }

            Console.WriteLine();
            Console.WriteLine("Why does StringBuilder usually perform better?");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("• string is immutable, so each '+=' operation creates a new string object.");
            Console.WriteLine("• StringBuilder modifies a reusable internal character buffer.");
            Console.WriteLine("• This reduces object allocations and minimizes unnecessary copying.");
            Console.WriteLine();
            Console.WriteLine("Note:");
            Console.WriteLine("- Run the application in Release mode for more representative timings.");
            Console.WriteLine("- Execute the comparison multiple times, as results may vary depending on hardware, JIT compilation and current system load.");
        }

        /// <summary>
        /// Displays the resulting StringBuilder along with its
        /// length and current capacity.
        /// </summary>
        /// <param name="builder">
        /// The StringBuilder instance to display.
        /// </param>
        private static void DisplayResult(StringBuilder builder)
        {
            Console.WriteLine("Result");
            Console.WriteLine("--------------------------------------------------------------");
            if (builder.Length == 0)
            {
                Console.WriteLine("(Empty StringBuilder)");
            }
            else
            {
                Console.WriteLine(builder.ToString());
            }

            Console.WriteLine("\nStatistics");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Length   : {builder.Length}");
            Console.WriteLine($"Capacity : {builder.Capacity}");
        }
    }
}
