using Assets.Scripts.Grid;
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
    }
}
