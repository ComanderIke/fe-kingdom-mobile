using System.Collections.Generic;
using Game.GameActors.Units.OnGameObject;
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
        MoveType MoveType { get; set; }
        GameTransformManager GameTransformManager { get; set; }
        TurnStateManager TurnStateManager { get; set; }
        Transform GetTransform();
        bool CanMoveOnTo(Tile tile);
        bool CanMoveThrough(IGridActor unit);
        bool IsEnemy(IGridActor unit);
        void SetAttackTarget(bool selected);
        Vector3 GetGameTransformPosition();
        void SetGameTransformPosition(int x, int y);
        void ResetPosition();
        bool CanAttackFrom(GridPosition attackFromPosition, GridPosition targetPosition);
        
        void SetPosition(int oldX, int oldY);

        bool CanAttack(int x, int y);
    }
}