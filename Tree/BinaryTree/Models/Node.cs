namespace BinaryTree
{
    internal class Node
    {
        public Node LeftChild;
        public char Info;
        public Node RightChild;

        public Node(char info)
        {
            LeftChild = null;
            Info = info;
            RightChild = null;
        }
    }
}
