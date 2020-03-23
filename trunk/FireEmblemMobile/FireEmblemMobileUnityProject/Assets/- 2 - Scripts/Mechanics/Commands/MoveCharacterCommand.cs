using Assets.Core;
using Assets.Core.GameStates;
using Assets.GameActors.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics.Commands
{
    internal class MoveCharacterCommand : Command
    {
        private readonly Unit unit;
        private readonly int x;
        private readonly int y;
        private readonly int oldX;
        private readonly int oldY;
        private readonly List<Vector2> path;

        public MoveCharacterCommand(Unit unit, int x, int y)
        {
            this.unit = unit;
            oldX = unit.GridPosition.X;
            oldY = unit.GridPosition.Y;
            this.x = x;
            this.y = y;
        }

        public MoveCharacterCommand(Unit unit, int x, int y, List<Vector2> path) : this(unit, x, y)
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