using UnityEngine;
using System.Collections.Generic;
using Assets.GameActors.Units;
using Assets.GameActors.Players;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameEngine;
using Assets.Game.GameStates;
using Assets.Game.Manager;

public class FogOfWarSystem : MonoBehaviour, IEngineSystem
{
    List<Unit> PlayerUnits;
    private GridGameManager gridGameManager;
    public int sightRange = 2;
    void Start() {
        gridGameManager = GridGameManager.Instance;
        GridGameManager.OnStartGame += UpdateFogOfWar;
        MovementState.OnMovementFinished += (Unit u) =>UpdateFogOfWar();
    }
    public void UpdateFogOfWar()
    {
        foreach (Unit updateUnit in gridGameManager.FactionManager.GetPlayerControlledFaction().Units)
        {
            foreach (Faction faction in gridGameManager.FactionManager.Factions)
            {
                if (faction.Id == updateUnit.Faction.Id)
                    continue;
                foreach (Unit unit in faction.Units)
                {
                    unit.IsVisible = false;
                }
            }
        }
        foreach (Unit updateUnit in gridGameManager.FactionManager.GetPlayerControlledFaction().Units)
        {
            foreach (Faction faction in gridGameManager.FactionManager.Factions)
            {
                if (faction.Id == updateUnit.Faction.Id)
                    continue;
                foreach (Unit unit in faction.Units)
                {
                    if (unit.IsVisible)
                        continue;
                    CheckUnits(updateUnit, unit);
                        
                }
            } 
        }
    }
    private void CheckUnits(Unit playerUnit, Unit enemyUnit)
    {
        int delta = (int)Mathf.Abs(playerUnit.GridPosition.X - enemyUnit.GridPosition.X)+ (int)Mathf.Abs(playerUnit.GridPosition.Y - enemyUnit.GridPosition.Y);
        if(delta <= sightRange)
        {
            SetVisible(enemyUnit, true);
        }
        else
        {
            SetVisible(enemyUnit, false);
        }
    }
    private void SetVisible(Unit unit, bool visible)
    {
        unit.IsVisible = visible;
        unit.GameTransform.GameObject.GetComponent<UnitRenderer>().SetVisible(visible);
    }
}