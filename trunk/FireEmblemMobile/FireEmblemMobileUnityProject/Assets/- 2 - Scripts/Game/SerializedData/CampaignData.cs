using System.Collections.Generic;
using Game.WorldMapStuff.UI;

namespace Game.Systems
{
    public class CampaignData
    {
        public int campaignId;
        public List<LocationData>locationData;
        public int turnCount;

        public CampaignData(Campaign campaign)
        {
            campaignId = campaign.campaignId;
            turnCount = campaign.turnCount;
            locationData = new List<LocationData>();
            foreach (var locationController in campaign.locations)
            {
                locationData.Add(locationController.GetSaveData());
            }
        }

    }
}