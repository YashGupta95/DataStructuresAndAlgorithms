# String Searching

## Overview

String Searching is the process of finding one or more occurrences of a **pattern** within a larger **text**.

It is one of the most commonly asked topics in Software Engineering interviews because it introduces several important algorithmic concepts such as preprocessing, rolling hash, prefix functions and linear-time pattern matching.

This project implements multiple string searching algorithms, ranging from the basic brute-force approach to advanced linear-time algorithms.

---

# Algorithms Implemented

| Algorithm | Time Complexity | Space Complexity | Preprocessing |
|-----------|-----------------|------------------|---------------|
| Naive Pattern Search | O((n − m + 1) × m) | O(k) | None |
| Knuth-Morris-Pratt (KMP) | O(n + m) | O(m + k) | LPS Array |
| Rabin-Karp | Average: O(n + m)<br>Worst: O(n × m) | O(k) | Polynomial Rolling Hash |
| Z-Algorithm | O(n + m) | O(n + m + k) | Z-Array |

Where:

- **n** = Length of the text
- **m** = Length of the pattern
- **k** = Number of matches found

---

# Project Structure

```
StringSearching
│
├── Program.cs
│
└── StringSearchingOperations.cs
        │
        ├── NaivePatternSearch()
        ├── ComputeLpsArray()
        ├── KnuthMorrisPrattSearch()
        ├── RabinKarpSearch()
        ├── ComputeZArray()
        └── ZAlgorithmSearch()
```

---

# Algorithms Explained

## 1. Naive Pattern Search

### Idea

Compare the pattern with every possible substring of the text.

### Advantages

- Extremely easy to understand
- No preprocessing required
- Good for small inputs

### Disadvantages

- Repeats many unnecessary comparisons
- Inefficient for large strings

---

## 2. Knuth-Morris-Pratt (KMP)

### Idea

Instead of starting over after every mismatch, KMP remembers previously matched characters using the **Longest Prefix Suffix (LPS)** array.

### Advantages

- Linear time complexity
- No redundant comparisons
- Very common interview question

### Key Concept

Longest Proper Prefix which is also a Suffix (LPS)

Example

Pattern

```
ABABCABAB
```

LPS

```
0 0 1 2 0 1 2 3 4
```

---

## 3. Rabin-Karp

### Idea

Uses a **Polynomial Rolling Hash** to compare hash values instead of comparing characters at every position.

Only when the hash values match are the actual characters compared.

### Advantages

- Excellent for searching multiple patterns
- Introduces Rolling Hash
- Elegant implementation

### Key Concepts

- Polynomial Rolling Hash
- Rolling Hash
- Hash Collision
- Prime Modulus

---

## 4. Z-Algorithm

### Idea

Builds a combined string:

```
Pattern + Delimiter + Text
```

Computes the Z-array of the combined string.

Whenever

```
Z[i] == Pattern.Length
```

a pattern match has been found.

### Advantages

- Linear time complexity
- Elegant preprocessing
- Very useful for competitive programming

### Key Concept

Z-array

Example

```
String

aabcaabxaaaz

Z

0 1 0 0 3 1 0 0 2 2 1 0
```

---

# Choosing the Right Algorithm

| Scenario | Recommended Algorithm |
|----------|-----------------------|
| Learning pattern matching | Naive Pattern Search |
| General interview questions | KMP |
| Hashing related interviews | Rabin-Karp |
| Competitive programming | Z-Algorithm |
| Multiple pattern searching | Rabin-Karp |
| Large text searching | KMP / Z-Algorithm |

---

# Comparison Summary

| Feature | Naive | KMP | Rabin-Karp | Z |
|----------|------|-----|------------|---|
| Easy to Understand | ✅ | ⚠️ | ⚠️ | ⚠️ |
| Requires Preprocessing | ❌ | ✅ | ✅ | ✅ |
| Uses Extra Data Structure | ❌ | LPS | Rolling Hash | Z-array |
| Linear Time | ❌ | ✅ | Average | ✅ |
| Supports Overlapping Matches | ✅ | ✅ | ✅ | ✅ |

---

# Common Interview Questions

### Q1. What is the difference between KMP and Z-Algorithm?

- KMP preprocesses the **pattern** using the LPS array.
- Z-Algorithm preprocesses the **combined string** using the Z-array.

---

### Q2. Why does Rabin-Karp still compare characters after the hash values match?

Because different strings can produce the same hash value (**Hash Collision**).

---

### Q3. Which algorithm is easiest to implement?

Naive Pattern Search.

---

### Q4. Which algorithm is most commonly asked in interviews?

Knuth-Morris-Pratt (KMP).

---

### Q5. Why is Polynomial Rolling Hash used?

It allows efficient computation of the next window's hash in constant time without recalculating the entire hash.

---

# Common Mistakes

- Forgetting to handle overlapping matches.
- Returning after the first match instead of continuing the search.
- Incorrect computation of the LPS array.
- Forgetting to verify matches after a hash collision in Rabin-Karp.
- Using an invalid delimiter in the Z-Algorithm.
- Off-by-one errors while converting indices from the combined string back to the original text.

---

# Learning Order

It is recommended to study the algorithms in the following order:

1. Naive Pattern Search
2. Compute LPS Array
3. Knuth-Morris-Pratt (KMP)
4. Rabin-Karp
5. Compute Z-Array
6. Z-Algorithm

Each algorithm builds upon concepts introduced by the previous one.

---

# Key Takeaways

- Naive Pattern Search is the foundation of all pattern matching algorithms.
- KMP eliminates redundant comparisons using the LPS array.
- Rabin-Karp uses Polynomial Rolling Hash and Rolling Hash techniques.
- Z-Algorithm performs pattern matching using prefix comparisons on a combined string.
- All advanced algorithms achieve linear-time searching by preprocessing useful information before the actual search begins.