# Core String Operations in .NET

This document serves as a quick reference for commonly used `string` APIs in C#.
It is intended for interview revision and complements the manual implementations present in this repository.

> **Note:** Unless otherwise specified, `string` is **immutable** in .NET. Most operations return a **new string** instead of modifying the original one.

---

# 1. String Properties

## Length

Returns the total number of characters in the string.

```csharp
string str = "Hello";

Console.WriteLine(str.Length);    // 5
```

**Time Complexity:** O(1)

---

# 2. Character Access

## Access Character by Index

```csharp
string str = "Hello";

Console.WriteLine(str[1]);        // e
```

**Time Complexity:** O(1)

---

# 3. Comparison Operations

## Equals()

Checks whether two strings are equal.

```csharp
string a = "Hello";
string b = "Hello";

Console.WriteLine(a.Equals(b));   // True
```

**Time Complexity:** O(n)

---

## == Operator

```csharp
Console.WriteLine(a == b);
```

Internally performs value comparison for strings.

**Time Complexity:** O(n)

---

## Compare()

Returns

- < 0
- = 0
- > 0

depending on lexical order.

```csharp
string.Compare("Apple", "Banana");
```

**Time Complexity:** O(n)

---

# 4. Searching Operations

## Contains()

Checks whether a substring exists.

```csharp
string str = "Interview Preparation";

Console.WriteLine(str.Contains("Prep"));
```

**Time Complexity:** O(n)

---

## StartsWith()

```csharp
str.StartsWith("Inter");
```

**Time Complexity:** O(n)

---

## EndsWith()

```csharp
str.EndsWith("tion");
```

**Time Complexity:** O(n)

---

## IndexOf()

Returns first occurrence.

```csharp
str.IndexOf('e');
```

Returns **-1** if not found.

**Time Complexity:** O(n)

---

## LastIndexOf()

Returns last occurrence.

```csharp
str.LastIndexOf('e');
```

**Time Complexity:** O(n)

---

# 5. Substring Operations

## Substring()

Extracts part of a string.

```csharp
string str = "Interview";

Console.WriteLine(str.Substring(0,5));
```

Output

```
Inter
```

**Time Complexity:** O(n)

---

# 6. Modification Operations

> Since strings are immutable, these methods return a **new string**.

---

## Replace()

```csharp
string str = "Hello World";

Console.WriteLine(str.Replace("World", "C#"));
```

Output

```
Hello C#
```

**Time Complexity:** O(n)

---

## Remove()

```csharp
string str = "Interview";

Console.WriteLine(str.Remove(5));
```

Output

```
Inter
```

**Time Complexity:** O(n)

---

## Insert()

```csharp
string str = "Hello";

Console.WriteLine(str.Insert(5, " World"));
```

Output

```
Hello World
```

**Time Complexity:** O(n)

---

# 7. Case Conversion

## ToUpper()

```csharp
str.ToUpper();
```

---

## ToLower()

```csharp
str.ToLower();
```

Both run in

**Time Complexity:** O(n)

---

# 8. Whitespace Operations

## Trim()

Removes whitespace from both ends.

```csharp
str.Trim();
```

---

## TrimStart()

```csharp
str.TrimStart();
```

---

## TrimEnd()

```csharp
str.TrimEnd();
```

All run in

**Time Complexity:** O(n)

---

# 9. Split & Join

## Split()

Splits a string using a delimiter.

```csharp
string csv = "A,B,C";

string[] values = csv.Split(',');
```

**Time Complexity:** O(n)

---

## string.Join()

Joins multiple strings.

```csharp
string.Join(",", values);
```

**Time Complexity:** O(total characters)

---

# 10. String Validation

## IsNullOrEmpty()

```csharp
string.IsNullOrEmpty(str);
```

Returns

- True → null or ""
- False → otherwise

**Time Complexity:** O(1)

---

## IsNullOrWhiteSpace()

```csharp
string.IsNullOrWhiteSpace(str);
```

Returns true for

```
null
""
"   "
```

**Time Complexity:** O(n)

---

# 11. Conversion

## ToCharArray()

```csharp
char[] chars = str.ToCharArray();
```

Useful for manual string algorithms.

**Time Complexity:** O(n)

---

# 12. StringBuilder (Mutable Strings)

Use `StringBuilder` when repeatedly modifying strings.

```csharp
StringBuilder sb = new();

sb.Append("Hello");
sb.Append(" World");

Console.WriteLine(sb.ToString());
```

Appending repeatedly using

```csharp
str += value;
```

creates many temporary strings and is inefficient.

---

# Common Interview Tips

### ✔ Strings are immutable.

```csharp
string str = "Hello";

str.Replace("H", "Y");
```

`str` remains unchanged unless the returned string is assigned.

Correct:

```csharp
str = str.Replace("H", "Y");
```

---

### ✔ Prefer StringBuilder

Instead of

```csharp
for(...)
{
    result += value;
}
```

Use

```csharp
StringBuilder sb = new();

for(...)
{
    sb.Append(value);
}

string result = sb.ToString();
```

---

### ✔ Ordinal vs Culture Comparison

For interview coding questions, prefer

```csharp
string.Equals(a, b, StringComparison.Ordinal)
```

unless culture-aware comparison is explicitly required.

---

### ✔ Remember

`string` implements

- IEnumerable<char>

Therefore it can be traversed directly.

```csharp
foreach(char c in str)
{
    Console.Write(c);
}
```

---

# Complexity Cheat Sheet

| Operation | Time |
|------------|------|
| Length | O(1) |
| Indexing | O(1) |
| Equals | O(n) |
| Compare | O(n) |
| Contains | O(n) |
| StartsWith | O(n) |
| EndsWith | O(n) |
| IndexOf | O(n) |
| LastIndexOf | O(n) |
| Substring | O(n) |
| Replace | O(n) |
| Insert | O(n) |
| Remove | O(n) |
| Trim | O(n) |
| Split | O(n) |
| Join | O(total characters) |
| ToUpper | O(n) |
| ToLower | O(n) |
| ToCharArray | O(n) |
| IsNullOrEmpty | O(1) |
| IsNullOrWhiteSpace | O(n) |

---

# Related Projects

This repository contains manual implementations and interview-focused examples for:

- StringManipulation
- StringSearching
- StringBuilder
- StringParsing

Refer to those projects for algorithmic implementations rather than framework APIs.