using System;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics.Commands
{
    public class AttackCommand : Command
    {
        private readonly IBattleActor attacker;
        private readonly IAttackableTarget target;

        public AttackCommand(IBattleActor attacker, IAttackableTarget target)
        {
            this.attacker = attacker;
            this.target = target;
        }

        public override void Execute()
        {
            
            GridGameManager.Instance.GameStateManager.BattleState.Start(attacker, target);
            BattleSystem.OnBattleFinished -= BattleFinished;
            BattleSystem.OnBattleFinished += BattleFinished;

        }

        void BattleFinished(AttackResult result)
        {
            BattleSystem.OnBattleFinished -= BattleFinished;
            IsFinished = true;
        }
        public override void Undo()
        {
            Debug.LogWarning("Undo Attack should not be undoable!");
        }

        public override void Update()
        {
            if ( GridGameManager.Instance.GameStateManager.BattleState.IsFinished)
            {
                Debug.Log("BattleFinished");
                IsFinished = true;
            }
        }
    }
}