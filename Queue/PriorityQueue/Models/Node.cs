namespace PriorityQueue
{
    public class Node
    {
        public int Priority;
        public int Info;
        public Node Link;

        public Node(int info, int priority)
        {
            Priority = priority;
            Info = info;
            Link = null;
        }
    }
}
