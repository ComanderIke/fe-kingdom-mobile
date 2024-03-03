using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Grid.Tiles
{
    public class BigTile
    {
        private readonly List<Vector2> positions;

        public BigTile(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft, Vector2 topRight)
        {
            positions = new List<Vector2> {bottomLeft, bottomRight, topLeft, topRight};
        }

        public Vector2 BottomLeft()
        {
            return positions[0];
        }

        public Vector2 BottomRight()
        {
            return positions[1];
        }

        public Vector2 TopLeft()
        {
            return positions[2];
        }

        public Vector2 TopRight()
        {
            return positions[3];
        }

        public bool Contains(Vector2 pos)
        {
            return positions.Any(p => p == pos);
        }

        public Vector2 CenterPos()
        {
            return new Vector2(BottomLeft().x + 1f, BottomLeft().y + 1f);
        }

        public override bool Equals(object obj)
        {
            if (obj is BigTile bt)
            {
                return bt.BottomLeft() == BottomLeft() && bt.BottomRight() == BottomRight() &&
                       bt.TopLeft() == TopLeft() && bt.TopRight() == TopRight();
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return BottomLeft().x + " " + BottomLeft().y + " | " + BottomRight().x + " " + BottomRight().y + " | " +
                   TopLeft().x + " " + TopLeft().y + " | " + TopRight().x + " " + TopRight().y;
            ;
        }

        public override int GetHashCode()
        {
            return -1378504013 + EqualityComparer<List<Vector2>>.Default.GetHashCode(positions);
        }
    }
}