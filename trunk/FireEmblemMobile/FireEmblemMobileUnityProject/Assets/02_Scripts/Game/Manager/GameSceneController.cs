using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI;
using Game.Manager;
using Game.Systems;
using Game.WorldMapStuff.Model;
using LostGrace;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.WorldMapStuff.Controller
{
    public class GameSceneController
    {
        private static GameSceneController instance;

        public static GameSceneController Instance => instance ??= new GameSceneController();

        
        public void LoadBattleLevel(Scenes buildIndex, BattleMap battleMap)//, EncounterNode node)
        {
            Vector3 cameraPos = GameObject.FindObjectOfType<EncounterAreaCameraController>().transform.position;
            PlayerPrefs.SetFloat("CameraX", cameraPos.x);
            PlayerPrefs.SetFloat("CameraX", cameraPos.y);
            PlayerPrefs.Save();
            SceneTransferData.Instance.Reset();
            
           // Debug.Log("Load Battle Level: "+enemyParty.level + " " + enemyParty.name);
            Debug.Log("Scene: "+buildIndex);
            SceneTransferData.Instance.BattleMap = battleMap;
            //SceneTransferData.Instance.EnemyPartyID = enemyParty.name;
            //SceneTransferData.Instance.NodeData =new NodeData(node.UniqueId);
            
            //SceneController.UnLoadSceneAsync(Scenes.EncounterArea);
            SaveGameManager.Save();
            AreaGameManager.Instance.CleanUp();
            SceneController.LoadSceneAsync(buildIndex, false);
        }
     

        public void LoadEncounterAreaBeforeBattle()
        {
            Debug.Log("Save Before LoadScene:");
            Debug.Log(SaveGameManager.currentSaveData.playerData.partyData.movedEncounterIds.Count);
            SaveGameManager.Save();
            GridGameManager.Instance.CleanUp();
            SceneController.LoadSceneAsync(Scenes.EncounterArea , false);
            //SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
            
           // SceneController.OnSceneCompletelyFinished += UnloadBattleScene;

            // SceneTransferData.Instance.Reset();
            //
            // SceneController.UnLoadSceneAsync(Scenes.Battle1);
            // SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);

        }

      
    
        public void LoadEncounterAreaAfterBattle(bool victory)
        {
            // SceneTransferData.Instance.BattleOutCome = victory?BattleOutcome.Victory:  BattleOutcome.Defeat;
            //
            Player.Instance.Party.ResetFoodBuffs();
            SaveGameManager.Save();
            GridGameManager.Instance.CleanUp();
             SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
             //SceneController.OnSceneCompletelyFinished += UnloadBattleScene;
            
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);

        }

        public void LoadSanctuary()
        {
            SaveGameManager.Save();
            if(AreaGameManager.Instance!=null)
                AreaGameManager.Instance.CleanUp();
            if(GridGameManager.Instance!=null)
                GridGameManager.Instance.CleanUp();
            SceneController.LoadSceneAsync(Scenes.Sanctuary, false);
        }
    }
}
