using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;

namespace Game.Systems
{
    [System.Serializable]
    public class SaveData
    {
        public PlayerData playerData;
        public CampaignData campaignData;

        public SaveData()
        {
            
        }
        public SaveData(Player player, Campaign campaign)
        {
            playerData = player.GetSaveData();
            campaignData = campaign.GetSaveData();
        }
    }
}