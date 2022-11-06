using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.WorldMapStuff.Model;
using LostGrace;
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

        public static void Reset()
        {
            _instance = null;
        }
        

        private Player()
        {
            Name = "Player1";
            upgrades = new List<MetaUpgrade>();
        }

        public Party Party { get; set; }
        [HideInInspector]

        public string Name;

        public int startPartyMemberCount = 2;

        private List<MetaUpgrade> upgrades;

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
            Party = data.partyData.Load();
            //data.factionData.Load((WM_Faction)faction);

        }

        public event Action onMetaUpgradesChanged;
        public void LearnMetaUpgrade(MetaUpgrade metaUpgrade)
        {
            if (metaUpgrade.IsMaxed())
                return;
            if(!HasLearned(metaUpgrade))
                upgrades.Add(metaUpgrade);
            metaUpgrade.level++;
            
            onMetaUpgradesChanged?.Invoke();
        }

        public bool HasLearned(MetaUpgrade upg)
        {
            return upgrades.Contains(upg);
        }

        public void LoadUpgradeDataFromConfig()
        {
            Debug.Log("Load Upgrades!");
            upgrades = GameConfig.Instance.config.GetUpgrades();
        }

        public MetaUpgrade GetMetaUpgrade(MetaUpgrade upg)
        {
            if (HasLearned(upg))
                return upgrades.Find(u=> u.Equals(upg));
            return null;
        }
    }
}
