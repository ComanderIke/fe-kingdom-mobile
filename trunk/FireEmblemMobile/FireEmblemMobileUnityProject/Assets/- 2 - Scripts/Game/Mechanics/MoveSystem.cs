using System.Collections.Generic;
using Game.Grid.PathFinding;
using Game.Map;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class MoveSystem : IEngineSystem
    {
        private readonly MapSystem mapSystem;
        private AStar PathFindingManager { get; set; }
        private readonly NodeHelper nodeHelper;

        public MoveSystem(MapSystem mapSystem)
        {
            this.mapSystem = mapSystem;
            nodeHelper = new NodeHelper(mapSystem.GridData.Width, mapSystem.GridData.Height);
            PathFindingManager = new AStar(mapSystem, mapSystem.GridData.Width, mapSystem.GridData.Height);
        }

        public List<Vector2> GetMovement(int x, int y, int movRange, int playerId)
        {
            nodeHelper.Reset();
            var locations = new List<Vector2>();
            GetMovementLocations(x, y, movRange, 0, playerId, locations);
            return locations;
        }

        private void GetMovementLocations(int x, int y, int range, int c, int playerId, ICollection<Vector2> locations)
        {
            if (range < 0)
            {
                return;
            }

            locations.Add(new Vector2(x, y));
            nodeHelper.Nodes[x, y].C = c;
            c++;
            if (mapSystem.GridLogic.CheckField(x - 1, y, playerId, range) && nodeHelper.NodeFaster(x - 1, y, c))
                GetMovementLocations(x - 1, y, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.CheckField(x + 1, y, playerId, range) && nodeHelper.NodeFaster(x + 1, y, c))
                GetMovementLocations(x + 1, y, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.CheckField(x, y - 1, playerId, range) && nodeHelper.NodeFaster(x, y - 1, c))
                GetMovementLocations(x, y - 1, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.CheckField(x, y + 1, playerId, range) && nodeHelper.NodeFaster(x, y + 1, c))
                GetMovementLocations(x, y + 1, range - 1, c, playerId, locations);
        }

        public int GetDistance(int x1, int y1, int x2, int y2, int playerId)
        {
            var path = PathFindingManager.FindPath(x1, y1, x2, y2, playerId, false, null);
            return path?.GetLength() ?? int.MaxValue;
        }

        public MovementPath GetPath(int x, int y, int x2, int y2, int team, bool toAdjacentPos, List<int> range)
        {
            //GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            return PathFindingManager.GetPath(x, y, x2, y2, team, toAdjacentPos, range);
        }

    }
}