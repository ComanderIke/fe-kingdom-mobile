﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.EncounterAreas.Model;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.Manager;
using UnityEngine;

namespace Game.GameActors.Factions
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
            aiGroups = new Dictionary<int, AIGroup>();
        }

        [SerializeField] private string factionPrefix;
        private int unitAutoId;
        public Faction(FactionId number, string name, bool isPlayerControlled):this()
        {
            Id = number;
            Name = name;
           
            unitAutoId = 0;
            IsPlayerControlled = isPlayerControlled;
        }

        public virtual bool IsActive()
        {
            return GridGameManager.Instance.FactionManager.ActiveFaction == this;
        }

        public List<Unit> GetActiveUnits()
        {
            return Units.Where(c => !c.TurnStateManager.IsWaiting && !c.TurnStateManager.HasMoved&&c.IsAlive()).ToList();
        }
        public bool AllUnitsMoved()
        {
            return Units.Where(c => !c.TurnStateManager.HasMoved && c.IsAlive()).Count()==0;
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
            unit.OriginalFaction = this;
            OnAddUnit?.Invoke(unit);
            UnitAddedStatic?.Invoke(unit);
        }
        public void AddUnitTemporarily(Unit unit)
        {
            unit.Faction = this;
            unit.Faction.Id = Id;
            Units.Add(unit);
            OnAddUnit?.Invoke(unit);
            UnitAddedStatic?.Invoke(unit);
        }
        public static event Action<Unit> UnitAddedStatic;
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

        private Dictionary<int,AIGroup> aiGroups;

        public void AddtoAIGroup(int aiGroupId,AIBehaviour.State state, Unit unit)
        {
            if (aiGroups.ContainsKey(aiGroupId))
            {
                aiGroups[aiGroupId].AddAgent(unit);
            }
            else
            {
                aiGroups.Add(aiGroupId, new AIGroup(new List<IAIAgent>(){unit}, state));
            }
        }
    }
}