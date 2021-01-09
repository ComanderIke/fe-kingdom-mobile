using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.Grid;

namespace Game.GameInput
{
    public interface IGameInput
    {
        void SelectUnit(Unit u);
        void MoveUnit(Unit u, GridPosition position, List<GridPosition> path);
        void CheckAttackPreview(IBattleActor u, IBattleActor attackTarget, GridPosition attackPosition);
        void AttackUnit(Unit u, Unit attackTarget);
        void DeselectUnit();
        void UseItem(Item i);
    
        void ExecuteInputActions(Action after);
    
    }
}