using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameInput
{
    public class GameplayCommands : IGameInput
    {
        public delegate void Event();
 
        public static event Event OnExecuteInputActions;
        public delegate void UnitEvent(IGridActor u);
        public static event UnitEvent OnDeselectUnit;
        public delegate void SelectEvent(IGridActor actor);
        public static event SelectEvent OnSelectUnit;
        public static event SelectEvent OnWait;
        public static event Event OnUndoUnit;
        public static event UnitEvent OnViewUnit;
        public delegate void MoveUnitEvent(IGridActor u, GridPosition position, List<GridPosition> movePath);
        public static event MoveUnitEvent OnMoveUnit;
        public delegate void AttackUnitEvent(IBattleActor u, IAttackableTarget attackTarget);
        public static event AttackUnitEvent OnAttackUnit;
        public delegate void CheckAttackPreviewEvent(IBattleActor u, IAttackableTarget attackTarget, GridPosition attackPosition);
        public static event CheckAttackPreviewEvent OnCheckAttackPreview;

        public static event Action<Skill> OnSelectSkill;
        public static event Action<Item> OnSelectItem;

        public void AttackUnit(IBattleActor u, IAttackableTarget attackTarget)
        {
            Debug.Log("GameInput: Attack Unit: " + u+" Target: " +attackTarget);
            OnAttackUnit?.Invoke(u, attackTarget);
        }
        public void DeselectUnit(IGridActor unit)
        {
            Debug.Log("GameInput: Deselect Unit");
            if(unit!=null)
                OnDeselectUnit?.Invoke(unit);
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
            Debug.Log("GameInput: View Unit: " + u.name);
            OnViewUnit?.Invoke(u);
        }
        public void Wait(IGridActor u)
        {
            Debug.Log("GameInput: Wait Unit: " + u);
            OnWait?.Invoke(u);
        }
        public void SelectItem(Item item)
        {
            Debug.Log("SelectItem");
            OnSelectItem(item);
        }
        public void DeselectItem()
        {
            Debug.Log("DeSelectItem");
            OnDeselectItem();
        }
        public void SelectSkill(Skill s)
        {
            Debug.Log("SelectSkill");
            OnSelectSkill(s);
        }
        public void DeselectSkill()
        {
            Debug.Log("DeSelectSkill");
            OnDeselectSkill();
        }
        public void UseItem(ItemBP i)
        {
            throw new NotImplementedException();
        }
        public void ExecuteInputActions(Action invokeAfter)
        {
            UnitActionSystem.OnAllCommandsFinished += invokeAfter;
            OnExecuteInputActions?.Invoke();
        }

        public void CheckAttackPreview(IBattleActor u, IAttackableTarget attackTarget, GridPosition attackPosition)
        {
            OnCheckAttackPreview?.Invoke(u, attackTarget, attackPosition);
        }

        public void UndoUnit(IGridActor unit)
        {
            OnUndoUnit?.Invoke();
        }


        public static event Action OnDeselectItem;
        public static event Action OnDeselectSkill;
    }
}