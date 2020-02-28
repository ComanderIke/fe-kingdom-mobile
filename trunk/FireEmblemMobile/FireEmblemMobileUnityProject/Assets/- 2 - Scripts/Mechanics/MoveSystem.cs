using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.__2___Scripts.Mechanics
{
    public class MoveSystem : EngineSystem
    {
        private MapSystem mapSystem;
        private AStar PathFindingManager { get; set; }
        private NodeHelper nodeHelper;

        public MoveSystem(MapSystem mapSystem)
        {
            this.mapSystem = mapSystem;
            Debug.Log(mapSystem.grid);
            nodeHelper = new NodeHelper(mapSystem.grid.width, mapSystem.grid.height);
            PathFindingManager = new AStar(mapSystem, mapSystem.grid.width, mapSystem.grid.height);
        }
        public List<Vector2> GetMovement(int x, int y, int movRange, int playerId)
        {
            nodeHelper.Reset();
            List<Vector2> locations = new List<Vector2>();
            GetMovementLocations(x, y, movRange, 0, playerId, locations);
            return locations;
        }
        private void GetMovementLocations(int x, int y, int range, int c, int playerId, List<Vector2> locations)
        {
            if (range < 0)
            {
                return;
            }

            locations.Add(new Vector2(x, y));
            nodeHelper.nodes[x, y].c = c;
            c++;
            if (mapSystem.GridLogic.checkField(x - 1, y, playerId, range) && nodeHelper.nodeFaster(x - 1, y, c))
                GetMovementLocations(x - 1, y, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.checkField(x + 1, y, playerId, range) && nodeHelper.nodeFaster(x + 1, y, c))
                GetMovementLocations(x + 1, y, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.checkField(x, y - 1, playerId, range) && nodeHelper.nodeFaster(x, y - 1, c))
                GetMovementLocations(x, y - 1, range - 1, c, playerId, locations);
            if (mapSystem.GridLogic.checkField(x, y + 1, playerId, range) && nodeHelper.nodeFaster(x, y + 1, c))
                GetMovementLocations(x, y + 1, range - 1, c, playerId, locations);
        }

        public int GetDistance(int x1, int y1, int x2, int y2, int playerId)
        {
            MovementPath path = PathFindingManager.findPath(x1, y1, x2, y2, playerId, false, null);
            if (path != null)
                return path.getLength();
            else return int.MaxValue;
        }
        public MovementPath getPath(int x, int y, int x2, int y2, int team, bool toadjacentPos, List<int> range)
        {
            MainScript.instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            return PathFindingManager.getPath(x, y, x2, y2, team, toadjacentPos, range);
        }
        public MovementPath GetMonsterPath(Monster monster, BigTile position)
        {
            MainScript.instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            PathFindingNode[,] nodes = new PathFindingNode[mapSystem.grid.width, mapSystem.grid.height];
            for (int x = 0; x < mapSystem.grid.width; x++)
            {
                for (int y = 0; y < mapSystem.grid.height; y++)
                {
                    bool isAccesible = mapSystem.Tiles[x, y].isAccessible;
                    if (mapSystem.Tiles[x, y].character != null && mapSystem.Tiles[x, y].character.Player.ID != monster.Player.ID)
                        isAccesible = false;
                    nodes[x, y] = new PathFindingNode(x, y, isAccesible);
                }
            }
            AStar2x2 aStar = new AStar2x2(nodes);
            MovementPath p = aStar.GetPath(((BigTilePosition)monster.GridPosition).Position, position);
            return p;
        }

    }
}
