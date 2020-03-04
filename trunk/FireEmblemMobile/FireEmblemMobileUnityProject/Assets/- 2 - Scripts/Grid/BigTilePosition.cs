using Assets.Core;
using Assets.GameActors.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Grid
{
    internal class BigTilePosition : GridPosition
    {
        public BigTile Position { get; set; }

        public BigTilePosition(Unit o) : base(o)
        {
        }

        public override void SetPosition(int newX, int newY)
        {
            if (GridScript == null)
                GridScript = MainScript.Instance.GetSystem<Map.MapSystem>();
            if (Position != null)
            {
                GridScript.Tiles[(int) Position.BottomLeft().x, (int) Position.BottomLeft().y].Unit = null;
                GridScript.Tiles[(int) Position.BottomRight().x, (int) Position.BottomRight().y].Unit = null;
                GridScript.Tiles[(int) Position.TopLeft().x, (int) Position.TopLeft().y].Unit = null;
                GridScript.Tiles[(int) Position.TopRight().x, (int) Position.TopRight().y].Unit = null;
            }

            X = newX;
            Y = newY;
            Position = new BigTile(new Vector2(X, Y), new Vector2(X + 1, Y), new Vector2(X, Y + 1),
                new Vector2(X + 1, Y + 1));
            GridScript.Tiles[(int) Position.BottomLeft().x, (int) Position.BottomLeft().y].Unit = Character;
            GridScript.Tiles[(int) Position.BottomRight().x, (int) Position.BottomRight().y].Unit = Character;
            GridScript.Tiles[(int) Position.TopLeft().x, (int) Position.TopLeft().y].Unit = Character;
            GridScript.Tiles[(int) Position.TopRight().x, (int) Position.TopRight().y].Unit = Character;
        }

        public override void RemoveCharacter()
        {
            GridScript.Tiles[(int) Position.BottomLeft().x, (int) Position.BottomLeft().y].Unit = null;
            GridScript.Tiles[(int) Position.BottomRight().x, (int) Position.BottomRight().y].Unit = null;
            GridScript.Tiles[(int) Position.TopLeft().x, (int) Position.TopLeft().y].Unit = null;
            GridScript.Tiles[(int) Position.TopRight().x, (int) Position.TopRight().y].Unit = null;
        }

        public override bool CanAttack(List<int> range, GridPosition enemyPosition)
        {
            throw new NotImplementedException();
            /* if (!(enemyPosition is BigTilePosition))
             {
                 return range.Contains(DeltaPos(enemyPosition.x, enemyPosition.y));
             }
             else
             {
                 BigTilePosition bigTile = (BigTilePosition)enemyPosition;
                 if (range.Contains(DeltaPos((int)bigTile.Position.BottomLeft().x, (int)bigTile.Position.BottomLeft().y)))
                 {
                     return true;
                 }
                 else if (range.Contains(DeltaPos((int)bigTile.Position.BottomRight().x, (int)bigTile.Position.BottomRight().y)))
                 {
                     return true;
                 }
                 else if (range.Contains(DeltaPos((int)bigTile.Position.TopRight().x, (int)bigTile.Position.TopRight().y)))
                 {
                     return true;
                 }
                 else if (range.Contains(DeltaPos((int)bigTile.Position.TopLeft().x, (int)bigTile.Position.TopLeft().y)))
                 {
                     return true;
                 }
             }
             return false;*/
        }
    }
}