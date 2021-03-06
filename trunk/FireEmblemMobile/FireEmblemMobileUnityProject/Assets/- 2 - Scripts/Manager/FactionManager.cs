﻿using Assets.GameActors.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Manager
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

        public FactionManager()
        {
            Factions = new List<Faction>();
            var transform = Object.FindObjectOfType<PlayerConfig>();
            foreach (var p in transform.Factions)
            {
                Factions.Add(p);
                p.Init();
            }
        }

        public Faction GetPlayerControlledFaction()
        {
            return Factions[0];
        }
    }
}