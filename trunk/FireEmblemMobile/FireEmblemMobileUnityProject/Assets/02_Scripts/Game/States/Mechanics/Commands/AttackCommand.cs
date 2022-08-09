using System;
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
            Debug.Log("Execute Attack Command!");
            GridGameManager.Instance.GameStateManager.BattleState.Start(attacker, target);
            
            
        }

        public override void Undo()
        {
            Debug.Log("Undo Attack should not be undoable!");
        }

        public override void Update()
        {
            if ( GridGameManager.Instance.GameStateManager.BattleState.IsFinished)
            {
                IsFinished = true;
            }
        }
    }
}