using System.Collections.Generic;
using UnityEngine;


namespace Game.Grid
{
    public class GridSessionData
    {
        private List<Vector2Int> validPositions;
        private List<Vector2Int> validAttackPositions;
        private List<Vector2Int> validTargetPositions;
        private List<Vector2Int> savedValidPositions;
        private List<Vector2Int> savedValidAttackPositions;
        private List<Vector2Int> savedValidTargetPositions;

        public GridSessionData()
        {
            validPositions = new List<Vector2Int>();
            validTargetPositions = new List<Vector2Int>();
            validAttackPositions= new List<Vector2Int>();
            savedValidTargetPositions = new List<Vector2Int>();
            savedValidAttackPositions = new List<Vector2Int>();
            savedValidPositions = new List<Vector2Int>();
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

        public void AddValidTargetPosition(int x, int y)
        {
            validTargetPositions.Add(new Vector2Int(x,y));
        }
        public void AddValidTargetPosition(Vector2 v)
        {
            AddValidTargetPosition((int)v.x,(int)v.y);
        }

        public void Clear()
        {
            validPositions.Clear();
            validAttackPositions.Clear();
            validTargetPositions.Clear();
        }

        public void Save()
        {
            savedValidAttackPositions = new List<Vector2Int>(validAttackPositions);
            savedValidPositions = new List<Vector2Int>(validPositions);
            savedValidTargetPositions = new List<Vector2Int>(validTargetPositions);
        }
        public void Restore()
        {
            validPositions = new List<Vector2Int>(savedValidPositions);
            validAttackPositions = new List<Vector2Int>(savedValidAttackPositions);
            validTargetPositions = new List<Vector2Int>(savedValidTargetPositions);
        }

        public bool IsTargetable(int x, int y)
        {
            return validTargetPositions.Contains(new Vector2Int(x, y));
        }
    }
}