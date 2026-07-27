namespace DataStructures.Trees.BinarySearchTree
{
    /// <summary>
    /// Represents a node in a binary search tree.
    /// </summary>
    internal class Node
    {
        public Node LeftChild { get; set; }

        public int Info { get; set; }

        public Node RightChild { get; set; }

        public Node(int info)
        {
            LeftChild = null;
            Info = info;
            RightChild = null;
        }
    }
}
