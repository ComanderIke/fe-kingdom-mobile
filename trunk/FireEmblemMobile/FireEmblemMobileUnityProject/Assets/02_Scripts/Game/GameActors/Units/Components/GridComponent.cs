using System;
using Game.Grid;
using Game.Grid.Tiles;

namespace Game.GameActors.Units.Components
{
    public class GridComponent
    {
        public GridPosition GridPosition { get; set; }
      
        public virtual void ResetPosition()
        {
            if (previousTile != null)
            {
                Tile = previousTile;
                //OnTileChanged?.Invoke(Tile);
              
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
                bool changed = !(previousTile!=null&&previousTile.Equals(value));
                if (previousTile == null)
                    previousTile = value;
                else
                {
                    previousTile = tile;
                }
                
                tile = value;
                if (changed)
                {
                    OnTileChanged?.Invoke(Tile);
                    OnTileChangedStatic?.Invoke(Tile);
                }
                    
            }
        }

        public Tile OriginTile { get; set; }
        public int Canto { get; set; }

        protected Tile previousTile;

        public virtual void SetPosition(Tile tile, bool moveTransform=true)
        {
            GridPosition.SetPosition(tile.X, tile.Y);
            Tile = tile;
            OriginTile = Tile;
        }
        public virtual void SetInternPosition(Tile tile)
        {
           
            GridPosition.SetPosition(tile.X, tile.Y);
            Tile = tile;
        }

        public bool IsInRange(GridComponent gridComponent, int range)
        {
            return Math.Abs(GridPosition.X - gridComponent.GridPosition.X) + Math.Abs(GridPosition.Y -
                gridComponent.GridPosition.Y) <=range;

        }


        public event Action<Tile> OnTileChanged;
        public static event Action<Tile> OnTileChangedStatic;


        public void SetToOriginPosition()
        {
           SetPosition(OriginTile, false);
        }
    }
}