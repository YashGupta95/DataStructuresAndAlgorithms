namespace DataStructures.Trees.Trie
{
    /// <summary>
    /// Represents a single node in a Trie (prefix tree).
    ///
    /// <para>
    /// A Trie node holds only two pieces of information:
    /// <list type="bullet">
    /// <item> <description> <see cref="Children"/> — a map from "next character" to "next node". A missing key means that character has never been seen at this position. </description> </item>
    /// <item> <description> <see cref="IsEndOfWord"/> — <see langword="true"/> when the path from the root to this node spells out a stored word. A node may still be an internal waypoint for longer words when this flag is <see langword="false"/>. </description> </item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// The character that leads to a node is NOT stored on the node itself — it is the key in the parent's <see cref="Children"/> dictionary. Storing it on the node would be redundant.
    /// </para>
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// Maps each outgoing edge's character to the child node reached by that character.
        /// </summary>
        public Dictionary<char, TrieNode> Children { get; }

        /// <summary>
        /// <see langword="true"/> if the path from the root to this node spells a stored word.
        /// </summary>
        public bool IsEndOfWord { get; set; }

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
        }
    }
}
