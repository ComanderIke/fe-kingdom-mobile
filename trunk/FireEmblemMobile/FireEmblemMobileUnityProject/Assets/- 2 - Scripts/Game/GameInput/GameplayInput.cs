﻿using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.Grid;
using Game.Mechanics;

namespace Game.GameInput
{
    public class GameplayInput : IGameInput
    {
        public delegate void Event();
        public static event Event OnDeselectUnit;
        public static event Event OnExecuteInputActions;
        public delegate void UnitEvent(Unit u);
        public static event UnitEvent OnSelectUnit;
        public static event UnitEvent OnWait;
        public static event UnitEvent OnViewUnit;
        public delegate void MoveUnitEvent(Unit u, GridPosition position, List<GridPosition> movePath);
        public static event MoveUnitEvent OnMoveUnit;
        public delegate void AttackUnitEvent(Unit u, Unit attackTarget);
        public static event AttackUnitEvent OnAttackUnit;
        public delegate void CheckAttackPreviewEvent(IBattleActor u, IBattleActor attackTarget, GridPosition attackPosition);
        public static event CheckAttackPreviewEvent OnCheckAttackPreview;
        public void AttackUnit(IBattleActor u, IBattleActor attackTarget)
        {
            //Debug.Log("GameInput: Attack Unit: " + u.name+" Target: " +attackTarget.name);
            OnAttackUnit?.Invoke(u, attackTarget);
        }
        public void DeselectUnit()
        {
            //Debug.Log("GameInput: Deselect Unit");
            OnDeselectUnit?.Invoke();
        }

        public void MoveUnit(IGridActor u, GridPosition position, List<GridPosition> movePath)
        {
            //Debug.Log("GameInput: Move Unit: " + u.name +" to [" +position.X +"/"+position.Y+"]");
            OnMoveUnit?.Invoke(u, position, movePath);
        }

        public void SelectUnit(IGridActor u)
        {
            //Debug.Log("GameInput: Select Unit: " + u.name);
            OnSelectUnit?.Invoke(u);
        }
        public void ViewUnit(Unit u)
        {
            //Debug.Log("GameInput: View Unit: " + u.name);
            OnViewUnit?.Invoke(u);
        }
        public void Wait(IGridActor u)
        {
            //Debug.Log("GameInput: Wait Unit: " + u.name);
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
    }
}