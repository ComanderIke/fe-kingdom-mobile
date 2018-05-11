using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    class BigTilePosition : GridPosition
    {
        public BigTile Position { get; set; }

        public BigTilePosition(LivingObject o) : base(o)
        {

        }

        public override void SetPosition(int newX, int newY)
        {
            if (gridScript == null)
                gridScript = MainScript.GetInstance().GetSystem<GridSystem>();
            if (Position != null)
            {
                gridScript.Tiles[(int)Position.BottomLeft().x, (int)Position.BottomLeft().y].character = null;
                gridScript.Tiles[(int)Position.BottomRight().x, (int)Position.BottomRight().y].character = null;
                gridScript.Tiles[(int)Position.TopLeft().x, (int)Position.TopLeft().y].character = null;
                gridScript.Tiles[(int)Position.TopRight().x, (int)Position.TopRight().y].character = null;
            }
            x = newX;
            y = newY;
            Position = new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1));
            gridScript.Tiles[(int)Position.BottomLeft().x, (int)Position.BottomLeft().y].character = character;
            gridScript.Tiles[(int)Position.BottomRight().x, (int)Position.BottomRight().y].character = character;
            gridScript.Tiles[(int)Position.TopLeft().x, (int)Position.TopLeft().y].character = character;
            gridScript.Tiles[(int)Position.TopRight().x, (int)Position.TopRight().y].character = character;
        }
        public override void RemoveCharacter()
        {
            gridScript.Tiles[(int)Position.BottomLeft().x, (int)Position.BottomLeft().y].character = null;
            gridScript.Tiles[(int)Position.BottomRight().x, (int)Position.BottomRight().y].character = null;
            gridScript.Tiles[(int)Position.TopLeft().x, (int)Position.TopLeft().y].character = null;
            gridScript.Tiles[(int)Position.TopRight().x, (int)Position.TopRight().y].character = null;
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
