using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid;
using Game.Manager;
using UnityEngine;

namespace Game.Mechanics.Commands
{
    internal class MoveCharacterCommand : Command
    {
        private readonly IGridActor unit;
        private readonly int x;
        private readonly int y;
        private readonly int oldX;
        private readonly int oldY;
        private readonly List<GridPosition> path;

        public MoveCharacterCommand(IGridActor unit, GridPosition destination)
        {
            this.unit = unit;
            oldX = unit.GridPosition.X;
            oldY = unit.GridPosition.Y;
            x = destination.X;
            y = destination.Y;
        }

        public MoveCharacterCommand(IGridActor unit, GridPosition destination, List<GridPosition> path) : this(unit, destination)
        {
            this.path = path;
        }

        public override void Execute()
        {
            Debug.Log("Execute Movement!");
            GameStateManager.MovementState.StartMovement(unit, x, y, path);
        }

        public override void Undo()
        {
            unit.SetPosition(oldX, oldY);
            unit.UnitTurnState.Reset();
        }
    }
}