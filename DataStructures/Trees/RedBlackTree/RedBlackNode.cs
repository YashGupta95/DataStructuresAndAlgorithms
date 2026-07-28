namespace DataStructures.Trees.RedBlackTree
{
    /// <summary>
    /// Represents the color of a node in a Red-Black Tree.
    /// </summary>
    public enum NodeColor
    {
        Red,
        Black
    }

    public class RedBlackNode
    {
        public int Value { get; set; }

        public RedBlackNode? Left { get; set; }

        public RedBlackNode? Right { get; set; }

        public RedBlackNode? Parent { get; set; }

        public NodeColor Color { get; set; }

        public RedBlackNode(int value)
        {
            Value = value;
            Color = NodeColor.Red; // Newly inserted nodes are always colored red to preserve the black-height invariant.
        }
    }
}
