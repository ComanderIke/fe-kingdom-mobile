using Game.GameResources;
using Game.Systems;
using Game.WorldMapStuff.UI;

namespace Game.WorldMapStuff.Model
{
    public class Campaign
    {
        private static Campaign _instance;
        public CampaignVictoryDefeatCondition[] victoryDefeatConditions;
        public string name;
        public int campaignId;
        public int turnCount = 1;
        // [HideInInspector]
        // public List<LocationController> locations;
        
        public WM_Faction EnemyFaction;
        public bool dataLoaded;

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
            dataLoaded = true;
            victoryDefeatConditions = GameData.Instance.campaigns[campaignId].victoryDefeatConditions;
            EnemyFaction = new WM_Faction();
           
            campaignData.enemyFactionData.Load(EnemyFaction);
            // for (int i=0; i < locations.Count; i++)
            // {
            //     locations[i].LoadData(campaignData.locationData[i]);
            // }
        }

        public void LoadConfig(CampaignConfig instanceCampaign)
        {
            name=instanceCampaign.name;
            campaignId = instanceCampaign.campaignId;
            victoryDefeatConditions = instanceCampaign.victoryDefeatConditions;
            
        }
    }
}