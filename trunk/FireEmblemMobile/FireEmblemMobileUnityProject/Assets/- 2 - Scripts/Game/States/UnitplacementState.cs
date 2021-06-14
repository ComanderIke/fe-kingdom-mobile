﻿using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
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
    public class UnitPlacementState : GameState<NextStateTrigger>, IDependecyInjection
    {
        private const float DELAY = 0.5f;
        private const int MAX_UNITS = 4;
        private float time = 0;
        private bool finished;
        public List<Unit> units;
        private UnitSpawner[] spawner;
        public IUnitPlacementUI UnitPlacementUI { get; set; }
        public UnitPlacementInputSystem UnitPlacementInputSystem { get; set; }

        private FactionManager factionManager;
        private UnitInstantiator unitInstantiator;
        private GridSystem gridSystem;
        public Chapter chapter;
        private CameraSystem cameraSystem;
        private StartPosition[] startPositions;

        public UnitPlacementState()
        {
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
        }

        private void InstantiateUnits(List<Unit>units)
        {
            SpawnPlayerUnits(units);
        }

        private void InitCamera()
        {
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(TagManager.UnitTag),new MouseCameraInputProvider());
            int height =  GridGameManager.Instance.GetSystem<GridSystem>().GridData.height;
            int width =  GridGameManager.Instance.GetSystem<GridSystem>().GridData.width;
            cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<ViewOnGridMixin>().zoom = 0;
        }

        private void SpawnEnemies()
        {
            var tmpEnemyUnits = factionManager.Factions[1].Units.Select(item => (Unit)item.Clone()).ToList();
            factionManager.Factions[1].Units.Clear();
            foreach (var faction in factionManager.Factions)
            {
                foreach (var spawn in spawner.Where(a => a.FactionId == faction.Id))
                {

                    if (tmpEnemyUnits.Count > spawn.id && tmpEnemyUnits[spawn.id] != null)
                    {
                        
                        var unit = GameObject.Instantiate(tmpEnemyUnits[spawn.id]) as Unit;
                      
                        faction.AddUnit(unit);
                        unit.Initialize();
                        unit.AIComponent.WeightSet = spawn.AIWeightSet;
                        unit.Faction = faction;

                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);

                    }
                    else
                    {
                        var unit = GameObject.Instantiate(spawn.unit) as Unit;
                        faction.AddUnit(unit);
                        unit.Initialize();
                        unit.AIComponent.WeightSet = spawn.AIWeightSet;

                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                    }
                }
            }
        }

        private void DeleteSpawnedPlayerUnit(StartPosition startPos)
        {
            unitInstantiator.UnPlaceCharacter(startPos.Actor);
            startPos.Actor = null;
        }
        private void SpawnPlayerUnits(List<Unit> selectedUnits)
        {
            //Debug.Log("Spawn Player Units: "+selectedUnits.Count);
           
            var playerFaction = factionManager.GetPlayerControlledFaction();
            int unitCnt = 0;
            List<Unit> newSpawnUnits = new List<Unit>();
            List<Unit> alreadySpawnUnits = new List<Unit>();
            for (int i = 0; i < startPositions.Length; i++)
            {
                if (startPositions[i].Actor != null)
                {
                    if (!selectedUnits.Contains(startPositions[i].Actor))
                    {
                        DeleteSpawnedPlayerUnit(startPositions[i]);
                    }
                    else
                    {
                        alreadySpawnUnits.Add(startPositions[i].Actor);
                    }
                }
            }

            newSpawnUnits = selectedUnits.Except(alreadySpawnUnits).ToList();
          //  Debug.Log("new Spawn Units: "+newSpawnUnits.Count);
            for (int i = 0; i < startPositions.Length; i++)
            {
                if (startPositions[i].Actor == null&& unitCnt < newSpawnUnits.Count)
                {
                    SpawnUnit(playerFaction, newSpawnUnits[unitCnt], startPositions[i].GetXOnGrid(),
                        startPositions[i].GetYOnGrid());
                    startPositions[i].Actor = newSpawnUnits[unitCnt];
                    unitCnt++;
                }
                

                
            }
            SetUpInputForUnits();
        }

        private void DestorySpawns()
        {
            foreach (var spawn in spawner)
            {
                GameObject.Destroy(spawn.gameObject);
            }
        }

        private void ShowStartPos()
        {
            foreach (var startpos in startPositions)
            { 
                var tile= gridSystem.GetTile(startpos.GetXOnGrid(), startpos.GetYOnGrid());
                tile.tileVfx.ShowSwapable(tile);
                //tile.TileRenderer.SwapVisual();
            }
        }
        private void HideStartPos()
        {
            foreach (var startpos in startPositions)
            { 
                var tile= gridSystem.GetTile(startpos.GetXOnGrid(), startpos.GetYOnGrid());
                tile.tileVfx.Hide(tile);
                //tile.TileRenderer.SwapVisual();
            }
        }
        public override void Enter()
        {
            finished = false;
            factionManager = GridGameManager.Instance.FactionManager;
            spawner = GameObject.FindObjectsOfType<UnitSpawner>();
      
            unitInstantiator = GameObject.FindObjectOfType<UnitInstantiator>();
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            UnitPlacementUI.unitSelectionChanged += InstantiateUnits;
            NextState =  GridGameManager.Instance.GameStateManager.PhaseTransitionState;
            startPositions = GameObject.FindObjectsOfType<StartPosition>();
            InitUnits();
            InitFactions();
            InitCamera();
            UnitPlacementInputSystem = new UnitPlacementInputSystem();
            UnitPlacementInputSystem.unitDroppedOnStartPos += SwapPosition;
            UnitPlacementInputSystem.unitDroppedOnOtherUnit += SwapUnits;
            SetUpInputForStartPos();
            SetUnits(factionManager.Factions[0].Units);
            SpawnEnemies();
            UnitPlacementUI.Show(units, chapter);
            UnitPlacementUI.OnFinished += () => { finished = true;};
        
           
            SpawnPlayerUnits(units);
            DestorySpawns();
            ShowStartPos();
            
            

           

            UnitSelectionSystem.OnDeselectCharacter += ShowSwapable;
            UnitSelectionSystem.OnSelectedCharacter += HideSwapable;
        }

        private void ShowSwapable(IGridActor unit)
        {
            var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            tile.tileVfx.ShowSwapable(tile);
        }

        private void HideSwapable(IGridActor unit)
        {
            var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            tile.tileVfx.Hide(tile);
        }

        void SwapPosition(Unit unit, StartPosition startPos)
        {
            if (startPos.Actor == unit)
            {
                //TODO RESET POSITION
            }
            if (startPos.Actor == null)
            {

                startPositions.FirstOrDefault(s => s.Actor == unit).Actor = null;
                startPos.Actor = unit;
                gridSystem.SetUnitPosition(unit,startPos.GetXOnGrid(), startPos.GetYOnGrid());
            }
        }
        void SwapUnits(Unit unit, Unit unit2)
        {
            if (unit == unit2)
            {
                return;
            }

            var startPos1= startPositions.FirstOrDefault(s => s.Actor == unit);
            var startPos2= startPositions.FirstOrDefault(s => s.Actor == unit2);
            startPos1.Actor = unit2;
            startPos2.Actor = unit;
            gridSystem.SwapUnits(unit,unit2);
            

            //unitInputController.transform.position = new Vector3(currentSelectedUnitController.unit.GridComponent.GridPosition.Xtransform.position);

        }

        void SpawnUnit(Faction faction, Unit unit, int x, int y)
        {
            
            unit.Faction = faction;
            unit.Initialize();
                       
            unitInstantiator.PlaceCharacter(unit, x, y);
            //Debug.Log("Spawn Unit"+ unit.name + " " + spawn.X + " " + spawn.Y+" ");
        }

        void InitFactions()
        {
            factionManager.Factions[0].ClearUnits();
            factionManager.Factions[1].ClearUnits();
            int cnt = 0;
            foreach (var unit in SceneTransferData.Instance.UnitsGoingIntoBattle)
            {
                cnt++;
                if(cnt <= 4)
                    factionManager.Factions[0].AddUnit(unit);
            }
        
            if (SceneTransferData.Instance.EnemyUnits != null)
            {
                foreach (var unit in SceneTransferData.Instance.EnemyUnits)
                {

                    factionManager.Factions[1].AddUnit(unit);
                }
            }
            
        }
        
        void InitUnits()
        {
            if (SceneTransferData.Instance.UnitsGoingIntoBattle == null || SceneTransferData.Instance.UnitsGoingIntoBattle.Count == 0)
            {
                Debug.Log("Create Demo Characters");
                var unit1 = GameData.Instance.GetHuman("Leila");
                var unit2 = GameData.Instance.GetHuman("Flora");
                var unit3 = GameData.Instance.GetHuman("Eldric");
                var unit4 = GameData.Instance.GetHuman("Hector");
                unit1.Initialize();
                unit2.Initialize();
                unit3.Initialize();
                unit4.Initialize();
                unit4.Inventory.AddItem(GameData.Instance.GetWeapon("Iron Sword"));
                unit3.Inventory.AddItem(GameData.Instance.GetWeapon("Iron Bow"));
                unit1.Inventory.AddItem(GameData.Instance.GetWeapon("Steel Bow"));
                unit2.Inventory.AddItem(GameData.Instance.GetWeapon("Fire"));
                SceneTransferData.Instance.UnitsGoingIntoBattle = new List<Unit>
                {
                    unit1,
                    unit2,
                    unit3,
                    unit4
                };
            }
    
        }

        private void SetUpInputForStartPos()
        {
           //Debug.Log("SETUP");
            foreach (var startPos in startPositions)
            {
               // Debug.Log("Set Up startPos inputReveiver: " + UnitPlacementInputSystem);
                startPos.touchInputReceiver = UnitPlacementInputSystem;
            }
        }
        private void SetUpInputForUnits()
        {
  

            // Debug.Log("TODO Only for Instantiated PartyMembers not extra Ones!");
            foreach (var unit in GridGameManager.Instance.FactionManager.Factions.SelectMany(faction => faction.Units))
            {
                // Debug.Log(unit.name+" "+unit.Faction.Name);
                // Debug.Log(unit.name+" "+  unit.GameTransformManager);
                // Debug.Log(unit.name+" "+  unit.GameTransformManager.UnitController);
                if(unit.GameTransformManager.GameObject!=null)
                    unit.GameTransformManager.UnitController.touchInputReceiver = UnitPlacementInputSystem;
                
            }
        }
        public void SetUnits(List<Unit> units)
        {
            this.units = units;
        }
        public override void Exit()
        {
            cameraSystem.RemoveMixin<DragCameraMixin>();
            cameraSystem.RemoveMixin<ClampCameraMixin>();
            cameraSystem.RemoveMixin<ViewOnGridMixin>();
            UnitPlacementUI.Hide();
            HideStartPos();
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            UnitPlacementInputSystem.unitDroppedOnOtherUnit -= SwapUnits;
            foreach (var unit in   factionManager.Factions[0].Units)
            {
                var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
                tile.tileVfx.Hide(tile);
            }
            UnitSelectionSystem.OnDeselectCharacter -= ShowSwapable;
            UnitSelectionSystem.OnSelectedCharacter -= HideSwapable;
        }

        public override GameState<NextStateTrigger> Update()
        {
            time += Time.deltaTime;
            if (time >= DELAY)
            {
                if (finished)
                {
                    // if (time >= DELAY)
                        return NextState;
                }
            }

            return null;
        }

        public void Init()
        {
            cameraSystem.Init();
        }
    }
}