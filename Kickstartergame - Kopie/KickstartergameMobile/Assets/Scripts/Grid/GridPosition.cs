using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class GridPosition
    {

        public int x=-1;
        public int y=-1;

        protected LivingObject character;
        protected GridManager gridScript;

        public GridPosition(LivingObject character)
        {
            this.character = character;
            gridScript = MainScript.GetInstance().gridManager;
        }

        public virtual void SetPosition(int newX, int newY)
        {
            if(x!=-1&&y!=-1)
                gridScript.Tiles[x, y].character = null;
            gridScript.Tiles[newX, newY].character = character;
            x = newX;
            y = newY;
        }
    }
}
