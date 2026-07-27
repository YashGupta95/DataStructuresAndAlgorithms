namespace DataStructures.Trees.BinaryTree
{
    public class BinaryTreeNode
    {
        public int Value { get; set; }

        public BinaryTreeNode Left { get; set; }

        public BinaryTreeNode Right { get; set; }

        public BinaryTreeNode(int value)
        {
            Left = null;
            Value = value;
            Right = null;
        }
    }
}
