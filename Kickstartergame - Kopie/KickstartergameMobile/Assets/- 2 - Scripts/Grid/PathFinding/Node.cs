using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Grid
{
    [System.Serializable]
    public class Node
    {

        public int x;
        public int y;
        public int c;
        public Node parent;
        public int cost;
        public int costfromStart;
        public bool check = false;
        public int depth;
        public Node(int x, int y, int c)
        {
            this.x = x;
            this.y = y;
            this.c = c;
        }
        public int setParent(Node parent)
        {
            depth = parent.depth + 1;
            this.parent = parent;
            return depth;
        }
    }
}
