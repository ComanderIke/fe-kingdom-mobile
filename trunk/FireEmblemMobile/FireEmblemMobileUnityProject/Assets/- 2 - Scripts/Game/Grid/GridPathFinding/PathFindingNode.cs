namespace Game.Grid.GridPathFinding
{
    public class PathFindingNode
    {
        public int X { get; }
        public int Y { get; }
        public bool Accessible { get; }

        public PathFindingNode(int x, int y, bool accessible)
        {
            X = x;
            Y = y;
            Accessible = accessible;
        }
    }
}