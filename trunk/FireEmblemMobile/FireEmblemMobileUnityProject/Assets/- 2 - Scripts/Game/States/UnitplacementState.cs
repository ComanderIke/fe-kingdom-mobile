using System.Collections.Generic;
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
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class UnitPlacementState : GameState<NextStateTrigger>, IDependecyInjection
    {
        private const float DELAY = 1.0f;
        private float time = 0;
        public List<Unit> units;
        public IUnitPlacementUI UnitPlacementUI { get; set; }
        public IUnitTouchInputReceiver UnitPlacementInputSystem { get; set; }

        private FactionManager factionManager;
        private UnitInstantiator unitInstantiator;
        public override void Enter()
        {
            factionManager = GridGameManager.Instance.FactionManager;
            var spawner = GameObject.FindObjectsOfType<UnitSpawner>();
            unitInstantiator = GameObject.FindObjectOfType<UnitInstantiator>();
            var resources = GameObject.FindObjectOfType<ResourceScript>();
            var data = GameObject.FindObjectOfType<DataScript>();
           

            InitUnits();
            InitFactions();
            //Debug.Log("LevelConfig");
          
            GameStateManager.UnitPlacementState.SetUnits(factionManager.Factions[0].Units);
            int[] indexes = new int [factionManager.Factions.Count];
            foreach(var faction in factionManager.Factions)
                foreach (var spawn in spawner.Where(a => a.FactionId == faction.Id))
                {
                    if (spawn.unit != null)
                    {
                        var unit = GameObject.Instantiate(spawn.unit) as Unit;
        
                       
                        faction.AddUnit(unit);
                        unit.Initialize();
                        unit.AIComponent.WeightSet = spawn.AIWeightSet;
                       
                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                        //Debug.Log("Spawn Unit"+unit.name +" "+spawn.X+" "+spawn.Y);
                    }
                    else if(faction.Units.Count!=0 && indexes[faction.Id]< faction.Units.Count)
                    {
                        var unit = faction.Units[indexes[faction.Id]++];
                        SpawnUnit(faction, unit, spawn.X, spawn.Y);
                    }
                }
           
           
          //  Debug.Log("UnitPlacement"+units.Count());
            NextState = GameStateManager.PlayerPhaseState;
            UnitPlacementUI.Show(units);
            var startPositions = GameObject.FindObjectsOfType<StartPosition>();
            UnitPlacementInputSystem = new UnitPlacementInputSystem();

            var playerFaction = factionManager.GetPlayerControlledFaction();
            for (int i = 0;
                i < startPositions.Length && i < playerFaction.Units.Count();
                i++)
            {
                SpawnUnit(playerFaction,playerFaction.Units[i], startPositions[i].GetXOnGrid(), startPositions[i].GetYOnGrid());
            }
            
            SetUpInputForUnits();
            foreach (var spawn in spawner)
            {
                GameObject.Destroy(spawn.gameObject);
            }
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
            foreach (var unit in Player.Instance.Units)
            {
                factionManager.Factions[0].AddUnit(unit);
            }
            factionManager.Factions[0].Name = Player.Instance.Name;
        }
        void InitUnits()
        {
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
        }
        private void SetUpInputForUnits()
        {
            
            foreach (var unit in GridGameManager.Instance.FactionManager.Factions.SelectMany(faction => faction.Units))
            {
                unit.GameTransformManager.UnitController.touchInputReceiver = UnitPlacementInputSystem;
                
            }
        }
        public void SetUnits(List<Unit> units)
        {
            this.units = units;
        }
        public override void Exit()
        {
            UnitPlacementUI.Hide();
        }

        public override GameState<NextStateTrigger> Update()
        {
            time += Time.deltaTime;
            if (time >= DELAY)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // if (time >= DELAY)
                    //     return NextState;
                }
            }

            return null;
        }
    }
}