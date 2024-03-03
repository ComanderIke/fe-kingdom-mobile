using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Factions;
using UnityEngine;

namespace Game.Manager
{
    public class FactionManager
    {
        public List<Faction> Factions { get; set; }
        public Faction ActiveFaction { get; set; }
        private int activeFactionNumber;
        private const int ENEMY_FACTION_INDEX = 1;

        public int ActivePlayerNumber
        {
            get => activeFactionNumber;
            set
            {
                activeFactionNumber = value >= Factions.Count ? 0 : value;
                ActiveFaction = Factions[activeFactionNumber];
            }
        }

        public Faction EnemyFaction
        {
            get
            {
                return Factions[ENEMY_FACTION_INDEX];
            }
            
        }

        

        public FactionManager()
        {
            Factions = new List<Faction>();
            
          
        }

        public void AddFaction(Faction faction)
        {
            Factions.Add(faction);
            
        }

        public Faction GetPlayerControlledFaction()
        {
            return Factions[0];
        }
        
        public bool IsActiveFaction(Faction faction)
        {
            return ActiveFaction.Id == faction.Id;
        }

        public Faction FactionFromId(FactionId factionId)
        {
            return Factions.FirstOrDefault(f => factionId == f.Id);
        }
    }
}