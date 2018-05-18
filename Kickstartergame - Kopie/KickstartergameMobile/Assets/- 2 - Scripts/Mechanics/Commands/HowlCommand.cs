using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class HowlCommand : Command
    {
        public HowlCommand(Unit unit, List<Vector2> positions)
        {
            //this.positions = positions;
            //this.unit = unit;
        }
        public override void Execute()
        {
            Debug.Log("Howl Animation");
            UnitActionSystem.onCommandFinished();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
