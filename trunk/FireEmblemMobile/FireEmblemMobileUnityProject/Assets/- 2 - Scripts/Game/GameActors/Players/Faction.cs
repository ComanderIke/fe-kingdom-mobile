using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Manager;

namespace Game.GameActors.Players
{
    [Serializable]
    public class Faction
    {
        public int Id;
        public bool IsPlayerControlled;
        public string Name;
        public List<Unit> Units;

        public Faction(int number, string name, bool isPlayerControlled)
        {
            Id = number;
            Name = name;
            IsPlayerControlled = isPlayerControlled;
            Units = new List<Unit>();
        }

        public List<Unit> GetActiveUnits()
        {
            return Units.Where(c => c.IsActive() && c.IsAlive()).ToList();
        }

        public void Init()
        {
            Unit.UnitDied += RemoveUnit;
        }

        private void RemoveUnit(Unit unit)
        {
            if (Units.Contains(unit)) Units.Remove(unit);
        }

        public bool IsAlive()
        {
            return Units.Any(unit => unit.IsAlive());
        }

        public void AddUnit(Unit unit)
        {
            if (Units == null)
                Units = new List<Unit>();
            unit.Faction = this;
            unit.Faction.Id = Id;
            Units.Add(unit);
        }

        public List<Faction> GetOpponentFactions()
        {
            return GridGameManager.Instance.FactionManager.Factions.Where(faction => faction.Id != Id).ToList();
        }

        public override string ToString()
        {
            return Units.Aggregate("Faction: "+Name+" Unit Count: "+Units.Count, (current, u) => current + u.ToString());
        }
    }
}