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

        public static Action OnCommandFinished;

        public static event Action OnAllCommandsFinished;

        public static event Action OnUndo;
        public static Action TriggerUndo;

        public delegate void OnCheckAttackPreviewEvent(Unit u, Unit target);
        public static event OnCheckAttackPreviewEvent OnCheckAttackPreview;

        #endregion

        private Stack<Command> lastActions;
        private Queue<Command> currentActions;

        private void Start()
        {
            GameplayInput.OnWait += Wait;
            GameplayInput.OnAttackUnit += Fight;
            GameplayInput.OnMoveUnit += MoveCharacter;
            GameplayInput.OnCheckAttackPreview += CheckAttackPreview;
            GameplayInput.OnExecuteInputActions += ExecuteActions;
            lastActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            OnCommandFinished += ExecuteActions;
            TriggerUndo += Undo;
        }

        public void Undo()
        {
            OnUndo?.Invoke();
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
            OnCheckAttackPreview?.Invoke(u, target);
        }
        public void MoveCharacter(Unit c, GridPosition destination, List<GridPosition> path = null)
        {
            var mCc = new MoveCharacterCommand(c, destination, path);
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