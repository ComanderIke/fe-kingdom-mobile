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
        private bool facingLeft = false;

        public bool FacingLeft
        {
            get => facingLeft;
            set
            {
                facingLeft = value;
                Character.GameTransform.SetYRotation(facingLeft ? 180 : 0);
            }
        }

        protected Unit Character;
        protected GridSystem GridScript;

        public GridPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
            Character = null;
        }
        public GridPosition(Unit character)
        {
            this.Character = character;
        }

        public Vector2 GetPos()
        {
            return new Vector2(X, Y);
        }

        public virtual void SetPosition(int newX, int newY)
        {
            if (GridScript == null)
                GridScript = GridGameManager.Instance.GetSystem<GridSystem>();
            if (X != -1 && Y != -1)
                GridScript.Tiles[X, Y].Actor = null;
            GridScript.Tiles[newX, newY].Actor = Character;
            X = newX;
            Y = newY;
        }

      

        public virtual void RemoveCharacter()
        {
            if (GridScript == null)
                GridScript = GridGameManager.Instance.GetSystem<GridSystem>();
            GridScript.Tiles[X, Y].Actor = null;
        }

        public int GetManhattanDistance(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2);
        }

        

        public static List<GridPosition> GetFromVectorList(List<Vector2> movePath)
        {
            List<GridPosition> ret = new List<GridPosition>();
            for(int i=0; i < movePath.Count; i++)
            {
                ret.Add(new GridPosition((int)movePath[i].x, (int)movePath[i].y));
            }
            return ret;
        }
    }
}