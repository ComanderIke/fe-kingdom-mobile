using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics.Battle;
using Game.Mechanics.Commands;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class UnitActionSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public static event Action OnCommandFinished;

        public static event Action OnAllCommandsFinished;

        public static event Action OnUndo;

        public delegate void OnCheckAttackPreviewEvent(BattlePreview battlePreview);
        public static event OnCheckAttackPreviewEvent OnCheckAttackPreview;

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
            GameplayInput.OnUndoUnit -= Undo;
            GameplayInput.OnWait -= Wait;
            GameplayInput.OnAttackUnit -= Fight;
            GameplayInput.OnMoveUnit -= MoveCharacter;
            GameplayInput.OnCheckAttackPreview -= CheckAttackPreview;
            GameplayInput.OnExecuteInputActions -= ExecuteActions;
            GridGameManager.Instance.GetSystem<TurnSystem>().OnEndTurn -= ResetCharacterActions;
            OnCommandFinished -= ExecuteActions;
        }

        public void Activate()
        {
            GameplayInput.OnUndoUnit += Undo;
            GameplayInput.OnWait += Wait;
            GameplayInput.OnAttackUnit += Fight;
            GameplayInput.OnMoveUnit += MoveCharacter;
            GameplayInput.OnCheckAttackPreview += CheckAttackPreview;
            GameplayInput.OnExecuteInputActions += ExecuteActions;
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
            Debug.Log("Undo: " + lastActions.Count);
            Debug.Log("Undo only recent character related Actions!!!");
            Debug.Log("Undo attackpreview movement also");
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
        public void Fight(IBattleActor attacker, IBattleActor target)
        {
            if((IGridActor)attacker!=lastCharacter)
                ResetCharacterActions();
            //ResetCharacterActions();
            lastCharacter = (IGridActor)attacker;
            var mCc = new AttackCommand(attacker, target);
            currentActions.Enqueue(mCc);
        }
        public void CheckAttackPreview(IBattleActor u, IBattleActor target, GridPosition attackPosition)
        {
            Debug.Log("GetBattlePreview from AttackPosition: "+attackPosition.X+" "+attackPosition.Y);
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

        private void OnDestroy()
        {
            OnCommandFinished = null;
            OnAllCommandsFinished = null;
            OnUndo = null;
        }
    }
}