using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid.PathFinding;
using Game.Map;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class MoveSystem : IEngineSystem, IPathProvider
    {
        private readonly GridSystem gridSystem;
        private AStar PathFindingManager { get; set; }
        private readonly NodeHelper nodeHelper;

        public MoveSystem(GridSystem gridSystem)
        {
            this.gridSystem = gridSystem;
            nodeHelper = new NodeHelper(gridSystem.GridData.width, gridSystem.GridData.height);
            PathFindingManager = new AStar(gridSystem, gridSystem.GridData.width, gridSystem.GridData.height);
        }

        public List<Vector2> GetMovement(int x, int y, int movRange, Unit unit)
        {
            nodeHelper.Reset();
            var locations = new List<Vector2>();
            GetMovementLocations(x, y, movRange, 0, unit, locations);
            return locations;
        }

        private void GetMovementLocations(int x, int y, int range, int c, Unit unit, ICollection<Vector2> locations)
        {
            if (range < 0)
            {
                return;
            }

            locations.Add(new Vector2(x, y));
            nodeHelper.Nodes[x, y].C = c;
            c++;
            if (gridSystem.GridLogic.CheckField(x - 1, y, unit, range) && nodeHelper.NodeFaster(x - 1, y, c))
                GetMovementLocations(x - 1, y, range - 1, c, unit, locations);
            if (gridSystem.GridLogic.CheckField(x + 1, y, unit, range) && nodeHelper.NodeFaster(x + 1, y, c))
                GetMovementLocations(x + 1, y, range - 1, c, unit, locations);
            if (gridSystem.GridLogic.CheckField(x, y - 1, unit, range) && nodeHelper.NodeFaster(x, y - 1, c))
                GetMovementLocations(x, y - 1, range - 1, c, unit, locations);
            if (gridSystem.GridLogic.CheckField(x, y + 1, unit, range) && nodeHelper.NodeFaster(x, y + 1, c))
                GetMovementLocations(x, y + 1, range - 1, c, unit, locations);
        }

        public int GetDistance(int x1, int y1, int x2, int y2, IGridActor unit)
        {
            var path = PathFindingManager.FindPath(x1, y1, x2, y2, unit, false, null);
            return path?.GetLength() ?? int.MaxValue;
        }

        public MovementPath GetPath(int x, int y, int x2, int y2, IGridActor unit, bool toAdjacentPos, IEnumerable<int> range)
        {
            //GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            return PathFindingManager.GetPath(x, y, x2, y2, unit, toAdjacentPos, range);
        }

       
    }
}