using System.Collections.Generic;
using Game.GameActors.Players;
using UnityEngine;

namespace Game.Manager
{
    public class FactionManager
    {
        public List<Faction> Factions { get; set; }
        public Faction ActiveFaction { get; set; }
        private int activeFactionNumber;

        public int ActivePlayerNumber
        {
            get => activeFactionNumber;
            set
            {
                activeFactionNumber = value >= Factions.Count ? 0 : value;
                ActiveFaction = Factions[activeFactionNumber];
            }
        }

        public FactionManager(List<Faction> factions)
        {
            Factions = new List<Faction>();
            
            foreach (var p in factions)
            {
                Factions.Add(p);
                p.Init();
            }
        }

        public Faction GetPlayerControlledFaction()
        {
            return Factions[0];
        }

        public bool IsActiveFaction(int actorFactionId)
        {
            return ActiveFaction.Id == actorFactionId;
        }
        public bool IsActiveFaction(Faction faction)
        {
            return ActiveFaction == faction;
        }
    }
}