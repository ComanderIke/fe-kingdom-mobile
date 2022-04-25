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

        private Player()
        {
            Name = "Player1";
        }

        public Party Party { get; set; }
        [HideInInspector]

        public string Name;
        



        public override string ToString()
        {
            string player = "Name: " + Name+", ";
            if(Party!=null)
                player += "Party: " + Party.ToString();
            return player;
        }
        public PlayerData GetSaveData()
        {
            var playerData = new PlayerData(this);
            return playerData;
        }

        public void LoadData(PlayerData data)
        {
            
            Name = data.Name;
            data.partyData.Load(Party);
            //data.factionData.Load((WM_Faction)faction);

        }
       
    }
}
