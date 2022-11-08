using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

namespace Game.Systems
{
    [System.Serializable]
    public class SaveData
    {
        public int saveSlot;
        public string fileLabel;
        public static SaveData currentSaveData;
        public PlayerData playerData;
        public EncounterTreeData encounterTreeData;
        public CampaignData campaignData;
        
        public SaveData(int saveSlot, string label)
        {
            this.saveSlot = saveSlot;
            this.fileLabel = label;
        }
        public SaveData(int saveSlot,string label, Player player, Campaign campaign, EncounterTree encounterTree):this(saveSlot, label)
        {
            playerData = player.GetSaveData();
            campaignData = campaign.GetSaveData();
            encounterTreeData = encounterTree.GetSaveData();
            // PlayerPrefs.SetFloat("CameraX",cameraPos.x);
            // PlayerPrefs.SetFloat("CameraY",cameraPos.y);
            // PlayerPrefs.SetFloat("CameraZ",cameraPos.z);
            // PlayerPrefs.Save();
        }
        
    }
}