using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Monsters;
using Assets.GameInput;
using Assets.Grid;
using Assets.GUI;
using Assets.Mechanics.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics
{
    public class UnitActionSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public delegate void OnCommandFinishedEvent();

        public static OnCommandFinishedEvent OnCommandFinished;

        public static Action OnAllCommandsFinished;

        public delegate void OnUndoEvent();

        public static OnUndoEvent OnUndo;

        public delegate void OnCheckAttackPreviewEvent(Unit u, Unit target);
        public static OnCheckAttackPreviewEvent OnCheckAttackPreview;

        #region UnitActions

        public delegate void OnDeselectCharacterEvent();

        public static OnDeselectCharacterEvent OnDeselectCharacter;

        public delegate void OnSelectCharacterEvent();

        public static OnSelectCharacterEvent OnSelectedCharacter;

        #endregion

        #endregion

        private GridGameManager gridGameManager;
        private Stack<Command> lastActions;
        private Queue<Command> currentActions;
        private GameplayInput gameplayInput;

        private void Start()
        {
            gameplayInput = new GameplayInput();
            gridGameManager = GridGameManager.Instance;
            
            
            GameplayInput.OnWait += Wait;
            GameplayInput.OnAttackUnit += Fight;
            GameplayInput.OnMoveUnit += MoveCharacter;
            GameplayInput.OnCheckAttackPreview += CheckAttackPreview;
            GameplayInput.OnExecuteInputActions += ExecuteActions;
            lastActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            OnUndo += Undo;
            OnCommandFinished += ExecuteActions;
        }

        private void Undo()
        {
            Debug.Log("Undo: " + lastActions.Count);
            lastActions.Pop().Undo();
        }

        public void AddCommand(Command c)
        {
            currentActions.Enqueue(c);
        }

        public void ExecuteActions()
        {
            if (currentActions.Count != 0)
            {
                var current = currentActions.Dequeue();
                if (currentActions.Count == 0)
                {
                    OnCommandFinished = null;
                    OnCommandFinished += ExecuteActions;
                    OnCommandFinished += AllCommandsFinished;
                }

                current.Execute();

                lastActions.Push(current);
            }
        }

        private void AllCommandsFinished()
        {
            OnCommandFinished -= AllCommandsFinished;
            OnAllCommandsFinished?.Invoke();
            OnAllCommandsFinished = null;
        }

        #region GameplayCommands
        public void Wait(Unit unit)
        {
            var mCc = new WaitCommand(unit);
            currentActions.Enqueue(mCc);
        }
        public void Fight(Unit attacker, Unit target)
        {
            var mCc = new AttackCommand(attacker, target);
            currentActions.Enqueue(mCc);
        }
        public void CheckAttackPreview(Unit u, Unit target, GridPosition attackPosition)
        {
            gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter.GameTransform
                            .SetPosition(attackPosition.X, attackPosition.Y);
            OnCheckAttackPreview?.Invoke(u, target);
        }
        public void MoveCharacter(Unit c, GridPosition destination, List<GridPosition> path = null)
        {
            var mCc = new MoveCharacterCommand(c, destination, path);
            Unit.UnitShowActiveEffect(c, false, false);
            UiSystem.OnShowCursor(destination.X, destination.Y);
            currentActions.Enqueue(mCc);
        }
        #endregion

        private void OnDestroy()
        {
            OnCommandFinished = null;
            OnAllCommandsFinished = null;
            OnUndo = null;

            OnDeselectCharacter = null;
            OnSelectedCharacter = null;
        }
    }
}