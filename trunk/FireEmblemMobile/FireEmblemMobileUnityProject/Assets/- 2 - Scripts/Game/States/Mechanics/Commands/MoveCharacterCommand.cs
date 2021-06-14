using System.Collections.Generic;
using Game.AI;
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
      

        public MoveCharacterCommand(IGridActor unit, Vector2Int destination)
        {
            this.unit = unit;
            oldX = unit.GridComponent.GridPosition.X;
            oldY = unit.GridComponent.GridPosition.Y;
            x = destination.x;
            y = destination.y;
        }

        public MoveCharacterCommand(IGridActor unit, Vector2Int destination, List<GridPosition> path) : this(unit, destination)
        {
            this.path = path;
        }

        public override void Execute()
        {
           // Debug.Log("Execute Movement!");
            GridGameManager.Instance.GameStateManager.MovementState.StartMovement(unit, x, y, path);
        }

        public override void Undo()
        {
            unit.GridComponent.SetPosition(oldX, oldY);
            unit.TurnStateManager.Reset();
        }

        public override void Update()
        {
            IsFinished =  GridGameManager.Instance.GameStateManager.MovementState.IsFinished;
        }
    }
}