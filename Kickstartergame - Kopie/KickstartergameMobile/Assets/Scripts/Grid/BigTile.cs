using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class BigTile
    {
        List<Vector2> positions;

        public BigTile(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft, Vector2 topRight)
        {
            positions = new List<Vector2>();
            positions[0] = bottomLeft;
            positions[1] = bottomRight;
            positions[2] = topLeft;
            positions[3] = topRight;
        }
        public Vector2 BottomLeft()
        {
            return positions[0];
        }
        public Vector2 BottomRight()
        {
            return positions[1];
        }
        public Vector2 ToplLeft()
        {
            return positions[2];
        }
        public Vector2 TopRight()
        {
            return positions[3];
        }
    }
}
