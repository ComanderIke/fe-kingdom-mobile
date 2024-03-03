using System;
using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Items;
using Game.GameActors.Units.Interfaces;
using Game.Grid;

namespace Game.GameInput.Interfaces
{
    public interface IGameInput
    {
         void SelectUnit(IGridActor u);
        void MoveUnit(IGridActor u, GridPosition position, List<GridPosition> path);
        void CheckAttackPreview(IBattleActor u, IAttackableTarget attackTarget, GridPosition attackPosition);
        void AttackUnit(IBattleActor u, IAttackableTarget attackTarget);
        void DeselectUnit(IGridActor u);
        void UseItem(ItemBP i);
    
        void ExecuteInputActions(Action after);
    
    }
}