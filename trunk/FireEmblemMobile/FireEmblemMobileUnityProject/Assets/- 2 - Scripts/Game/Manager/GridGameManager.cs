using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Audio;
using Assets.Game;
using Assets.GameActors;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameCamera;
using Assets.GameEngine;
using Assets.GameInput;
using Assets.GameResources;
using Assets.Grid;
using Assets.Grid.PathFinding;
using Assets.GUI;
using Assets.GUI.PopUpText;
using Assets.Map;
using Assets.Mechanics;
using Assets.Mechanics.Battle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Game.Manager
{
    public class GridGameManager : MonoBehaviour
    {

        public static GridGameManager Instance;

        public static event Action OnStartGame;
        private bool init;

        public List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public GameStateManager GameStateManager { get; set; }

        private void Awake()
        {
            Instance = this;

            //Debug.Log("Initialize");
            AddSystems();
            FactionManager = new FactionManager();
            Application.targetFrameRate = 60;
            JITHelper.PreJitAll<MapSystem>();
            JITHelper.PreJitAll<UiSystem>();
            JITHelper.PreJitAll<GridLogic>();
            JITHelper.PreJitAll<GridRenderer>();
            JITHelper.PreJitAll<InputSystem>();
            JITHelper.PreJitAll<UnitInputController>();
            JITHelper.PreJitAll<PlayerInputFeedback>();
            JITHelper.PreJitAll<TopUi>();
            JITHelper.PreJitAll<NodeHelper>();
            JITHelper.PreJitAll<DragManager>();
            JITHelper.PreJitAll<FilledBarController>();
            //JITHelper.PreJitAll<Unit>(); Does crash because of IClonealbe or ScriptableObject
            JITHelper.PreJitAll<BattleStats>();
            JITHelper.PreJitAll<ExpBarController>();
            JITHelper.PreJitAll<Image>();
            JITHelper.PreJitAll<ExperienceManager>();
            JITHelper.PreJitAll<AttackPreviewStatBar>();
            JITHelper.PreJitAll<AttackPreviewUI>();
            JITHelper.PreJitAll<CursorAnimationBlinkController>();
            JITHelper.PreJitAll<BattleSimulation>();
            JITHelper.PreJitAll<BattleSystem>();
            JITHelper.PreJitAll<UILoopPingPongFade>();
            JITHelper.PreJitAll<RawImageUVOffsetAnimation>();
            JITHelper.PreJitAll<AStar>();
            
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
                FindObjectOfType<UiSystem>(),
                FindObjectOfType<CameraSystem>(),
                FindObjectOfType<MapSystem>(),
                FindObjectOfType<AudioSystem>(),
                FindObjectOfType<PopUpTextSystem>(),
                FindObjectOfType<UnitActionSystem>(),
                FindObjectOfType<InputSystem>(),
                FindObjectOfType<UnitsSystem>(),
                FindObjectOfType<TurnSystem>(),
                FindObjectOfType<FogOfWarSystem>(),
                new BattleSystem(),
                FindObjectOfType<UnitSelectionSystem>()
            };
        }

        private void Initialize()
        {
            LevelConfig();
            GameStateManager = new GameStateManager();
            GameStateManager.Init();
            Systems.Add(new MoveSystem(GetSystem<MapSystem>()));
            OnStartGame?.Invoke();
            GetSystem<TurnSystem>().StartTurn();
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
            var turnManager = GetSystem<TurnSystem>();
            var player = FactionManager.GetPlayerControlledFaction();
            var spawner = FindObjectsOfType<UnitSpawner>();
            var unitInstantiator = FindObjectOfType<UnitInstantiator>();
            var resources = FindObjectOfType<ResourceScript>();
            var data = FindObjectOfType<DataScript>();

            if (Player.Instance.Units == null || Player.Instance.Units.Count == 0)
            {
                //Debug.Log("Create Demo Characters");
                var unit1 = DataScript.Instance.GetHuman("Leila");
                var unit2 = DataScript.Instance.GetHuman("Flora");
                var unit3 = DataScript.Instance.GetHuman("Eldric");
                var unit4 = DataScript.Instance.GetHuman("Hector");
                unit1.Initialize();
                unit2.Initialize();
                unit3.Initialize();
                unit4.Initialize();
                unit4.Inventory.AddItem(DataScript.Instance.GetWeapon("Iron Sword"));
                unit3.Inventory.AddItem(DataScript.Instance.GetWeapon("Iron Bow"));
                unit1.Inventory.AddItem(DataScript.Instance.GetWeapon("Steel Bow"));
                unit2.Inventory.AddItem(DataScript.Instance.GetWeapon("Fire"));
                Player.Instance.Units = new List<Unit>
                {
                    unit1,
                    unit2,
                    unit3,
                    unit4
                };
            }
            //Debug.Log("LevelConfig");
           
            FactionManager.Factions[0].Units = Player.Instance.Units;
            FactionManager.Factions[0].Name = Player.Instance.Name;
            int[] indexes = new int [FactionManager.Factions.Count];
            foreach(var faction in FactionManager.Factions)
                foreach (var spawn in spawner.Where(a => a.FactionId == faction.Id))
                {
                    if (spawn.Unit != null)
                    {
                        var unit = Instantiate(spawn.Unit) as Unit;
                        faction.AddUnit(unit);
                        unit.Initialize();
                       
                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                        //Debug.Log("Spawn Unit"+unit.name +" "+spawn.X+" "+spawn.Y);
                    }
                    else if(faction.Units.Count!=0 && indexes[faction.Id]< faction.Units.Count)
                    {
                        var unit = faction.Units[indexes[faction.Id]++];
                        unit.Faction = faction;
                        unit.Initialize();
                       
                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                        //Debug.Log("Spawn Unit"+ unit.name + " " + spawn.X + " " + spawn.Y+" ");
                    }
                }
           
            foreach (var spawn in spawner)
            {
                Destroy(spawn.gameObject);
            }
            GameplayInput input = new GameplayInput();
            input.SelectUnit(FactionManager.Factions[0].Units[0]);
            input.DeselectUnit();
            
            
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