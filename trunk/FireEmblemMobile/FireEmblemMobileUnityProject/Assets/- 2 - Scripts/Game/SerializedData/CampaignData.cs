using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;

namespace Game.Systems
{
    [System.Serializable]
    public class CampaignData
    {
        public int campaignId;
        //public List<LocationData>locationData;
        public int turnCount;
        public FactionData enemyFactionData;
        public CampaignData(Campaign campaign)
        {
            campaignId = campaign.campaignId;
            turnCount = campaign.turnCount;
            enemyFactionData = new FactionData(campaign.EnemyFaction);
            // locationData = new List<LocationData>();
            // foreach (var locationController in campaign.locations)
            // {
            //     locationData.Add(locationController.GetSaveData());
            // }
        }

    }
}