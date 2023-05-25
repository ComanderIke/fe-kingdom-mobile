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

        public string currentEventDialogID;

        public BattleOutcome lastBattleOutcome;
        //currentEventNodeUserData
        public PlayerData (Player player)
        {
            Name = player.Name;
            partyData = new PartyData(player.Party);
            metaUpgradeManagerData = new MetaUpgradeManagerSaveData(player.MetaUpgradeManager);
            acceptedQuests = new SerializableDictionary<string, bool>();
            currentEventDialogID = player.CurrentEventDialogID;
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