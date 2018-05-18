using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Commands
{
    public class AttackCommand : Command
    {
        Unit attacker;
        Unit target;
        public AttackCommand(Unit attacker, Unit target)
        {
            this.attacker = attacker;
            this.target = target;
        }
        public override void Execute()
        {
            MainScript.instance.SwitchState(new FightState(attacker,target));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
