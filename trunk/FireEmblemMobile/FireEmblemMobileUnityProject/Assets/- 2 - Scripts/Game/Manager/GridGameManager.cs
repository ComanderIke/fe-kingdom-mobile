using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.AI;
using Game.GameActors;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GameResources;
using Game.Graphics;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.GUI;
using Game.GUI.PopUpText;
using Game.GUI.Text;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Battle;
using GameCamera;
using GameEngine;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.Manager
{
    public class GridGameManager : MonoBehaviour
    {

        public static GridGameManager Instance;

        public static event Action OnStartGame;
        private bool init;

        private List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public GridGameStateManager GameStateManager { get; set; }

        private void Awake()
        {
            Instance = this;

            //Debug.Log("Initialize");
            AddSystems();
            FactionManager = new FactionManager(FindObjectOfType<PlayerConfig>().Factions.ToList());
            GameStateManager = new GridGameStateManager();
            Application.targetFrameRate = 60;
            // JITHelper.PreJitAll<GridSystem>();
            // JITHelper.PreJitAll<UiSystem>();
            // JITHelper.PreJitAll<GridLogic>();
            // JITHelper.PreJitAll<GridRenderer>();
            // JITHelper.PreJitAll<GridInputSystem>();
            // JITHelper.PreJitAll<UnitInputController>();
            // JITHelper.PreJitAll<Visuals>();
            // JITHelper.PreJitAll<TopUi>();
            // JITHelper.PreJitAll<NodeHelper>();
            // JITHelper.PreJitAll<DragManager>();
            // JITHelper.PreJitAll<UIFilledBarController>();
            // //JITHelper.PreJitAll<Unit>(); Does crash because of IClonealbe or ScriptableObject
            // JITHelper.PreJitAll<BattleStats>();
            // JITHelper.PreJitAll<ExpBarController>();
            // JITHelper.PreJitAll<Image>();
            // JITHelper.PreJitAll<ExperienceManager>();
            // JITHelper.PreJitAll<AttackPreviewStatBar>();
            // JITHelper.PreJitAll<AttackPreviewUI>();
            // JITHelper.PreJitAll<CursorAnimationBlinkController>();
            // JITHelper.PreJitAll<BattleSimulation>();
            // JITHelper.PreJitAll<BattleSystem>();
            // JITHelper.PreJitAll<UILoopPingPongFade>();
            // JITHelper.PreJitAll<RawImageUVOffsetAnimation>();
            // JITHelper.PreJitAll<AStar>();
            
            //JITHelper.PreJitAll<String>();
            //JITHelper.PreJitAll<GameObject>();
            //var method = typeof(MapSystem).GetMethod("HideMovementRangeOnGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //if (method != null)
            //{
            //    method.MethodHandle.GetFunctionPointer();
            //    Debug.Log("JIT Compilation Complete");
            //}
            //method = typeof(MapSystem).GetMethod("ShowMovementRangeOnGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //if (method != null)
            //{
            //    method.MethodHandle.GetFunctionPointer();
            //    Debug.Log("JIT Compilation Complete");
            //}
        }

        private void AddSystems()
        {
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<GridSystem>(),
                FindObjectOfType<AudioSystem>(),
                FindObjectOfType<UnitActionSystem>(),
                FindObjectOfType<UnitsSystem>(),
                FindObjectOfType<TurnSystem>(),
                FindObjectOfType<UiSystem>(),
                new BattleSystem(),
                new MoveSystem(),
                new UnitProgressSystem(),
                new PopUpTextSystem(),
                FindObjectOfType<UnitSelectionSystem>()
            };

        }

        private void Initialize()
        {
           
            InjectDependencies();
            foreach (var system in Systems)
            {
                system.Init();
                system.Activate();
            }


            LevelConfig();
          
            GameStateManager.Init();
            
            OnStartGame?.Invoke();
            //GetSystem<TurnSystem>().StartPhase();
        }

        private void InjectDependencies()
        {
            // var type = typeof(IDependecyInjection);
            // var types = AppDomain.CurrentDomain.GetAssemblies()
            //     .SelectMany(s => s.GetTypes())
            //     .Where(p => type.IsAssignableFrom(p));
            
            var battleRenderers = FindObjectsOfType<MonoBehaviour>().OfType<IBattleRenderer>();
            GetSystem<BattleSystem>().BattleRenderer = battleRenderers.First();
            var gridSystem = GetSystem<GridSystem>();
            var tileChecker = new GridTileChecker(gridSystem.Tiles, gridSystem.GridData.width, gridSystem.GridData.height);
            gridSystem.GridLogic.tileChecker = tileChecker;
            var pathFinder = new AStar(tileChecker);
            gridSystem.pathFinder = pathFinder;
            ScoreCalculater.pathFinder = pathFinder;
          
            GetSystem<MoveSystem>().tileChecker = tileChecker;
            GetSystem<MoveSystem>().pathFinder = pathFinder;
            GetSystem<TurnSystem>().factionManager = FactionManager;
            GetSystem<TurnSystem>().gameStateManager = GameStateManager;
            GameStateManager.WinState.renderer =  FindObjectsOfType<MonoBehaviour>().OfType<IWinRenderer>().First();
            GameStateManager.GameOverState.renderer =  FindObjectsOfType<MonoBehaviour>().OfType<IGameOverRenderer>().First();
            GameStateManager.BattleState.battleSystem = GetSystem<BattleSystem>();
            var chapterConfig = FindObjectOfType<ChapterConfig>();
            GameStateManager.ConditionScreenState.chapter = chapterConfig.chapter;
            GameStateManager.UnitPlacementState.UnitPlacementUI =  FindObjectsOfType<MonoBehaviour>().OfType<IUnitPlacementUI>().First();
            GameStateManager.PhaseTransitionState.phaseRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPhaseRenderer>().First();
            GameStateManager.PlayerPhaseState.playerPhaseUI = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPhaseUI>().First();
            GetSystem<UnitProgressSystem>().levelUpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<ILevelUpRenderer>().First();
            GetSystem<UnitProgressSystem>().ExpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<TopUi>().First().expRenderer;
        }

        private void Update()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }

            GameStateManager.Update();
        }

        private void LevelConfig()
        {
            

            
            // GameplayInput input = new GameplayInput();
            // input.SelectUnit(FactionManager.Factions[0].Units[0]);
            //input.DeselectUnit();


            //GetSystem<BattleSystem>().GetBattlePreview(FactionManager.Factions[0].Units[0], FactionManager.Factions[1].Units[0]);
        }

        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }
    }
}