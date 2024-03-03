using System;
using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.GameInput.Interfaces;
using Game.Grid;
using Game.Systems;

namespace Game.GameInput.GameplayCommands
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
            MyDebug.LogLogic("Attack Unit: " + u+" Target: " +attackTarget);
            OnAttackUnit?.Invoke(u, attackTarget);
        }
        public void DeselectUnit(IGridActor unit)
        {
            // Debug.Log("GameInput: Deselect Unit");
            if(unit!=null)
                OnDeselectUnit?.Invoke(unit);
        }

        public void MoveUnit(IGridActor u, GridPosition position, List<GridPosition> movePath)
        {
            
            MyDebug.LogLogic("Move Unit: " + u +" to [" +position.X +"/"+position.Y+"]");
            OnMoveUnit?.Invoke(u, position, movePath);
        }

        public void SelectUnit(IGridActor u)
        {
            MyDebug.LogLogic("Select Unit: " + u);
            OnSelectUnit?.Invoke(u);
        }
        public void ViewUnit(Unit u)
        {
            MyDebug.LogLogic("View Unit: " + u.name);
            OnViewUnit?.Invoke(u);
        }
        public void Wait(IGridActor u)
        {
            MyDebug.LogLogic("Wait Unit: " + u+" "+u.GridComponent.GridPosition.AsVector());
            OnWait?.Invoke(u);
        }
        public void SelectItem(Item item)
        {
            MyDebug.LogLogic("SelectItem");
            OnSelectItem(item);
        }
        public void DeselectItem()
        {
            MyDebug.LogLogic("DeselectItem");
            OnDeselectItem();
        }
        public void SelectSkill(Skill s)
        {
            MyDebug.LogLogic("SelectSkill "+s.Name);
            OnSelectSkill(s);
        }
        public void DeselectSkill()
        {
            MyDebug.LogLogic("DeselectSkill");
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