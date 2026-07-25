# SetOperations

## Overview

The **SetOperations** project demonstrates how to use the .NET `HashSet<T>` collection to efficiently perform common set-based operations.

Unlike a `Dictionary<TKey, TValue>`, which stores **key-value pairs**, a `HashSet<T>` stores **only unique values** and provides highly optimized operations such as:

- Removing duplicates
- Union
- Intersection
- Difference
- Symmetric Difference
- Subset checks

This project focuses on using the built-in `HashSet<T>` APIs instead of manually implementing these algorithms.

---

# Learning Objectives

After completing this project, you should be able to:

- Understand the purpose of `HashSet<T>`
- Know when to use a HashSet instead of a Dictionary
- Remove duplicate elements efficiently
- Perform common set operations
- Solve interview problems involving uniqueness and membership
- Apply HashSet in practical scenarios

---

# Project Structure

## Iteration 1 – Fundamental Set Operations

| Method | Description |
|---------|-------------|
| RemoveDuplicates() | Removes duplicate elements from a collection |
| Union() | Combines two collections while keeping unique values |
| Intersection() | Finds common elements between two collections |

---

## Iteration 2 – Advanced Set Operations

| Method | Description |
|---------|-------------|
| Difference() | Finds elements present only in the first collection |
| SymmetricDifference() | Finds elements present in exactly one collection |
| IsSubset() | Determines whether one collection is a subset of another |

---

## Iteration 3 – Real-world Examples

| Method | Description |
|---------|-------------|
| FindUniqueVisitors() | Finds all unique visitors across multiple days |
| FindCommonVisitors() | Finds visitors common to multiple days |

---

# Time Complexities

| Operation | Average |
|----------|---------|
| Add | O(1) |
| Remove | O(1) |
| Contains | O(1) |
| UnionWith | O(n + m) |
| IntersectWith | O(n + m) |
| ExceptWith | O(n + m) |
| SymmetricExceptWith | O(n + m) |
| IsSubsetOf | O(n + m) |

Worst-case complexity can degrade due to excessive hash collisions, although this is uncommon in practice.

---

# Dictionary vs HashSet

Both `Dictionary<TKey, TValue>` and `HashSet<T>` are hash-based collections that provide average-case constant-time lookups. The primary difference lies in **what they store** and **which problems they solve**.

| Scenario | Dictionary | HashSet |
|-----------|------------|---------|
| Store key-value pairs | ✅ | ❌ |
| Fast lookup using a key | ✅ | ❌ |
| Frequency counting | ✅ | ❌ |
| Store unique values | ❌ | ✅ |
| Remove duplicates | ❌ | ✅ |
| Membership testing | ✅ | ✅ |
| Union / Intersection | ❌ | ✅ |
| Difference | ❌ | ✅ |

---

# When Should You Use Dictionary?

Use a Dictionary when every value is associated with another value.

Examples:

- Employee Id → Employee
- Product Id → Product
- Character → Frequency
- Username → User Details

Example:

```csharp
Dictionary<int, Employee> employees = new();

employees[101] = employee;
```

---

# When Should You Use HashSet?

Use a HashSet when only uniqueness matters.

Examples:

- Unique usernames
- Unique visitors
- Duplicate removal
- Membership testing
- Set operations

Example:

```csharp
HashSet<int> uniqueNumbers = new();

uniqueNumbers.Add(5);
uniqueNumbers.Add(5);

Console.WriteLine(uniqueNumbers.Count);

// Output:
// 1
```

---

# Common HashSet APIs

| Method | Purpose |
|---------|---------|
| Add() | Adds an element |
| Remove() | Removes an element |
| Contains() | Checks whether an element exists |
| UnionWith() | Combines two sets |
| IntersectWith() | Keeps only common elements |
| ExceptWith() | Removes elements found in another set |
| SymmetricExceptWith() | Keeps elements present in only one set |
| IsSubsetOf() | Checks subset relationship |
| Clear() | Removes all elements |

---

# Interview Tips

### Prefer HashSet when:

- Only uniqueness matters.
- Fast membership checks are required.
- Performing set operations.
- Removing duplicate values.

### Prefer Dictionary when:

- Each key has an associated value.
- Frequency counting is required.
- Fast retrieval by key is needed.
- Caching or lookup tables are required.

---

# Key Takeaways

- `HashSet<T>` stores **unique values only**.
- Duplicate insertions are ignored automatically.
- Most HashSet operations execute in **O(1)** average time.
- Built-in set operations are more expressive than manual implementations.
- Choose **Dictionary** when data has associated values.
- Choose **HashSet** when only uniqueness and membership are important.

---

# Related Projects

- DictionaryBasics
- FrequencyCounting
- LookupProblems

These projects demonstrate scenarios where `Dictionary<TKey, TValue>` is a better choice than `HashSet<T>`.