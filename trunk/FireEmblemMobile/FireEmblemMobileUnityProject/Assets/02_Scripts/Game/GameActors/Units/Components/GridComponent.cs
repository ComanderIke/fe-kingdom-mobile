using System;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    public class GridComponent
    {
        public GridPosition GridPosition { get; set; }

        public virtual void ResetPosition()
        {
            if(previousTile!=null)
                Tile = previousTile;
        }

        public GridComponent()
        {
            GridPosition = new GridPosition(-1,-1);
        }
        private Tile tile;
        public Tile Tile
        {
            get
            {
                return tile;
            }
            set
            {
                if (previousTile == null)
                    previousTile = value;
                else
                {
                    previousTile = tile;
                }
                tile = value;
            }
        }

        protected Tile previousTile;

        public virtual void SetPosition(int x, int y)
        {
            GridPosition.SetPosition(x, y);
        }
        public virtual void SetInternPosition( int x, int y)
        {
            //previousTile = Tile;
            Debug.Log("SetInternPosition: "+ x +" "+y);
            GridPosition.SetPosition(x, y);
            //gridActor.GameTransformManager.SetPosition(x, y);
        }

        public bool IsInRange(GridComponent gridComponent, int range)
        {
            return Math.Abs(GridPosition.X - gridComponent.GridPosition.X) + Math.Abs(GridPosition.Y -
                gridComponent.GridPosition.Y) <=range;

        }

       
    }
}