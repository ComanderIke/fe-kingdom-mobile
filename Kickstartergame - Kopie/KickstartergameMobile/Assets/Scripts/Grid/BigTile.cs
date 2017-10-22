﻿using System;
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
        public override bool Equals(object obj)
        {
            if(obj is BigTile)
            {
                BigTile bt = (BigTile)obj;
                return bt.BottomLeft() == BottomLeft() && bt.BottomRight() == BottomRight() && bt.TopLeft() == TopLeft() && bt.TopRight() == TopRight();
            }
            return base.Equals(obj);
        }
    }
}
