using Assets.Core;
using Assets.Core.GameStates;
using Assets.GameActors.Units;
using System;

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
            MainScript.Instance.GameStateManager.SwitchState(new BattleState(attacker, target));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}