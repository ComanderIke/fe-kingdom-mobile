using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Game.AI;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Encounters.Battle;
using Game.EncounterAreas.Model;
using Game.GameActors.Factions;
using Game.GameActors.Units;
using Game.Graphics.BattleAnimations;
using Game.Grid;
using Game.Grid.GridPathFinding;
using Game.Grid.Tiles;
using Game.GUI;
using Game.GUI.Controller;
using Game.GUI.Renderer;
using Game.GUI.Text;
using Game.Interfaces;
using Game.Map;
using Game.States;
using Game.Systems;
using GameEngine;
using UnityEngine;

namespace Game.Manager
{
    public class GridGameManager : MonoBehaviour, IServiceProvider
    {

        public static GridGameManager Instance;

        public static event Action OnStartGame;
        private bool init;
        public Transform Scene1InstantiatedContentParent;

        private List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public GridGameStateManager GameStateManager { get; set; }
        public BattleMap BattleMap { get; set; }

        public bool tutorial = false;

        private void Awake()
        {
            Instance = this;
            MyDebug.LogEngine("Awake " +gameObject.name);
            if (SceneTransferData.Instance != null &&SceneTransferData.Instance.BattleMap!=null )
                BattleMap = SceneTransferData.Instance.BattleMap;
            else
                BattleMap = FindObjectOfType<DemoUnits>().battleMap;
            MyDebug.LogLogic("BattleMap: "+BattleMap);
            if (SceneTransferData.Instance != null)
            {
                tutorial = SceneTransferData.Instance.TutorialBattle1;
            }
            Instantiate(BattleMap.mapPrefab, Scene1InstantiatedContentParent);
            //Debug.Log("Initialize");
            FactionManager = new FactionManager();
            var playerFaction=new Faction();
            var enemyFaction=new Faction();
            playerFaction.Id = FactionId.PLAYER;
            playerFaction.IsPlayerControlled = true;
            enemyFaction.Id = FactionId.ENEMY;
            FactionManager.AddFaction(playerFaction);
            FactionManager.AddFaction(enemyFaction);
            AddNoMonoBehaviourSystems();
            // Debug.Log(FactionManager.Factions[1].Units.Count);
            GameStateManager = new GridGameStateManager();
        }

        

        private void AddNoMonoBehaviourSystems()
        {
            MyDebug.LogLogic("Creating systems!");
            Systems = new List<IEngineSystem>
            {
                new TurnSystem(),
                new BattleSystem(),
                new BattleStatsSystem(),
                new MoveSystem(),
                new UnitProgressSystem(FactionManager),
                new PopUpTextSystem(),
                new ReinforcementSystem(FindObjectsOfType<Reinforcement>(), new UnitSpawnHelper(FactionManager, null)),
                new SkillSystem(GameBPData.Instance.SkillGenerationConfig,FindObjectsOfType<MonoBehaviour>().OfType<ISkillUIRenderer>().First()),
            };


        }

       
        private void AddMonoBehaviourSystems()
        {
        
            Systems.Add(FindObjectOfType<GridSystem>());
            Systems.Add(FindObjectOfType<UnitActionSystem>());
            Systems.Add(FindObjectOfType<UiSystem>());
            Systems.Add(FindObjectOfType<UnitSelectionSystem>());
            if (tutorial)
            {
                Systems.Add(FindObjectOfType<TutorialSystem>());
                GetSystem<TutorialSystem>().Activate();
                Debug.Log("Adding TutorialSystem!");
            }


        }

        private void Initialize()
        {
            AddMonoBehaviourSystems();
            active = true;
            InjectDependencies();
            SetUpEvents();
            foreach (var system in Systems)
            {
                system.Init();
                system.Activate();
            }
           
            
          
            GameStateManager.Init();
            
            OnStartGame?.Invoke();
            //GetSystem<TurnSystem>().StartPhase();
        }

