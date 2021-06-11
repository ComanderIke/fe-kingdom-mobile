﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Manager;
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
        public List<Unit> Units { get; private set; }

        public Faction()
        {
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

   
    }
}