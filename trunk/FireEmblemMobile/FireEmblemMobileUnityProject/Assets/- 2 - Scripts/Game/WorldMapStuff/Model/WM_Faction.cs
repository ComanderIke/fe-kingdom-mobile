using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.Manager;

namespace Game.WorldMapStuff.Model
{
    [Serializable]
    public class WM_Faction:Faction
    {
   
        public List<Party> Parties { get; private set; }
        
        public WM_Faction(int number, string name, bool isPlayerControlled):base(number, name, isPlayerControlled)
        {
            Parties = new List<Party>();
        }

        public override bool IsActive()
        {
            return WorldMapGameManager.Instance.FactionManager.ActiveFaction == this;
        }


        public void Init()
        {
            Party.PartyDied += RemoveParty;
        }

        private void RemoveParty(Party unit)
        {
            if (Parties.Contains(unit)) Parties.Remove(unit);
        }

        public override bool IsAlive()
        {
            return Parties.Any(unit => unit.IsAlive());
        }

        public void AddParty(Party unit)
        {
            unit.Faction = this;
            unit.Faction.Id = Id;
            Parties.Add(unit);
        }


        public override string ToString()
        {
            return Parties.Aggregate("Faction: "+Name+" Unit Count: "+Parties.Count, (current, u) => current + u.ToString());
        }

    }
}