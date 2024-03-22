using System;
using Game.AI.DecisionMaking;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.Manager;
using Game.Systems;
using UnityEngine;

namespace Game.States.Mechanics.Commands
{
    public class AttackCommand : Command
    {
        private readonly IBattleActor attacker;
        private readonly IAttackableTarget target;
        public static event Action<Unit> OnAttackCommandPerformed;

        public AttackCommand(IBattleActor attacker, IAttackableTarget target)
        {
            this.attacker = attacker;
            this.target = target;
        }

        public override void Execute()
        {
            Debug.Log("EXECUTE FIGHT ACTION");
            GridGameManager.Instance.GameStateManager.BattleState.Start(attacker, target);
            BattleSystem.OnBattleFinished -= BattleFinished;
            BattleSystem.OnBattleFinished += BattleFinished;
            OnAttackCommandPerformed?.Invoke((Unit)attacker);

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