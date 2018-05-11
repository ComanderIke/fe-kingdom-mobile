using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class HowlCommand : Command
    {
        List<Vector2> positions;
        LivingObject unit;
        public HowlCommand(LivingObject unit, List<Vector2> positions)
        {
            this.positions = positions;
            this.unit = unit;
        }
        public override void Execute()
        {
            Debug.Log("Howl Animation");
            EventContainer.commandFinished();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
