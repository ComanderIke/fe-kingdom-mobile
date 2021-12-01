using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameInput
{
    public class GameplayInput : IGameInput
    {
        public delegate void Event();
        public static event Event OnDeselectUnit;
        public static event Event OnExecuteInputActions;
        public delegate void UnitEvent(IGridActor u);
        public delegate void SelectEvent(IGridActor actor);
        public static event SelectEvent OnSelectUnit;
        public static event SelectEvent OnWait;
        public static event Event OnUndoUnit;
        public static event UnitEvent OnViewUnit;
        public delegate void MoveUnitEvent(IGridActor u, GridPosition position, List<GridPosition> movePath);
        public static event MoveUnitEvent OnMoveUnit;
        public delegate void AttackUnitEvent(IBattleActor u, IBattleActor attackTarget);
        public static event AttackUnitEvent OnAttackUnit;
        public delegate void CheckAttackPreviewEvent(IBattleActor u, IBattleActor attackTarget, GridPosition attackPosition);
        public static event CheckAttackPreviewEvent OnCheckAttackPreview;
        public void AttackUnit(IBattleActor u, IBattleActor attackTarget)
        {
            Debug.Log("GameInput: Attack Unit: " + u+" Target: " +attackTarget);
            OnAttackUnit?.Invoke(u, attackTarget);
        }
        public void DeselectUnit()
        {
            Debug.Log("GameInput: Deselect Unit");
            OnDeselectUnit?.Invoke();
        }

        public void MoveUnit(IGridActor u, GridPosition position, List<GridPosition> movePath)
        {
            
            Debug.Log("GameInput: Move Unit: " + u +" to [" +position.X +"/"+position.Y+"]");
            OnMoveUnit?.Invoke(u, position, movePath);
        }

        public void SelectUnit(IGridActor u)
        {
            Debug.Log("GameInput: Select Unit: " + u);
            OnSelectUnit?.Invoke(u);
        }
        public void ViewUnit(Unit u)
        {
            Debug.Log("GameInput: View Unit: " + ((Object) u).name);
            OnViewUnit?.Invoke(u);
        }
        public void Wait(IGridActor u)
        {
            Debug.Log("GameInput: Wait Unit: " + u);
            OnWait?.Invoke(u);
        }

        public void UseItem(Item i)
        {
            throw new NotImplementedException();
        }
        public void ExecuteInputActions(Action invokeAfter)
        {
            UnitActionSystem.OnAllCommandsFinished += invokeAfter;
            OnExecuteInputActions?.Invoke();
        }

        public void CheckAttackPreview(IBattleActor u, IBattleActor attackTarget, GridPosition attackPosition)
        {
            OnCheckAttackPreview?.Invoke(u, attackTarget, attackPosition);
        }

        public void UndoUnit(IGridActor unit)
        {
            OnUndoUnit?.Invoke();
        }
    }
}