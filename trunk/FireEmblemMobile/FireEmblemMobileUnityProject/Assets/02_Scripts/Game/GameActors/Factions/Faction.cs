using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{
    [Serializable]
    //[CreateAssetMenu(fileName="Faction", menuName = "GameData/Faction")]
    public class Faction
    {
        public FactionId Id;
        public bool IsPlayerControlled;
        public string Name;
        public Party party;
        public List<Unit> Units { get; private set; }
        public List<Destroyable> Destroyables { get; private set; }

        public List<Unit> FieldedUnits
        {
            get
            {
                return Units.Where(a => a.Fielded).ToList();
            }
        }


        public Faction()
        {
            Destroyables = new List<Destroyable>();
            Units = new List<Unit>();
        }
        public Faction(FactionId number, string name, bool isPlayerControlled):this()
        {
            Id = number;
            Name = name;
            IsPlayerControlled = isPlayerControlled;
        }

        public virtual bool IsActive()
        {
            return GridGameManager.Instance.FactionManager.ActiveFaction == this;
        }

        public List<Unit> GetActiveUnits()
        {
            return Units.Where(c => !c.TurnStateManager.IsWaiting && c.IsAlive()).ToList();
        }

      

        public void RemoveUnit(Unit unit)
        {
            
            if (Units.Contains(unit)) Units.Remove(unit);
        }

        public virtual bool IsAlive()
        {
            return Units.Any(unit => unit.IsAlive());
        }
        public void AddDestroyable(Destroyable dest)
        {
            dest.Faction = this;
            dest.Faction.Id = Id;
            Destroyables.Add(dest);
        }
        public void AddUnit(Unit unit)
        {
            unit.Faction = this;
            unit.Faction.Id = Id;
            Units.Add(unit);
            OnAddUnit?.Invoke(unit);
        }

        public List<Faction> GetOpponentFactions()
        {
            return GridGameManager.Instance.FactionManager.Factions.Where(faction => faction.Id != Id).ToList();
        }

        public override string ToString()
        {
            return Units.Aggregate("Faction: "+Name+" Unit Count: "+Units.Count, (current, u) => current + u.ToString());
        }

        public event Action<Unit> OnAddUnit;

        public void ClearUnits()
        {
            Units.Clear();
        }


        public void RemoveDestroyable(Destroyable dest)
        {
            if (Destroyables.Contains(dest)) Destroyables.Remove(dest);
        }

        public bool IsOpponentFaction(Faction targetUnitFaction)
        {
            return GetOpponentFactions().Contains(targetUnitFaction);
        }
    }
}