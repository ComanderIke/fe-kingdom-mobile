using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Grid
{
    public class Node2x2
    {
        public BigTile Position { get; set; }
        public int c;
        public Node2x2 parent;
        public int cost;
        public int costfromStart;
        public bool check = false;
        public int depth;
        public Node2x2(BigTile pos, int c)
        {
            this.Position = pos;
            this.c = c;
        }
        public int setParent(Node2x2 parent)
        {
            depth = parent.depth + 1;
            this.parent = parent;
            return depth;
        }
        public override bool Equals(object obj)
        {
            if(obj is Node2x2)
            {
                return ((Node2x2)obj).Position.Equals(Position);
            }
            return base.Equals(obj);
        }
    }
}
