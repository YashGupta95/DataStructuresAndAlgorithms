namespace DataStructures.Core.HashMap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                LOOKUP PROBLEMS DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateContainsDuplicate();

            DemonstrateTwoSum();

            DemonstrateFindEmployeeById();

            DemonstrateIsSubset();

            DemonstrateFindMissingNumber();

            DemonstrateIntersection();

            DemonstrateInvertDictionary();

            DemonstrateGroupEmployeesByDepartment();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates checking whether an array contains duplicate elements.
        /// </summary>
        private static void DemonstrateContainsDuplicate()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Contains Duplicate");
            Console.WriteLine("==============================================================");

            var numbers = new[] { 4, 2, 7, 1, 4, 9 };

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Check whether any element appears more than once");

            var containsDuplicate = LookupProblemsOperations.ContainsDuplicate(numbers);

            Console.WriteLine("\nResult:");
            Console.WriteLine(containsDuplicate);
        }

        /// <summary>
        /// Demonstrates finding two numbers whose sum equals the target.
        /// </summary>
        private static void DemonstrateTwoSum()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Two Sum");
            Console.WriteLine("==============================================================");

            var numbers = new[] { 2, 7, 11, 15 };
            var target = 9;

            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Target: {target}");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find two indices whose values add up to the target");

            var result = LookupProblemsOperations.TwoSum(numbers, target);

            Console.WriteLine("\nResult:");

            if (result is not null)
            {
                Console.WriteLine($"Indices: [{result[0]}, {result[1]}]");
                Console.WriteLine($"Values : {numbers[result[0]]} + {numbers[result[1]]} = {target}");
            }
            else
            {
                Console.WriteLine("No valid pair found.");
            }
        }

        /// <summary>
        /// Demonstrates finding an employee using a dictionary lookup.
        /// </summary>
        private static void DemonstrateFindEmployeeById()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Find Employee By Id");
            Console.WriteLine("==============================================================");

            var employees = new List<Employee>
            {
                new Employee { Id = 101, Name = "Alice", Department = "IT" },
                new Employee { Id = 102, Name = "Bob", Department = "HR" },
                new Employee { Id = 103, Name = "Charlie", Department = "Finance" }
            };

            Console.WriteLine("Employees:");

            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }

            var employeeId = 102;

            Console.WriteLine($"\nLookup Employee Id: {employeeId}");
            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Build a lookup dictionary");
            Console.WriteLine("• Retrieve employee using Employee Id");

            var employeeFound = LookupProblemsOperations.FindEmployeeById(employees, employeeId);

            Console.WriteLine("\nResult:");

            if (employeeFound is not null)
            {
                Console.WriteLine($"Found Employee: {employeeFound}");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        /// <summary>
        /// Demonstrates checking whether one array is a subset of another.
        /// </summary>
        private static void DemonstrateIsSubset()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. Is Subset");
            Console.WriteLine("==============================================================");

            var source = new[] { 1, 2, 3, 4, 5 };
            var subset = new[] { 2, 4 };

            Console.WriteLine($"Source Array : [{string.Join(", ", source)}]");
            Console.WriteLine($"Subset Array : [{string.Join(", ", subset)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Verify whether every element of the subset exists in the source array");

            var result = LookupProblemsOperations.IsSubset(source, subset);

            Console.WriteLine($"\nResult: {result}");
        }

        /// <summary>
        /// Demonstrates finding a missing number using dictionary lookup.
        /// </summary>
        private static void DemonstrateFindMissingNumber()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. Find Missing Number");
            Console.WriteLine("==============================================================");

            var numbers = new[] { 1, 2, 4, 5 };

            Console.WriteLine($"Input Array    : [{string.Join(", ", numbers)}]");
            Console.WriteLine("Expected Range : 1 - 5");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Build a lookup table");
            Console.WriteLine("• Find the missing number within the expected range");

            var missingNumber = LookupProblemsOperations.FindMissingNumber(numbers, 1, 5);

            Console.WriteLine($"\nResult: {missingNumber?.ToString() ?? "No missing number found."}");
        }

        /// <summary>
        /// Demonstrates finding the common elements between two arrays.
        /// </summary>
        private static void DemonstrateIntersection()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Intersection");
            Console.WriteLine("==============================================================");

            var first = new[] { 1, 2, 3, 4 };
            var second = new[] { 3, 4, 5, 6 };

            Console.WriteLine($"First Array  : [{string.Join(", ", first)}]");
            Console.WriteLine($"Second Array : [{string.Join(", ", second)}]");

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Find all common elements");

            var intersection = LookupProblemsOperations.Intersection(first, second);

            Console.WriteLine($"\nResult: {string.Join(", ", intersection)}");
        }

        /// <summary>
        /// Demonstrates reversing keys and values of a dictionary.
        /// </summary>
        private static void DemonstrateInvertDictionary()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Invert Dictionary");
            Console.WriteLine("==============================================================");

            var employees = new Dictionary<int, string>()
            {
                { 101, "Alice" },
                { 102, "Bob" },
                { 103, "Charlie" }
            };

            Console.WriteLine("Original Dictionary:");

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Key} -> {emp.Value}");
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Swap dictionary keys and values");

            var invertedDictionary = LookupProblemsOperations.InvertDictionary(employees);

            Console.WriteLine("\nResult:");

            foreach (var emp in invertedDictionary.OrderBy(e => e.Value))
            {
                Console.WriteLine($"{emp.Key} -> {emp.Value}");
            }
        }

        /// <summary>
        /// Demonstrates grouping employees according to department.
        /// </summary>
        private static void DemonstrateGroupEmployeesByDepartment()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. Group Employees By Department");
            Console.WriteLine("==============================================================");

            var employees = new List<Employee>()
            {
                new Employee { Id = 101, Name = "Alice", Department = "IT" },
                new Employee { Id = 102, Name = "Bob", Department = "IT" },
                new Employee { Id = 103, Name = "Charlie", Department = "HR" },
                new Employee { Id = 104, Name = "David", Department = "HR" },
                new Employee { Id = 105, Name = "Eve", Department = "Finance" }
            };

            Console.WriteLine("Employees:");

            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Group employees based on department");

            var groupedEmployees = LookupProblemsOperations.GroupEmployeesByDepartment(employees);

            Console.WriteLine("\nResult:");

            foreach (var dept in groupedEmployees.OrderBy(d => d.Key))
            {
                Console.WriteLine($"\n{dept.Key}");

                foreach (var emp in dept.Value.OrderBy(e => e.Id))
                {
                    Console.WriteLine($"  {emp}");
                }
            }
        }
    }
}
