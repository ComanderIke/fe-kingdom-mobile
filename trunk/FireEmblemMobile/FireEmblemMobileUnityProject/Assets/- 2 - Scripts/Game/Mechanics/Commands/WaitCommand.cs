using System;
using Game.GameActors.Units;
using Game.Manager;
using UnityEngine;

namespace Game.Mechanics.Commands
{
    public class WaitCommand : Command
    {
        private readonly IGridActor unit;

        public WaitCommand(IGridActor unit)
        {
            this.unit = unit;
        }

        public override void Execute()
        {
            Debug.Log("Execute Wait Command!");
            var unitSelectionManager = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            if (unit != null && !unit.TurnStateManager.IsWaiting)
            {
                GridGameManager.Instance.GetSystem<Map.GridSystem>().HideMoveRange();
                unit.TurnStateManager.IsWaiting = true;
                unit.TurnStateManager.IsSelected = false;
                unit.TurnStateManager.HasMoved = true;
            }
            unitSelectionManager.DeselectActiveCharacter();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}