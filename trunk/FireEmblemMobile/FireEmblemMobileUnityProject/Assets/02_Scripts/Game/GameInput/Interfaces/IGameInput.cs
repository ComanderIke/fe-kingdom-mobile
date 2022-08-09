using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Grid;

namespace Game.GameInput
{
    public interface IGameInput
    {
         void SelectUnit(IGridActor u);
        void MoveUnit(IGridActor u, GridPosition position, List<GridPosition> path);
        void CheckAttackPreview(IBattleActor u, IAttackableTarget attackTarget, GridPosition attackPosition);
        void AttackUnit(IBattleActor u, IAttackableTarget attackTarget);
        void DeselectUnit();
        void UseItem(Item i);
    
        void ExecuteInputActions(Action after);
    
    }
}