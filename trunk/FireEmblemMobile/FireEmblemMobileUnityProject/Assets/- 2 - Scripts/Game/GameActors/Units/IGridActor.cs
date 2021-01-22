using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    public interface IGridActor
    {
        GridPosition GridPosition { get; }
        int MovementRage { get; }
        IEnumerable<int> AttackRanges { get; }
        int FactionId { get; }
        UnitTurnState UnitTurnState { get; }
        Transform GetTransform();
        bool CanMoveOnTo(Tile tile);
        bool CanMoveThrough(IGridActor unit);
        bool IsEnemy(IGridActor unit);
        
        bool CanAttack(int x, int y);
        Vector3 GetGameTransformPosition();
        void SetGameTransformPosition(int x, int y);
        void ResetPosition();
        bool CanAttackFrom(GridPosition attackFromPosition, GridPosition targetPosition);

        bool HasMoved();
        void SetPosition(int oldX, int oldY);
        
    }
}