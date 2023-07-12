namespace SeparateChaining
{
    internal class Node
    {
        public StudentRecord Info;
        public Node Link;

        public Node(StudentRecord studentRecord)
        {
            Info = studentRecord;
            Link = null;
        }
    }
}
