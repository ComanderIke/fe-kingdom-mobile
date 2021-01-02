using Assets.Game.Manager;
using Assets.GameActors.Units;
using Assets.GameEngine;
using System;
using UnityEngine;

namespace Assets.Mechanics.Commands
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