using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid.PathFinding;
using Game.Map;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class MoveSystem : IEngineSystem
    {
        public IPathFinder pathFinder
        {
            get;
            set;
        }
        private NodeHelper nodeHelper;
        public ITileChecker tileChecker
        {
            get;
            set;
        }
        public void Init()
        {
            this.nodeHelper = new NodeHelper(tileChecker.GetWidth(), tileChecker.GetHeight());
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
          
        }

        public List<Vector2> GetMovement(int x, int y, int movRange, Unit unit)
        {
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
            if (tileChecker.CheckField(x - 1, y, unit, range) && nodeHelper.NodeFaster(x - 1, y, c))
                GetMovementLocations(x - 1, y, range - 1, c, unit, locations);
            if (tileChecker.CheckField(x + 1, y, unit, range) && nodeHelper.NodeFaster(x + 1, y, c))
                GetMovementLocations(x + 1, y, range - 1, c, unit, locations);
            if (tileChecker.CheckField(x, y - 1, unit, range) && nodeHelper.NodeFaster(x, y - 1, c))
                GetMovementLocations(x, y - 1, range - 1, c, unit, locations);
            if (tileChecker.CheckField(x, y + 1, unit, range) && nodeHelper.NodeFaster(x, y + 1, c))
                GetMovementLocations(x, y + 1, range - 1, c, unit, locations);
        }
        

        public MovementPath GetPath(int x, int y, int x2, int y2, IGridActor unit, bool toAdjacentPos, IEnumerable<int> range)
        {
            //GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            return pathFinder.FindPath(x, y, x2, y2, unit, toAdjacentPos, range);
        }

       
    }
}