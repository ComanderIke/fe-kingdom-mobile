using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

namespace Game.Systems
{
    [System.Serializable]
    public class SaveData
    {
        public static SaveData currentSaveData;
        public PlayerData playerData;
        public EncounterTreeData encounterTreeData;
        public CampaignData campaignData;
        
        public SaveData()
        {
            
        }
        public SaveData(Player player, Campaign campaign, EncounterTree encounterTree)
        {
            playerData = player.GetSaveData();
            campaignData = campaign.GetSaveData();
            encounterTreeData = encounterTree.GetSaveData();
            // PlayerPrefs.SetFloat("CameraX",cameraPos.x);
            // PlayerPrefs.SetFloat("CameraY",cameraPos.y);
            // PlayerPrefs.SetFloat("CameraZ",cameraPos.z);
            // PlayerPrefs.Save();
        }

        public static void Reset()
        {
            currentSaveData = null;
            Player.Reset();
            Campaign.Reset();
            EncounterTree.Reset();
        }
    }
}