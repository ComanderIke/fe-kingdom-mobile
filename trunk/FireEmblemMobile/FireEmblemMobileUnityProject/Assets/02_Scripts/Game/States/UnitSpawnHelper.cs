using System.Collections.Generic;
using System.Linq;
using Game.GameActors;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.Manager;
using Game.Map;
using UnityEngine;
using UnityEngine.Rendering.UI;
using Utility;

namespace Game.States
{
    public class UnitSpawnHelper
    {
        private FactionManager factionManager;
        private UnitInstantiator unitInstantiator;
        private UnitSpawner[] spawner;
        private DestroyableController[]  destroyableControllers;
        private IUnitTouchInputReceiver touchInputReceiver;
        
        public UnitSpawnHelper( FactionManager factionManager,IUnitTouchInputReceiver touchInputReceiver)
        {
            this.factionManager = factionManager;
            unitInstantiator = GameObject.FindObjectOfType<UnitInstantiator>();
            spawner = GameObject.FindObjectsOfType<UnitSpawner>();
            destroyableControllers = GameObject.FindObjectsOfType<DestroyableController>();
            this.touchInputReceiver = touchInputReceiver;

        }
        public void InstantiateUnits(List<Unit>units,StartPosition[] startPositions)
        {
            SpawnPlayerUnits(units, startPositions);
        }

        public void SpawnDestroyables()
        {
            foreach (var faction in factionManager.Factions)
            {
                foreach (var desController in destroyableControllers.Where(a => a.factionID == faction.Id))
                {
                    //Create Instan otherwise object is shared
                    Destroyable dest =GameObject.Instantiate(desController.Destroyable);
                    faction.AddDestroyable(dest);
                    desController.Destroyable = dest;//Overwrite Blueprint
                    dest.Init();

                    unitInstantiator.PlaceDestroyable(desController, desController.X, desController.Y);
                }
            }
        }
        public void SpawnEnemies()
        {
           // var tmpEnemyUnits = factionManager.Factions[1].Units.Select(item => (Unit)item.Clone()).ToList();
            factionManager.Factions[1].Units.Clear();
            foreach (var faction in factionManager.Factions)
            {
                foreach (var spawn in spawner.Where(a => a.FactionId == faction.Id))
                {

                    // if (tmpEnemyUnits.Count > spawn.id && tmpEnemyUnits[spawn.id] != null)
                    // {
                    //     
                    //     var unit = GameObject.Instantiate(tmpEnemyUnits[spawn.id]) as Unit;
                    //   
                    //     faction.AddUnit(unit);
                    //     unit.Initialize();
                    //     unit.AIComponent.WeightSet = spawn.AIWeightSet;
                    //     unit.Faction = faction;
                    //
                    //     unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                    //
                    // }
                    // else
                    // {
                        var unit = GameObject.Instantiate(spawn.unit) as Unit;
                        faction.AddUnit(unit);
                        unit.Initialize();
                        unit.AIComponent.WeightSet = spawn.AIWeightSet;

                        unitInstantiator.PlaceCharacter(unit, spawn.X, spawn.Y);
                    //}
                }
            }
        }
        public void DeleteSpawnedPlayerUnit(StartPosition startPos)
        {
            unitInstantiator.UnPlaceCharacter(startPos.Actor);
            startPos.Actor = null;
        }
        public void SpawnPlayerUnits(List<Unit> selectedUnits, StartPosition[] startPositions)
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
            SetUpInputForUnits(selectedUnits);
        }
        public void SetUpInputForUnits(List<Unit> units)
        {
  

            // Debug.Log("TODO Only for Instantiated PartyMembers not extra Ones!");
            foreach (var unit in units)
            {

                if (unit.GameTransformManager!=null &&unit.GameTransformManager.GameObject != null)
                {
                    //Debug.Log("Setup Input for Unit: "+unit.name);
                    unit.GameTransformManager.UnitController.touchInputReceiver = touchInputReceiver;
                }

            }
        }
        public void DestroySpawns()
        {
            foreach (var spawn in spawner)
            {
                GameObject.Destroy(spawn.gameObject);
            }
        }
        public void SpawnUnit(Faction faction, Unit unit, int x, int y)
        {
            
            unit.Faction = faction;
            unit.Initialize();
            unit.Fielded = true;          
            unitInstantiator.PlaceCharacter(unit, x, y);
            Debug.Log("Spawn Unit"+ unit.name + " " + x + " " + y+" ");
        }
        
       
      

    }
}