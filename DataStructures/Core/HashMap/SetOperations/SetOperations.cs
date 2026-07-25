namespace DataStructures.Core.HashMap
{
    internal class SetOperations
    {
        /// <summary>
        /// Removes duplicate elements from the specified integer array.
        ///
        /// <para>
        /// The method creates a <see cref="HashSet{T}"/> from the input array. Since a hash set only stores unique values, all duplicate elements are automatically discarded.
        /// The resulting unique elements are then converted back into a list.
        /// </para>
        ///
        /// <para>
        /// This is one of the most common real-world applications of <see cref="HashSet{T}"/>, providing a simple and efficient way to eliminate duplicate values from a collection.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Input: [4, 2, 7, 4, 1, 2, 9]
        ///
        /// Output: [4, 2, 7, 1, 9]
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
        /// O(n), for storing the unique elements.
        /// </para>
        /// </summary>
        /// <param name="numbers">
        /// The array from which duplicate elements are to be removed.
        /// </param>
        /// <returns>
        /// A list containing only the unique elements.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="numbers"/> is null. </exception>
        public static List<int> RemoveDuplicates(int[] numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            var uniqueNumbers = new HashSet<int>(numbers);

            var uniqueNumsList = uniqueNumbers.ToList();
            uniqueNumsList.Sort();
            
            return uniqueNumsList;
        }

        /// <summary>
        /// Computes the union of two integer arrays.
        ///
        /// <para>
        /// The union contains every distinct element that appears in either input array. Duplicate values are automatically eliminated by the underlying <see cref="HashSet{T}"/>.
        /// The method initializes a hash set using the first array and then merges the second array using
        /// <see cref="HashSet{T}.UnionWith(IEnumerable{int})"/>.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// First Array: [1, 2, 3]
        /// Second Array: [3, 4, 5]
        ///
        /// Output: [1, 2, 3, 4, 5]
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
        /// O(n + m)
        /// </para>
        /// </summary>
        /// <param name="first">
        /// The first input array.
        /// </param>
        /// <param name="second">
        /// The second input array.
        /// </param>
        /// <returns>
        /// A list containing the union of both arrays.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static List<int> Union(int[] first, int[] second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            var result = new HashSet<int>(first);

            result.UnionWith(second);

            var uniqueList = result.ToList();
            uniqueList.Sort();

            return uniqueList;
        }

        /// <summary>
        /// Computes the intersection of two integer arrays.
        ///
        /// <para>
        /// The intersection contains only those elements that are present in both input arrays.
        /// The method initializes a <see cref="HashSet{T}"/> using the first array and then retains only the elements that also exist in the second array by calling <see cref="HashSet{T}.IntersectWith(IEnumerable{int})"/>.
        /// </para>
        ///
        /// <para>
        /// Unlike the dictionary-based implementation demonstrated in the LookupProblems project, this implementation leverages the native set operations provided by <see cref="HashSet{T}"/>, resulting in cleaner and more expressive code.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// First Array: [1, 2, 3, 4]
        /// Second Array: [3, 4, 5, 6]
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
        /// O(n), for storing the hash set.
        /// </para>
        /// </summary>
        /// <param name="first">
        /// The first input array.
        /// </param>
        /// <param name="second">
        /// The second input array.
        /// </param>
        /// <returns>
        /// A list containing the common elements present in both arrays.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static List<int> Intersection(int[] first, int[] second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            var result = new HashSet<int>(first);

            result.IntersectWith(second);

            var uniqueList = result.ToList();
            uniqueList.Sort();

            return uniqueList;
        }

        /// <summary>
        /// Computes the difference between two integer arrays.
        ///
        /// <para>
        /// The difference contains all elements that are present in the first array but not in the second array.
        /// The method initializes a <see cref="HashSet{T}"/> using the first array and removes every element that appears in the second array by calling <see cref="HashSet{T}.ExceptWith(IEnumerable{int})"/>.
        /// </para>
        ///
        /// <para>
        /// This operation is commonly used when determining which elements are unique to a particular collection.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// First Array: [1, 2, 3, 4]
        /// Second Array: [3, 4, 5]
        ///
        /// Output: [1, 2]
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para>
        /// Average Case:O(n + m)
        /// Worst Case: O((n + m)²), due to excessive hash collisions.
        /// </para>
        ///
        /// <b>Space Complexity</b>
        /// <para>
        /// O(n), for storing the elements of the first array.
        /// </para>
        /// </summary>
        /// <param name="first">
        /// The first input array.
        /// </param>
        /// <param name="second">
        /// The second input array.
        /// </param>
        /// <returns>
        /// A sorted list containing the elements that exist only in the first
        /// array.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static List<int> Difference(int[] first, int[] second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            var result = new HashSet<int>(first);

            result.ExceptWith(second);

            var difference = result.ToList();
            difference.Sort();

            return difference;
        }

        /// <summary>
        /// Computes the symmetric difference between two integer arrays.
        ///
        /// <para>
        /// The symmetric difference contains all elements that appear in exactly one of the two input arrays. Any element present in both arrays is EXCLUDED from the result.
        ///
        /// The method utilizes <see cref="HashSet{T}.SymmetricExceptWith(IEnumerable{int})"/> to perform the operation efficiently.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// First Array: [1, 2, 3]
        ///
        /// Second Array: [3, 4, 5]
        ///
        /// Output: [1, 2, 4, 5]
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
        /// O(n + m)
        /// </para>
        /// </summary>
        /// <param name="first">
        /// The first input array.
        /// </param>
        /// <param name="second">
        /// The second input array.
        /// </param>
        /// <returns>
        /// A sorted list containing the symmetric difference.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static List<int> SymmetricDifference(int[] first, int[] second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            var result = new HashSet<int>(first);

            result.SymmetricExceptWith(second); // Removes elements that are present in both sets

            var symmetricDifference = result.ToList();
            symmetricDifference.Sort();

            return symmetricDifference;
        }

        /// <summary>
        /// Determines whether all elements of one array are contained within another.
        ///
        /// <para>
        /// The method creates two <see cref="HashSet{T}"/> instances and uses <see cref="HashSet{T}.IsSubsetOf(IEnumerable{int})"/> to determine whether every element of the potential subset exists in the source collection.
        ///
        /// Unlike the implementation in the LookupProblems project, this method demonstrates the built-in subset operation provided by <see cref="HashSet{T}"/>.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Source Array: [1, 2, 3, 4, 5]
        /// Subset Array: [2, 4]
        ///
        /// Output: True
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
        /// O(n + m), for constructing the two hash sets.
        /// </para>
        /// </summary>
        /// <param name="source">
        /// The source array.
        /// </param>
        /// <param name="subset">
        /// The array that may represent a subset.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="subset"/> is a subset of <paramref name="source"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either input array is null. </exception>
        public static bool IsSubset(int[] source, int[] subset)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(subset);

            var sourceSet = new HashSet<int>(source);
            var subsetSet = new HashSet<int>(subset);

            return subsetSet.IsSubsetOf(sourceSet);
        }

        /// <summary>
        /// Finds all unique visitors across two collections of visitor names.
        ///
        /// <para>
        /// The method creates a <see cref="HashSet{T}"/> containing the visitors from the first collection and then merges the second collection using <see cref="HashSet{T}.UnionWith(IEnumerable{string})"/>.
        /// Since a hash set stores only unique elements, duplicate visitor names are automatically ignored.
        /// </para>
        ///
        /// <para>
        /// This is a common real-world scenario when combining visitor logs, customer lists, or user records from multiple sources.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Day 1:
        /// Alice
        /// Bob
        /// Charlie
        ///
        /// Day 2:
        /// Bob
        /// David
        /// Alice
        ///
        /// Output:
        /// Alice
        /// Bob
        /// Charlie
        /// David
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
        /// O(n + m),
        /// for storing the unique visitors.
        /// </para>
        /// </summary>
        /// <param name="dayOneVisitors">
        /// The visitors recorded on the first day.
        /// </param>
        /// <param name="dayTwoVisitors">
        /// The visitors recorded on the second day.
        /// </param>
        /// <returns>
        /// A sorted list containing all unique visitors.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either collection is null. </exception>
        public static List<string> FindUniqueVisitors(IEnumerable<string> dayOneVisitors, IEnumerable<string> dayTwoVisitors)
        {
            ArgumentNullException.ThrowIfNull(dayOneVisitors);
            ArgumentNullException.ThrowIfNull(dayTwoVisitors);

            var uniqueVisitors = new HashSet<string>(dayOneVisitors);

            uniqueVisitors.UnionWith(dayTwoVisitors); // Merges the second collection, automatically discarding duplicates

            var result = uniqueVisitors.ToList();
            result.Sort();

            return result;
        }

        /// <summary>
        /// Finds the visitors that are common to two collections of visitor names.
        ///
        /// <para>
        /// The method creates a <see cref="HashSet{T}"/> containing the visitors from the first collection and then retains only those visitors that also appear in the second collection by calling <see cref="HashSet{T}.IntersectWith(IEnumerable{string})"/>.
        /// </para>
        ///
        /// <para>
        /// This technique is frequently used to identify common customers, shared users, repeat visitors, or overlapping datasets.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Day 1:
        /// Alice
        /// Bob
        /// Charlie
        ///
        /// Day 2:
        /// Bob
        /// David
        /// Alice
        ///
        /// Output:
        /// Alice
        /// Bob
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
        /// O(n), for storing the first collection.
        /// </para>
        /// </summary>
        /// <param name="dayOneVisitors">
        /// The visitors recorded on the first day.
        /// </param>
        /// <param name="dayTwoVisitors">
        /// The visitors recorded on the second day.
        /// </param>
        /// <returns>
        /// A sorted list containing the visitors common to both collections.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Thrown when either collection is null. </exception>
        public static List<string> FindCommonVisitors(IEnumerable<string> dayOneVisitors, IEnumerable<string> dayTwoVisitors)
        {
            ArgumentNullException.ThrowIfNull(dayOneVisitors);
            ArgumentNullException.ThrowIfNull(dayTwoVisitors);

            var commonVisitors = new HashSet<string>(dayOneVisitors);

            commonVisitors.IntersectWith(dayTwoVisitors); // Retains only those visitors that are present in both collections

            var result = commonVisitors.ToList();
            result.Sort();

            return result;
        }
    }
}
