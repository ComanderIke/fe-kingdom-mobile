using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GameResources;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using GameCamera;
using GameEngine;
using GameEngine.GameStates;
using GameEngine.Input;
using GameEngine.Tools;
using Menu;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class UnitPlacementState : GameState<NextStateTrigger>
    {
        private const float EXIT_DELAY = 0.5f;
        private float time = 0;
        private bool finished;
        
        public List<Unit> wholePartyUnits;
        public IUnitPlacementUI UnitPlacementUI { get; set; }
        public UnitPlacementInputSystem UnitPlacementInputSystem { get; set; }
        
        private FactionManager factionManager;
        private CameraSystem cameraSystem;
        private UnitSpawnHelper unitSpawnHelper;
        private StartPositionManager startPositionManager;
        public UnitPlacementState()
        {
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
          
        }
        public void Init()
        {
            cameraSystem.Init();
        }

        private void UnitSelectionChanged(List<Unit> units)
        {
            factionManager.Factions[0].ClearUnits();
            foreach (var unit in units)
            {
                factionManager.Factions[0].AddUnit(unit);
            }
            unitSpawnHelper.InstantiateUnits(units, startPositionManager.startPositions);
        }
        public override void Enter()
        {
            
            finished = false;
            factionManager = GridGameManager.Instance.FactionManager;
            NextState =  GridGameManager.Instance.GameStateManager.PhaseTransitionState;
            UnitPlacementUI.unitSelectionChanged += UnitSelectionChanged;
            UnitPlacementInputSystem = new UnitPlacementInputSystem();
            InitFactions();
            unitSpawnHelper = new UnitSpawnHelper(factionManager, UnitPlacementInputSystem);
            startPositionManager = new StartPositionManager(UnitPlacementInputSystem);
            wholePartyUnits = Player.Instance.Party.members;
            InitCamera();
            
            startPositionManager.SetUpInputForStartPos();
            startPositionManager.Init();
            startPositionManager.ShowStartPos();
            UnitPlacementUI.Show(wholePartyUnits, GridGameManager.Instance.BattleMap);
            UnitPlacementUI.OnFinished += () =>
            {
                //GameObject.FindObjectOfType<UIFactionCharacterCircleController>().Show(factionManager.Factions[0].Units);
                finished = true;
                
            };
            unitSpawnHelper.SpawnDestroyables();
            unitSpawnHelper.SpawnEnemies();
            unitSpawnHelper.SpawnPlayerUnits(factionManager.Factions[0].Units, startPositionManager.startPositions);
            unitSpawnHelper.DestroySpawns();
            InitUnits();
            
        }

      
        private void InitCamera()
        {
            int height = GridGameManager.Instance.BattleMap.height;
            int width = GridGameManager.Instance.BattleMap.width;
            //Order of Mixins Matters!!!Laggy Camera otherwise
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(TagManager.UnitTag),new MouseCameraInputProvider());

            cameraSystem.AddMixin<ViewOnGridMixin>().Construct(width, height);
        }

        private void CreateDemoParty()
        {
            Debug.Log("Create Demo Units!");
            var demoUnits = GameObject.FindObjectOfType<DemoUnits>().GetUnits();
            Player.Instance.Party.members = demoUnits;
            Player.Instance.Party.Initialize();
        }
        void InitFactions()
        {
            factionManager.Factions[0].ClearUnits();
            factionManager.Factions[1].ClearUnits();
            //Debug.Log("PartySizeBeforeInitFactions: "+ Player.Instance.Party.members.Count);
            if (Player.Instance.Party==null||Player.Instance.Party.members.Count==0)
            {
               CreateDemoParty();
            }
            
            foreach (var unit in Player.Instance.Party.members)
            {
                factionManager.Factions[0].AddUnit(unit);
            }
            

            // if (SceneTransferData.Instance.EnemyUnits != null)
            // {
            //     foreach (var unit in SceneTransferData.Instance.EnemyUnits.unitLayout)
            //     {
            //         factionManager.Factions[1].AddUnit(unit.unit);
            //     }
            // }
            
        }
        
        void InitUnits()
        {
            foreach (var faction in factionManager.Factions)
            {
                foreach (var unit in faction.FieldedUnits)
                {
                    unit.TurnStateManager.HasAttacked = false;
                    unit.TurnStateManager.HasMoved = false;
                    unit.TurnStateManager.IsSelected = false;
                    unit.TurnStateManager.IsWaiting = false;
                }
            }
        }
        
        public override GameState<NextStateTrigger> Update()
        {
            time += Time.deltaTime;
            if (time >= EXIT_DELAY)
            {
                if (finished)
                {
                    // if (time >= DELAY)
                    return NextState;
                }
            }

            return null;
        }

       
        public override void Exit()
        {
            DeInitializeCamera();
            UnitPlacementUI.Hide();
          
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            
            foreach (var unit in factionManager.Factions[0].FieldedUnits)
            {
                
                var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
                tile.tileVfx.Hide(tile);
            }

            startPositionManager.DeInitialize();
            startPositionManager.HideStartPos();
            
        }
        private void DeInitializeCamera()
        {
            if (cameraSystem != null && cameraSystem.gameObject != null)
            {
                cameraSystem.RemoveMixin<DragCameraMixin>();
                cameraSystem.RemoveMixin<ClampCameraMixin>();
                cameraSystem.RemoveMixin<ViewOnGridMixin>();
            }
        }

     


    }
}