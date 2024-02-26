using System;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.Systems;
using Game.WorldMapStuff.Model;
using GameEngine;
using LostGrace;
using Unity.IO.LowLevel.Unsafe;
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
                Debug.LogWarning("Destroy player instance(duplicate)");
                Destroy(gameObject);
                return;

            }
            else
            {
                MyDebug.LogEngine("No Player Instance found! Making this new instance");
                instance = this;
                Flags = new PlayerFlags();
                Modifiers = new Modifiers();
                DontDestroyOnLoad(gameObject);
                SaveGameManager.RegisterDataPersistanceObject(this);
            }
        }


        public Modifiers Modifiers;
        public PlayerFlags Flags;
        private void OnDestroy()
        {
            SaveGameManager.UnregisterDataPersistanceObject(this);
        }

        [field: SerializeField]
        public Party Party { get; set; }

        private Dictionary<string, bool> quests;
        [HideInInspector]
        public Dictionary<string, bool> Quests
        {
            get
            {
                return quests ??= new Dictionary<string, bool>(); 
                
            }
          
        }
        private List<LGDialogChoiceData> dialogOptionsExperienced;
        public  List<LGDialogChoiceData> DialogOptionsExperienced
        {
            get
            {
                return dialogOptionsExperienced ??= new List<LGDialogChoiceData>(); 
                
            }
          
        }
        [HideInInspector]
        [field:SerializeField]
        public List<string> UnlockedCharacterIds
        {
            get
            {
                return unlockedCharacterIds ??= new List<string>(); 
                
            }
          
        }
        [HideInInspector]
        [field:SerializeField]
        public List<string> SeenSinsIds
        {
            get
            {
                return seenSinsIds ??= new List<string>(); 
                
            }
          
        }
        [field:SerializeField]
        public List<string> SeenEnemyIds
        {
            get
            {
                return seenEnemiesIds ??= new List<string>(); 
                
            }
          
        }
        [field:SerializeField]
        public List<string> SeenGodsIds
        {
            get
            {
                return seenGodsIds ??= new List<string>(); 
                
            }
          
        }
        private List<string> completedQuests;
        [HideInInspector]
        public List <string> CompletedQuests
        {
            get
            {
                return completedQuests ??= new List<string>(); 
                
            }
          
        }
        
        public string Name;

        public int startPartyMemberCount = 2;
        [SerializeField]
        private List<string> unlockedCharacterIds;
        [SerializeField]
        private List<string> seenSinsIds;
        [SerializeField]
        private List<string> seenGodsIds;
        [SerializeField]
        private List<string> seenEnemiesIds;
        [field: SerializeField] public MetaUpgradeManager MetaUpgradeManager { get; set; }
        public string CurrentEventDialogID { get; set; }
        public BattleOutcome LastBattleOutcome { get; set; }
        private int grace;
        public int Grace
        {
            get { return grace;}
            set
            {
                grace = value;
                MyDebug.LogTest("Grace: "+grace);
                onGraceValueChanged?.Invoke();
            }
        }
        private int corruptedGrace;
        public int CorruptedGrace
        {
            get { return corruptedGrace;}
            set
            {
                corruptedGrace = value;
                onCorruptedGraceValueChanged?.Invoke();
            }
        }
        private int deathstones;
        public int DeathStones
        {
            get { return deathstones;}
            set
            {
                deathstones = value;
                onDeathStoneValueChanged?.Invoke();
            }
        }

        public event Action onGraceValueChanged;
        public event Action onCorruptedGraceValueChanged;
        public event Action onDeathStoneValueChanged;
    

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

        public void LearnMetaUpgrade(MetaUpgradeBP metaUpgradeBp)
        {
            var metaUpgrade = metaUpgradeBp.Create();
            int levelBefore = 0;
            if (!HasLearned(metaUpgrade))
            {
                MetaUpgradeManager.Add(metaUpgrade);
            }
            else
            {
                metaUpgrade=MetaUpgradeManager.GetUpgrade(metaUpgradeBp);
                if (metaUpgrade.IsMaxed())
                    return;
                levelBefore = metaUpgrade.level;
                metaUpgrade.level++;
            }

            switch (metaUpgradeBp.costType)
            {
                case MetaUpgradeCost.Grace:
                    Grace -= metaUpgradeBp.costToLevel[levelBefore];break;
                case MetaUpgradeCost.CorruptedGrace: 
                    CorruptedGrace -= metaUpgradeBp.costToLevel[levelBefore]; break;
                case MetaUpgradeCost.DeathStone: 
                    DeathStones -= metaUpgradeBp.costToLevel[levelBefore]; break;
            }
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
            //Debug.Log("Data" + data.playerData);
            //Debug.Log("Data" + data.playerData.partyData);
            Name = data.playerData.Name;
            Party.Load(data.playerData.partyData);
            MetaUpgradeManager.Load(data.playerData.metaUpgradeManagerData);
            quests = new Dictionary<string, bool>();
            CurrentEventDialogID = data.playerData.currentEventDialogID;
            completedQuests = new List<string>();
            foreach (var quest in data.playerData.acceptedQuests)
            {
                Quests.Add(quest.Key, quest.Value);
            }

            unlockedCharacterIds = new List<string>();
            // Debug.Log("unlockedCharacterCount: "+data.playerData.unlockedCharacterIds.Count);
            foreach (var id in data.playerData.unlockedCharacterIds)
            {
                unlockedCharacterIds.Add(id);
            }

            LastBattleOutcome = data.playerData.lastBattleOutcome;
        }

        public void SaveData(ref SaveData data)
        {
          //  Debug.Log("Save Player Data");
            data.playerData = new PlayerData(this);
        }


        public void AddQuest(string questName)
        {
            
            if(!Quests.ContainsKey(questName))
                Quests.Add(questName, false);
        }

        public void CompleteQuest(string questName)
        {
            if(!Quests.ContainsKey(questName))
                Quests.Add(questName, true);
            else
                Quests[questName] = true;
        }

        public event Action<int> FlameLevelChanged;
        public event Action<float> FlameExpChanged;
        private float flameExp = 0;
        private int flameLevel = 1;

        public int GetFlameLevel()
        {
            return flameLevel;
        }
        public void AddFlameExp(int exp)
        {
            flameExp += exp;
            if (flameExp >= 100)
            {
                flameExp = 0;
                flameLevel++;
                FlameLevelChanged?.Invoke(flameLevel);
            }
               
            FlameExpChanged?.Invoke(flameExp);
        }

        public bool CanAfford(int cost, MetaUpgradeCost costType)
        {
            switch (costType)
            {
                case MetaUpgradeCost.Grace:
                    return Grace >= cost;break;
                case MetaUpgradeCost.CorruptedGrace:
                    return CorruptedGrace >= cost;break;
                case MetaUpgradeCost.DeathStone:
                    return DeathStones >= cost;break;
            }

            return false;
        }
    }
}
