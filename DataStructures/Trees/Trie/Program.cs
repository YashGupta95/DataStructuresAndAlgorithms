namespace DataStructures.Trees.Trie
{
    internal class Program
    {
        private static readonly string[] DemoVocabulary = { "cat", "car", "card", "care", "dog", "dot", "dose" };

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================");
            Console.WriteLine("                    TRIE DEMONSTRATIONS");
            Console.WriteLine("==============================================================");

            DemonstrateInsertAndVisualize();

            DemonstrateSearchHit();

            DemonstrateSearchMiss();

            DemonstrateStartsWithHit();

            DemonstrateStartsWithMiss();

            DemonstrateDeleteNonSharedBranch();

            DemonstrateDeleteSharedBranch();

            DemonstrateGetAllWords();

            DemonstrateGetWordsWithPrefix();

            DemonstratePropertyQueries();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Builds a fresh Trie populated with the shared demo vocabulary.
        /// </summary>
        private static TrieNode BuildDemoTrie()
        {
            var root = new TrieNode();

            foreach (var word in DemoVocabulary)
            {
                TrieOperations.Insert(root, word);
            }

            return root;
        }

        /// <summary>
        /// Demonstrates inserting the demo vocabulary and visualizing the resulting Trie.
        /// </summary>
        private static void DemonstrateInsertAndVisualize()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("1. Insert & Visualize");
            Console.WriteLine("==============================================================");

            Console.WriteLine($"Words Inserted: [{string.Join(", ", DemoVocabulary)}]");

            var root = BuildDemoTrie();

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Each word walks down from the root, one character per level.");
            Console.WriteLine("• If the character's edge already exists, we reuse it; otherwise a new node is created.");
            Console.WriteLine("• The final node of each word is marked with * (end-of-word).");

            Console.WriteLine("\nTrie Structure:");
            PrintTrie(root);
        }

        /// <summary>
        /// Demonstrates a successful Search for a stored word.
        /// </summary>
        private static void DemonstrateSearchHit()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("2. Search: Word Present (Hit)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "card";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nSearching for: \"{target}\"");

            var found = TrieOperations.Search(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Walk 'c' → 'a' → 'r' → 'd' from the root.");
            Console.WriteLine($"• The final node ('d') has IsEndOfWord = true, so \"{target}\" is a stored word.");

            Console.WriteLine($"\nResult: {found}");
        }

        /// <summary>
        /// Demonstrates Search returning false for a value that is only a prefix, not a stored word.
        /// </summary>
        private static void DemonstrateSearchMiss()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("3. Search: Prefix-Only (Miss)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "ca";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nSearching for: \"{target}\"");

            var found = TrieOperations.Search(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Walk 'c' → 'a' from the root. The path exists.");
            Console.WriteLine($"• BUT the 'a' node has IsEndOfWord = false — no stored word ends there.");
            Console.WriteLine($"• Search returns false. (The next demo will show StartsWith reaching the opposite conclusion for the same input.)");

            Console.WriteLine($"\nResult: {found}");
        }

        /// <summary>
        /// Demonstrates StartsWith returning true for a valid prefix — highlighting the contrast with Search.
        /// </summary>
        private static void DemonstrateStartsWithHit()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("4. StartsWith: Prefix Present (Hit)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "ca";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nTesting prefix: \"{target}\"");

            var starts = TrieOperations.StartsWith(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Walk 'c' → 'a' from the root. The path exists — that's all StartsWith needs.");
            Console.WriteLine($"• End-of-word status of the final node is irrelevant here.");
            Console.WriteLine($"• StartsWith returns true because at least one stored word (\"cat\", \"car\", ...) begins with \"{target}\".");

            Console.WriteLine($"\nResult: {starts}");
        }

        /// <summary>
        /// Demonstrates StartsWith returning false when the prefix's character path is broken.
        /// </summary>
        private static void DemonstrateStartsWithMiss()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("5. StartsWith: Prefix Absent (Miss)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "bat";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nTesting prefix: \"{target}\"");

            var starts = TrieOperations.StartsWith(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Walk from the root — the root has no 'b' edge, so the descent halts immediately.");
            Console.WriteLine($"• StartsWith returns false.");

            Console.WriteLine($"\nResult: {starts}");
        }

        /// <summary>
        /// Demonstrates Delete on a word whose branch is not shared with any other stored word — the trailing nodes get pruned.
        /// </summary>
        private static void DemonstrateDeleteNonSharedBranch()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("6. Delete: Non-Shared Branch (Nodes are Pruned)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "dose";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nDeleting: \"{target}\"");

            Console.WriteLine("\nTrie Before Delete:");
            PrintTrie(root);

            var removed = TrieOperations.Delete(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Descend 'd' → 'o' → 's' → 'e' and clear the end-of-word flag on the final node.");
            Console.WriteLine($"• Ascend: 'e' has no children and is no longer an end-of-word → prune.");
            Console.WriteLine($"• Ascend: 's' now has no children and is not an end-of-word → prune.");
            Console.WriteLine($"• Ascend: 'o' still has children ('g', 't') → stop pruning here.");

            Console.WriteLine($"\nTrie After Delete (removed = {removed}):");
            PrintTrie(root);
        }

        /// <summary>
        /// Demonstrates Delete on a word whose nodes are also on the path to other stored words — the nodes are kept and only the end-of-word flag is cleared.
        /// </summary>
        private static void DemonstrateDeleteSharedBranch()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("7. Delete: Shared Branch (Nodes Are Kept, Flag is Cleared)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var target = "car";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nDeleting: \"{target}\"");

            Console.WriteLine("\nTrie Before Delete:");
            PrintTrie(root);

            var removed = TrieOperations.Delete(root, target);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Descend 'c' → 'a' → 'r' and clear the end-of-word flag on the 'r' node.");
            Console.WriteLine($"• Ascend: 'r' still has children ('d', 'e') on the paths to \"card\" and \"care\" → NOT pruned.");
            Console.WriteLine($"• No nodes are removed; the tree simply forgets that \"car\" was a stored word.");

            Console.WriteLine($"\nTrie After Delete (removed = {removed}):");
            PrintTrie(root);
        }

        /// <summary>
        /// Demonstrates listing every word currently stored in the Trie via a depth-first walk.
        /// </summary>
        private static void DemonstrateGetAllWords()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("8. GetAllWords (Depth-First Enumeration)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();

            Console.WriteLine($"Words Inserted: [{string.Join(", ", DemoVocabulary)}]");

            var allWords = TrieOperations.GetAllWords(root);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine("• Depth-first walk of the Trie; a StringBuilder tracks the path so far.");
            Console.WriteLine("• Every node with IsEndOfWord = true snapshots the current path into the result.");

            Console.WriteLine($"\nStored Words: [{string.Join(", ", allWords)}]");
        }

        /// <summary>
        /// Demonstrates the classic autocomplete use case using <see cref="TrieOperations.GetWordsWithPrefix"/>.
        /// </summary>
        private static void DemonstrateGetWordsWithPrefix()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("9. GetWordsWithPrefix (Autocomplete)");
            Console.WriteLine("==============================================================");

            var root = BuildDemoTrie();
            var prefix = "ca";

            Console.WriteLine($"Words in Trie: [{string.Join(", ", DemoVocabulary)}]");
            Console.WriteLine($"\nAutocomplete Prefix: \"{prefix}\"");

            var suggestions = TrieOperations.GetWordsWithPrefix(root, prefix);

            Console.WriteLine("\nOperation Performed:");
            Console.WriteLine($"• Walk 'c' → 'a' from the root to reach the prefix node.");
            Console.WriteLine($"• Depth-first walk the subtree, seeding the path with \"{prefix}\" so full words are reconstructed.");

            Console.WriteLine($"\nSuggestions: [{string.Join(", ", suggestions)}]");
        }

        /// <summary>
        /// Demonstrates the two "property query" operations: <see cref="TrieOperations.LongestCommonPrefix"/> and <see cref="TrieOperations.CountWords"/>.
        /// </summary>
        private static void DemonstratePropertyQueries()
        {
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("10. Property Queries: LongestCommonPrefix & CountWords");
            Console.WriteLine("==============================================================");

            // A tighter vocabulary so the LCP is non-trivial.
            var narrowVocabulary = new[] { "car", "card", "care" };
            var narrowRoot = new TrieNode();

            foreach (var word in narrowVocabulary)
            {
                TrieOperations.Insert(narrowRoot, word);
            }

            Console.WriteLine($"Words in Trie: [{string.Join(", ", narrowVocabulary)}]");

            var narrowLcp = TrieOperations.LongestCommonPrefix(narrowRoot);
            var narrowCount = TrieOperations.CountWords(narrowRoot);

            Console.WriteLine("\nOperation Performed (LongestCommonPrefix):");
            Console.WriteLine("• Descend from the root while the current node has exactly one child AND is not an end-of-word.");
            Console.WriteLine("• Stop when the words diverge or when a shorter stored word is reached.");
            Console.WriteLine($"• Result: \"{narrowLcp}\"  (walk stops at 'r' because it is an end-of-word for \"car\").");

            Console.WriteLine("\nOperation Performed (CountWords):");
            Console.WriteLine("• Depth-first walk summing every node whose IsEndOfWord = true.");
            Console.WriteLine($"• Result: {narrowCount}");

            // Also run against the full demo trie so the reader sees the "diverges at the root" case for LCP.
            var fullRoot = BuildDemoTrie();
            var fullLcp = TrieOperations.LongestCommonPrefix(fullRoot);
            var fullCount = TrieOperations.CountWords(fullRoot);

            Console.WriteLine($"\nAgainst the full demo vocabulary [{string.Join(", ", DemoVocabulary)}]:");
            Console.WriteLine($"• LongestCommonPrefix = \"{fullLcp}\"  (empty — words split at the root between 'c' and 'd').");
            Console.WriteLine($"• CountWords          = {fullCount}");
        }

        /// <summary>
        /// Prints the Trie in a sideways structure. Each node is indented by its depth, labeled by the character that leads to it, and suffixed with '*' if it marks the end of a stored word.
        /// </summary>
        /// <param name="root"> The root of the Trie. </param>
        private static void PrintTrie(TrieNode root)
        {
            if (root is null)
            {
                Console.WriteLine("Trie is empty.");
                return;
            }

            Console.WriteLine("[root]");
            PrintTrie(root, 1);
        }

        /// <summary>
        /// Recursive helper for <see cref="PrintTrie(TrieNode)"/>. Children are printed in alphabetical order so the output is deterministic and easy to eyeball between runs.
        /// </summary>
        /// <param name="node"> The current node. </param>
        /// <param name="depth"> The current depth from the root (used for indentation). </param>
        private static void PrintTrie(TrieNode node, int depth)
        {
            const int IndentSize = 4;

            foreach (var pair in node.Children.OrderBy(kvp => kvp.Key))
            {
                var indent = new string(' ', depth * IndentSize);
                var flag = pair.Value.IsEndOfWord ? "*" : "";
                Console.WriteLine($"{indent}{pair.Key}{flag}");
                PrintTrie(pair.Value, depth + 1);
            }
        }
    }
}
