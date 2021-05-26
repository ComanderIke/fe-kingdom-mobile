namespace Game.Grid.GridPathFinding
{
    public class NodeHelper
    {
        private readonly int width;
        private readonly int height;

        public NodeHelper(int width, int height)
        {
            this.width = width;
            this.height = height;
            Nodes = new Node[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Nodes[i, j] = new Node(i, j, 1000);
                }
            }
        }

        public Node[,] Nodes;

        public bool NodeFaster(int x, int y, int c)
        {
            if (Nodes[x, y].C < c)
                return false;
            return true;
        }

        public void Reset()
        {
            Nodes = new Node[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Nodes[i, j] = new Node(i, j, 1000);
                }
            }
        }
    }
}