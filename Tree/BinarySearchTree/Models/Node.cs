namespace BinarySearchTree
{
    internal class Node
    {
        public Node LeftChild;
        public int Info;
        public Node RightChild;

        public Node(int info)
        {
            LeftChild = null;
            Info = info;
            RightChild = null;
        }
    }
}
