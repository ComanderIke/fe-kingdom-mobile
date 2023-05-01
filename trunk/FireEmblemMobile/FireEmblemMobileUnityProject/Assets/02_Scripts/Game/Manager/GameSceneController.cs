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

        public void LoadInside(Party playerParty)
        {
        // Vector3 cameraPos = GameObject.FindObjectOfType<WorldMapCameraController>().transform.position;
        //             PlayerPrefs.SetFloat("CameraX", cameraPos.x);
        //             PlayerPrefs.SetFloat("CameraY", cameraPos.y);
        //             PlayerPrefs.Save();
        //             Debug.Log("Save Camera Pos: "+cameraPos);
        //     SceneTransferData.Instance.Reset();
        //     SceneTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
        //     Debug.Log("Load Inside" + playerParty.members[0].ExperienceManager.Exp);
        //     SceneTransferData.Instance.PartyID = playerParty.name;
        //     SceneTransferData.Instance.Party = playerParty;
        //     //SceneTransferData.Instance.LocationData =new LocationData(playerParty.location.UniqueId, playerParty.location.worldMapPosition.Village);
        //     SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
        //     SceneController.UnLoadSceneAsync(Scenes.Campaign1);
        //     SceneController.LoadSceneAsync(Scenes.InsideLocation,true);
        }
        public void LoadBattleLevel(Scenes buildIndex, EnemyArmyData enemyParty, EncounterNode node)
        {
            Vector3 cameraPos = GameObject.FindObjectOfType<EncounterAreaCameraController>().transform.position;
            PlayerPrefs.SetFloat("CameraX", cameraPos.x);
            PlayerPrefs.SetFloat("CameraX", cameraPos.y);
            PlayerPrefs.Save();
            SceneTransferData.Instance.Reset();
            
            Debug.Log("Load Battle Level: "+enemyParty.level + " " + enemyParty.name);
            Debug.Log("Scene: "+buildIndex);
            SceneTransferData.Instance.EnemyArmyData = enemyParty;
            //SceneTransferData.Instance.EnemyPartyID = enemyParty.name;
            //SceneTransferData.Instance.NodeData =new NodeData(node.UniqueId);
            
            //SceneController.UnLoadSceneAsync(Scenes.EncounterArea);
            SaveGameManager.Save();
            SceneController.LoadSceneAsync(buildIndex, false);
        }
        public void LoadWorldMapFromInside()
        {
            // SceneTransferData.Instance.Reset();
            // SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            // SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }

        public void LoadWorldMapBeforeBattle()
        {
            SaveGameManager.Save();
            SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
            
           // SceneController.OnSceneCompletelyFinished += UnloadBattleScene;

            // SceneTransferData.Instance.Reset();
            //
            // SceneController.UnLoadSceneAsync(Scenes.Battle1);
            // SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);

        }

        // void UnloadBattleScene()
        // {
        //     SceneController.OnSceneCompletelyFinished -= UnloadBattleScene;
        //     SceneController.UnLoadSceneAsync(Scenes.Battle1);
        // }
    
        public void LoadWorldMapAfterBattle(bool victory)
        {
            // SceneTransferData.Instance.BattleOutCome = victory?BattleOutcome.Victory:  BattleOutcome.Defeat;
            //
            SaveGameManager.Save();
             SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
             //SceneController.OnSceneCompletelyFinished += UnloadBattleScene;
            
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);

        }

        public void UnloadAllExceptMainMenu()
        {
            Debug.Log("Unload All ExceptMain!");
            // SceneTransferData.Instance.Reset();
            MainMenuController.Instance.Show();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                Scenes buildIndex = (Scenes)scene.buildIndex;
                if (buildIndex != Scenes.MainMenu)
                {
                    SceneController.UnLoadSceneAsync((buildIndex));
                }
            }
            
            // SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            // SceneController.UnLoadSceneAsync(Scenes.Battle1);
            // SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            // SceneController.UnLoadSceneAsync(Scenes.UI);
            // SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }
    }
}
