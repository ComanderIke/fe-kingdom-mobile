using System.Collections.Generic;
using Game.Grid;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    [CreateAssetMenu(fileName = "campaign", menuName = "GameData/Campaign")]
    public class Campaign:ScriptableObject
    {
        public CampaignVictoryDefeatCondition[] victoryDefeatConditions;
        public string name;
        public int campaignId;
        [HideInInspector]
        public int turnCount = 1;
        // [HideInInspector]
        // public List<LocationController> locations;

        [HideInInspector]
        public WM_Faction EnemyFaction;

        public CampaignData GetSaveData()
        {
            return new CampaignData(this);
        }

        public void LoadData(CampaignData campaignData)
        {
            campaignId = campaignData.campaignId;
            turnCount = campaignData.turnCount;
            // for (int i=0; i < locations.Count; i++)
            // {
            //     locations[i].LoadData(campaignData.locationData[i]);
            // }
        }
    }
}