using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.Systems;
using Game.WorldMapStuff.Model;
using GameEngine;
using LostGrace;
using UnityEditor;
using UnityEngine;

namespace Game.GameActors.Players
{

    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Player")]
    public class Player :SingletonScriptableObject<Player>, IDataPersistance
    {
        private void OnEnable()
        {
            SaveGameManager.RegisterDataPersistanceObject(this);
        }
        private void OnDisable()
        {
            SaveGameManager.RegisterDataPersistanceObject(this);
        }

        [field: SerializeField]
        public Party Party { get; set; }
        [HideInInspector]

        public string Name;

        public int startPartyMemberCount = 2;
        [field: SerializeField] public MetaUpgradeManager MetaUpgradeManager { get; set; }

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

        

        // public void LoadData(PlayerData data)
        // {
        //     
        //     Name = data.Name;
        //     Party = data.partyData.Load();
        //     MetaUpgradeManager.Load(data.metaUpgradeManagerData);
        //     //data.factionData.Load((WM_Faction)faction);
        //
        // }

        public event Action onMetaUpgradesChanged;
        public void LearnMetaUpgrade(MetaUpgrade metaUpgrade)
        {
            if (metaUpgrade.IsMaxed())
                return;
            if(!HasLearned(metaUpgrade))
                MetaUpgradeManager.Add(metaUpgrade);
            metaUpgrade.level++;
            
            onMetaUpgradesChanged?.Invoke();
        }

        public bool HasLearned(MetaUpgrade upg)
        {
            return MetaUpgradeManager.Contains(upg);
        }
        

        public MetaUpgrade GetMetaUpgrade(MetaUpgrade upg)
        {
            if (HasLearned(upg))
                return MetaUpgradeManager.Find(upg);
            return null;
        }

        public void LoadData(SaveData data)
        {
            Debug.Log("LoadPlayerData");
            Name = data.playerData.Name;
            Party.Load(data.playerData.partyData);
            MetaUpgradeManager.Load(data.playerData.metaUpgradeManagerData);
        }

        public void SaveData(ref SaveData data)
        {
            Debug.Log("Save Player Data");
            data.playerData = new PlayerData(this);
        }
    }
}
