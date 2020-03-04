using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Audio;
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
    public class MainScript : MonoBehaviour
    {

        public static MainScript Instance;

        private bool init;

        public List<IEngineSystem> Systems { get; set; }
        public PlayerManager PlayerManager { get; set; }
        public GameStateManager GameStateManager { get; set; }

        private void Awake()
        {
            Instance = this;

            Debug.Log("Initialize");
            AddSystems();
            PlayerManager = new PlayerManager();
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
            var player = PlayerManager.GetRealPlayer();
            var startPositions = FindObjectsOfType<StartPosition>();
            var unitInstantiator = FindObjectOfType<UnitInstantiator>();
            var resources = FindObjectOfType<ResourceScript>();
            var data = FindObjectOfType<DataScript>();

            var unit1 = DataScript.Instance.GetHuman("Leila");
            var unit2 = DataScript.Instance.GetHuman("Flora");
            var unit3 = DataScript.Instance.GetHuman("Eldric");
            var unit4 = DataScript.Instance.GetHuman("Hector");
            player.AddUnit(unit1);
            player.AddUnit(unit2);
            player.AddUnit(unit3);
            player.AddUnit(unit4);
            unitInstantiator.PlaceCharacter(unit1, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
            unitInstantiator.PlaceCharacter(unit2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
            unitInstantiator.PlaceCharacter(unit3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
            unitInstantiator.PlaceCharacter(unit4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());

            var player2Units = GameObject.Find("Player2").GetComponentsInChildren<UnitController>();
            foreach (var uc in player2Units)
            {
                var unit = uc.Unit;
                PlayerManager.Players[1].AddUnit(unit);
                unit.GameTransform.GameObject = uc.gameObject;
                unit.SetPosition((int) unit.GameTransform.GetPosition().x, (int) unit.GameTransform.GetPosition().y);
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