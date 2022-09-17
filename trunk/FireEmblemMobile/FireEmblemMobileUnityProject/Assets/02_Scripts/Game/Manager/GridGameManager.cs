using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Grid;
using Game.Grid.GridPathFinding;
using Game.GUI;
using Game.GUI.Text;
using Game.Map;
using Game.Mechanics;
using Game.States;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

namespace Game.Manager
{
    public class GridGameManager : MonoBehaviour
    {

        public static GridGameManager Instance;

        public static event Action OnStartGame;
        private bool init;
        public Transform Scene1InstantiatedContentParent;

        private List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public GridGameStateManager GameStateManager { get; set; }
        public BattleMap BattleMap { get; set; }

        private void Awake()
        {
            Instance = this;
            if (SceneTransferData.Instance != null &&SceneTransferData.Instance.EnemyArmyData!=null&&SceneTransferData.Instance.EnemyArmyData.battleMapPool != null)
                BattleMap = SceneTransferData.Instance.EnemyArmyData.battleMapPool.GetRandomMap();
            else
                BattleMap = FindObjectOfType<DemoUnits>().battleMap;
            Debug.Log("Choose BattleMap: "+BattleMap);
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
            AddSystems();
            
            GameStateManager = new GridGameStateManager();
            Application.targetFrameRate = 60;
        }

        private void AddSystems()
        {
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<GridSystem>(),
                FindObjectOfType<AudioSystem>(),
                FindObjectOfType<UnitActionSystem>(),
             new TurnSystem(),
                FindObjectOfType<UiSystem>(),
                new BattleSystem(),
                new MoveSystem(),
                new UnitProgressSystem(FactionManager),
                new PopUpTextSystem(),
                
                FindObjectOfType<UnitSelectionSystem>()
            };
            

        }

        private void Initialize()
        {
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
            var tileChecker = new GridTileChecker(gridSystem.Tiles, BattleMap.width, BattleMap.height);
            gridSystem.GridLogic.tileChecker = tileChecker;
            var pathFinder = new GridAStar(tileChecker);
            gridSystem.pathFinder = pathFinder;
            var combatInfo = GetSystem<BattleSystem>();
            Systems.Add(new AISystem(FactionManager.Factions[1], GetSystem<UnitActionSystem>(),gridSystem.GridLogic,combatInfo, pathFinder));
            GetSystem<MoveSystem>().tileChecker = tileChecker;
            GetSystem<MoveSystem>().pathFinder = pathFinder;
            GetSystem<TurnSystem>().factionManager = FactionManager;
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            GameStateManager.WinState.renderer =  FindObjectsOfType<MonoBehaviour>().OfType<IBattleSuccessRenderer>().First();
            GameStateManager.GameOverState.renderer =  FindObjectsOfType<MonoBehaviour>().OfType<IBattleLostRenderer>().First();
            GameStateManager.BattleState.battleSystem = GetSystem<BattleSystem>();
            GameStateManager.BattleState.BattleAnimation = FindObjectsOfType<MonoBehaviour>().OfType<IBattleAnimation>().First();
            GameStateManager.BattleState.MapBattleAnimation = FindObjectsOfType<MonoBehaviour>().OfType<IBattleAnimation>().Last();
            GameStateManager.BattleState.BattleAnimation.Hide();
            GameStateManager.BattleState.MapBattleAnimation.Hide();
            GameStateManager.UnitPlacementState.UnitPlacementUI =  FindObjectsOfType<MonoBehaviour>().OfType<IUnitPlacementUI>().First();
            GameStateManager.PhaseTransitionState.phaseRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPhaseRenderer>().First();
            GameStateManager.PlayerPhaseState.mainState.playerPhaseUI = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPhaseUI>().First();
            GameStateManager.PlayerPhaseState.chooseTargetState.UI = FindObjectsOfType<MonoBehaviour>().OfType<IChooseTargetUI>().First();
            GetSystem<UnitProgressSystem>().levelUpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<ILevelUpRenderer>().First();
            
        }

        private void Update()
        {
            if (!init)
            {
                Initialize();
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

        private bool active = true;
        public void Deactivate()
        {
            foreach (var system in Systems)
            {
                system.Deactivate();
            }

            active = false;
        }

        private void OnDestroy()
        {
            Deactivate();
        }
    }
}