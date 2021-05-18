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
using Game.Map;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class UnitPlacementState : GameState<NextStateTrigger>, IDependecyInjection
    {
        private const float DELAY = 0.5f;
        private float time = 0;
        private bool finished;
        public List<Unit> units;
        public IUnitPlacementUI UnitPlacementUI { get; set; }
        public UnitPlacementInputSystem UnitPlacementInputSystem { get; set; }

        private FactionManager factionManager;
        private UnitInstantiator unitInstantiator;
        private GridSystem gridSystem;
        public override void Enter()
        {
            finished = false;
            factionManager = GridGameManager.Instance.FactionManager;
            var spawner = GameObject.FindObjectsOfType<UnitSpawner>();
            unitInstantiator = GameObject.FindObjectOfType<UnitInstantiator>();
            var resources = GameObject.FindObjectOfType<ResourceScript>();
            var data = GameObject.FindObjectOfType<DataScript>();
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();

            InitUnits();
            InitFactions();

          
            GridGameManager.Instance.GameStateManager.UnitPlacementState.SetUnits(factionManager.Factions[0].Units);
            int[] indexes = new int [factionManager.Factions.Count];
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

            NextState =  GridGameManager.Instance.GameStateManager.PhaseTransitionState;
            UnitPlacementUI.Show(units);
            UnitPlacementUI.OnFinished += () =>
            {
                finished = true;};
            var startPositions = GameObject.FindObjectsOfType<StartPosition>();
            UnitPlacementInputSystem = new UnitPlacementInputSystem();
            UnitPlacementInputSystem.unitDroppedOnOtherUnit += SwapUnits;
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

            foreach (var unit in Player.Instance.Units)
            {
                var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
               tile.tileVfx.ShowSwapable(tile);
               //tile.TileRenderer.SwapVisual();
            }
        }

        void SwapUnits(Unit unit, Unit unit2)
        {
            

      
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
            foreach (var unit in BattleTransferData.Instance.UnitsGoingIntoBattle)
            {

                factionManager.Factions[0].AddUnit(unit);
            }

            if (BattleTransferData.Instance.EnemyUnits != null)
            {
                foreach (var unit in BattleTransferData.Instance.EnemyUnits)
                {

                    factionManager.Factions[1].AddUnit(unit);
                }
            }

            factionManager.Factions[0].Name = BattleTransferData.Instance.PlayerName;
        }
        void InitUnits()
        {
            if (BattleTransferData.Instance.UnitsGoingIntoBattle == null || BattleTransferData.Instance.UnitsGoingIntoBattle.Count == 0)
            {
                Debug.Log("Create Demo Characters");
                Debug.Log("WHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY??????????????");
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
                BattleTransferData.Instance.UnitsGoingIntoBattle = new List<Unit>
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
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            UnitPlacementInputSystem.unitDroppedOnOtherUnit -= SwapUnits;
            foreach (var unit in Player.Instance.Units)
            {
                var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
                tile.tileVfx.Hide(tile);
            }
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
    }
}