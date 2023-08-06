using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Units;
using Game.Grid;
using Game.Manager;
using Game.Map;
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
        private readonly bool skipAnimation;

        private readonly List<GridPosition> path;


        public MoveCharacterCommand(IGridActor unit, Vector2Int destination, bool skipAnimation = false)
        {
            this.unit = unit;
            oldX = unit.GridComponent.OriginTile.X;
            oldY = unit.GridComponent.OriginTile.Y;
            x = destination.x;
            y = destination.y;
            this.skipAnimation = skipAnimation;
        }

        public MoveCharacterCommand(IGridActor unit,Vector2Int destination, List<GridPosition> path,bool skipAnimation = false) : this(unit, destination, skipAnimation)
        {
            this.path = path;
        }

        public override void Execute()
        {
            //Debug.Log("Execute Movement!");
            GridGameManager.Instance.GameStateManager.MovementState.StartMovement(unit, x, y, skipAnimation, path);
        }

        public override void Undo()
        {
            Debug.Log("Undo Move! " + unit+" From: "+unit.GridComponent.GridPosition.X+" "+unit.GridComponent.GridPosition.Y+" To: "+oldX+" "+oldY);
            GridGameManager.Instance.GetSystem<GridSystem>().SetUnitPosition(unit,oldX,oldY);
            //unit.GridComponent.SetPosition(oldX, oldY);
            unit.TurnStateManager.Reset();
            
        }

        public override void Update()
        {
            IsFinished =  GridGameManager.Instance.GameStateManager.MovementState.IsFinished;
        }
    }
}