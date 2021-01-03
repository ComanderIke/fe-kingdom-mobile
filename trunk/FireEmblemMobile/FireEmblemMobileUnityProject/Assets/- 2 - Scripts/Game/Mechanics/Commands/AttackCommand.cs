using System;
using Game.GameActors.Units;
using Game.Manager;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics.Commands
{
    public class AttackCommand : Command
    {
        private readonly Unit attacker;
        private readonly Unit target;

        public AttackCommand(Unit attacker, Unit target)
        {
            this.attacker = attacker;
            this.target = target;
        }

        public override void Execute()
        {
            Debug.Log("Execute Attack Command!");
            GameStateManager.BattleState.SetParticipants(attacker, target);
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleStarted);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}