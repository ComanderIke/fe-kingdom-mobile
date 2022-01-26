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
 
        public PlayerData (Player player)
        {
            Name = player.Name;
       
            //factionData = new FactionData((WM_Faction)player.faction);

        }

        
    }
}