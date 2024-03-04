using Game.DataAndReferences.Data;
using Game.EncounterAreas.Model;
using Game.SerializedData;

namespace Game.Campaigns
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

         public GridBattleData GetSaveData()
        {
            return new GridBattleData(this);
        }
        
        public void LoadData(GridBattleData gridBattleData)
        {
            campaignId = gridBattleData.campaignId;
            turnCount = gridBattleData.turnCount;
            scene =  GameBPData.Instance.campaigns[campaignId].scene;
            dataLoaded = true;
           
    
             for (int i=0; i < 3; i++)
             {
            //     locations[i].LoadData(campaignData.locationData[i]);
             }
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