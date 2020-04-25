using UnityEngine;
using UnityEditor;
using Assets.GameActors.Units;
using Assets.Grid;
using Assets.GameActors.Items;
using System.Collections.Generic;
using System;
using Assets.Mechanics;

public class GameplayInput : IGameInput
{
    public delegate void Event();
    public static Event OnDeselectUnit;
    public static Event OnExecuteInputActions;
    public delegate void UnitEvent(Unit u);
    public static UnitEvent OnSelectUnit;
    public static UnitEvent OnWait;
    public delegate void MoveUnitEvent(Unit u, GridPosition position, List<GridPosition> movePath);
    public static MoveUnitEvent OnMoveUnit;
    public delegate void AttackUnitEvent(Unit u, Unit attackTarget);
    public static AttackUnitEvent OnAttackUnit;
    public delegate void CheckAttackPreviewEvent(Unit u, Unit attackTarget, GridPosition attackPosition);
    public static CheckAttackPreviewEvent OnCheckAttackPreview;
    public void AttackUnit(Unit u, Unit attackTarget)
    {
        OnAttackUnit?.Invoke(u, attackTarget);
    }
    public void DeselectUnit()
    {
        OnDeselectUnit?.Invoke();
    }

    public void MoveUnit(Unit u, GridPosition position, List<GridPosition> movePath)
    {
        OnMoveUnit?.Invoke(u, position, movePath);
    }

    public void SelectUnit(Unit u)
    {
        OnSelectUnit?.Invoke(u);
    }
    public void Wait(Unit u)
    {
        OnWait?.Invoke(u);
    }

    public void UseItem(Item i)
    {
        throw new System.NotImplementedException();
    }
    public void ExecuteInputActions(Action invokeAfter)
    {
        UnitActionSystem.OnAllCommandsFinished += invokeAfter;
        OnExecuteInputActions?.Invoke();
    }

    public void CheckAttackPreview(Unit u, Unit attackTarget, GridPosition attackPosition)
    {
        OnCheckAttackPreview?.Invoke(u, attackTarget, attackPosition);
    }
}