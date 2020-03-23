using System.Collections.Generic;
using System.Linq;
using Assets.Core;
using Assets.GameActors.Units;
using Assets.Map;
using UnityEngine;

namespace Assets.AI
{
    public class AIGameData
    {
        private readonly MapSystem gridManager;

        public AIGameData()
        {
            gridManager = GridGameManager.Instance.GetSystem<MapSystem>();
        }

        public List<Unit> GetAttackTargets(Unit c)
        {
            int x = c.GridPosition.X;
            int z = c.GridPosition.Y;
            var characters = new List<Unit>();
            foreach (int range in c.Stats.AttackRanges) GetAttackableUnits(c, x, z, range, characters, new List<int>());
            return characters;
        }

        public List<Unit> GetAttackableUnitsAtLocation(Vector3 location, Unit character)
        {
            var attackTargets = new List<Unit>();
            var x = (int) location.x;
            var z = (int) location.z;
            foreach (int range in character.Stats.AttackRanges)
                GetAttackableUnits(character, x, z, range, attackTargets, new List<int>());
            return attackTargets;
        }

        public void GetAttackableUnits(Unit character, int x, int y, int range, List<Unit> characters,
            List<int> direction)
        {
            var unit = gridManager.Tiles[x, y].Unit;
            if (range <= 0)
            {
                if (unit != null && unit.Faction.Id != character.Faction.Id && unit.IsAlive() &&
                    characters.All(a => a != unit)) characters.Add(unit);
                return;
            }

            if (!direction.Contains(2) && gridManager.GridLogic.CheckAttackField(x + 1, y))
            {
                var newDirection = new List<int>(direction) {1};
                GetAttackableUnits(character, x + 1, y, range - 1, characters, newDirection);
            }

            if (!direction.Contains(1) && gridManager.GridLogic.CheckAttackField(x - 1, y))
            {
                var newDirection = new List<int>(direction) {2};
                GetAttackableUnits(character, x - 1, y, range - 1, characters, newDirection);
            }

            if (!direction.Contains(4) && gridManager.GridLogic.CheckAttackField(x, y + 1))
            {
                var newDirection = new List<int>(direction) {3};
                GetAttackableUnits(character, x, y + 1, range - 1, characters, newDirection);
            }

            if (!direction.Contains(3) && gridManager.GridLogic.CheckAttackField(x, y - 1))
            {
                var newDirection = new List<int>(direction) {4};
                GetAttackableUnits(character, x, y - 1, range - 1, characters, newDirection);
            }
        }
    }
}