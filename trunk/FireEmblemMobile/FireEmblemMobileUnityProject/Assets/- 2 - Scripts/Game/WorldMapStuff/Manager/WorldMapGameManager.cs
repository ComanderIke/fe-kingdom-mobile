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
using IPartyActionRenderer = Game.WorldMapStuff.Interfaces.IPartyActionRenderer;

namespace Game.WorldMapStuff.Manager
{
    public class WorldMapGameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static WorldMapGameManager Instance;
        private bool init;
        private List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public World World { get; set; }
        public WM_GameStateManager GameStateManager { get; set; }
        private bool active = true;

       

        private void Awake()
        {
            // if (Instance != null)//Reentering Scene
            //     return;
            Instance = this;
            World =  FindObjectOfType<World>();
            if (SaveData.currentSaveData != null)//TODO loading a second time when loading game from main Menu
            {
                Player.Instance.LoadData(SaveData.currentSaveData.playerData);
                Debug.Log("+++++++++++++++++"+Player.Instance.faction.Parties[0].members[0].name+" EXP:"+Player.Instance.faction.Parties[0].members[0].ExperienceManager.Exp);
                Campaign.Instance.LoadData(SaveData.currentSaveData.campaignData);
                
            }
            else
            {
                InitializePlayerData();
                InitializeCampaignData();
                Debug.Log("IF NOT LOADED!");
                IfNotLoaded();
            }

            FactionManager = new FactionManager();
            FactionManager.AddFaction(Player.Instance.faction);
            FactionManager.AddFaction(Campaign.Instance.EnemyFaction);
            AddSystems();
            Debug.Log("================="+Player.Instance.faction.Parties[0].members[0].name+" EXP:"+Player.Instance.faction.Parties[0].members[0].ExperienceManager.Exp);
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
            Player.Instance.faction = new WM_Faction(FactionId.PLAYER, "Player", true);
            Player.Instance.Name = "Player";
            Player.Instance.money = 0;
        }
        private void InitializeCampaignData()
        {
            Campaign.Instance.LoadConfig(GameData.Instance.campaigns[0]);
            Campaign.Instance.EnemyFaction = new WM_Faction(FactionId.ENEMY, "Enemy", false);
        }

        private void AddSystems()
        {
            var xyz = FindObjectsOfType<MonoBehaviour>().OfType<IWM_AttackPreviewRenderer>().First();
            var previewSystem = new WM_PreviewSystem(xyz);
            var selectionSystem = new WM_PartySelectionSystem(FactionManager);
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<AudioSystem>(),
                new TurnSystem(),
                selectionSystem,
                previewSystem,
                new WM_PartyActionSystem(previewSystem,selectionSystem),
            
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
            GetSystem<TurnSystem>().factionManager = FactionManager;
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            active = true;

        }

        private void DestroySpawns()
        {
            var partySpawns= FindObjectsOfType<PartySpawn>();
            for (int i = partySpawns.Length - 1; i >= 0; i-- )
            {
                GameObject.Destroy(partySpawns[i].gameObject);
            }
        }
        private void IfNotLoaded()
        {
            var startingParty = GameData.Instance.GetCampaignParty(0);
            var partySpawns= FindObjectsOfType<PartySpawn>();
            var startSpawn= FindObjectOfType<StartSpawn>();
            foreach (var spawn in partySpawns)
            {
                var partyInst = Instantiate(spawn.party);
                partyInst.members = new List<Unit>();
              //  Debug.Log("Spawn Party: "+spawn.party.name);
                foreach (Unit u in spawn.party.members)
                {
                    var instUnit = Instantiate(u);
                    instUnit.Initialize();
                   // Debug.Log("Unit: "+u.name);
                   
                    partyInst.members.Add(instUnit);
                    
                }

                partyInst.location = spawn.location.locationControllers[0];
               
                Campaign.Instance.EnemyFaction.AddParty(partyInst);
                
            }
          

            startingParty.location = startSpawn.location; 
            startingParty.members[0].ExperienceManager.AddExp(25);
            
            Player.Instance.faction.AddParty(startingParty);
            CreateSaveData();
        }



        private void InstantiateUnits()
        {

            var instantiator = FindObjectOfType<PartyInstantiator>();
            foreach (var faction1 in FactionManager.Factions)
            {
                var faction = (WM_Faction) faction1;
                foreach (Party actor in faction.Parties)
                {
                  
                    instantiator.InstantiateParty(actor, actor.location.worldMapPosition);
                }
            }

            
        }
        private void Initialize()
        {

            InjectDependencies();
            foreach (var system in Systems)
            {
                system.Init();
                system.Activate();
            }

           

            DestroySpawns();
            InstantiateUnits();
            
            GameStateManager.Init();
        }
        private void InjectDependencies()
        {

            GetSystem<TurnSystem>().factionManager = FactionManager;
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            GetSystem<WM_PartyActionSystem>().partyInstantiator = FindObjectOfType<PartyInstantiator>();
            GetSystem<WM_PartyActionSystem>().partyActionRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPartyActionRenderer>().First();
            GetSystem<WM_PartyActionSystem>().partyActionRenderer.Hide();
            GetSystem<WM_PartySelectionSystem>().partySelectionRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPartySelectionRenderer>().First();
            GetSystem<WM_PartySelectionSystem>().partySelectionRenderer.Hide();
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
