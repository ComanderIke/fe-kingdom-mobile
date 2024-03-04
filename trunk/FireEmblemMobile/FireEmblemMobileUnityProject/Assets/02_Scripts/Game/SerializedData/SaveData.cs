using Game.Campaigns;
using Game.EncounterAreas.AreaConstruction;
using Game.GameActors.Player;
using Game.GameMechanics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.SerializedData
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
      
        public EncounterAreaData EncounterAreaData;
        [FormerlySerializedAs("campaignData")] public GridBattleData gridBattleData;
        public SlotData slotData;
        
        public SaveData(int saveSlot, string label, string difficulty)
        {
            this.saveSlot = saveSlot;
            slotData = new SlotData(label, difficulty);
            EncounterAreaData = new EncounterAreaData();
            //this.fileLabel = label;
        }
        public SaveData(int saveSlot,string label, string difficulty, Player player, Campaign campaign, EncounterTree encounterTree):this(saveSlot, label, difficulty)
        {
            Debug.Log("Create Advanced Save Data");
            // playerData = player.GetSaveData();
            // gridBattleData = campaign.GetSaveData();
            //encounterTreeData = encounterTree.GetSaveData();
            // this.difficulty = difficulty;
            // PlayerPrefs.SetFloat("CameraX",cameraPos.x);
            // PlayerPrefs.SetFloat("CameraY",cameraPos.y);
            // PlayerPrefs.SetFloat("CameraZ",cameraPos.z);
            // PlayerPrefs.Save();
        }
        
    }
}