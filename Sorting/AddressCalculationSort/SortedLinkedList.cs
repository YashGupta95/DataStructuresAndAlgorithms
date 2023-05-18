namespace AddressCalculationSort
{
    internal class SortedLinkedList
    {
        private Node start;

        public SortedLinkedList()
        {
            start = null;
        }

        internal void InsertInOrder(int data)
        {
            var tempNode = new Node(data);

            //// List empty or new node to be inserted before first node
            if (start == null || data < start.Info)
            {
                tempNode.Link = start;
                start = tempNode;
                return;
            }

            var node = start;
            while (node.Link != null && node.Link.Info <= data)
                node = node.Link;

            tempNode.Link = node.Link;
            node.Link = tempNode;
        }

        internal Node GetStart()
        {
            return start;
        }
    }
}
