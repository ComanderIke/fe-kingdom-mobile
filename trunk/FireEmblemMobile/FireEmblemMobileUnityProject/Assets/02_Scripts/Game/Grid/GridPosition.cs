using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class GridPosition
    {
        public int X;
        public int Y;
        
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public static List<GridPosition> GetFromVectorList(List<Vector2Int> movePath)
        {
            return movePath.Select(t => new GridPosition(t.x, t.y)).ToList();
        }

        public Vector2 AsVector()
        {
            return new Vector2(X, Y);
        }
    }
}