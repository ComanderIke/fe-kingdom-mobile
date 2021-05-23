using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Input;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
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
        public WM_GameStateManager GameStateManager { get; set; }
        private bool active = true;

        private void Awake()
        {
            Instance = this;
            var config = GameObject.FindObjectOfType<WM_Playerconfig>();
            FactionManager = new FactionManager(config.GetFactions().Cast<Faction>().ToList());
            AddSystems();
     
        
            GameStateManager = new WM_GameStateManager();
            Application.targetFrameRate = 60;
        }

        private void AddSystems()
        {
            var xyz = FindObjectsOfType<MonoBehaviour>().OfType<IWM_AttackPreviewRenderer>().First();
            var previewSystem = new WM_PreviewSystem(xyz);
            var selectionSystem = new WM_PartySelectionSystem(FactionManager);
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<AudioSystem>(),
                FindObjectOfType<TurnSystem>(),
                selectionSystem,
                previewSystem,
                new WM_PartyActionSystem(previewSystem,selectionSystem),
            
            };

        }

        public void Deactivate()
        {
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

            var startingParty = FindObjectOfType<DataScript>().GetCampaignParty(0);
            var partySpawns= FindObjectsOfType<PartySpawn>();
            var instantiator = FindObjectOfType<PartyInstantiator>();
            foreach (var spawn in partySpawns)
            {
                var partyInst = Instantiate(spawn.party);
                partyInst.members = new List<Unit>();
                foreach (Unit u in spawn.party.members)
                {
                    var instUnit = Instantiate(u);
                    instUnit.Initialize();
                    partyInst.members.Add(instUnit);
                    
                }
                instantiator.InstantiateParty(partyInst, spawn.location);
                ((WM_Faction)FactionManager.Factions[partyInst.Faction.Id]).AddParty(partyInst);
                
            }

            for (int i = partySpawns.Length - 1; i >= 0; i-- )
            {
                GameObject.Destroy(partySpawns[i].gameObject);
            }
            instantiator.InstantiatePartyAtStartPoint(startingParty);
            ((WM_Faction)FactionManager.Factions[startingParty.Faction.Id]).AddParty(startingParty);
            GameStateManager.Init();
            //GetSystem<TurnSystem>().StartPhase();
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
           
            GameStateManager.Update();
        }
        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }

    }
}
