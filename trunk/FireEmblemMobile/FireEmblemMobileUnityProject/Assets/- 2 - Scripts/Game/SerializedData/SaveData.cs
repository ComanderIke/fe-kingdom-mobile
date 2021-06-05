using Game.GameActors.Players;
using Game.WorldMapStuff.UI;

namespace Game.Systems
{
    public class SaveData
    {
        public PlayerData playerData;
        public CampaignData campaignData;

        public SaveData(Player player, Campaign campaign)
        {
            playerData = player.GetSaveData();
            campaignData = campaign.GetSaveData();
        }
    }
}