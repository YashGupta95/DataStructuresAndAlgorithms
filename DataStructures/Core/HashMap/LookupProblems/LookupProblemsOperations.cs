namespace DataStructures.Core.HashMap
{
    internal class LookupProblemsOperations
    {
        /// <summary>
        /// Determines whether the specified array contains any duplicate elements.
        ///
        /// <para>
        /// The method traverses the array while maintaining a lookup table of previously encountered elements. If the current element already exists in the lookup table, a duplicate has been found and the method immediately returns <see langword="true"/>. Otherwise, the element is added to the lookup table and the traversal continues.
        /// </para>
        ///
        /// <para>
        /// This approach avoids the need for nested loops and reduces the average-case time complexity from O(n²) to O(n).
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: [4, 2, 7, 1, 4, 9]
        ///
        /// Traversal:
        /// 4 ✓
        /// 2 ✓
        /// 7 ✓
        /// 1 ✓
        /// 4 ✗ (Already Exists)
        ///
        /// Output: True
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(n)
        ///
        /// Worst Case: O(n²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), for storing previously encountered elements.
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array to examine.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a duplicate exists; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static bool ContainsDuplicate(int[] numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            Dictionary<int, bool> visitedNumbers = new();

            foreach (var num in numbers)
            {
                if (visitedNumbers.ContainsKey(num))
                {
                    return true;
                }

                visitedNumbers[num] = true;
            }

            return false;
        }

        /// <summary>
        /// Finds two indices whose corresponding values add up to the specified target.
        ///
        /// <para>
        /// The method traverses the array once while maintaining a lookup table that maps each previously encountered number to its index.
        /// For every element, it computes the required complement and checks whether that complement has already been encountered. If so, the pair of indices is returned immediately.
        /// </para>
        ///
        /// <para>
        /// This is the optimal solution to the classic "Two Sum" interview problem and demonstrates how a dictionary can reduce the time complexity from O(n²) to O(n).
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// Numbers = [2, 7, 11, 15]
        /// Target = 9
        ///
        /// Traversal:
        /// 2 → Need 7
        /// 7 → Need 2 ✓
        ///
        /// Output: [0, 1]
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(n)
        /// Worst Case: O(n²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The input array.
        /// </param>
        /// <param name="target">
        /// The required sum.
        /// </param>
        /// <returns>
        /// An array containing the two matching indices, or <c>null</c> if no valid pair exists.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static int[]? TwoSum(int[] numbers, int target)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            Dictionary<int, int> visitedNumbers = new();

            for (var currentIndex = 0; currentIndex < numbers.Length; currentIndex++)
            {
                var currentNum = numbers[currentIndex];
                var requiredComplement = target - currentNum;

                if (visitedNumbers.TryGetValue(requiredComplement, out var complementIndex)) // Check if the required complement has already been encountered.
                {
                    return new[] { complementIndex, currentIndex };
                }

                visitedNumbers[currentNum] = currentIndex; // Store the current number and its index for future reference.
            }

            return null;
        }

        /// <summary>
        /// Finds an employee having the specified identifier.
        ///
        /// <para>
        /// The method first creates a dictionary that maps employee IDs to employee objects. It then performs a constant-time lookup using the specified employee ID.
        ///
        /// This demonstrates a common real-world optimization where a collection is indexed once and subsequently queried multiple times.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Employees:
        /// 101 → Alice
        /// 102 → Bob
        /// 103 → Charlie
        ///
        /// Lookup: 102
        ///
        /// Output: Bob
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Building Dictionary: O(n)
        /// Lookup: Average Case: O(1)
        ///
        /// Worst Case: O(n), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="employees">
        /// The employee collection.
        /// </param>
        /// <param name="employeeId">
        /// The employee ID to search for.
        /// </param>
        /// <returns>
        /// The matching employee if found; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="employees"/> is null. </exception>
        public static Employee? FindEmployeeById(List<Employee> employees, int employeeId)
        {
            ArgumentNullException.ThrowIfNull(employees);

            Dictionary<int, Employee> employeeLookup = new();

            foreach (var emp in employees)
            {
                employeeLookup[emp.Id] = emp;
            }

            employeeLookup.TryGetValue(employeeId, out var matchingEmployee);

            return matchingEmployee;
        }

        /// <summary>
        /// Determines whether all elements of one array are present in another.
        ///
        /// <para>
        /// The method first builds a lookup table containing all elements of the larger array. It then verifies that every element of the potential subset exists within the lookup table.
        /// Using a dictionary avoids repeated linear searches and reduces the average-case time complexity from O(n × m) to O(n + m).
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Array A: [1, 2, 3, 4, 5]
        ///
        /// Array B: [2, 4]
        ///
        /// Lookup:
        /// 2 ✓
        /// 4 ✓
        ///
        /// Output: rue
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(n + m)
        ///
        /// Worst Case: O((n + m)²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), where n is the number of elements in the source array.
        /// </para>
        /// </summary>
        /// <param name="source">
        /// The array that may contain all required elements.
        /// </param>
        /// <param name="subset">
        /// The array to verify.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if every element in <paramref name="subset"/> exists in <paramref name="source"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static bool IsSubset(int[] source, int[] subset)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(subset);

            Dictionary<int, bool> lookup = new();

            foreach (var num in source)
            {
                lookup[num] = true;
            }

            foreach (var num in subset)
            {
                if (!lookup.ContainsKey(num))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the missing number from a sequence of consecutive integers.
        ///
        /// <para>
        /// The method builds a lookup table containing all numbers present in the input array. It then scans the expected range to locate the first missing value.
        /// Although mathematical solutions exist with O(1) extra space, this implementation intentionally uses a dictionary to demonstrate fast lookup operations.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Expected Range: 1 to 5
        ///
        /// Input: [1, 2, 4, 5]
        ///
        /// Lookup:
        /// 1 ✓
        /// 2 ✓
        /// 3 ✗
        ///
        /// Output: 3
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(n)
        /// Worst Case: O(n²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array containing the sequence with one missing number.
        /// </param>
        /// <param name="start">
        /// The first expected number.
        /// </param>
        /// <param name="end">
        /// The last expected number.
        /// </param>
        /// <returns>
        /// The missing number if found; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static int? FindMissingNumber(int[] numbers, int start, int end)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            Dictionary<int, bool> lookup = new();

            foreach (var num in numbers)
            {
                lookup[num] = true;
            }

            for (var num = start; num <= end; num++)
            {
                if (!lookup.ContainsKey(num))
                {
                    return num;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the common elements between two integer arrays.
        ///
        /// <para>
        /// The method builds a lookup table containing all elements of the first array. It then traverses the second array and adds each matching element to the result.
        /// To avoid duplicate values in the output, each matched element is removed from the lookup table immediately after being added to the result.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Array A: [1, 2, 3, 4]
        ///
        /// Array B: [3, 4, 5, 6]
        ///
        /// Lookup:
        /// 3 ✓
        /// 4 ✓
        ///
        /// Output: [3, 4]
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case: O(n + m)
        /// Worst Case: O((n + m)²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n)
        /// </para>
        /// </summary>
        /// <param name="first">
        /// The first input array.
        /// </param>
        /// <param name="second">
        /// The second input array.
        /// </param>
        /// <returns>
        /// A list containing the common elements shared by both arrays.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static List<int> Intersection(int[] first, int[] second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            Dictionary<int, bool> lookup = new();

            foreach (var num in first)
            {
                lookup[num] = true; // Populate the lookup table with elements from the first array.
            }

            List<int> intersection = new();

            foreach (var num in second)
            {
                if (lookup.ContainsKey(num))
                {
                    intersection.Add(num);

                    // Remove the element to avoid duplicate entries in the result if it appears multiple times in the second array.
                    lookup.Remove(num);
                }
            }

            return intersection;
        }

        /// <summary>
        /// Creates a new dictionary by swapping the keys and values of the specified dictionary.
        ///
        /// <para>
        /// The method traverses the input dictionary and constructs a new dictionary where:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Original Value </description>
        /// </item>
        /// <item>
        /// <description> Value = Original Key </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// Dictionary inversion is useful when reverse lookups are required. 
        /// Since dictionary keys must be unique, this method assumes that all values in the input dictionary are unique. If duplicate values are encountered, an exception is thrown.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// 101 → Alice
        /// 102 → Bob
        /// 103 → Charlie
        ///
        /// Output:
        /// Alice → 101
        /// Bob → 102
        /// Charlie → 103
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of entries in the dictionary.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), for constructing the inverted dictionary.
        /// </para>
        /// </summary>
        /// <param name="employees">
        /// The dictionary to invert.
        /// </param>
        /// <returns>
        /// A new dictionary with keys and values exchanged.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="employees"/> is null. </exception>
        /// <exception cref="ArgumentException"> Thrown when duplicate values exist in the input dictionary. </exception>
        public static Dictionary<string, int> InvertDictionary(Dictionary<int, string> employees)
        {
            ArgumentNullException.ThrowIfNull(employees);

            Dictionary<string, int> invertedDictionary = new();

            foreach (var emp in employees)
            {
                if (invertedDictionary.ContainsKey(emp.Value))
                {
                    throw new ArgumentException("Dictionary cannot be inverted because duplicate values were found.");
                }

                invertedDictionary[emp.Value] = emp.Key;
            }

            return invertedDictionary;
        }

        /// <summary>
        /// Groups employees according to their department.
        ///
        /// <para>
        /// The method traverses the employee collection and builds a dictionary where:
        /// <list type="bullet">
        /// <item>
        /// <description> Key = Department Name </description>
        /// </item>
        /// <item>
        /// <description> Value = List of employees belonging to that department </description>
        /// </item>
        /// </list>
        /// </para>
        ///
        /// <para>
        /// This is a common real-world application of dictionaries where related objects are grouped for efficient access and reporting.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input:
        /// 101 Alice     IT
        /// 102 Bob       IT
        /// 103 Charlie   HR
        /// 104 David     HR
        /// 105 Eve       Finance
        ///
        /// Output:
        /// IT
        /// ├── Alice
        /// └── Bob
        ///
        /// HR
        /// ├── Charlie
        /// └── David
        ///
        /// Finance
        /// └── Eve
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// O(n), where n is the number of employees.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), since every employee is stored in exactly one department group.
        /// </para>
        /// </summary>
        /// <param name="employees">
        /// The collection of employees.
        /// </param>
        /// <returns>
        /// A dictionary that groups employees by department.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="employees"/> is null. </exception>
        public static Dictionary<string, List<Employee>> GroupEmployeesByDepartment(List<Employee> employees)
        {
            ArgumentNullException.ThrowIfNull(employees);

            Dictionary<string, List<Employee>> deptGroups = new();

            foreach (var emp in employees)
            {
                if (!deptGroups.ContainsKey(emp.Department))
                {
                    deptGroups[emp.Department] = new List<Employee>();
                }

                deptGroups[emp.Department].Add(emp);
            }

            return deptGroups;
        }
    }
}
