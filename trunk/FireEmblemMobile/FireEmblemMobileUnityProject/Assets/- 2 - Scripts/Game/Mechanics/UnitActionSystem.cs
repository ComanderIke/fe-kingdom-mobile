﻿using System;
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

        public static Action OnCommandFinished;

        public static event Action OnAllCommandsFinished;

        public static event Action OnUndo;
        public static Action TriggerUndo;

        public delegate void OnCheckAttackPreviewEvent(BattlePreview battlePreview);
        public static event OnCheckAttackPreviewEvent OnCheckAttackPreview;

        #endregion

        private Stack<Command> lastActions;
        private Queue<Command> currentActions;

        public void Init()
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

        public void Wait(IGridActor unit)
        {
            var mCc = new WaitCommand(unit);
            currentActions.Enqueue(mCc);
        }
        public void Fight(IBattleActor attacker, IBattleActor target)
        {
            var mCc = new AttackCommand(attacker, target);
            currentActions.Enqueue(mCc);
        }
        public void CheckAttackPreview(IBattleActor u, IBattleActor target, GridPosition attackPosition)
        {
            var preview = GridGameManager.Instance.GetSystem<BattleSystem>().GetBattlePreview(u, target);
            OnCheckAttackPreview?.Invoke(preview);
        }
        public void MoveCharacter(IGridActor c, GridPosition destination, List<GridPosition> path = null)
        {
            Debug.Log("Create Movement Command!");
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