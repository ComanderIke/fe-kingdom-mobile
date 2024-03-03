using System;
using System.Collections.Generic;
using Game.GameActors.Units.Interfaces;
using Game.Grid;
using Game.Grid.Tiles;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Components
{
    public class GridActorComponent:GridComponent
    {
       
        public IGridActor gridActor;
        public static event Action<IGridActor> AnyUnitChangedPosition;
        public static event Action<IGridActor> AnyUnitChangedPositionAfter;
      
        public Dictionary<TerrainType, int> BonusMovementCosts;
        public int MovedTileCount { get; set; }

        public int GetMovementCosts(TerrainType m)
        {
            return gridActor.MoveType.GetMovementCost(m)+ (BonusMovementCosts.ContainsKey(m)?BonusMovementCosts[m]:0);
        }
        public override void ResetPosition()
        {
            base.ResetPosition();
            if(gridActor.IsAlive())
                gridActor.GameTransformManager.SetPosition(GridPosition.X, GridPosition.Y);
            // Debug.Log("RESET MOVEDTILECOUNT");
            MovedTileCount = 0;
            AnyUnitChangedPosition?.Invoke(gridActor);
            AnyUnitChangedPositionAfter?.Invoke(gridActor);
            
        }
        public void ResetPosition(bool updateMovedTiles)
        {
            base.ResetPosition();
            if(gridActor.IsAlive())
                gridActor.GameTransformManager.SetPosition(GridPosition.X, GridPosition.Y);
            Debug.Log("RESET MOVEDTILECOUNT");
            if(updateMovedTiles)
                MovedTileCount = 0;
            AnyUnitChangedPosition?.Invoke(gridActor);
            AnyUnitChangedPositionAfter?.Invoke(gridActor);
            
        }
        public override void SetPosition( Tile tile, bool moveTransform=true)
        {
            //previousTile = Tile
            
            if (OriginTile != null)
            {
                MovedTileCount = DeltaPos(tile.X, tile.Y, OriginTile.X, OriginTile.Y);
                
            }

            base.SetPosition(tile, moveTransform);
            if (moveTransform)
            {
                // Debug.Log("also Move Transform to: "+tile.X+" "+tile.Y);
                gridActor.GameTransformManager.SetPosition(tile.X, tile.Y);
            }

            AnyUnitChangedPosition?.Invoke(gridActor);
            AnyUnitChangedPositionAfter?.Invoke(gridActor);
        }
        public override void SetInternPosition(Tile tile)
        {
            if (OriginTile != null)
            {
                MovedTileCount = DeltaPos(tile.X, tile.Y, OriginTile.X, OriginTile.Y);
               // Debug.Log("MOVEDTILEOCUNT: "+MovedTileCount);
            }

            base.SetInternPosition(tile);
            
            AnyUnitChangedPosition?.Invoke(gridActor);
            AnyUnitChangedPositionAfter?.Invoke(gridActor);
        }
        
        public GridActorComponent(IGridActor actor):base()
        {
            gridActor = actor;
            BonusMovementCosts = new Dictionary<TerrainType, int>();
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

        public void AddBonusMovementCosts(TerrainType key, int value)
        {
            if (BonusMovementCosts.ContainsKey(key))
            {
                BonusMovementCosts[key] += value;
            }
            else
            {
                BonusMovementCosts.Add(key,value);
            }
        }
        public void RemoveBonusMovementCosts(TerrainType key, int value)
        {
            if (BonusMovementCosts.ContainsKey(key))
            {
                BonusMovementCosts[key] -= value;
            }
            else
            {
                BonusMovementCosts.Add(key,-value);
            }
        }
    }
}