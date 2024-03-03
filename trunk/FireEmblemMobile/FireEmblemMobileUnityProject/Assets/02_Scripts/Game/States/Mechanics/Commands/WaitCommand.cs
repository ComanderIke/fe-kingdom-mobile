using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.Grid;
using Game.Manager;
using Game.Systems;
using UnityEngine;

namespace Game.States.Mechanics.Commands
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
            MyDebug.LogLogic("Execute Wait Command!");
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            var unitSelectionManager = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            if (unit != null && !unit.TurnStateManager.IsWaiting)
            {
                gridSystem.HideMoveRange();
                if (unit.GridComponent.Canto <= 0 || unit.TurnStateManager.HasCantoed ||
                    !unit.TurnStateManager.HasAttacked)
                {
                    unit.TurnStateManager.Wait();
                    if (unit.GridComponent.Tile.HasGlowSpot()&& unit.Faction.IsPlayerControlled)
                    {
                        unit.GridComponent.Tile.RemoveGlowSpot();
                        ((Unit)unit).ExperienceManager.AddExp(30);
                    }
                       
                    

                }
                else
                {
                    GridGameManager.Instance.GetSystem<GridSystem>().ShowMovementRangeOnGrid(unit, unit.GridComponent.Canto>0&& !unit.TurnStateManager.HasCantoed);
                }
            }
           // unitSelectionManager.DeselectActiveCharacter();
            IsFinished = true;
        }

        public override void Undo()
        {
            Debug.Log("Undo Wait!" + unit);
            unit.TurnStateManager.IsWaiting = false;
        }

        public override void Update()
        {
            
        }
    }
}