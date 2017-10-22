using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Grid
{
    public class PathFindingNode
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Accesible { get; private set; }
        public PathFindingNode(int x, int y, bool accesible)
        {
            X = x;
            Y = y;
            Accesible = accesible;
        }
    }
}
