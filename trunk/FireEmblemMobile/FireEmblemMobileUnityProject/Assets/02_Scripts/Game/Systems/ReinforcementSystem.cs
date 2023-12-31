using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.States;
using GameEngine;
using UnityEngine;

namespace LostGrace
{
    public class ReinforcementSystem : IEngineSystem
    {
        private List<Reinforcement> reinforcements;
        private TurnSystem turnSystem;
        private UnitSpawnHelper spawnHelper;

        public ReinforcementSystem(Reinforcement[] reinforcements, UnitSpawnHelper spawnHelper)
        {
            this.reinforcements = reinforcements.ToList();
            this.spawnHelper = spawnHelper;
        }

        public void Init()
        {
            turnSystem =GridGameManager.Instance.GetSystem<TurnSystem>();
           
            turnSystem.OnStartTurn += CheckTurnReinforcements;
            MyDebug.LogTODO("TODO Only on Wait otherwise it procks when dragged there");
            GridActorComponent.AnyUnitChangedPosition += CheckAreaReinforcements;
        }

        public void Deactivate()
        {
            turnSystem.OnStartTurn -= CheckTurnReinforcements;
            GridActorComponent.AnyUnitChangedPosition -= CheckAreaReinforcements;

        }

        void CheckAreaReinforcements(IGridActor movedUnit)
        {
            if (!movedUnit.Faction.IsPlayerControlled)
                return;
          
            foreach (var reinforcement in reinforcements)
            {
                
                List<ReinforcementUnit>addedUnits = new List<ReinforcementUnit>();
                foreach (var unit in reinforcement.ReinforcementUnits)
                {
                   
                    if (unit.Trigger == ReinforcementTrigger.Area)
                    {
                       
                        if (unit.area.Contains(movedUnit.GridComponent.GridPosition.AsVector()))
                        {
                            
                            //SpawnReinforcement(reinforcement, unit);
                           
                            var addedUnit = new ReinforcementUnit
                            {
                                turn = turnSystem.TurnCount,
                                unitBp = unit.unitBp,
                                AIWeightSet = unit.AIWeightSet,
                                FactionId = unit.FactionId,
                                Trigger = ReinforcementTrigger.Turn
                            };
                            addedUnits.Add(addedUnit);
                            
                            
                        }
                    }
                }

                foreach(var addedUnit in addedUnits)
                {
                    Debug.Log("Add as Turn Reinforcement Unit");
                    reinforcement.ReinforcementSpawned(addedUnit);
                    reinforcement.ReinforcementUnits.Add(addedUnit);
                   
                }
                    
            }
        }

        void CheckTurnReinforcements()
        {
            
            foreach (var reinforcement in reinforcements)
            {
                List<ReinforcementUnit> spawnedUnits = new List<ReinforcementUnit>();
                foreach (var unit in reinforcement.ReinforcementUnits)
                {
                    if (unit.Trigger == ReinforcementTrigger.Turn)
                    {
                        if (turnSystem.TurnCount >= unit.turn)
                        {
                            Debug.Log("SPAWN TURN UNIT ");
                            bool ret=SpawnReinforcement(reinforcement, unit);
                            if(ret)
                                spawnedUnits.Add(unit);
                           
                        }
                    }
                }

                foreach (var unit in spawnedUnits)
                {
                    reinforcement.ReinforcementSpawned(unit);
                    
                }
                if (reinforcement.ReinforcementUnits.Count == 0)
                    reinforcements.Remove(reinforcement);
                
            }
           
        }

        bool SpawnReinforcement(Reinforcement reinforcement, ReinforcementUnit unit)
        {
            
            //UnitPlacementState look how to spawn unit
            //add unit to enemy faction
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            var spawnedUnit=unit.unitBp.Create(Guid.NewGuid());
            if (!gridSystem.Tiles[reinforcement.X, reinforcement.Y].CanMoveOnto(spawnedUnit))
            {
                return false;
            }
          
           
            spawnedUnit.AIComponent.WeightSet = unit.AIWeightSet;
            SpawnFactionUnit(unit.FactionId, spawnedUnit, reinforcement.X, reinforcement.Y);

            return true;

        }

        void SpawnFactionUnit(FactionId factionId, Unit spawnedUnit, int x, int y)
        {
            foreach (var faction in GridGameManager.Instance.FactionManager.Factions)
            {
                if (faction.Id == factionId)
                {
                    faction.AddUnit(spawnedUnit);
                    spawnHelper.SpawnReinforcement(faction, spawnedUnit, x, y);
                }
            }
        }
        
        public void Activate()
        {
            turnSystem.OnStartTurn -= CheckTurnReinforcements;
            turnSystem.OnStartTurn += CheckTurnReinforcements;
            GridActorComponent.AnyUnitChangedPosition -= CheckAreaReinforcements;
            GridActorComponent.AnyUnitChangedPosition += CheckAreaReinforcements;
        }

        public void SpawnUnit(Unit summon, FactionId factionId, int posX, int posY)
        {
            Debug.Log("SPAWN UNIT");
           SpawnFactionUnit(factionId,summon, posX, posY);
        }
    }
}