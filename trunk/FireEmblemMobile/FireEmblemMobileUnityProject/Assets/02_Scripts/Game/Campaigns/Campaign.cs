using System.Numerics;
using Game.GameResources;
using Game.Systems;
using Game.WorldMapStuff.UI;

namespace Game.WorldMapStuff.Model
{
    public class Campaign
    {
        private static Campaign _instance;
        public string name;
        public int campaignId;
        public int turnCount = 1;
        public bool dataLoaded;
        public Scenes scene;

        public static Campaign Instance
        {
            get { return _instance ??= new Campaign(); }
        }

         public CampaignData GetSaveData()
        {
            return new CampaignData(this);
        }
        
        public void LoadData(CampaignData campaignData)
        {
            campaignId = campaignData.campaignId;
            turnCount = campaignData.turnCount;
            scene =  GameData.Instance.campaigns[campaignId].scene;
            dataLoaded = true;
           
    
            // for (int i=0; i < locations.Count; i++)
            // {
            //     locations[i].LoadData(campaignData.locationData[i]);
            // }
        }

        public void LoadConfig(CampaignConfig instanceCampaign)
        {
            name=instanceCampaign.name;
            campaignId = instanceCampaign.campaignId;
            scene = instanceCampaign.scene;
        }

        public static void Reset()
        {
            _instance = null;
        }
    }
}