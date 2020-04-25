using UnityEngine;
using UnityEditor;
using Assets.GameActors.Units;
using Assets.Grid;
using Assets.GameActors.Items;
using System.Collections.Generic;
using System;

public interface IGameInput
{
    void SelectUnit(Unit u);
    void MoveUnit(Unit u, GridPosition position, List<GridPosition> path);
    void CheckAttackPreview(Unit u, Unit attackTarget, GridPosition attackPosition);
    void AttackUnit(Unit u, Unit attackTarget);
    void DeselectUnit();
    void UseItem(Item i);
    
    void ExecuteInputActions(Action after);
    
}