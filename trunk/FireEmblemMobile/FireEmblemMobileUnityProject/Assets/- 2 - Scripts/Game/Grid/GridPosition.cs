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
        public int X;
        public int Y;
        private Unit character;
       // private GridSystem gridScript;


        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
           // gridScript = GridGameManager.Instance.GetSystem<GridSystem>();
        }

        public GridPosition(Unit character):this(-1, -1)
        {
            this.character = character;
        }

        public void SetPosition(int newX, int newY)
        {
            
           
            X = newX;
            Y = newY;
        }

        // public void RemoveCharacter()
        // {
        //     gridScript.Tiles[X, Y].Actor = null;
        // }

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