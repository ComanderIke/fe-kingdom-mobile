using System.Collections.Generic;
using Game.GameActors.Items;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{

    [System.Serializable]
    public class Player
    {
        private static Player _instance;
        public static Player Instance
        {
            get { return _instance ??= new Player(); }
        }

        public Party Party { get; set; }
        [HideInInspector]

        public string Name;
        [SerializeField] public List<Item> convoy = default;
        [SerializeField] public int money = default;
        [HideInInspector]
        public bool dataLoaded = false;

        public Player()
        {
            convoy = new List<Item>();

        }

        public PlayerData GetSaveData()
        {
            var playerData = new PlayerData(this);
            return playerData;
        }

        public void LoadData(PlayerData data)
        {
            money = data.money;
            Name = data.Name;
            //data.factionData.Load((WM_Faction)faction);
            convoy=data.convoy;
            dataLoaded = true;
        }
       
    }
}
