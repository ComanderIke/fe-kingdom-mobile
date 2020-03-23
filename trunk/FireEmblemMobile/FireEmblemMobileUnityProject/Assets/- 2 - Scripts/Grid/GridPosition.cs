using Assets.Core;
using Assets.GameActors.Units;
using Assets.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Grid
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
        protected MapSystem GridScript;

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
                GridScript = GridGameManager.Instance.GetSystem<MapSystem>();
            if (X != -1 && Y != -1)
                GridScript.Tiles[X, Y].Unit = null;
            GridScript.Tiles[newX, newY].Unit = Character;
            X = newX;
            Y = newY;
        }

        protected int DeltaPos(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2);
        }

        public virtual void RemoveCharacter()
        {
            if (GridScript == null)
                GridScript = GridGameManager.Instance.GetSystem<MapSystem>();
            GridScript.Tiles[X, Y].Unit = null;
        }

        public int GetManhattanDistance(int x2, int y2)
        {
            return Math.Abs(X - x2) + Math.Abs(Y - y2);
        }

        public virtual bool CanAttack(List<int> range, GridPosition enemyPosition)
        {
            if (!(enemyPosition is BigTilePosition))
            {
                return range.Contains(DeltaPos(enemyPosition.X, enemyPosition.Y));
            }
            else
            {
                var bigTile = (BigTilePosition) enemyPosition;
                if (range.Contains(DeltaPos((int) bigTile.Position.BottomLeft().x,
                    (int) bigTile.Position.BottomLeft().y)))
                {
                    return true;
                }
                else if (range.Contains(DeltaPos((int) bigTile.Position.BottomRight().x,
                    (int) bigTile.Position.BottomRight().y)))
                {
                    return true;
                }
                else if (range.Contains(DeltaPos((int) bigTile.Position.TopRight().x,
                    (int) bigTile.Position.TopRight().y)))
                {
                    return true;
                }
                else if (range.Contains(
                    DeltaPos((int) bigTile.Position.TopLeft().x, (int) bigTile.Position.TopLeft().y)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}