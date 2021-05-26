using System.Collections.Generic;

namespace Game.Grid.GridPathFinding
{
    public class Node2X2
    {
        public BigTile Position { get; set; }
        public int C;
        public Node2X2 Parent;
        public int Cost;
        public int CostFromStart;
        public bool Check = false;
        public int Depth;
        public Node2X2(BigTile pos, int c)
        {
            Position = pos;
            C = c;
        }
        public int SetParent(Node2X2 parent)
        {
            Depth = parent.Depth + 1;
            Parent = parent;
            return Depth;
        }
        public override bool Equals(object obj)
        {
            return obj is Node2X2 node2X2 ? node2X2.Position.Equals(Position) : base.Equals(obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 1085674238;
            hashCode = hashCode * -1521134295 + EqualityComparer<BigTile>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Node2X2>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + Cost.GetHashCode();
            hashCode = hashCode * -1521134295 + CostFromStart.GetHashCode();
            hashCode = hashCode * -1521134295 + Check.GetHashCode();
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            return hashCode;
        }
    }
}
