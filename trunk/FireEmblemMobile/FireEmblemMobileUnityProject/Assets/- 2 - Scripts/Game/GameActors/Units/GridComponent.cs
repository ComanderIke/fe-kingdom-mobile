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
       
        public IGridActor gridActor;
        public GridPosition GridPosition { get; set; }

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

        private Tile previousTile;

        public void ResetPosition()
        {
           // Debug.Log("GridActor: "+gridActor);
            if(previousTile!=null)
                 Tile = previousTile;
            if(gridActor.IsAlive())
                gridActor.GameTransformManager.SetPosition(GridPosition.X, GridPosition.Y);
            // GameTransform.EnableCollider();
        }
        public virtual void SetPosition( int x, int y)
        {
            //previousTile = Tile;
            GridPosition.SetPosition(x, y);
            gridActor.GameTransformManager.SetPosition(x, y);
        }
        public virtual void SetInternPosition( int x, int y)
        {
            //previousTile = Tile;
            Debug.Log("SetInternPosition: "+ x +" "+y);
            GridPosition.SetPosition(x, y);
            //gridActor.GameTransformManager.SetPosition(x, y);
        }


  
        public GridComponent(IGridActor actor)
        {
            gridActor = actor;
            GridPosition = new GridPosition(-1,-1);

        }
       

        public bool CanMoveOnTo(Tile field)
        {
            //Debug.Log("Can move on to: "+field.X+ " "+field.Y+ " "+field.TileData.name+" "+field.TileData.CanMoveThrough(gridActor.MoveType) );
            return field.TileData.CanMoveThrough(gridActor.MoveType);
        }
        public bool CanMoveThrough(IGridActor unit)
        {
            return gridActor.Faction.Id != unit.Faction.Id;
        }
        public bool CanAttack(int range)
        {
            return gridActor.AttackRanges.Contains(range);
        }
        public bool CanAttack(int x, int y)
        {
            Debug.Log("Can attack: "+x+ " "+y+ " from " + GridPosition.X+" "+GridPosition.Y);
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