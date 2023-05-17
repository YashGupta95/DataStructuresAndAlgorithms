namespace BinaryTreeSort
{
    internal class BinarySearchTree
    {
        private TreeNode root;
        private static int k;

        public BinarySearchTree()
        {
            root = null;
        }

        internal void Insert(int element)
        {
            root = Insert(root, element);
        }

        private static TreeNode Insert(TreeNode node, int element)
        {
            if (node == null)
                node = new TreeNode(element);
            else if (element < node.Info)
                node.LeftChild = Insert(node.LeftChild, element);
            else
                node.RightChild = Insert(node.RightChild, element);

            return node;
        }

        internal void Inorder(int[] arr)
        {
            k = 0;  
            Inorder(root, arr);
        }

        private static void Inorder(TreeNode node, int[] arr)
        {
            if (node == null)
                return;

            Inorder(node.LeftChild, arr);
            arr[k++] = node.Info; //// Construct the Inorder sequence of elements & store it in array
            Inorder(node.RightChild, arr);
        }
    }
}
