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
    public class Player : MonoBehaviour, IDataPersistance
    {
        
        public event Action onMetaUpgradesChanged;
        
        private static Player instance;
        public static Player Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
           
            if (instance != null)
            {
                Debug.LogWarning("Destroy player instance (duplicate)");
                Destroy(gameObject);
                return;

            }
            else
            {
                Debug.Log("No Player Instance found! Making this new instance");
                instance = this;
                DontDestroyOnLoad(gameObject);
                SaveGameManager.RegisterDataPersistanceObject(this);
            }
        }
        

        private void OnDestroy()
        {
            SaveGameManager.UnregisterDataPersistanceObject(this);
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
          //  Debug.Log("LoadPlayerData" + Party);
            //Debug.Log("Data" + data.playerData);
            //Debug.Log("Data" + data.playerData.partyData);
            Name = data.playerData.Name;
            Party.Load(data.playerData.partyData);
            MetaUpgradeManager.Load(data.playerData.metaUpgradeManagerData);
        }

        public void SaveData(ref SaveData data)
        {
          //  Debug.Log("Save Player Data");
            data.playerData = new PlayerData(this);
        }
    }
}
