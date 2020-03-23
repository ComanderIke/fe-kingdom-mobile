using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Audio;
using Assets.GameActors;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameCamera;
using Assets.GameInput;
using Assets.GameResources;
using Assets.GUI;
using Assets.GUI.PopUpText;
using Assets.Manager;
using Assets.Map;
using Assets.Mechanics;
using Assets.Mechanics.Dialogs;
using Assets.Utility;
using UnityEngine;

namespace Assets.Core
{
    public class GridGameManager : MonoBehaviour
    {

        public static GridGameManager Instance;

        private bool init;

        public List<IEngineSystem> Systems { get; set; }
        public FactionManager FactionManager { get; set; }
        public GameStateManager GameStateManager { get; set; }

        private void Awake()
        {
            Instance = this;

            Debug.Log("Initialize");
            AddSystems();
            FactionManager = new FactionManager();
        }

        private void AddSystems()
        {
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<UiSystem>(),
                FindObjectOfType<CameraSystem>(),
                FindObjectOfType<MapSystem>(),
                FindObjectOfType<AudioSystem>(),
                FindObjectOfType<SpeechBubbleSystem>(),
                FindObjectOfType<PopUpTextSystem>(),
                FindObjectOfType<UnitActionSystem>(),
                FindObjectOfType<InputSystem>(),
                FindObjectOfType<UnitsSystem>(),
                FindObjectOfType<TurnSystem>(),
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
            TurnSystem.OnStartTurn();
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
                Debug.Log("Create Demo Characters");
                var unit1 = DataScript.Instance.GetHuman("Leila");
                var unit2 = DataScript.Instance.GetHuman("Flora");
                var unit3 = DataScript.Instance.GetHuman("Eldric");
                var unit4 = DataScript.Instance.GetHuman("Hector");
                Player.Instance.Units = new List<Unit>
                {
                    unit1,
                    unit2,
                    unit3,
                    unit4
                };
            }
            Debug.Log("LevelConfig");
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
                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                        Debug.Log("Spawn Unit"+unit.name +" "+spawn.X+" "+spawn.Y);
                    }
                    else if(faction.Units.Count!=0 && indexes[faction.Id]< faction.Units.Count)
                    {
                        var unit = faction.Units[indexes[faction.Id]++];
                        unit.Faction = faction;
                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                        Debug.Log("Spawn Unit"+ unit.name + " " + spawn.X + " " + spawn.Y);
                    }
                }
            Debug.Log("Destroy Spawner");
            foreach (var spawn in spawner)
            {
                Destroy(spawn.gameObject);
            }
        }

        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }
    }
}