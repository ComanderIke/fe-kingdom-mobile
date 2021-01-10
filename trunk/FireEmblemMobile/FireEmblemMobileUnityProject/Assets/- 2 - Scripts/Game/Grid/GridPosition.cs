using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class GridPosition
    {
        public int X = -1;
        public int Y = -1;
        private Unit character;
        private GridSystem gridScript;

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
            character = null;
            gridScript = GridGameManager.Instance.GetSystem<GridSystem>();
        }

        public GridPosition(Unit character)
        {
            this.character = character;
        }

        public void SetPosition(int newX, int newY)
        {
            if (X != -1 && Y != -1)
                gridScript.Tiles[X, Y].Actor = null;
            gridScript.Tiles[newX, newY].Actor = character;
            X = newX;
            Y = newY;
        }

        public void RemoveCharacter()
        {
            gridScript.Tiles[X, Y].Actor = null;
        }

        public static List<GridPosition> GetFromVectorList(List<Vector2> movePath)
        {
            var ret = new List<GridPosition>();
            for (int i = 0; i < movePath.Count; i++)
            {
                ret.Add(new GridPosition((int) movePath[i].x, (int) movePath[i].y));
            }
            return ret;
        }
    }
}