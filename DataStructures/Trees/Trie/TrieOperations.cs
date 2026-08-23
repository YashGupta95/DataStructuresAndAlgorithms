using System.Text;

namespace DataStructures.Trees.Trie
{
    // A Trie (pronounced "try", from "retrieval") is a tree-shaped data structure specialized for
    // storing and looking up strings by their CHARACTER SEQUENCE. Unlike a Binary Search Tree —
    // which routes on the ordering of a whole key — a Trie routes on each successive character of
    // the key, one character per level.
    //
    // -----------------------------------------------------------------------
    // How a Trie Stores Words
    // -----------------------------------------------------------------------
    // Every edge is labeled by a single character. To store a word, walk down from the root
    // following (or creating) the edge for each character in turn, then mark the final node as an
    // "end of word" so the Trie can distinguish "car" (a stored word) from a mere prefix on the
    // way to "card".
    //
    // The character that leads to a node lives on the EDGE (i.e., in the parent's dictionary key),
    // NOT on the node itself. Each node therefore holds only:
    //   • Children       — a map from "next character" to "next node".
    //   • IsEndOfWord    — a flag that says "a stored word ends exactly here".
    //
    // -----------------------------------------------------------------------
    // Example
    // -----------------------------------------------------------------------
    // After inserting { "car", "card", "care", "cat" }:
    //
    //     [root]
    //         c
    //             a
    //                 r*
    //                     d*
    //                     e*
    //                 t*
    //
    // A node marked with * is an end-of-word. Notice that the node reached by "car" is BOTH an
    // end-of-word (for "car") AND an internal waypoint (for "card" and "care").
    //
    // -----------------------------------------------------------------------
    // Why not just use HashSet<string> or SortedSet<string>?
    // -----------------------------------------------------------------------
    //   • HashSet<string>    — O(1) average existence check, but has NO efficient way to answer
    //                          "give me every word starting with 'ca'". It must scan the whole
    //                          set. Tries answer that same question in time proportional only to
    //                          the length of the prefix plus the size of the answer.
    //   • SortedSet<string>  — Supports range queries, but each Contains still costs O(log n · m),
    //                          where m is the key length. A Trie is O(m) regardless of set size.
    //
    // Tries shine for autocomplete, spell-check dictionaries, longest-common-prefix queries, and
    // (with specialised bitwise variants) IP-routing tables.
    //
    // -----------------------------------------------------------------------
    // Time Complexity  (m = length of the input string, n = number of stored words)
    // -----------------------------------------------------------------------
    //   Insert(word)             O(m)
    //   Search(word)             O(m)
    //   StartsWith(prefix)       O(m)
    //   Delete(word)             O(m)
    //   GetAllWords()            O(total characters stored)
    //   GetWordsWithPrefix(p)    O(m + characters in the answer)
    //   LongestCommonPrefix()    O(length of the LCP)
    //   CountWords()             O(number of nodes)
    //
    // -----------------------------------------------------------------------
    // A note on this implementation
    // -----------------------------------------------------------------------
    // Each node's children are held in a Dictionary<char, TrieNode>. This keeps the code short,
    // supports any character (case-sensitive, digits, Unicode), and doesn't waste memory on
    // unused letters. The classic textbook alternative — a fixed-size TrieNode[] with 26 slots
    // for 'a'..'z' — is faster and more compact when the alphabet is small and known in advance,
    // and is a common follow-up optimization once the base algorithm is understood.
    //
    // Unlike the BST / AVL / Red-Black Tree operations in this repo, none of these methods return
    // a "new root": a Trie's root is always the same permanent empty node and is never rotated
    // or replaced. Methods that logically mutate the tree therefore return void (Insert) or a
    // bool describing the outcome (Delete).
    // =============================================================================================
    internal static class TrieOperations
    {
        /// <summary>
        /// Inserts the specified word into the Trie.
        ///
        /// <para>
        /// The method walks <paramref name="word"/> one character at a time starting from <paramref name="root"/>. For each character it either follows the existing child edge or creates a new child node if the character has not been seen at this position before. Once the last character has been consumed, the final node is marked as an end-of-word so future <see cref="Search"/> calls can distinguish this word from a mere prefix.
        /// </para>
        ///
        /// <para>
        /// Inserting a word that is already stored is a safe no-op — every character walk reuses the existing edges, and re-flagging the final node has no effect.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Insert("car"), then Insert("card"):
        ///
        /// After Insert("car"):        After Insert("card"):
        ///   [root]                       [root]
        ///       c                            c
        ///           a                            a
        ///               r*                           r*
        ///                                                d*
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(m), where m is the length of <paramref name="word"/>. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(m), the worst case being that every character introduces a new node. </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <param name="word"> The word to insert. If <see langword="null"/> or empty, the method is a no-op. </param>
        public static void Insert(TrieNode root, string word)
        {
            if (root is null || string.IsNullOrEmpty(word))
            {
                return;
            }

            var current = root;

            foreach (var c in word)
            {
                if (!current.Children.TryGetValue(c, out var next))
                {
                    // No edge for this character yet — create one.
                    next = new TrieNode();
                    current.Children[c] = next;
                }

                current = next;
            }

            // Mark the terminal node so this exact word can be distinguished from a mere prefix.
            current.IsEndOfWord = true;
        }

