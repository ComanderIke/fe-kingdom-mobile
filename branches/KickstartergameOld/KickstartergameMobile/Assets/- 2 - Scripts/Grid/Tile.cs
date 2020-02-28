using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    [System.Serializable]
    public class Tile
    {
        public GameObject gameObject;
        public Unit character;
        public int movementCost = 1;
        public int x;
        public int y;
        public bool isActive = false;
        public bool isAccessible = true;

        public Tile(int i, int j, GameObject gameObject)
        {
            this.gameObject = gameObject;
            x = i;
            y = j;
        }

    }
}
