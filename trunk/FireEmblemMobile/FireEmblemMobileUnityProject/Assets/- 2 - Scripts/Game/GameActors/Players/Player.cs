using System;
using System.Collections.Generic;
using Assets.GameActors.Items;
using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors.Players
{

    [System.Serializable]
    public class Player
    {
        private static Player _instance;
        public static Player Instance
        {
            get { return _instance = _instance ?? new Player(); }
        }

        [SerializeField] private int playerId = default;
        public List<Unit> Units;
        public string Name;
        [SerializeField] private List<Item> convoy = default;
        [SerializeField] private int money = default;

        public Player()
        {
            convoy = new List<Item>();
            Units = new List<Unit>();
            Name = "Player1";
            playerId = 0;
        }

        public void LoadPlayer(Player player)
        {
            _instance = player;
        }
    }
}