        /// <summary>
        /// Determines whether the specified word is stored in the Trie.
        ///
        /// <para>
        /// The method walks <paramref name="word"/> character by character, following child edges from <paramref name="root"/>. If any character has no matching edge, the word is not present. Otherwise, the result is the <see cref="TrieNode.IsEndOfWord"/> flag on the final node — reaching a node whose flag is <see langword="false"/> means the input is only a prefix, not a stored word.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Trie holds { "car", "card" }:
        ///
        ///   [root]
        ///       c
        ///           a
        ///               r*
        ///                   d*
        ///
        /// Search("car")   → true  (r is marked *)
        /// Search("card")  → true  (d is marked *)
        /// Search("ca")    → false (walk reaches 'a', but 'a' is NOT marked *)
        /// Search("cart")  → false (no 't' edge under 'r')
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(m), where m is the length of <paramref name="word"/>. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <param name="word"> The word to search for. </param>
        /// <returns> <see langword="true"/> if <paramref name="word"/> is stored in the Trie; otherwise <see langword="false"/>. </returns>
        public static bool Search(TrieNode root, string word)
        {
            if (root is null || string.IsNullOrEmpty(word))
            {
                return false;
            }

            var current = root;

            foreach (var c in word)
            {
                if (!current.Children.TryGetValue(c, out var next))
                {
                    return false; // A character is missing along the path.
                }

                current = next;
            }

            // The path exists; the word is stored only if this node is flagged as an end-of-word.
            return current.IsEndOfWord;
        }

        /// <summary>
        /// Determines whether any stored word begins with the specified prefix.
        ///
        /// <para>
        /// <c>StartsWith</c> is almost identical to <see cref="Search"/>, but it does NOT require the terminal node to be an end-of-word. The mere existence of the character path from <paramref name="root"/> guarantees that at least one longer stored word passes through this point.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Trie holds { "car", "card" }:
        ///
        /// StartsWith("ca")   → true  (path 'c' → 'a' exists; irrelevant that 'a' is not *)
        /// StartsWith("car")  → true  (path exists; 'r' happens to also be *)
        /// StartsWith("cat")  → false (no 't' edge under 'a')
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(m), where m is the length of <paramref name="prefix"/>. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(1) </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <param name="prefix"> The prefix to test. </param>
        /// <returns> <see langword="true"/> if some stored word starts with <paramref name="prefix"/>; otherwise <see langword="false"/>. </returns>
        public static bool StartsWith(TrieNode root, string prefix)
        {
            if (root is null || string.IsNullOrEmpty(prefix))
            {
                return false;
            }

            var current = root;

            foreach (var c in prefix)
            {
                if (!current.Children.TryGetValue(c, out var next))
                {
                    return false;
                }

                current = next;
            }

            return true; // The full path exists; end-of-word status doesn't matter for a prefix query.
        }

