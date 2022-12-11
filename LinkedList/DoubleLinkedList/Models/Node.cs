namespace DoubleLinkedList
{
    public class Node
    {
        public Node Previous;
        public int Info;
        public Node Next;

        public Node(int info)
        {
            Previous = null;
            Info = info;
            Next = null;
        }
    }
}
