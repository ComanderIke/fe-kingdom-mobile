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
            if (previousTile != null)
            {
                Tile = previousTile;
                OnTileChanged?.Invoke(Tile);
            }

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
                OnTileChanged?.Invoke(Tile);
            }
        }

        protected Tile previousTile;

        public virtual void SetPosition(int x, int y)
        {
            GridPosition.SetPosition(x, y);
           
        }
 

        public bool IsInRange(GridComponent gridComponent, int range)
        {
            return Math.Abs(GridPosition.X - gridComponent.GridPosition.X) + Math.Abs(GridPosition.Y -
                gridComponent.GridPosition.Y) <=range;

        }


        public event Action<Tile> OnTileChanged;
    }
}