        /// <summary>
        /// Removes the specified word from the Trie if it is present, pruning any nodes that become unused.
        ///
        /// <para>
        /// Deletion is a two-phase, post-order recursive walk:
        /// <list type="number">
        /// <item> <description> <b>Descend</b> — follow the character path from <paramref name="root"/> down to the terminal node. If any character has no matching edge, the word isn't stored and the method returns <see langword="false"/> without touching anything. </description> </item>
        /// <item> <description> <b>Ascend and prune</b> — at the terminal node, clear its <see cref="TrieNode.IsEndOfWord"/> flag. Then, as the recursion unwinds, each parent asks: "is my child now redundant?" A child is redundant iff its <see cref="TrieNode.IsEndOfWord"/> is <see langword="false"/> AND its own <see cref="TrieNode.Children"/> map is empty. Redundant children are removed. Pruning stops naturally at the first node that either represents a shorter stored word or is on the path to some other stored word — that node must be kept. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <b>Example — non-shared branch</b>
        /// <code>
        /// Delete("dose") from { "dog", "dot", "dose" }:
        ///
        /// Before:                    After:
        ///   [root]                     [root]
        ///       d                          d
        ///           o                          o
        ///               g*                         g*
        ///               s                          t*
        ///                   e*
        ///               t*
        /// </code>
        /// The <c>s → e</c> branch was used by no other word, so both nodes are pruned. The <c>o</c> and <c>d</c> nodes are kept because <c>dog</c> and <c>dot</c> still need them.
        ///
        /// <b>Example — shared branch</b>
        /// <code>
        /// Delete("car") from { "car", "card", "care" }:
        ///
        /// Before:                    After:
        ///   [root]                     [root]
        ///       c                          c
        ///           a                          a
        ///               r*                         r
        ///                   d*                         d*
        ///                   e*                         e*
        /// </code>
        /// The node reached by <c>car</c> is no longer marked as an end-of-word, but it stays in the tree because it still has children on the paths to <c>card</c> and <c>care</c>.
        ///
        /// <b>Time Complexity</b>
        /// <para> O(m), where m is the length of <paramref name="word"/>. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(m), due to the recursion depth. </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <param name="word"> The word to remove. If <see langword="null"/> or empty, the method returns <see langword="false"/>. </param>
        /// <returns> <see langword="true"/> if <paramref name="word"/> was found and removed; otherwise <see langword="false"/>. </returns>
        public static bool Delete(TrieNode root, string word)
        {
            if (root is null || string.IsNullOrEmpty(word))
            {
                return false;
            }

            return DeleteHelper(root, word, 0);
        }

        /// <summary>
        /// Recursive helper for <see cref="Delete"/>. Walks down to the terminal node, clears the end-of-word flag on the way back up, and prunes any child that has become both un-flagged and childless.
        /// </summary>
        /// <param name="node"> The current node in the descent. </param>
        /// <param name="word"> The word being removed. </param>
        /// <param name="depth"> The index into <paramref name="word"/> that <paramref name="node"/> corresponds to. </param>
        /// <returns> <see langword="true"/> if the word was found and its terminal flag was cleared on this descent; otherwise <see langword="false"/>. </returns>
        private static bool DeleteHelper(TrieNode node, string word, int depth)
        {
            if (depth == word.Length) // Reached the end of the word being deleted.
            {
                // Reached the position for the last character. If it isn't flagged as an end-of-word, the word wasn't stored.
                if (!node.IsEndOfWord)
                {
                    return false;
                }

                node.IsEndOfWord = false;
                return true;
            }

            var c = word[depth]; // Fetch the character at the current depth.

            // Follow the edge labeled by character c from node down to its child (c is a char / edge label, not a node).
            if (!node.Children.TryGetValue(c, out var child))
            {
                return false; // Character path is broken — the word isn't stored.
            }

            var removed = DeleteHelper(child, word, depth + 1);

            // On the way back up, prune the child if it no longer represents a word and has no children of its own.
            if (removed && !child.IsEndOfWord && child.Children.Count == 0)
            {
                node.Children.Remove(c);
            }

            return removed;
        }

        /// <summary>
        /// Returns every word currently stored in the Trie, in depth-first order (the child dictionary's iteration order determines sibling order).
        ///
        /// <para>
        /// This is a depth-first traversal from <paramref name="root"/>. A running <see cref="StringBuilder"/> path tracks the characters walked so far; whenever a visited node has <see cref="TrieNode.IsEndOfWord"/> set, the current path is a stored word and is added to the result.
        /// </para>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(N), where N is the total number of characters across all stored words. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(m) recursion depth, where m is the length of the longest stored word. </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <returns> A list containing every word stored in the Trie. </returns>
        public static List<string> GetAllWords(TrieNode root)
        {
            var result = new List<string>();

            if (root is null)
            {
                return result;
            }

            // A running buffer of characters from the root down to the current node. Whenever a node is flagged as an end-of-word, this path is snapshotted into the result.
            var path = new StringBuilder(); 

            CollectWords(root, path, result);

            return result;
        }

