using System;
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
        private readonly IBattleActor target;

        public AttackCommand(IBattleActor attacker, IBattleActor target)
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
            throw new NotImplementedException();
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