        private void SetUpEvents()
        {
           
        }
        private void InjectDependencies()
        {
            // var type = typeof(IDependecyInjection);
            // var types = AppDomain.CurrentDomain.GetAssemblies()
            //     .SelectMany(s => s.GetTypes())
            //     .Where(p => type.IsAssignableFrom(p));
            
            // var battleRenderers = FindObjectsOfType<MonoBehaviour>().OfType<IBattleRenderer>();
            // GetSystem<BattleSystem>().BattleRenderer = battleRenderers.First();
            var gridSystem = GetSystem<GridSystem>();
            var tileChecker = new GridTileChecker(gridSystem.Tiles, BattleMap.GetWidth(), BattleMap.GetHeight());
            gridSystem.GridLogic.tileChecker = tileChecker;
            var pathFinder = new GridAStar(tileChecker);
            gridSystem.pathFinder = pathFinder;
            var combatInfo = GetSystem<BattleSystem>();
            Systems.Add(new AISystem(FactionManager.Factions[1], GetSystem<UnitActionSystem>(),gridSystem.GridLogic,combatInfo, pathFinder));
            GetSystem<MoveSystem>().tileChecker = tileChecker;
            GetSystem<MoveSystem>().pathFinder = pathFinder;
            GetSystem<TurnSystem>().factionManager = FactionManager;
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
           // GameStateManager.WinState.successRenderer =  FindObjectsOfType<MonoBehaviour>().OfType<IBattleSuccessRenderer>().First();
            GameStateManager.GameOverState.renderer =  FindObjectsOfType<MonoBehaviour>().OfType<IBattleLostRenderer>().First();
            GetSystem<BattleSystem>().BattleAnimation = ((IBattleAnimation)FindObjectOfType<BattleAnimationRenderer>()); //TODO .First can be different result inBuild
            GameStateManager.BattleState.battleSystem = GetSystem<BattleSystem>();
            GetSystem<BattleSystem>().BattleAnimation.Hide();
            GameStateManager.UnitPlacementState.UnitPlacementUI =  FindObjectsOfType<MonoBehaviour>().OfType<IUnitPlacementUI>().First();
            GameStateManager.PhaseTransitionState.phaseRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPhaseRenderer>().First();
            GameStateManager.PlayerPhaseState.mainState.playerPhaseUI = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPhaseUI>().First();
            GameStateManager.PlayerPhaseState.chooseTargetState.UI = FindObjectsOfType<MonoBehaviour>().OfType<IChooseTargetUI>().First();
            GetSystem<UnitProgressSystem>().levelUpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<ILevelUpRenderer>().First();
            
            GetSystem<UnitProgressSystem>().expRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IExpRenderer>().First();
            var expBars = FindObjectsOfType<MonoBehaviour>().OfType<ExpBarController>();
            foreach (var expBar in expBars)
            {
                if (expBar.CompareTag("MainExpBar"))
                    GetSystem<UnitProgressSystem>().ExpBarController = expBar;
            }
           // GetSystem<UnitProgressSystem>().ExpBarController = FindObjectsOfType<MonoBehaviour>().OfType<ExpBarController>().First();
            // Debug.Log("ExpBarController: "+GetSystem<UnitProgressSystem>().ExpBarController.gameObject.name);
        }

        private void Update()
        {
            if (!init)
            {
                MyDebug.LogLogic("Initialize GridGameManager");
                Initialize();
                // Debug.Log("INITIALIZECOMPLETE");
                init = true;
            }
            if (!active)
                return;
           

            GameStateManager.Update();
        }



        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }

        public Coroutine StartChildCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StopChildCoroutine(Coroutine coroutine)
        {
            throw new NotImplementedException();
        }

        private bool active = true;

        private void Deactivate()
        {
            // Debug.Log("DEACTIVATE SYSTEMS: "+Systems.Count);
            foreach (var system in Systems)
            {
                system.Deactivate();
            }

            active = false;
            
        }

        public void CleanUp()
        {
            Debug.Log("Do Stuff before loading new scene");
            Deactivate();
            GameStateManager.OnDisable();
        }
        private void OnDisable()
        {
            // Debug.Log("ON DISABLE GRID GAME MANAGER");
            Deactivate();
            
            // Debug.Log("Before Disable GameStateManager");
            GameStateManager.OnDisable();
           
        }
        // private void OnDestroy()
        // {
        //     GameStateManager.OnDestroy();
        //     Deactivate();
        //    
        // }
    }
}