        /// <summary>
        /// Returns every stored word that begins with the specified prefix.
        ///
        /// <para>
        /// The algorithm has two stages:
        /// <list type="number">
        /// <item> <description> <b>Walk to the prefix node</b> — descend from <paramref name="root"/> along the characters of <paramref name="prefix"/>. If any character is missing, no word matches and an empty list is returned. </description> </item>
        /// <item> <description> <b>Collect the subtree</b> — perform a depth-first walk of the prefix node's subtree, adding the prefix in front of each discovered suffix to reconstruct full words. </description> </item>
        /// </list>
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Trie holds { "cat", "car", "card", "care", "dog" }:
        ///
        /// GetWordsWithPrefix("ca") → [ "cat", "car", "card", "care" ]  (order depends on dictionary iteration)
        /// GetWordsWithPrefix("do") → [ "dog" ]
        /// GetWordsWithPrefix("z")  → [ ]
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(m + N'), where m is the length of <paramref name="prefix"/> and N' is the total number of characters in the returned words. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(m'), the maximum recursion depth in the collected subtree. </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <param name="prefix"> The prefix to match. </param>
        /// <returns> All stored words that begin with <paramref name="prefix"/>. Empty if the prefix does not exist. </returns>
        public static List<string> GetWordsWithPrefix(TrieNode root, string prefix)
        {
            var result = new List<string>();

            if (root is null || prefix is null)
            {
                return result;
            }

            var current = root;

            // Stage 1: walk down to the node that represents the end of the prefix.
            foreach (var c in prefix)
            {
                if (!current.Children.TryGetValue(c, out var next))
                {
                    return result; // Prefix not present — nothing to collect.
                }

                current = next;
            }

            // Stage 2: DFS from the prefix node, seeding the path with the prefix so full words are reconstructed.
            var path = new StringBuilder(prefix);
            CollectWords(current, path, result);

            return result;
        }

        /// <summary>
        /// Depth-first helper used by <see cref="GetAllWords"/> and <see cref="GetWordsWithPrefix"/>.
        ///
        /// <para>
        /// Appends the current character to <paramref name="path"/> before recursing into each child and removes it on the way back, so <paramref name="path"/> always reflects the characters from the traversal's starting node down to the current node. Whenever the current node is flagged as an end-of-word, the path is snapshotted into <paramref name="result"/>.
        /// </para>
        ///
        /// <b>Example — trace over { "car", "cat" }</b>
        /// <code>
        ///     [root]
        ///         c
        ///             a
        ///                 r*
        ///                 t*
        ///
        /// Starting at root with path = "":
        ///
        ///     visit root                       (not *)
        ///       Append 'c'                     → path = "c"
        ///       recurse into 'c' node          (not *)
        ///         Append 'a'                   → path = "ca"
        ///         recurse into 'a' node        (not *)
        ///           Append 'r'                 → path = "car"
        ///           recurse into 'r' node      (*) → result += "car"; no children
        ///           path.Length--              → path = "ca"     (undo 'r')
        ///           Append 't'                 → path = "cat"
        ///           recurse into 't' node      (*) → result += "cat"; no children
        ///           path.Length--              → path = "ca"     (undo 't')
        ///         path.Length--                → path = "c"      (undo 'a')
        ///       path.Length--                  → path = ""       (undo 'c')
        ///
        /// Final: result = [ "car", "cat" ] and path is empty again.
        /// </code>
        ///
        /// <para>
        /// So <c>path.Length--</c> executes once for every child edge ever entered — every <c>path.Append(c)</c> is paired with a matching pop on the way back out. Without it, when the loop moves from the 'r' sibling to the 't' sibling, path would still be "car" and Append('t') would produce "cart" instead of "cat".
        /// </para>
        /// </summary>
        /// <param name="node"> The node currently being visited. </param>
        /// <param name="path"> A running buffer of characters from the traversal's starting node to <paramref name="node"/>. </param>
        /// <param name="result"> The accumulator that collected words are appended to. </param>
        private static void CollectWords(TrieNode node, StringBuilder path, List<string> result)
        {
            if (node.IsEndOfWord)
            {
                result.Add(path.ToString());
            }

            // If it's end of word and no children, foreach won't execute.
            // If it's end of word and has children, foreach will execute to find the other words that share the same prefix.
            foreach (var (c, child) in node.Children)
            {
                path.Append(c);
                CollectWords(child, path, result);
                path.Length--; // Backtrack: remove this character before exploring the next sibling once the recursive call comes back.
            }
        }

        /// <summary>
        /// Returns the longest string that is a prefix of every stored word.
        ///
        /// <para>
        /// The walk starts at <paramref name="root"/> and descends while two conditions hold at the current node:
        /// <list type="bullet">
        /// <item> <description> The current node is NOT an end-of-word (going further would drop a shorter stored word out of the "every-word" set). </description> </item>
        /// <item> <description> The current node has EXACTLY ONE child (more than one child means the words split here, so the current path is the longest possible common prefix). </description> </item>
        /// </list>
        /// The characters walked form the longest common prefix.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Trie holds { "car", "card", "care" }:
        ///     [root]
        ///         c
        ///             a
        ///                 r*         <- IsEndOfWord for "car"; walk stops here
        ///                     d*
        ///                     e*
        /// LongestCommonPrefix() = "car"
        ///
        /// Trie holds { "cat", "car" }:
        ///     [root]
        ///         c
        ///             a              <- has TWO children (r, t); walk stops here
        ///                 r*
        ///                 t*
        /// LongestCommonPrefix() = "ca"
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(p), where p is the length of the returned prefix. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(p) </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <returns> The longest common prefix of every stored word, or the empty string if the Trie is empty or the words diverge at the root. </returns>
        public static string LongestCommonPrefix(TrieNode root)
        {
            if (root is null)
            {
                return string.Empty;
            }

            var prefix = new StringBuilder();
            var current = root;

            while (!current.IsEndOfWord && current.Children.Count == 1)
            {
                // Exactly one child: it's forced to be part of the common prefix. Grab its character and descend.
                var only = current.Children.First();
                prefix.Append(only.Key);
                current = only.Value;
            }

            return prefix.ToString();
        }

        /// <summary>
        /// Counts the number of words stored in the Trie.
        ///
        /// <para>
        /// This is a straight depth-first traversal that sums the <see cref="TrieNode.IsEndOfWord"/> flag across every node reachable from <paramref name="root"/>. Each stored word contributes exactly one <see langword="true"/> flag, so the total equals the number of stored words.
        /// </para>
        ///
        /// <para>
        /// Each recursive call is self-contained: it returns the total word count of the entire subtree rooted at the node it was given. The current frame just adds its own end-of-word flag (0 or 1) to the sum of what each child's recursive call returns. A <see langword="true"/> flag on a deep node propagates up one level per return, folded into every ancestor's local <c>count</c> along the way, until it lands in the value returned to the caller of the top-level invocation.
        /// </para>
        ///
        /// <b>Example</b>
        /// <code>
        /// Trie holds { "car", "card", "cat" }; * marks IsEndOfWord = true.
        ///
        ///     [root]
        ///         c
        ///             a
        ///                 r*     (word: "car")
        ///                     d* (word: "card")
        ///                 t*     (word: "cat")
        ///
        /// Each frame computes: count = (own flag ? 1 : 0) + sum of CountWords(child).
        ///
        ///     Frame       Own flag    Child calls return              Returns
        ///     -----       --------    ------------------              -------
        ///     d node      1           (no children)                   1
        ///     r node      1           CountWords(d) → 1               1 + 1 = 2
        ///     t node      1           (no children)                   1
        ///     a node      0           CountWords(r) → 2,
        ///                             CountWords(t) → 1               0 + 2 + 1 = 3
        ///     c node      0           CountWords(a) → 3               0 + 3 = 3
        ///     root        0           CountWords(c) → 3               0 + 3 = 3
        ///
        /// Final: root returns 3, matching the three stored words.
        /// </code>
        ///
        /// <b>Time Complexity</b>
        /// <para> O(V), where V is the number of nodes in the Trie. </para>
        ///
        /// <b>Space Complexity</b>
        /// <para> O(m), the maximum recursion depth (equal to the longest word's length). </para>
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        /// <returns> The number of stored words. </returns>
        public static int CountWords(TrieNode root)
        {
            if (root is null)
            {
                return 0;
            }

            var count = root.IsEndOfWord ? 1 : 0;

            foreach (var child in root.Children.Values)
            {
                count += CountWords(child);
            }

            return count;
        }
    }
}
