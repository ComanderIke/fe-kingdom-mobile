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
            positions.Add(bottomLeft);
            positions.Add(bottomRight);
            positions.Add(topLeft);
            positions.Add(topRight);
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
            foreach(Vector2 p in positions)
            {
                if (p == pos)
                {
                    return true;
                }
            }
            return false;
        }
        public Vector2 CenterPos()
        {
            return new Vector2(BottomLeft().x + 0.5f, BottomLeft().y + 0.5f);
        }
        public override bool Equals(object obj)
        {
            if(obj is BigTile)
            {
                BigTile bt = (BigTile)obj;
                return bt.BottomLeft() == BottomLeft() && bt.BottomRight() == BottomRight() && bt.TopLeft() == TopLeft() && bt.TopRight() == TopRight();
            }
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return BottomLeft().x + " " + BottomLeft().y + " | " + BottomRight().x + " " + BottomRight().y + " | " + TopLeft().x + " " + TopLeft().y + " | " + TopRight().x + " " + TopRight().y; ;
        }
    }
}
