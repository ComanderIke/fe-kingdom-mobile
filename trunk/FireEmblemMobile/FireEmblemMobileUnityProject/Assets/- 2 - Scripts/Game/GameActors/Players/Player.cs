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

        [HideInInspector]
        public bool dataLoaded = false;

       

        public PlayerData GetSaveData()
        {
            var playerData = new PlayerData(this);
            return playerData;
        }

        public void LoadData(PlayerData data)
        {
            
            Name = data.Name;
            //data.factionData.Load((WM_Faction)faction);
           
            dataLoaded = true;
        }
       
    }
}
