using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.GUI;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using Game.Menu;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using Game.WorldMapStuff.UI;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public class WorldMapGameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static WorldMapGameManager Instance;
        private bool init;
        private List<IEngineSystem> Systems { get; set; }
        public WM_GameStateManager GameStateManager { get; set; }
        private bool active = true;

       

        private void Awake()
        {
            // if (Instance != null)//Reentering Scene
            //     return;
            Instance = this;
            if (SaveData.currentSaveData != null)//TODO loading a second time when loading game from main Menu
            {
                Player.Instance.LoadData(SaveData.currentSaveData.playerData);
                //Debug.Log("+++++++++++++++++"+Player.Instance.faction.Parties[0].members[0].name+" EXP:"+Player.Instance.faction.Parties[0].members[0].ExperienceManager.Exp);
                Campaign.Instance.LoadData(SaveData.currentSaveData.campaignData);
                
            }
            else
            {
                InitializePlayerData();
                InitializeCampaignData();
                Debug.Log("IF NOT LOADED!");
            }


            //FactionManager.AddFaction(Player.Instance.faction);

            AddSystems();
            //Debug.Log("================="+Player.Instance.faction.Parties[0].members[0].name+" EXP:"+Player.Instance.faction.Parties[0].members[0].ExperienceManager.Exp);
            GameStateManager = new WM_GameStateManager();
            Application.targetFrameRate = 60;
            float x = PlayerPrefs.GetFloat("CameraX");
            float y = PlayerPrefs.GetFloat("CameraY");
            float z = GameObject.FindObjectOfType<WorldMapCameraController>().transform.position.z;
            Debug.Log("Load Camera Pos: "+new Vector3(x,y,z));
            GameObject.FindObjectOfType<WorldMapCameraController>().transform.position = new Vector3(x,y,z);
        }

        private void CreateSaveData()
        {
            SaveData.currentSaveData = new SaveData(Player.Instance, Campaign.Instance);
        }
        private void InitializePlayerData()
        {
            Player.Instance.Name = "Player";
            Player.Instance.money = 0;
        }
        private void InitializeCampaignData()
        {
            Campaign.Instance.LoadConfig(GameData.Instance.campaigns[0]);
        }

        private void AddSystems()
        {

            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<AudioSystem>(),
                new TurnSystem(),
                new Area_ActionSystem(),
            };

        }

        private void OnDestroy()
        {
            Deactivate();
        }

        public void Deactivate()
        {
            GameStateManager.Deactivate();
            foreach (var system in Systems)
            {
                system.Deactivate();
            }

            active = false;
        }
        public void Activate()
        {
            foreach (var system in Systems)
            {
                system.Activate();
            }
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            active = true;

        }

        private void Initialize()
        {

            InjectDependencies();
            foreach (var system in Systems)
            {
                system.Init();
                system.Activate();
            }
            GameStateManager.Init();
        }
        private void InjectDependencies()
        {
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            GameStateManager.PhaseTransitionState.phaseRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPhaseRenderer>().First();
            GameStateManager.PlayerPhaseState.playerPhaseUI = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPhaseUI>().First();
            GameStateManager.WM_WinState.renderer = FindObjectsOfType<MonoBehaviour>().OfType<IWinRenderer>().First();
            GameStateManager.WM_GameOverState.renderer = FindObjectsOfType<MonoBehaviour>().OfType<IGameOverRenderer>().First();

        }
        private void Update()
        {
            if (!active)
                return;
            if (!init)
            {
                Initialize();
                init = true;
            }
           
            Instance.GameStateManager.Update();
        }
        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }

       
    }
}
