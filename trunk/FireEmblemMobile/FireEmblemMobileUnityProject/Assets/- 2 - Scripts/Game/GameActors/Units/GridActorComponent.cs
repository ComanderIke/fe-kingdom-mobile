using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.OnGameObject;
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

    }
    public class GridActorComponent:GridComponent
    {
       
        public IGridActor gridActor;

        public override void ResetPosition()
        {
            base.ResetPosition();
            if(gridActor.IsAlive())
                gridActor.GameTransformManager.SetPosition(GridPosition.X, GridPosition.Y);
        }
        public override void SetPosition( int x, int y)
        {
            //previousTile = Tile;
            base.SetPosition(x, y);
            gridActor.GameTransformManager.SetPosition(x, y);
        }
        
        public GridActorComponent(IGridActor actor):base()
        {
            gridActor = actor;
        }
       

        public bool CanMoveOnTo(Tile field)
        {
            //Debug.Log("Can move on to: "+field.X+ " "+field.Y+ " "+field.TileData.name+" "+field.TileData.CanMoveThrough(gridActor.MoveType) );
            return field.TileData.CanMoveThrough(gridActor.MoveType);
        }
        public bool CanAttack(int range)
        {
            return gridActor.AttackRanges.Contains(range);
        }
        public bool CanAttack(int x, int y)
        {
            return gridActor.AttackRanges.Contains(DeltaPos(x, y));
        }
        public bool CanAttackFrom(GridPosition attackFromPosition, GridPosition targetPosition)
        {
            return gridActor.AttackRanges.Contains(DeltaPos(attackFromPosition.X, attackFromPosition.Y, targetPosition.X, targetPosition.Y));
        }
        private int DeltaPos(int x, int y)
        {
            return Math.Abs(GridPosition.X - x) + Math.Abs(GridPosition.Y - y);
        }
        private int DeltaPos(int x, int y, int x2, int y2)
        {
            return Math.Abs(x - x2) + Math.Abs(y - y2);
        }
    }
}