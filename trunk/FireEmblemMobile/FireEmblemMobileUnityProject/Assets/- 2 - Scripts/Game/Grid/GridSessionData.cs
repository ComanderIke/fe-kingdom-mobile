using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Game.Grid
{
    public class GridSessionData
    {
        private List<Vector2Int> validPositions;
        private List<Vector2Int> validAttackPositions;

        public GridSessionData()
        {
            validPositions = new List<Vector2Int>();
            validAttackPositions= new List<Vector2Int>();
        }
        public bool IsMoveableAndActive(int x, int y)
        {
            return validPositions.Contains(new Vector2Int(x, y));
        }

        public bool IsAttackableAndActive(int x, int y)
        {
            return validAttackPositions.Contains(new Vector2Int(x, y));
        }

        public void AddValidPosition(int x, int y)
        {
            validPositions.Add(new Vector2Int(x,y));
        }
        public void AddValidAttackPosition(int x, int y)
        {
            validAttackPositions.Add(new Vector2Int(x,y));
        }

        public void Clear()
        {
            validPositions.Clear();
            validAttackPositions.Clear();
        }
    }
}