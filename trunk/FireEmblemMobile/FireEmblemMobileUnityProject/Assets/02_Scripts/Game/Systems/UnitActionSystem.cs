using System;
using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameInput.GameplayCommands;
using Game.Grid;
using Game.Manager;
using Game.States.Mechanics.Battle;
using Game.States.Mechanics.Commands;
using GameEngine;
using UnityEngine;

namespace Game.Systems
{
    public class UnitActionSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public static event Action OnCommandFinished;

        public static event Action OnAllCommandsFinished;

        public static event Action OnUndo;

        public delegate void OnCheckAttackPreviewEvent(BattlePreview battlePreview);
        public static event OnCheckAttackPreviewEvent OnCheckAttackPreview;
        public static event OnCheckAttackPreviewEvent OnUpdateAttackPreview;

        #endregion

        private Stack<Command> lastActions;
        private Stack<Command> lastCharacterActions;
        private Queue<Command> currentActions;

        private Command currentCommand;
        private IGridActor lastCharacter;
        public void Init()
        {
         
        }

        public void Deactivate()
        {
            GameplayCommands.OnUndoUnit -= Undo;
            GameplayCommands.OnWait -= Wait;
            GameplayCommands.OnAttackUnit -= Fight;
            GameplayCommands.OnMoveUnit -= MoveCharacter;
            GameplayCommands.OnCheckAttackPreview -= CheckAttackPreview;
            GameplayCommands.OnExecuteInputActions -= ExecuteActions;
            GridGameManager.Instance.GetSystem<TurnSystem>().OnEndTurn -= ResetCharacterActions;
            OnCommandFinished -= ExecuteActions;
        }

        public void Activate()
        {
            GameplayCommands.OnUndoUnit += Undo;
            GameplayCommands.OnWait += Wait;
            GameplayCommands.OnAttackUnit += Fight;
            GameplayCommands.OnMoveUnit += MoveCharacter;
            GameplayCommands.OnCheckAttackPreview += CheckAttackPreview;
            GameplayCommands.OnExecuteInputActions += ExecuteActions;
            GridGameManager.Instance.GetSystem<TurnSystem>().OnEndTurn += ResetCharacterActions;
            lastActions = new Stack<Command>();
            lastCharacterActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            OnCommandFinished += ExecuteActions;

        }
        private void ResetCharacterActions()
        {
            //Debug.Log("Clear Actions: "+lastCharacterActions.Count);
            // foreach (var action in lastCharacterActions)
            // {
            //     Debug.Log("Clear Action: "+action);
            // }
            lastCharacterActions.Clear();
        }
       
        public void Update()
        {
            if (currentCommand != null)
            {
                currentCommand.Update();
                if (currentCommand.IsFinished)
                {
                    OnCommandFinished();
                }
            }
        }

        public void Undo()
        {
            OnUndo?.Invoke();
            Debug.Log("Undo: " + lastCharacterActions.Count);
           // Debug.Log("Undo only recent character related Actions!!!");
           // Debug.Log("Undo attackpreview movement also");
            while (lastCharacterActions.Count != 0)
            {
                lastCharacterActions.Pop().Undo();
                lastActions.Pop();
            }
            
        }

   
        public void AddCommand(Command c)
        {
            currentActions.Enqueue(c);
        }

        public void ExecuteActions()
        {
            if (currentActions.Count != 0)
            {
                currentCommand = currentActions.Dequeue();
                if (currentActions.Count == 0)
                {
                    OnCommandFinished = null;
                    OnCommandFinished += ExecuteActions;
                    OnCommandFinished += AllCommandsFinished;
                }
                currentCommand.Execute();
                lastActions.Push(currentCommand);
                lastCharacterActions.Push(currentCommand);
            }
            else
            {
                currentCommand = null;
            }
        }

        private void AllCommandsFinished()
        {
            OnCommandFinished -= AllCommandsFinished;
            OnAllCommandsFinished?.Invoke();
            OnAllCommandsFinished = null;
        }

        #region GameplayCommands

        public void Wait(IGridActor unit)
        {
            if(unit!=lastCharacter)
                ResetCharacterActions();
            var mCc = new WaitCommand(unit);
            lastCharacter = unit;
            currentActions.Enqueue(mCc);
        }
        public void Fight(IBattleActor attacker, IAttackableTarget target)
        {
            Debug.Log("FIGHT ACTION");
            if((IGridActor)attacker!=lastCharacter)
                ResetCharacterActions();
            //ResetCharacterActions();
            lastCharacter = (IGridActor)attacker;
            var mCc = new AttackCommand(attacker, target);
            currentActions.Enqueue(mCc);
        }
        public void UpdateAttackpreview()
        {
            if (currentBattleActor != null && currentAttackedTarget != null&& currentAttackPosition!=null)
            {
                var preview = GridGameManager.Instance.GetSystem<BattleSystem>()
                    .GetBattlePreview(currentBattleActor, currentAttackedTarget, currentAttackPosition);
                OnUpdateAttackPreview?.Invoke(preview);
            }
        }

        private IBattleActor currentBattleActor;
        private IAttackableTarget currentAttackedTarget;
        private GridPosition currentAttackPosition;
        public void CheckAttackPreview(IBattleActor u, IAttackableTarget target, GridPosition attackPosition)
        {
            this.currentAttackPosition = attackPosition;
            this.currentBattleActor = u;
            this.currentAttackedTarget = target;
            MyDebug.LogTODO("Could be Future bug here?");
            var tiles = ServiceProvider.Instance.GetSystem<GridSystem>().Tiles;
            ((Unit) u).SetInternGridPosition(tiles[attackPosition.X, attackPosition.Y]);
           
            var preview = GridGameManager.Instance.GetSystem<BattleSystem>().GetBattlePreview(u, target, attackPosition);
          
            OnCheckAttackPreview?.Invoke(preview);
        }
        public void MoveCharacter(IGridActor c, GridPosition destination, List<GridPosition> path = null)
        {
            //ResetCharacterActions();
            if(c!=lastCharacter)
                ResetCharacterActions();
            lastCharacter = c;
            var mCc = new MoveCharacterCommand(c, new Vector2Int(destination.X, destination.Y), path);
            currentActions.Enqueue(mCc);
        }
        #endregion

        private void OnDisable()
        {
            Deactivate();
            OnCommandFinished = null;
            OnAllCommandsFinished = null;
            OnUndo = null;
        }

       
    }
}