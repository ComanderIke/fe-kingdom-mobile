namespace Assets.Grid.PathFinding
{
    [System.Serializable]
    public class Node
    {
        public int X;
        public int Y;
        public int C;
        public Node Parent;
        public int Cost;
        public int CostFromStart;
        public bool Check = false;
        public int Depth;

        public Node(int x, int y, int c)
        {
            X = x;
            Y = y;
            C = c;
        }

        public int SetParent(Node parent)
        {
            Depth = parent.Depth + 1;
            Parent = parent;
            return Depth;
        }
    }
}