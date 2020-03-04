using Assets.GameActors.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Manager
{
    public class PlayerManager
    {
        public List<Faction> Players { get; set; }
        public Faction ActivePlayer { get; set; }
        private int activePlayerNumber;

        public int ActivePlayerNumber
        {
            get => activePlayerNumber;
            set
            {
                activePlayerNumber = value >= Players.Count ? 0 : value;
                ActivePlayer = Players[activePlayerNumber];
            }
        }

        public PlayerManager()
        {
            Players = new List<Faction>();
            var transform = Object.FindObjectOfType<PlayerConfig>();
            foreach (var p in transform.Factions)
            {
                Players.Add(p);
                p.Init();
            }
        }

        public Faction GetRealPlayer()
        {
            return Players[0];
        }
    }
}