using System.Collections.Generic;
using Game.GameActors.Items;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class PlayerData
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public FactionData factionData;
        [SerializeField]
        public int money;
        [SerializeField]
        public List<Item> convoy;
        public PlayerData (Player player)
        {
            Name = player.Name;
            money = player.money;
            convoy = player.convoy;
            //factionData = new FactionData((WM_Faction)player.faction);

        }

        
    }
}