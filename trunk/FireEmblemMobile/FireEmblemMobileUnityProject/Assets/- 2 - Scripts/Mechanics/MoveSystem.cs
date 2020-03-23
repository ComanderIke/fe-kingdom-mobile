using Assets.Core;
using Assets.GameActors.Units.Monsters;
using Assets.GameInput;
using Assets.Grid;
using Assets.Grid.PathFinding;
using Assets.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics
{
    public class MoveSystem : IEngineSystem
    {
        private readonly MapSystem mapSystem;
        private AStar PathFindingManager { get; set; }
        private readonly NodeHelper nodeHelper;

        public MoveSystem(MapSystem mapSystem)
        {
            this.mapSystem = mapSystem;
            Debug.Log(mapSystem.GridData);
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
            GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            return PathFindingManager.GetPath(x, y, x2, y2, team, toAdjacentPos, range);
        }

        public MovementPath GetMonsterPath(Monster monster, BigTile position)
        {
            GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            var nodes = new PathFindingNode[mapSystem.GridData.Width, mapSystem.GridData.Height];
            for (int x = 0; x < mapSystem.GridData.Width; x++)
            {
                for (int y = 0; y < mapSystem.GridData.Height; y++)
                {
                    bool isAccessible = mapSystem.Tiles[x, y].IsAccessible;
                    if (mapSystem.Tiles[x, y].Unit != null && mapSystem.Tiles[x, y].Unit.Faction.Id != monster.Faction.Id)
                        isAccessible = false;
                    nodes[x, y] = new PathFindingNode(x, y, isAccessible);
                }
            }

            var aStar = new AStar2X2(nodes);
            var p = aStar.GetPath(((BigTilePosition) monster.GridPosition).Position, position);
            return p;
        }
    }
}