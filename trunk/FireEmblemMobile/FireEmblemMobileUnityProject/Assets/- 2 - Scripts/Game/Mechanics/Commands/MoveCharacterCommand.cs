using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid;
using Game.Manager;

namespace Game.Mechanics.Commands
{
    internal class MoveCharacterCommand : Command
    {
        private readonly Unit unit;
        private readonly int x;
        private readonly int y;
        private readonly int oldX;
        private readonly int oldY;
        private readonly List<GridPosition> path;

        public MoveCharacterCommand(Unit unit, GridPosition destination)
        {
            this.unit = unit;
            oldX = unit.GridPosition.X;
            oldY = unit.GridPosition.Y;
            x = destination.X;
            y = destination.Y;
        }

        public MoveCharacterCommand(Unit unit, GridPosition destination, List<GridPosition> path) : this(unit, destination)
        {
            this.path = path;
        }

        public override void Execute()
        {
            GameStateManager.MovementState.StartMovement(unit, x, y, path);
        }

        public override void Undo()
        {
            unit.SetPosition(oldX, oldY);
            unit.UnitTurnState.Reset();
        }
    }
}