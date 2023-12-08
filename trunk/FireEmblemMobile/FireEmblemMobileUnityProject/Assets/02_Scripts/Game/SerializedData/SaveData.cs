using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

namespace Game.Systems
{
    [System.Serializable]
    public class SlotData
    {
        public string difficulty;
        public string fileName;

        public SlotData(string fileName)
        {
            this.fileName = fileName;
        }

        public SlotData(string fileName, string difficulty)
        {
            this.fileName = fileName;
            this.difficulty = difficulty;
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int saveSlot;
        // public string fileLabel;
        //public static SaveData currentSaveData;
        public PlayerData playerData;
        public EncounterTreeData encounterTreeData;
        public CampaignData campaignData;
       
        public SlotData slotData;
        
        public SaveData(int saveSlot, string label, string difficulty)
        {
            this.saveSlot = saveSlot;
            slotData = new SlotData(label, difficulty);
            //this.fileLabel = label;
        }
        public SaveData(int saveSlot,string label, string difficulty, Player player, Campaign campaign, EncounterTree encounterTree):this(saveSlot, label, difficulty)
        {
            Debug.Log("Create Advanced Save Data");
            playerData = player.GetSaveData();
            campaignData = campaign.GetSaveData();
            encounterTreeData = encounterTree.GetSaveData();
            // this.difficulty = difficulty;
            // PlayerPrefs.SetFloat("CameraX",cameraPos.x);
            // PlayerPrefs.SetFloat("CameraY",cameraPos.y);
            // PlayerPrefs.SetFloat("CameraZ",cameraPos.z);
            // PlayerPrefs.Save();
        }
        
    }
}