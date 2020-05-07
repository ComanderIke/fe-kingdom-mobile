
using Assets.Game.Manager;
using Assets.GameActors.Units;
using System;
using UnityEngine;

namespace Assets.Mechanics.Commands
{
    public class WaitCommand : Command
    {
        private readonly Unit unit;

        public WaitCommand(Unit unit)
        {
            this.unit = unit;
        }

        public override void Execute()
        {
            Debug.Log("Execute Wait Command!");
            var unitSelectionManager = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            if (unit != null && !unit.UnitTurnState.IsWaiting)
            {
                GridGameManager.Instance.GetSystem<Map.MapSystem>().HideMovementRangeOnGrid();
                unit.UnitTurnState.IsWaiting = true;
                unit.UnitTurnState.Selected = false;
                unit.UnitTurnState.HasMoved = true;
            }
            unitSelectionManager.DeselectActiveCharacter();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}