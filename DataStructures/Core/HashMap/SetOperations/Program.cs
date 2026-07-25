namespace DataStructures.Core.HashMap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                HASHSET OPERATIONS DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateRemoveDuplicates();

            DemonstrateUnion();

            DemonstrateIntersection();

            DemonstrateDifference();

            DemonstrateSymmetricDifference();

            DemonstrateIsSubset();

            DemonstrateFindUniqueVisitors();

            DemonstrateFindCommonVisitors();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates removing duplicate elements from an array.
        /// </summary>
        private static void DemonstrateRemoveDuplicates()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Remove Duplicates");
            Console.WriteLine("==============================================================");

            var numbers = new[] { 4, 2, 7, 4, 1, 2, 9 };

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Remove duplicate elements using a HashSet");

            var uniqueNumbers = SetOperations.RemoveDuplicates(numbers);

            Console.WriteLine($"\nUnique Elements: [{string.Join(", ", uniqueNumbers)}]");
        }

        /// <summary>
        /// Demonstrates computing the union of two arrays.
        /// </summary>
        private static void DemonstrateUnion()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Union");
            Console.WriteLine("==============================================================");

            var first = new[] { 1, 2, 3 };
            var second = new[] { 3, 4, 5 };

            Console.WriteLine($"First Array : [{string.Join(", ", first)}]");
            Console.WriteLine($"Second Array: [{string.Join(", ", second)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find all distinct elements present in either array");

            var union = SetOperations.Union(first, second);

            Console.WriteLine($"\nUnion: [{string.Join(", ", union)}]");
        }

        /// <summary>
        /// Demonstrates computing the intersection of two arrays.
        /// </summary>
        private static void DemonstrateIntersection()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Intersection");
            Console.WriteLine("==============================================================");

            var first = new[] { 1, 2, 3, 4 };
            var second = new[] { 3, 4, 5, 6 };

            Console.WriteLine($"First Array : [{string.Join(", ", first)}]");
            Console.WriteLine($"Second Array: [{string.Join(", ", second)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find the elements common to both arrays");

            var intersection = SetOperations.Intersection(first, second);

            Console.WriteLine($"\nIntersection: [{string.Join(", ", intersection)}]");
        }

        /// <summary>
        /// Demonstrates computing the difference of two arrays.
        /// </summary>
        private static void DemonstrateDifference()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Difference");
            Console.WriteLine("==============================================================");

            var first = new[] { 1, 2, 3, 4 };
            var second = new[] { 3, 4, 5 };

            Console.WriteLine($"First Array : [{string.Join(", ", first)}]");
            Console.WriteLine($"Second Array: [{string.Join(", ", second)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find elements present only in the first array");

            var difference = SetOperations.Difference(first, second);

            Console.WriteLine($"\nDifference: [{string.Join(", ", difference)}]");
        }

        /// <summary>
        /// Demonstrates computing the symmetric difference of two arrays.
        /// </summary>
        private static void DemonstrateSymmetricDifference()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Symmetric Difference");
            Console.WriteLine("==============================================================");

            var first = new[] { 1, 2, 3 };
            var second = new[] { 3, 4, 5 };

            Console.WriteLine($"First Array : [{string.Join(", ", first)}]");
            Console.WriteLine($"Second Array: [{string.Join(", ", second)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find elements present in exactly one array");

            var symmetricDifference = SetOperations.SymmetricDifference(first, second);

            Console.WriteLine($"\nSymmetric Difference: [{string.Join(", ", symmetricDifference)}]");
        }

        /// <summary>
        /// Demonstrates checking whether one collection is a subset of another.
        /// </summary>
        private static void DemonstrateIsSubset()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Is Subset");
            Console.WriteLine("==============================================================");

            var source = new[] { 1, 2, 3, 4, 5 };
            var subset = new[] { 2, 4 };

            Console.WriteLine($"Source Array : [{string.Join(", ", source)}]");
            Console.WriteLine($"Subset Array : [{string.Join(", ", subset)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Determine whether every element of the subset exists in the source collection");

            var isSubset = SetOperations.IsSubset(source, subset);

            Console.WriteLine($"\nResult: {isSubset}");
        }

        /// <summary>
        /// Demonstrates finding all unique visitors across two days.
        /// </summary>
        private static void DemonstrateFindUniqueVisitors()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Find Unique Visitors");
            Console.WriteLine("==============================================================");

            var dayOneVisitors = new[]
            {
                "Alice",
                "Bob",
                "Charlie"
            };

            var dayTwoVisitors = new[]
            {
                "Bob",
                "David",
                "Alice"
            };

            Console.WriteLine($"Day 1 Visitors: [{string.Join(", ", dayOneVisitors)}]");
            Console.WriteLine($"Day 2 Visitors: [{string.Join(", ", dayTwoVisitors)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find all unique visitors across both days");

            var uniqueVisitors = SetOperations.FindUniqueVisitors(dayOneVisitors, dayTwoVisitors);

            Console.WriteLine($"\nUnique Visitors: [{string.Join(", ", uniqueVisitors)}]");
        }

        /// <summary>
        /// Demonstrates finding visitors common to two days.
        /// </summary>
        private static void DemonstrateFindCommonVisitors()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Find Common Visitors");
            Console.WriteLine("==============================================================");

            var dayOneVisitors = new[]
            {
                "Alice",
                "Bob",
                "Charlie"
            };

            var dayTwoVisitors = new[]
            {
                "Bob",
                "David",
                "Alice"
            };

            Console.WriteLine($"Day 1 Visitors: [{string.Join(", ", dayOneVisitors)}]");
            Console.WriteLine($"Day 2 Visitors: [{string.Join(", ", dayTwoVisitors)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find visitors present on both days");

            var commonVisitors = SetOperations.FindCommonVisitors(dayOneVisitors, dayTwoVisitors);

            Console.WriteLine($"\nCommon Visitors: [{string.Join(", ", commonVisitors)}]");
        }
    }
}
