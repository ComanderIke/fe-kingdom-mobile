using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;

namespace Game.GameActors.Units
{
    public class GridComponent
    {
       
        public IGridActor gridActor;
        public GridPosition GridPosition { get; set; }
                
        public int FactionId => gridActor.FactionId;
        public IEnumerable<int> AttackRanges => gridActor.AttackRanges;

        
        public void ResetPosition()
        {
            gridActor.GameTransformManager.SetPosition(GridPosition.X, GridPosition.Y);
            // GameTransform.EnableCollider();
        }
        public virtual void SetPosition(int x, int y)
        {
            GridPosition.SetPosition(x, y);
            gridActor.GameTransformManager.SetPosition(x, y);
        }

  
        public GridComponent(IGridActor actor)
        {
            gridActor = actor;
            GridPosition = new GridPosition(-1,-1);

        }
       

        public bool CanMoveOnTo(Tile field)
        {
            return field.TileType.CanMoveThrough(gridActor.MoveType);
        }
        public bool CanMoveThrough(IGridActor unit)
        {
            return FactionId != unit.FactionId;
        }
        public bool CanAttack(int range)
        {
            return gridActor.AttackRanges.Contains(range);
        }
        public bool CanAttack(int x, int y)
        {
            return AttackRanges.Contains(DeltaPos(x, y));
        }
        public bool CanAttackFrom(GridPosition attackFromPosition, GridPosition targetPosition)
        {
            return AttackRanges.Contains(DeltaPos(attackFromPosition.X, attackFromPosition.Y, targetPosition.X, targetPosition.Y));
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