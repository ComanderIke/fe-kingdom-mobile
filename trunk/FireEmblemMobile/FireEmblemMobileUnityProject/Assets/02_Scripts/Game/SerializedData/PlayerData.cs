using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
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
        // [SerializeField]
        // public FactionData factionData;
        [SerializeField]
        public PartyData partyData;
        [SerializeField]
        public MetaUpgradeManagerSaveData metaUpgradeManagerData;

        [SerializeField]
        public SerializableDictionary<string, bool> acceptedQuests;

        [SerializeField] public List<string> unlockedCharacterIds;
        public string currentEventDialogID;

        public BattleOutcome lastBattleOutcome;
        //currentEventNodeUserData
        public PlayerData (Player player)
        {
            MyDebug.LogTest("Create PlayerData");
            Name = player.Name;
            MyDebug.LogTest(player.Party);
            partyData = new PartyData(player.Party);
            MyDebug.LogTest("TEST");
            MyDebug.LogTest(player.MetaUpgradeManager);
            metaUpgradeManagerData = new MetaUpgradeManagerSaveData(player.MetaUpgradeManager);
            acceptedQuests = new SerializableDictionary<string, bool>();
            MyDebug.LogTest(player.CurrentEventDialogID);
            currentEventDialogID = player.CurrentEventDialogID;
            unlockedCharacterIds = new List<string>(); 
            MyDebug.LogTest("unlockedCharacterCount: "+player.UnlockedCharacterIds.Count);
            foreach (var id in player.UnlockedCharacterIds)
            {
                unlockedCharacterIds.Add(id);
            }
            foreach (var quest in player.Quests)
            {
                acceptedQuests.Add(quest.Key, quest.Value);
            }

            lastBattleOutcome = player.LastBattleOutcome;
            //factionData = new FactionData((WM_Faction)player.faction);

        }


        //public object PartyData { get; set; }
    }
}